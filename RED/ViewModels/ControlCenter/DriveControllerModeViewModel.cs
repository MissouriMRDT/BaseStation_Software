using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.ControlCenter;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class DriveControllerModeViewModel : PropertyChangedBase, IControllerMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveControllerModeModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        public int SpeedLeft
        {
            get
            {
                return _model.speedLeft;
            }
            set
            {
                _model.speedLeft = value;
                NotifyOfPropertyChange(() => SpeedLeft);
            }
        }
        public int SpeedRight
        {
            get
            {
                return _model.speedRight;
            }
            set
            {
                _model.speedRight = value;
                NotifyOfPropertyChange(() => SpeedRight);
            }
        }

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public int SpeedLimit
        {
            get
            {
                return _model.speedLimit;
            }
            set
            {
                _model.speedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
            }
        }

        public bool ParabolicScaling
        {
            get
            {
                return _model.parabolicScaling;
            }
            set
            {
                _model.parabolicScaling = value;
            }
        }

        public DriveControllerModeViewModel(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            _model = new DriveControllerModeModel();
            _controlCenter = cc;
            InputVM = inputVM;
            Name = "Drive";
        }

        public void EnterMode()
        {
            SpeedLeft = 0;
            SpeedRight = 0;
        }

        public void EvaluateMode()
        {
            Controller c = InputVM.ControllerOne;
            if (c != null && !c.IsConnected) return;

            int newSpeedLeft;
            int newSpeedRight;

            #region Normalization of joystick input
            {
                float CurrentRawControllerSpeedLeft = InputVM.JoyStick1Y;
                float CurrentRawControllerSpeedRight = InputVM.JoyStick2Y;

                //Scaling
                if (ParabolicScaling) //Squares the value (0..1)
                {
                    CurrentRawControllerSpeedLeft *= CurrentRawControllerSpeedLeft * (CurrentRawControllerSpeedLeft >= 0 ? 1 : -1);
                    CurrentRawControllerSpeedRight *= CurrentRawControllerSpeedRight * (CurrentRawControllerSpeedRight >= 0 ? 1 : -1);
                }

                float speedLimitFactor = (float)SpeedLimit / motorRangeFactor;
                if (speedLimitFactor > 1) speedLimitFactor = 1;
                if (speedLimitFactor < 0) speedLimitFactor = 0;
                CurrentRawControllerSpeedLeft *= speedLimitFactor;
                CurrentRawControllerSpeedRight *= speedLimitFactor;

                newSpeedLeft = (int)(CurrentRawControllerSpeedLeft * motorRangeFactor);
                newSpeedRight = (int)(CurrentRawControllerSpeedRight * motorRangeFactor);
            }
            #endregion

            if (newSpeedLeft == 0)
            {
                SpeedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorLeftSpeed"), SpeedLeft);
            }
            else if (newSpeedLeft != SpeedLeft)
            {
                SpeedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorLeftSpeed"), SpeedLeft);
            }
            if (newSpeedRight == 0)
            {
                SpeedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorRightSpeed"), SpeedRight);
            }
            else if (newSpeedRight != SpeedRight)
            {
                SpeedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorRightSpeed"), SpeedRight);
            }
        }

        public void ExitMode()
        {
            int speed = 0;
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorLeftSpeed"), speed);
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorRightSpeed"), speed);
        }
    }
}