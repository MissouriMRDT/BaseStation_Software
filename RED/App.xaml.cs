namespace RED
{
    using System.Windows;

    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var telemetry = new TelemetryWindow();
            telemetry.Show();
        }
    }
}
