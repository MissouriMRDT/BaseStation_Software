namespace RED.Views.Modules
{
    public partial class NetworkingView
    {
        public NetworkingView()
        {
            InitializeComponent();
        }

        private void NetworkConsoleText_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NetworkConsoleTextbox.ScrollToEnd();
        }
    }
}
