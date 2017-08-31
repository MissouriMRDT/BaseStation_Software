using RED.Contexts.Tools;

namespace RED.Configurations.Tools
{
    internal static class StopwatchToolConfig
    {
        internal static StopwatchContext DefaultSchedules = new StopwatchContext(new[] {
            new StopwatchScheduleContext("Sample Task", new[] {
                new StopwatchPhaseContext("Find Cache", 60),
                new StopwatchPhaseContext("Pick up Tools", 300),
                new StopwatchPhaseContext("To Astro #1", 120),
                new StopwatchPhaseContext("Drop Off #1", 60),
                new StopwatchPhaseContext("To Astro #2", 120),
                new StopwatchPhaseContext("Drop Off #1", 120),
                new StopwatchPhaseContext("Return to Start", 120)
            })
        });
    }
}
