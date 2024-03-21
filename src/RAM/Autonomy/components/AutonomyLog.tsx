import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const header: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '22px',
  width: '35%',
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

interface IProps {
  style?: CSS.Properties;
}
interface IState {
  consoleValue: number;
  fileValue: number;
  rovecommValue: number;
}

class AutonomyLog extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      consoleValue: 5,
      fileValue: 1,
      rovecommValue: 5,
    };
  }

  handleConsoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    this.setState({ consoleValue: parseInt(e.target.value) });
  };

  handleFileChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    this.setState({ fileValue: parseInt(e.target.value) });
  };

  handleRovecommChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    this.setState({ rovecommValue: parseInt(e.target.value) });
  };

  handleButtonPress = () => {
    rovecomm.sendCommand('SetLoggingLevels', 'Autonomy', {
      console: this.state.consoleValue,
      file: this.state.fileValue,
      rovecomm: this.state.rovecommValue,
    });
  };

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Autonomy Log</div>
        <div style={container}>
          <div style={row}>
            <div style={header}>Console</div>
            <div style={header}>File</div>
            <div style={header}>RoveComm</div>
          </div>
          <div style={row}>
            <select onChange={this.handleConsoleChange} defaultValue={5} style={{ flexGrow: 1, textAlign: 'center' }}>
              {[
                { label: 'TraceL3', value: 1 },
                { label: 'TraceL2', value: 2 },
                { label: 'TraceL1', value: 3 },
                { label: 'Debug', value: 4 },
                { label: 'Info', value: 5 },
                { label: 'Warning', value: 6 },
                { label: 'Error', value: 7 },
                { label: 'Critical', value: 8 },
              ].map((option) => {
                return (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                );
              })}
            </select>
            <select onChange={this.handleFileChange} defaultValue={1} style={{ flexGrow: 1, textAlign: 'center' }}>
              {[
                { label: 'TraceL3', value: 1 },
                { label: 'TraceL2', value: 2 },
                { label: 'TraceL1', value: 3 },
                { label: 'Debug', value: 4 },
                { label: 'Info', value: 5 },
                { label: 'Warning', value: 6 },
                { label: 'Error', value: 7 },
                { label: 'Critical', value: 8 },
              ].map((option) => {
                return (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                );
              })}
            </select>
            <select onChange={this.handleRovecommChange} defaultValue={5} style={{ flexGrow: 1, textAlign: 'center' }}>
              {[
                { label: 'TraceL3', value: 1 },
                { label: 'TraceL2', value: 2 },
                { label: 'TraceL1', value: 3 },
                { label: 'Debug', value: 4 },
                { label: 'Info', value: 5 },
                { label: 'Warning', value: 6 },
                { label: 'Error', value: 7 },
                { label: 'Critical', value: 8 },
              ].map((option) => {
                return (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                );
              })}
            </select>
          </div>
          <div style={row}>
            <button onClick={this.handleButtonPress}>Send</button>
          </div>
        </div>
      </div>
    );
  }
}

export default AutonomyLog;
