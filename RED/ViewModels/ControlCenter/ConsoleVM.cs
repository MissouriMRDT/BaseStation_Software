namespace RED.ViewModels.ControlCenter
{
    using Interfaces;
    using Models.ControlCenter;
    using RoverComs;
    using System;
    using System.Globalization;

    public class ConsoleVM : BaseVM, IModule
    {
        private static readonly ConsoleModel Model = new ConsoleModel();

        public string Title
        {
            get
            {
                return Model.Title;
            }
        }
        public bool InUse
        {
            get
            {
                return Model.InUse;
            }
            set
            {
                Model.InUse = value;
            }
        }
        public bool IsManageable
        {
            get
            {
                return Model.IsManageable;
            }
        }

        public string ConsoleText
        {
            get
            {
                return Model.ConsoleText;
            }
            set
            {
                SetField(ref Model.ConsoleText, value);
            }
        }
        private void WriteToConsole(string text)
        {
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture);
            var newText = String.Format("{0}: {1} {2}", timeStamp, text, Environment.NewLine);
            ConsoleText += newText;
        }

        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            WriteToConsole(message.Value.ToString());
        }
    }
}
