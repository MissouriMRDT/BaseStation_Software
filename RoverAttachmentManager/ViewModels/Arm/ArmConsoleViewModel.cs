using System;
using Caliburn.Micro;
using Core.Interfaces;
using Core;
using RoverAttachmentManager.Models.Arm;

namespace RoverAttachmentManager.ViewModels.Arm
{
    public class ArmConsoleViewModel : PropertyChangedBase, ILogger
    {
        private readonly ArmConsoleModel _model;

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

        public ArmConsoleViewModel()
        {
            _model = new ArmConsoleModel();
            CommonLog.Instance.MessageLogged += Instance_MessageLogged;
        }

        private void Instance_MessageLogged(object sender, string msg)
        {
            var newText = String.Format("{0:HH:mm:ss.ff}: {1}{2}", DateTime.Now, msg, Environment.NewLine);
            ConsoleText += newText;
        }

        public void Log(string message, params object[] args)
        {
            CommonLog.Instance.Log(message, args);
        }
    }
}
