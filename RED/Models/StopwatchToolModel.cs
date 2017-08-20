using RED.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace RED.Models
{
    public class StopwatchToolModel
    {
        internal ObservableCollection<StopwatchToolViewModel.ScheduleViewModel> Schedules;
        internal StopwatchToolViewModel.ScheduleViewModel SelectedSchedule;
        internal DispatcherTimer Timer;
        internal bool IsRunning;
        internal DateTime StartTime;
        internal TimeSpan ElapsedTime;

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
