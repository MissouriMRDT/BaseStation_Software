import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '36px',
  marginBlockStart: '0',
  marginBlockEnd: '0',
};
const container: CSS.Properties = {
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
  // pitch: number;
  yaw: number;
  // roll: number;
  horizontalAccur: number;
  verticalAccur: number;
  headingAccur: number;
  // distance: number;
  // quality: number;
}
class GPS extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: Readonly<IProps> | IProps) {
    super(props);
    this.state = {
      currentLat: 0,
      currentLon: 0,
      satelliteCount: 0,
      // pitch: 0,
      yaw: 0,
      // roll: 0,
      horizontalAccur: 0,
      verticalAccur: 0,
      headingAccur: 0,
      // distance: 0,
      // quality: 0,
    };

    rovecomm.on('GPSLatLon', (data: number[]) => this.GPSLatLon(data));
    rovecomm.on('IMUData', (data: number[]) => this.IMUData(data));
    // rovecomm.on('LidarData', (data: number[]) => this.LidarData(data));
    rovecomm.on('SatelliteCountData', (data: number[]) => this.SatelliteCountData(data));
    rovecomm.on('AccuracyData', (data: number[]) => this.accurData(data));
  }

  accurData(data: number[]): void {
    this.setState({
      horizontalAccur: data[0],
      verticalAccur: data[1],
      headingAccur: data[2],
    });
  }

  GPSLatLon(data: number[]) {
    const currentLat = data[0];
    const currentLon = data[1];
    this.setState({
      currentLat,
      currentLon,
    });
    this.props.onCoordsChange(currentLat, currentLon);
  }

  SatelliteCountData(data: number[]) {
    this.setState({
      satelliteCount: data[0],
    });
  }

  IMUData(data: number[]) {
    this.setState({
      // pitch: data[0],
      yaw: data[1],
      // roll: data[2],
    });
  }

  /*
  LidarData(data: number[]) {
    this.setState({
      distance: data[0],
      quality: data[1],
    });
  }
  */

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>GPS</div>
        <div style={container}>
          {[
            { title: 'Current Lat.', value: this.state.currentLat.toFixed(7) },
            { title: 'Current Lon.', value: this.state.currentLon.toFixed(7) },
            { title: 'Satellite Count', value: this.state.satelliteCount.toFixed(0) },
            // { title: 'Distance', value: this.state.distance.toFixed(3) },
            // { title: 'Quality', value: this.state.quality.toFixed(3) },
            // { title: 'Pitch', value: this.state.pitch.toFixed(3) },
            { title: 'Yaw', value: this.state.yaw.toFixed(3) },
            // { title: 'Roll', value: this.state.roll.toFixed(3) },
            { title: 'Pos. Acc', value: this.state.horizontalAccur.toFixed(3) },
            { title: 'Alt. Acc', value: this.state.verticalAccur.toFixed(3) },
            { title: 'Comp. Acc', value: this.state.headingAccur.toFixed(3) },
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
