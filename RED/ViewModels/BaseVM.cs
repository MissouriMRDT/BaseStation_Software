namespace RED.ViewModels
{
    using Addons;
    using ControlCenter;
    using Interfaces;
    using Models.ControlCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class BaseVM : INotifyPropertyChanged
    {
        // Module Save File Name
        public static readonly string SavesFileName = "ModuleStateSaves.xml";

        // Text-to-Speech handler
        public static SpeechCompositionEngine Hazel = new SpeechCompositionEngine();

        // Returns static module
        public T GetModuleViewModel<T>() where T : IModule
        {
            return (T)ControlCenterModel.AllModules.Single(m => m is T);
        }

        // Property Changed Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetField<T>(ref T field, T value, [CallerMemberName] string name = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public T ParseEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

        // Constructors
        public BaseVM(string title) : this()
        {
            ControlCenterVM.UpdateConsole(title + " Initialized.");
        }
        public BaseVM()
        {

        }
    }
}
