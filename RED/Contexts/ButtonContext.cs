namespace RED.Contexts
{
    using FirstFloor.ModernUI.Presentation;

    public class ButtonContext
    {
        public RelayCommand Command { get; private set; }
        public string Text { get; private set; }

        public ButtonContext(RelayCommand command, string text)
        {
            Command = command;
            Text = text;
        }
    }
}
