namespace RED.ViewModels.Modules
{
    using ControlCenter;
    using Interfaces;
    using Models.Modules;
    using RoverComs;
    using RoverComs.Rover;
    using System;

    public class PowerboardVM : BaseVM, IModule
    {
        private static readonly PowerboardModel Model = new PowerboardModel();

        public string Title
        {
            get { return Model.Title; }
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



        public PowerboardVM() : base(Model.Title)
        {

        }
        
        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            try
            {
                switch ((Powerboard.TelemetryId)message.Id)
                {

                    default:
                        throw new Exception(Title + " telemetry receiving failed.");
                }
            }
            catch (Exception e)
            {
                ControlCenterVM.ConsoleVM.TelemetryReceiver(new Protocol<string>(e.Message));
                throw;
            }
        }
    }
}