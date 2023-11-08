import React, { Component } from 'react';
import CSS from 'csstype';
import { MapContainer, Marker, Popup, ScaleControl, TileLayer, Circle } from 'react-leaflet';
import { LatLngTuple } from 'leaflet';

import icon from './Icon';
import compassNeedle from './CompassNeedle';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  height: 'calc(100% - 35px)',
};
const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};
const mapStyle: CSS.Properties = {
  height: '100%',
  width: '100%',
};

interface IProps {
  style?: CSS.Properties;
  storedWaypoints: {
    [key: string]: {
      onMap: boolean;
      color: string;
      name: string;
      latitude: number;
      longitude: number;
      displayRadius: number;
    };
  };
  currentCoords: { lat: number; lon: number };
  store: (name: string, coords: { lat: number; lon: number }) => void;
  name: string;
}

interface IState {
  centerLat: number;
  centerLon: number;
  zoom: number;
  maxZoom: number;
  heading: number;
}

class Map extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      // Default map to near the SDELC
      centerLat: 37.951631,
      centerLon: -91.770001,
      zoom: 15,
      maxZoom: 19,
      heading: 0,
    };

    rovecomm.on('IMUData', (data: number[]) => this.IMUData(data));
  }

  IMUData(data: number[]): void {
    this.setState({
      heading: data[1],
    });
  }

  render(): JSX.Element {
    const position: LatLngTuple = [this.state.centerLat, this.state.centerLon];
    return (
      <div style={this.props.style}>
        <div style={label}>Map</div>
        <div style={container}>
          <div style={mapStyle}>
            <MapContainer
              style={mapStyle}
              center={position}
              zoom={this.state.zoom}
              maxZoom={this.state.maxZoom}
              // eslint-disable-next-line @typescript-eslint/ban-ts-comment
              // @ts-ignore
              whenReady={(map: any): void =>
                map.target.on('click', (e: { latlng: { lat: number; lng: number } }) => {
                  this.props.store(new Date().toLocaleTimeString(), {
                    lat: e.latlng.lat,
                    lon: e.latlng.lng,
                  });
                })
              }
            >
              <TileLayer maxZoom={this.state.maxZoom} url="../assets/maps/{z}/{y}/{x}" />
              {this.props.currentCoords.lat && this.props.currentCoords.lon && (
                <Marker
                  position={[this.props.currentCoords.lat, this.props.currentCoords.lon]}
                  icon={compassNeedle(this.state.heading)}
                />
              )}
              {Object.keys(this.props.storedWaypoints).map((waypointName: string) => {
                const waypoint = this.props.storedWaypoints[waypointName];
                const post: LatLngTuple = [waypoint.latitude, waypoint.longitude];
                if (waypoint.onMap === true) {
                  return (
                    <div>
                      <Marker key={waypoint.name} position={post} icon={icon(waypoint.color)}>
                        <Popup>{waypoint.name}</Popup>
                      </Marker>
                      {waypoint.displayRadius > 0 ? (
                        <Circle center={post} radius={waypoint.displayRadius} pathOptions={{ color: waypoint.color }} />
                      ) : null}
                    </div>
                  );
                }
                return null;
              })}
              <ScaleControl position="topleft" />
            </MapContainer>
          </div>
        </div>
      </div>
    );
  }
}

export default Map;
