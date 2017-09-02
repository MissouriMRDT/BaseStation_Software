using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using System;
using System.IO;

namespace RED.ViewModels
{
    public class ConsoleViewModel : PropertyChangedBase, ILogger
    {
        private readonly ConsoleModel _model;

        private const string LogFilePath = "REDConsole.log";
        private StreamWriter LogFile;

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

        public ConsoleViewModel()
        {
            _model = new ConsoleModel();
            InitializeLogFile();
            LogToFile("New Logging Session Started");
        }

        private void InitializeLogFile()
        {
            try
            {
                LogFile = new StreamWriter(LogFilePath, true);
            }
            catch (Exception e)
            {
                LogToScreen("There was a problem opening the log file: " + e.ToString());
            }
        }

        public void Log(string text, params object[] args)
        {
            var msg = String.Format(text, args);
            LogToFile(msg);
            LogToScreen(msg);
        }

        public void LogToScreen(string msg)
        {
            var newText = String.Format("{0:HH:mm:ss.ff}: {1}{2}", DateTime.Now, msg, Environment.NewLine);
            ConsoleText += newText;
        }

        public void LogToFile(string msg)
        {
            LogFile?.WriteLine("{0:o}: {1}", DateTime.Now, msg);
            LogFile?.Flush();
        }
    }
}
