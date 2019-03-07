using Caliburn.Micro;
using Core.Interfaces;
using RED.Models;
using Core;
using System;

namespace RED.ViewModels
{
    public class ConsoleViewModel : PropertyChangedBase, ILogger
    {
        private readonly ConsoleModel _model;

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
			CommonLog.Instance.MessageLogged += Instance_MessageLogged;
        }

		private void Instance_MessageLogged(object sender, string msg) {
			var newText = String.Format("{0:HH:mm:ss.ff}: {1}{2}", DateTime.Now, msg, Environment.NewLine);
			ConsoleText += newText;
		}

		public void Log(string message, params object[] args) {
			CommonLog.Instance.Log(message, args);
		}
	}
}
