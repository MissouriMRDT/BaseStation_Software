/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from 'react';
import CSS from 'csstype';

import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  borderColor: '#990000',
  borderStyle: 'solid',
  borderTopWidth: '30px',
  borderBottomWidth: '3px',
  whiteSpace: 'pre-wrap',
  padding: '5px',
};
const row: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  fontFamily: 'arial',
  fontSize: '16px',
  color: 'black',
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
  style?: CSS.Properties;
}

interface IState {
  temperature: number;
  humidity: number;
}

class EnvironmentalData extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      temperature: 0,
      humidity: 0,
    };
    this.Temperature = this.Temperature.bind(this);
    this.Humidity = this.Humidity.bind(this);

    rovecomm.on('Temperature', (data: any) => this.Temperature(data));
    rovecomm.on('Humidity', (data: any) => this.Humidity(data));
  }

  Temperature(data: any): void {
    this.setState({ temperature: data });
  }

  Humidity(data: any): void {
    this.setState({ humidity: data });
  }

  render(): JSX.Element {
    return (
      <div id="EnvironmentalData" style={this.props.style}>
        <div style={label}>Environmental Data</div>
        <div style={{ ...container, width: '80%' }}>
          <div style={{ ...row, margin: '10px' }}>
            <div>Temperature: {this.state.temperature}</div>
            <div>Humidity: {this.state.humidity}</div>
          </div>
        </div>
      </div>
    );
  }
}

export default EnvironmentalData;
