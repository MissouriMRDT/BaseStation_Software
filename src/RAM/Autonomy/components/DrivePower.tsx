import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const header: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '22px',
  width: '35%',
};
const value: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '22px',
  width: '10%',
  textAlign: 'center',
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
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-around',
  alignItems: 'center',
  margin: '5px',
};
const slider: CSS.Properties = {
  background: '#990000',
  width: '40%',
  WebkitAppearance: 'none',
  appearance: 'none',
  height: '6px',
  outline: 'none',
};

interface IProps {
  style?: CSS.Properties;
}
interface IState {
  drivePower: {
    Max: number;
    Min: number;
  };
}

class DrivePower extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      drivePower: {
        Max: 250,
        Min: 50,
      },
    };
  }

  sliderChange(event: { target: { value: string } }, multiplier: string): void {
    this.setState((prevState) => ({
      drivePower: {
        ...prevState.drivePower,
        [multiplier]: parseInt(event.target.value, 10),
      },
    }));
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drive Power</div>
        <div style={container}>
          {Object.keys(this.state.drivePower).map((multipliers) => {
            return (
              <div key={multipliers} style={row}>
                <div style={header}>{multipliers} Control Multiplier</div>
                <div style={value}>{this.state.drivePower[multipliers]}</div>
                <input
                  type="range"
                  min="50"
                  max="1000"
                  value={this.state.drivePower[multipliers]}
                  style={slider}
                  onChange={(e) => this.sliderChange(e, multipliers)}
                />
              </div>
            );
          })}
        </div>
      </div>
    );
  }
}

export default DrivePower;
