using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for DropBaysView.xaml
    /// </summary>
    public partial class DropBaysView : UserControl
    {
        public DropBaysView()
        {
            InitializeComponent();
        }

        private async void OpenBay1_Click(object sender, RoutedEventArgs e)
        {
            await PromptOpenBay(0);
        }

        private async void OpenBay2_Click(object sender, RoutedEventArgs e)
        {
            await PromptOpenBay(1);
        }

        private async void OpenBay3_Click(object sender, RoutedEventArgs e)
        {
            await PromptOpenBay(2);
        }

        private async void OpenBay4_Click(object sender, RoutedEventArgs e)
        {
            await PromptOpenBay(3);
        }

        private async Task PromptOpenBay(byte index)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Open Bay",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Drop Bay Open",
                message: "This will command the drop bay to open. This bay cannot be closed remotely.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((DropBaysViewModel)DataContext).OpenBay(index);
            }
        }
    }
}
