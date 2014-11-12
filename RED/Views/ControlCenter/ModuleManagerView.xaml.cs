namespace RED.Views.ControlCenter
{
    using System.Windows;

    public partial class ModuleManagerView
    {
        private SaveModuleStateView _saveDialog;
        private RemoveModuleStateView _removeDialog;

        public ModuleManagerView()
        {
            InitializeComponent();
        }
        
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_removeDialog != null)
            {
                _removeDialog.Close();
            }
            if (_saveDialog == null)
            {
                _saveDialog = new SaveModuleStateView();
            }
            else
            {
                _saveDialog.Close();
                _saveDialog = new SaveModuleStateView();
            }
            _saveDialog.Show();
        }
        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_saveDialog != null)
            {
                _saveDialog.Close();
            }
            if (_removeDialog == null)
            {
                _removeDialog = new RemoveModuleStateView();
            }
            else
            {
                _removeDialog.Close();
                _removeDialog = new RemoveModuleStateView();
            }
            _removeDialog.Show();
        }
    }
}
