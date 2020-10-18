using Caliburn.Micro;
using Core.Cameras;
using Core.Interfaces;
using RED.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace RED.ViewModels
{
    public class CameraViewModel : PropertyChangedBase
    {
        private readonly CameraModel _model;
        ILogger _log;

        public CameraViewModel(ILogger log)
        {
            _model = new CameraModel();
            _log = log;
        }

        public Image CameraFeed
        {
            get
            {
                return _model.CameraFeed;
            }
            set
            {
                _model.CameraFeed = value;
                NotifyOfPropertyChange(() => CameraFeed);
            }
        }

        public int Rotation
        {
            get
            {
                return _model.Rotation;
            }
        }

        public void SetCamera(Camera camera)
        {
            CameraMultiplexer.RemoveSurface(CameraFeed);
            CameraMultiplexer.AddSurface(camera, CameraFeed);

            SetRotation(0);

            _model.DisplayedCamera = camera;
        }

        public void RotateCW()
        {
            int rotate = (Rotation + 90) % 360;
            SetRotation(rotate);
        }

        private void SetRotation(int rotate)
        {
            CameraFeed.RenderTransform = new RotateTransform(rotate, CameraFeed.ActualWidth / 2, CameraFeed.ActualHeight / 2);

            _model.Rotation = rotate;
        }

        public void Screenshot()
        {
            CameraMultiplexer.Screenshot(_model.DisplayedCamera);
        }

		#region Cameras
		public void Camera1() { SetCamera(Camera.LeftGimbal); }
        public void Camera2() { SetCamera(Camera.RightGimbal); }
        public void Camera3() { SetCamera(Camera.LeftSuspension); }
        public void Camera4() { SetCamera(Camera.RightSuspension); }
        public void Camera5() { SetCamera(Camera.LeftEndEffector); }
        public void Camera6() { SetCamera(Camera.RightEndEffector); }
        public void Camera7() { SetCamera(Camera.Elbow); }
		public void Camera8() { SetCamera(Camera.PlaceholderCamera8); }
        #endregion
    }
}
