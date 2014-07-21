namespace RED.Views.ControlCenter
{
    using System.Windows;

    public partial class ModuleManagerView
    {
        private SaveModuleStateView saveDialog;
        private RemoveModuleStateView removeDialog;

        public ModuleManagerView()
        {
            InitializeComponent();
        }
        
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (removeDialog != null)
            {
                removeDialog.Close();
            }
            if (saveDialog == null)
            {
                saveDialog = new SaveModuleStateView();
            }
            else
            {
                saveDialog.Close();
                saveDialog = new SaveModuleStateView();
            }
            saveDialog.Show();
        }
        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (saveDialog != null)
            {
                saveDialog.Close();
            }
            if (removeDialog == null)
            {
                removeDialog = new RemoveModuleStateView();
            }
            else
            {
                removeDialog.Close();
                removeDialog = new RemoveModuleStateView();
            }
            removeDialog.Show();
        }
    }
}
