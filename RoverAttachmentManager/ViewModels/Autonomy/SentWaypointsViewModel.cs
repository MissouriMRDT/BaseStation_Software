using Caliburn.Micro;
using Core;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels;
using RoverAttachmentManager.Models.Autonomy;
using System;
using System.Net;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class SentWaypointsViewModel : PropertyChangedBase
    {
        private readonly SentWaypointsModel _model;

        public string SentWaypointsText
        {
            get
            {
                return _model._waypointsText;
            }
            set
            {
                _model._waypointsText = value;
                NotifyOfPropertyChange();
            }
        }

        public SentWaypointsViewModel()
        {
            _model = new SentWaypointsModel();
        }

        public void SentWaypoints(Waypoint waypoint)
        {
            string tempstring = waypoint.Name + " | Longitude: " + waypoint.Longitude.ToString() + " | Latitude: " + waypoint.Latitude.ToString() + "\n";
            SentWaypointsText += tempstring;
        }

        public void ClearSentWaypoints()
        {
            string wpclear = "--cleared--\n";
            SentWaypointsText += wpclear;
        }
    }
}
