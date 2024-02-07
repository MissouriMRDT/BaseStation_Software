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
  maxBatVolt: number;
  voltage: number;
  level: number;
  temperature: number;
}

class DroneBattery extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      maxBatVolt: 0,
      level: 0,
      voltage: 0,
      temperature: 0,
    };

    rovecomm.on('droneBattery', (data: number[]) => {
      this.updateData(data);
    });
  }

  updateData(data: number[]) {
    //const celsius: number = Math.round(data[0] - -273.15);
    // eslint-disable-next-line react/no-access-state-in-setstate
    // may not be needed
    // const percentLevel: number = Math.round((data[2] / this.state.maxBatVolt) * 100);
    this.setState({ voltage: data[1] });
  }

  render(): JSX.Element {
    return (
      <div style={{ marginLeft: '5px', width: '40%' }}>
        <div style={label}>Drone Battery</div>
        <div style={container}>
          <div>Level: {this.state.level}%</div>
          <div>Voltage: {this.state.voltage}</div>
          <div>Temperature: {this.state.temperature}</div>
        </div>
      </div>
    );
  }
}

export default DroneBattery;
