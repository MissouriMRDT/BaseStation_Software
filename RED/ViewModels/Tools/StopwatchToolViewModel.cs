using Caliburn.Micro;
using RED.Configurations.Tools;
using RED.Contexts.Tools;
using Core.Interfaces;
using RED.Models.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace RED.ViewModels.Tools
{
    public class StopwatchToolViewModel : PropertyChangedBase
    {
        private readonly StopwatchToolModel _model;
        private readonly IConfigurationManager _configManager;

        private const string SchedulesConfigName = "StopwatchSchedules";

        public ObservableCollection<ScheduleViewModel> Schedules
        {
            get
            {
                return _model.Schedules;
            }
            private set
            {
                _model.Schedules = value;
                NotifyOfPropertyChange(() => Schedules);
            }
        }

        public ScheduleViewModel SelectedSchedule
        {
            get
            {
                return _model.SelectedSchedule;
            }
            set
            {
                _model.SelectedSchedule = value;
                Reset();
                NotifyOfPropertyChange(() => SelectedSchedule);
            }
        }
        public DispatcherTimer Timer
        {
            get
            {
                return _model.Timer;
            }
            private set
            {
                _model.Timer = value;
                NotifyOfPropertyChange(() => Timer);
            }
        }
        public bool IsRunning
        {
            get
            {
                return _model.IsRunning;
            }
            set
            {
                _model.IsRunning = value;
                NotifyOfPropertyChange(() => IsRunning);
            }
        }

        public TimeSpan FixTime
        {
            get
            {
                return _model.FixTime;
            }
            set
            {
                _model.FixTime = value;
                NotifyOfPropertyChange(() => FixTime);
            }
        }
        public ScheduleViewModel SelectedEditSchedule
        {
            get
            {
                return _model.SelectedEditSchedule;
            }
            set
            {
                _model.SelectedEditSchedule = value;
                NotifyOfPropertyChange(() => SelectedEditSchedule);
            }
        }
        public SchedulePhaseViewModel SelectedEditPhase
        {
            get
            {
                return _model.SelectedEditPhase;
            }
            set
            {
                _model.SelectedEditPhase = value;
                NotifyOfPropertyChange(() => SelectedEditPhase);
            }
        }

        public DateTime StartTime
        {
            get
            {
                return _model.StartTime;
            }
            private set
            {
                _model.StartTime = value;
                NotifyOfPropertyChange(() => StartTime);
                NotifyOfPropertyChange(() => ScheduleRemainingTime);
                NotifyOfPropertyChange(() => ScheduleElapsedPercent);
                NotifyOfPropertyChange(() => PhaseElapsedTime);
                NotifyOfPropertyChange(() => PhaseRemainingTime);
                NotifyOfPropertyChange(() => PhaseElapsedPercent);
            }
        }
        public TimeSpan ElapsedTime
        {
            get
            {
                return _model.ElapsedTime;
            }
            private set
            {
                _model.ElapsedTime = value;
                NotifyOfPropertyChange(() => ElapsedTime);
                NotifyOfPropertyChange(() => CurrentPhase);
                NotifyOfPropertyChange(() => ScheduleElapsedTime);
                NotifyOfPropertyChange(() => ScheduleRemainingTime);
                NotifyOfPropertyChange(() => ScheduleElapsedPercent);
                NotifyOfPropertyChange(() => PhaseElapsedTime);
                NotifyOfPropertyChange(() => PhaseRemainingTime);
                NotifyOfPropertyChange(() => PhaseElapsedPercent);
            }
        }

        public SchedulePhaseViewModel CurrentPhase
        {
            get
            {
                return SelectedSchedule.PhaseAtTime(TaskCount);
            }
        }
        public TimeSpan ScheduleElapsedTime
        {
            get
            {
                return ElapsedTime;
            }
        }
        public TimeSpan ScheduleRemainingTime
        {
            get
            {
                return SelectedSchedule.Duration - ScheduleElapsedTime;
            }
        }
        public float ScheduleElapsedPercent
        {
            get
            {
                return (float)((double)ScheduleElapsedTime.Ticks / SelectedSchedule.Duration.Ticks * 100);
            }
        }
        public TimeSpan PhaseElapsedTime
        {
            get
            {
                return SelectedSchedule.ElapsedTimeInPhase(ScheduleElapsedTime, SkipTime, TaskCount);
            }
        }
        public TimeSpan PhaseRemainingTime
        {
            get
            {
                return SelectedSchedule.RemainingTimeInPhase(ScheduleElapsedTime, SkipTime, TaskCount);
            }
        }
        public float PhaseElapsedPercent
        {
            get
            {
                return (float)((double)PhaseElapsedTime.Ticks / SelectedSchedule.PhaseAtTime(TaskCount).Duration.Ticks * 100);
            }
        }
        public TimeSpan SkipTime
        {
            get
            {
                return _model.SkipTime;
            }
            set
            {
                _model.SkipTime = value;
                NotifyOfPropertyChange(() => SkipTime);
                NotifyOfPropertyChange(() => SkipSeconds);
            }
        }
        public float SkipSeconds
        {
            get
            {
                return _model.SkipTime.Minutes*60 + _model.SkipTime.Seconds;
            }
        }
        public int TaskCount
        {
            get
            {
                return _model.TaskCount;
            }
            set
            {
                _model.TaskCount = value;
                NotifyOfPropertyChange(() => TaskCount);
            }
        }
        public StopwatchToolViewModel(IConfigurationManager configs)
        {
            _model = new StopwatchToolModel();
            _configManager = configs;

            Schedules = new ObservableCollection<ScheduleViewModel>();
            ElapsedTime = TimeSpan.FromSeconds(0);

            _configManager.AddRecord(SchedulesConfigName, StopwatchToolConfig.DefaultSchedules);

            var ctx = _configManager.GetConfig<StopwatchContext>(SchedulesConfigName);
            foreach (StopwatchScheduleContext schedule in ctx.Schedules)
                Schedules.Add(new ScheduleViewModel(schedule));

            SelectedSchedule = Schedules.FirstOrDefault();
            SkipTime = TimeSpan.Zero;
            TaskCount = 0;

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
        }

        public void Toggle()
        {
            if (IsRunning)
                Stop();
            else
                Start();
        }
        public void Start()
        {
            if (ElapsedTime >= SelectedSchedule.Duration) return;
            StartTime = DateTime.Now - ElapsedTime;
            Timer.Interval = TimeSpan.FromSeconds(Math.Ceiling(ElapsedTime.TotalSeconds) - ElapsedTime.TotalSeconds);
            Timer.Start();
            IsRunning = true;
        }
        public void Stop()
        {
            Timer.Stop();
            ElapsedTime = DateTime.Now - StartTime;
            IsRunning = false;
        }
        public void Reset()
        {
            if (IsRunning) Stop();
            ElapsedTime = TimeSpan.Zero;
            SkipTime = TimeSpan.Zero;
            TaskCount = 0;
        }
        public void SetTime()
        {
            ElapsedTime = FixTime;
            if (IsRunning)
                Start();
        }
        public void SkipTask()
        {
            SkipTime += PhaseRemainingTime;
            TaskCount += 1;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime = DateTime.Now - StartTime;
            Timer.Interval = TimeSpan.FromSeconds(1);
            if (ElapsedTime >= SelectedSchedule.Duration) Stop();
        }

        public void EditAddSchedule()
        {
            Schedules.Add(new ScheduleViewModel("New Schedule", new[] { new SchedulePhaseViewModel("New Phase", TimeSpan.Zero) }));
        }
        public void EditRemoveSchedule()
        {
            if (SelectedEditSchedule == null) return;
            Schedules.Remove(SelectedEditSchedule);
        }
        public void EditAddPhase()
        {
            if (SelectedEditSchedule == null) return;
            SelectedEditSchedule.Phases.Add(new SchedulePhaseViewModel("New Phase", TimeSpan.Zero));
        }
        public void EditRemovePhase()
        {
            if (SelectedEditSchedule == null || SelectedEditPhase == null) return;
            SelectedEditSchedule.Phases.Remove(SelectedEditPhase);
        }
        public void EditSwitch()
        {
            if (SelectedEditSchedule != null)
                SelectedSchedule = SelectedEditSchedule;
        }
        public void EditSave()
        {
            _configManager.SetConfig(SchedulesConfigName, new StopwatchContext(Schedules.Select(x => x.ToContext()).ToArray()));
        }

        public class ScheduleViewModel : PropertyChangedBase
        {
            private readonly StopwatchToolModel.ScheduleModel _model;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                set
                {
                    _model.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
            public ObservableCollection<SchedulePhaseViewModel> Phases
            {
                get
                {
                    return _model.Phases;
                }
                private set
                {
                    _model.Phases = value;
                    NotifyOfPropertyChange(() => Phases);
                    NotifyOfPropertyChange(() => Duration);
                }
            }
            public TimeSpan Duration
            {
                get
                {
                    return Phases.Aggregate(TimeSpan.Zero, (x, y) => x + y.Duration);
                }
            }

            private ScheduleViewModel()
            {
                _model = new StopwatchToolModel.ScheduleModel();
                Phases = new ObservableCollection<SchedulePhaseViewModel>();
                Phases.CollectionChanged += Phases_CollectionChanged;
            }
            public ScheduleViewModel(string name, SchedulePhaseViewModel[] phases)
                : this()
            {
                Name = name;
                foreach (var phase in phases)
                    Phases.Add(phase);
            }
            public ScheduleViewModel(StopwatchScheduleContext context)
                : this()
            {
                Name = context.Name;
                foreach (var phase in context.Phases)
                    Phases.Add(new SchedulePhaseViewModel(phase));
            }

            public StopwatchScheduleContext ToContext()
            {
                return new StopwatchScheduleContext(Name, Phases.Select(x => x.ToContext()).ToArray());
            }

            public SchedulePhaseViewModel PhaseAtTime(int PhaseCount)
            {
                return Phases[PhaseCount];
            }

            public TimeSpan ElapsedTimeInPhase(TimeSpan time, TimeSpan Skipped, int PhaseCount)
            {
                TimeSpan sum = TimeSpan.Zero;
                for(int i = 0; i<PhaseCount; i++)
                {
                    sum += Phases[i].Duration;
                }
                return time + Skipped - sum;
                //return Phases.Last().Duration;
            }
            public TimeSpan RemainingTimeInPhase(TimeSpan time, TimeSpan Skipped, int PhaseCount)
            {
                return Phases[PhaseCount].Duration - ElapsedTimeInPhase(time, Skipped, PhaseCount);
            }

            private void Phases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.OldItems != null)
                    foreach (INotifyPropertyChanged item in e.OldItems)
                        item.PropertyChanged -= Phase_PropertyChanged;
                if (e.NewItems != null)
                    foreach (INotifyPropertyChanged item in e.NewItems)
                        item.PropertyChanged += Phase_PropertyChanged;
                NotifyOfPropertyChange(() => Duration);
            }
            private void Phase_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(Duration))
                    NotifyOfPropertyChange(() => Duration);
            }
        }

        public class SchedulePhaseViewModel : PropertyChangedBase
        {
            private StopwatchToolModel.SchedulePhaseModel _model;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                set
                {
                    _model.Name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
            public TimeSpan Duration
            {
                get
                {
                    return _model.Duration;
                }
                set
                {
                    _model.Duration = value;
                    NotifyOfPropertyChange(() => Duration);
                }
            }

            public SchedulePhaseViewModel(string name, TimeSpan duration)
            {
                _model = new StopwatchToolModel.SchedulePhaseModel();
                Name = name;
                Duration = duration;
            }
            public SchedulePhaseViewModel(StopwatchPhaseContext context)
            {
                _model = new StopwatchToolModel.SchedulePhaseModel();
                Name = context.Name;
                Duration = TimeSpan.FromSeconds(context.Duration);
            }

            public StopwatchPhaseContext ToContext()
            {
                return new StopwatchPhaseContext(Name, (int)Duration.TotalSeconds);
            }
        }
    }
}
