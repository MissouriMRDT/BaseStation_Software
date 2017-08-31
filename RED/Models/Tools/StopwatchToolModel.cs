using RED.ViewModels.Tools;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace RED.Models.Tools
{
    internal class StopwatchToolModel
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

        internal class ScheduleModel
        {
            internal string Name;
            internal ObservableCollection<StopwatchToolViewModel.SchedulePhaseViewModel> Phases;
        }

        internal class SchedulePhaseModel
        {
            internal string Name;
            internal TimeSpan Duration;
        }
    }
}
