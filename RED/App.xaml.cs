using System;
using System.Windows;
using System.Windows.Threading;

namespace RED
{
    public partial class App
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            Clipboard.SetText(args.Exception.ToString());
            MessageBox.Show("An unhandled exception has occurred in RED. The following details have been copied to the clipboard." + Environment.NewLine + Environment.NewLine + args.Exception.ToString(), "RED Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
