using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System;

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

        public void Log(string text, params object[] args)
        {
            var msg = String.Format(text, args);
            var newText = String.Format("{0:HH:mm:ss.ff}: {1} {2}", DateTime.Now, msg, Environment.NewLine);
            ConsoleText += newText;
        }
    }
}
