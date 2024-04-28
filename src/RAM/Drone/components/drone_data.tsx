import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  height: '108px',
  borderColor: '#990000',
  borderStyle: 'solid',
  borderTopWidth: '30px',
  borderBottomWidth: '3px',
  whiteSpace: 'pre-wrap',
  padding: '5px',
};

const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  fontFamily: 'arial',
  fontSize: '16px',
  top: '24px',
  left: '3px',
  color: '#ffffff',
};

interface IProps {}

interface IState {
  //maxBatVolt: number;
  speed: number;
  Altitude: number;
  SignalStrength: number;
  //level: number;
  //temperature: number;
}

class DroneData extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      //maxBatVolt: 0,
      //level: 0,
      speed: 0,
      Altitude: 0,
      SignalStrength: 0,
      //temperature: 0,
    };

    rovecomm.on('DroneData', (data: number[]) => {
      this.updateData(data);
    });
  }

  updateData(data: number[]) {
    //const celsius: number = Math.round(data[0] - -273.15);
    // eslint-disable-next-line react/no-access-state-in-setstate
    // may not be needed
    // const percentLevel: number = Math.round((data[2] / this.state.maxBatVolt) * 100);
    this.setState({ speed: data[0] });
    this.setState({ Altitude: data[1] });
    this.setState({ SignalStrength: data[2] });
  }

  render(): JSX.Element {
    return (
      <div style={{ marginLeft: '5px', width: '100%' }}>
        <div style={label}>Drone Data</div>
        <div style={container}>
          <div>Speed: {this.state.speed}</div>
          <div>Altitude: {this.state.Altitude}</div>
          <div>Signal Strength: {this.state.SignalStrength}</div>
        </div>
      </div>
    );
  }
}

export default DroneData;
