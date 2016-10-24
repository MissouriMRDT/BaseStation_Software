using System.Windows.Controls;

namespace RED.Views
{
    public partial class ConsoleView
    {
        public ConsoleView()
        {
            InitializeComponent();
        }

        private void ConsoleText_Changed(object sender, TextChangedEventArgs e)
        {
            ConsoleTextBox.ScrollToEnd();
        }
    }
}
