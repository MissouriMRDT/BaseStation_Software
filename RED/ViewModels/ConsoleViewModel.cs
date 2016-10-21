namespace RED.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Globalization;
    using Models;

    public class ConsoleViewModel : PropertyChangedBase
    {
        private readonly ConsoleModel _model = new ConsoleModel();

        public string ConsoleText
        {
            get
            {
                return _model._consoleText;
            }
            set
            {
                _model._consoleText = value;
                NotifyOfPropertyChange();
            }
        }

        public void WriteToConsole(string text)
        {
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture);
            var newText = String.Format("{0}: {1} {2}", timeStamp, text, Environment.NewLine);
            ConsoleText += newText;
        }
    }
}
