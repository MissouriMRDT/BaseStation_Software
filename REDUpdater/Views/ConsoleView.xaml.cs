using REDUpdater.ViewModels;
using System.Windows.Controls;

namespace REDUpdater.Views
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : UserControl
    {
        public ConsoleView()
        {
            InitializeComponent();
            this.DataContext = new ConsoleViewModel();
        }
    }
}
