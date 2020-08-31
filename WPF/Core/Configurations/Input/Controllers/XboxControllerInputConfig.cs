using Core.Contexts.Input.Controllers;

namespace Core.Configurations.Input.Controllers
{
    internal static class XboxControllerInputConfig
    {
        internal static XboxControllerSettingsContext DefaultConfig = new XboxControllerSettingsContext()
        {
            AutoDeadzone = false,
            ManualDeadzone = 5000
        };
    }
}
