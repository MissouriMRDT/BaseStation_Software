namespace RED.Views
{
    public partial class ConsoleView
    {
        public ConsoleView()
        {
            InitializeComponent();
        }

        private void ConsoleText_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleTextBox.ScrollToEnd();
        }
    }
}
