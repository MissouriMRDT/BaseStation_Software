import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { DContainer, LContainer } from '../../Core/components/CssConstants';
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '36px',
  marginBlockStart: '0',
  marginBlockEnd: '0',
};
function container(theme: string): CSS.Properties {
  if (theme === 'light') {
    return LContainer;
  }
  return DContainer;
}
const localContainer: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  gridRowStart: '2 & {}',
  grid: 'repeat(4, 36px) / auto-flow dense',
  padding: '5px',
  height: 'calc(100% - 47px)',
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

interface IProps {
  onCoordsChange: (lat: number, lon: number) => void;
  style?: CSS.Properties;
}

interface IState {
  currentLat: number;
  currentLon: number;
  satelliteCount: number;
  pitch: number;
  yaw: number;
  roll: number;
  distance: number;
  quality: number;
  theme: string;
}
class GPS extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: any) {
    super(props);
    this.state = {
      currentLat: 0,
      currentLon: 0,
      satelliteCount: 0,
      pitch: 0,
      yaw: 0,
      roll: 0,
      distance: 0,
      quality: 0,
      theme: 'light',
    };

    rovecomm.on('GPSLatLon', (data: any) => this.GPSLatLon(data));
    rovecomm.on('IMUData', (data: any) => this.IMUData(data));
    rovecomm.on('LidarData', (data: any) => this.LidarData(data));
    rovecomm.on('SatelliteCountData', (data: any) => this.SatelliteCountData(data));
  }

  setTheme(): void {
    let currentTheme: string;
    if (this.state.theme === 'light') {
      currentTheme = 'dark';
      this.setState({ theme: currentTheme });
      console.log('set state to dark mode');
    } else {
      currentTheme = 'light';
      this.setState({ theme: currentTheme });
      console.log('set state to light mode');
    }
  }

  GPSLatLon(data: any) {
    const currentLat = data[0];
    const currentLon = data[1];
    this.setState({
      currentLat,
      currentLon,
    });
    this.props.onCoordsChange(currentLat, currentLon);
  }

  SatelliteCountData(data: any) {
    this.setState({
      satelliteCount: data[0],
    });
  }

  IMUData(data: any) {
    this.setState({
      pitch: data[0],
      yaw: data[1],
      roll: data[2],
    });
  }

  LidarData(data: any) {
    this.setState({
      distance: data[0],
      quality: data[1],
    });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>GPS</div>
        <div style={{ ...container(this.state.theme), ...localContainer }}>
          {[
            { title: 'Current Lat.', value: this.state.currentLat.toFixed(7) },
            { title: 'Current Lon.', value: this.state.currentLon.toFixed(7) },
            { title: 'Satellite Count', value: this.state.satelliteCount.toFixed(0) },
            { title: 'Distance', value: this.state.distance.toFixed(3) },
            { title: 'Quality', value: this.state.quality.toFixed(3) },
            { title: 'Pitch', value: this.state.pitch.toFixed(3) },
            { title: 'Yaw', value: this.state.yaw.toFixed(3) },
            { title: 'Roll', value: this.state.roll.toFixed(3) },
          ].map((datum) => {
            const { title, value } = datum;
            return (
              <div key={title}>
                <p style={h1Style}>
                  {title}: {value}
                </p>
              </div>
            );
          })}
        </div>
      </div>
    );
  }
}

export default GPS;
