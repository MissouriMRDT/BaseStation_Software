using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System;
using System.Globalization;

namespace RED.ViewModels
{
    public class ConsoleViewModel : PropertyChangedBase, ILogger
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

        public void Log(string text)
        {
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture);
            var newText = String.Format("{0}: {1} {2}", timeStamp, text, Environment.NewLine);
            ConsoleText += newText;
        }
    }
}
