using RED.ViewModels.Tools;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace RED.Models.Tools
{
    public class StopwatchToolModel
    {
        internal ObservableCollection<StopwatchToolViewModel.ScheduleViewModel> Schedules;
        internal StopwatchToolViewModel.ScheduleViewModel SelectedSchedule;
        internal DispatcherTimer Timer;
        internal bool IsRunning;
        internal DateTime StartTime;
        internal TimeSpan ElapsedTime;

        internal TimeSpan FixTime;
        public StopwatchToolViewModel.ScheduleViewModel SelectedEditSchedule;
        internal StopwatchToolViewModel.SchedulePhaseViewModel SelectedEditPhase;

        public class ScheduleModel
        {
            internal string Name;
            internal ObservableCollection<StopwatchToolViewModel.SchedulePhaseViewModel> Phases;
        }

        public class SchedulePhaseModel
        {
            internal string Name;
            internal TimeSpan Duration;
        }
    }
}
