﻿using Core;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels;
using RoverAttachmentManager.Models.Autonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class ControlsViewModel
    {
        private readonly IRovecomm _rovecomm;

        private readonly ControlsModel _model;

        private readonly WaypointManager _waypointManager;

        public SentWaypointsViewModel SentWaypoints
        {
          get
          {
            return _model.SentWaypoints;
          }
          set
          {
            _model.SentWaypoints = value;
          }
        }

        public void Enable() => _rovecomm.SendCommand(Packet.Create("AutonomousModeEnable"), true);

        public void Disable() => _rovecomm.SendCommand(Packet.Create("AutonomousModeDisable"), true);

        public ControlsViewModel(IRovecomm networkMessenger, AutonomyViewModel parent)
        {
            _model = new ControlsModel();
            _rovecomm = networkMessenger;
            _waypointManager = WaypointManager.Instance;
        }

        public void AddWaypoint()
        {
            Waypoint waypoint = _waypointManager.SelectedWaypoint;
            byte[] msg = new byte[2 * sizeof(double)];
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Longitude), 0, msg, 0 * sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Latitude), 0, msg, 1 * sizeof(double), sizeof(double));
            Array.Reverse(msg);

            SentWaypoints.SentWaypoints(waypoint);

            _rovecomm.SendCommand(new Packet("WaypointAdd", msg, 2, (byte)7), true);
        }

        public void ClearAllWaypoints()
        {
          _rovecomm.SendCommand(Packet.Create("WaypointsClearAll"), true);
          SentWaypoints.ClearAllWaypoints();
        }
    }
}