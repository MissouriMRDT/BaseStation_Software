using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Modules;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        private async void OpenBay_Click(object sender, RoutedEventArgs e)
        {
            byte bayIndex = Byte.Parse((string)((Button)sender).Tag);
            await PromptOpenBay(bayIndex);
        }

        private async void CloseBay_Click(object sender, RoutedEventArgs e)
        {
            byte bayIndex = Byte.Parse((string)((Button)sender).Tag);
            await PromptCloseBay(bayIndex);
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
                message: "This will command the drop bay to open.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((DropBaysViewModel)DataContext).OpenBay(index);
            }
        }

        private async Task PromptCloseBay(byte index)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Close Bay",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Drop Bay Close",
                message: "This will command the drop bay to close.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((DropBaysViewModel)DataContext).CloseBay(index);
            }
        }
    }
}
