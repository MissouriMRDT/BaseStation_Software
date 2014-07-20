namespace RED.Views.ControlCenter
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels.ControlCenter;

    public partial class RemoveModuleStateView
    {
        public RemoveModuleStateView()
        {
            InitializeComponent();

            DataContext = ControlCenterVM.RemoveModuleStateVM;

            Buttons = new[] {
                new Button {
                    Content = "remove",
                    Command = ((RemoveModuleStateVM)DataContext).RemoveStateCommand,
                    IsDefault = true,
                    IsCancel = false,
                    MinHeight = 21,
                    MinWidth = 65,
                    Margin = new Thickness(4, 0, 0, 0)
                }, 
                CancelButton
            };
            var removeButton = Buttons.Single(b => (string)b.Content == "remove");
            removeButton.Click += (sender, args) => Close();
        }
    }
}
