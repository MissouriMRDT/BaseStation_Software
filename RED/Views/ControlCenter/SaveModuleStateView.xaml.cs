namespace RED.Views.ControlCenter
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels.ControlCenter;

    public partial class SaveModuleStateView
    {
        public SaveModuleStateView()
        {
            InitializeComponent();

            DataContext = ControlCenterVM.SaveModuleStateVM;

            Buttons = new[] {
                new Button {
                    Content = "save",
                    Command = ((SaveModuleStateVM)DataContext).SaveStateCommand,
                    IsDefault = true,
                    IsCancel = false,
                    MinHeight = 21,
                    MinWidth = 65,
                    Margin = new Thickness(4, 0, 0, 0)
                }, 
                CancelButton
            };
            var saveButton = Buttons.Single(b => (string) b.Content == "save");
            saveButton.Click += (sender, args) => Close();
        }
    }
}
