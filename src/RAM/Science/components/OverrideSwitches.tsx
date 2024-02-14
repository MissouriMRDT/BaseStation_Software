import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
// import { BitmaskUnpack } from '../../../Core/BitmaskUnpack';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
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
  flexGrow: 1,
  justifyContent: 'space-around',
  height: '125px',
  marginTop: '5px',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  justifyContent: 'space-around',
};

const button: CSS.Properties = {
  width: '500px',
  height: '100px',
  alignSelf: 'center',
  margin: '0 5px 0 5px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  LimitSwitchOverride: boolean;
  WatchdogOverride: boolean;
}

let limitSwitchToggle = false;
let watchdogToggle = false;

class OverrideSwitches extends Component<IProps, IState> {
  toggleLimitSwitch = (): boolean => {
    limitSwitchToggle = !limitSwitchToggle;
    this.setState({ LimitSwitchOverride: limitSwitchToggle });
    console.log('Limit Switch Override: ' + limitSwitchToggle);
    return limitSwitchToggle;
  };

  toggleWatchdog = (): boolean => {
    watchdogToggle = !watchdogToggle;
    this.setState({ WatchdogOverride: watchdogToggle });
    console.log('Watchdog Override: ' + watchdogToggle);
    return watchdogToggle;
  };

  render(): JSX.Element {
    return (
      <div id="OverrideSwitches" style={this.props.style}>
        <div style={label}>Override Switches</div>
        <div style={container}>
          <div style={column}>
            <div style={row}>
              <button
                style={button}
                onClick={() => rovecomm.sendCommand('LimitSwitchOverride', this.toggleLimitSwitch())}
              >
                {`Limit Switch Override: ${limitSwitchToggle ? 'On' : 'Off'}`}
              </button>
              <button style={button} onClick={() => rovecomm.sendCommand('WatchdogOverride', this.toggleWatchdog())}>
                {`Watchdog Override: ${watchdogToggle ? 'On' : 'Off'}`}
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default OverrideSwitches;
