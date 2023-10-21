/* eslint-disable prettier/prettier */
import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

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

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
};

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  alignItems: 'left',
  margin: '10px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  encoderPositions: {
    [key: string]: number;
    scoopx: number;
    scoopz: number;
    sensorz: number;
    multiplexor: number;
  };
}

class EncoderPositions extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      encoderPositions: {
        scoopx: 0,
        scoopz: 0,
        sensorz: 0,
        multiplexor: 0,
      },
    };
    this.updateEncoderValues = this.updateEncoderValues.bind(this);
    rovecomm.on('EncoderPositions', (data: number[]) => this.updateEncoderValues(data));
  }

  updateEncoderValues(data: number[]): void {
    this.setState((prevState) => {
      const updatedEncoderPositions: any = { ...prevState.encoderPositions };
      Object.keys(updatedEncoderPositions).forEach((key, index) => {
        updatedEncoderPositions[key] = data[index];
      });
      return { encoderPositions: updatedEncoderPositions };
    });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Encoder Positions</div>
        <div style={container}>
          {Object.keys(this.state.encoderPositions).map((position) => {
            return (
              <div key={position} style={row}>
                {position}: {this.state.encoderPositions[position]}
              </div>
            );
          })}
        </div>
      </div>
    );
  }
}

export default EncoderPositions;
