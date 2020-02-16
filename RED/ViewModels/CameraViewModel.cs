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

        public void SetCamera(int camera)
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
		public void Camera1() { SetCamera(1); }
        public void Camera2() { SetCamera(2); }
        public void Camera3() { SetCamera(3); }
        public void Camera4() { SetCamera(4); }
        public void Camera5() { SetCamera(5); }
        public void Camera6() { SetCamera(6); }
        public void Camera7() { SetCamera(7); }
		public void Camera8() { SetCamera(8); }
        #endregion
    }
}
