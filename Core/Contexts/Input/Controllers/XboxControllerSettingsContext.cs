using Core.Contexts;

namespace Core.Contexts.Input.Controllers
{
    public class XboxControllerSettingsContext : ConfigurationFile
    {
        public bool AutoDeadzone;
        public int ManualDeadzone;
    }
}
