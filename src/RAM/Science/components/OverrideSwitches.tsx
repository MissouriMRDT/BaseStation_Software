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
const ToggleButton: CSS.Properties = {
  width: '200px',
  height: '100px',
  alignSelf: 'left',
  margin: '0 5px 0 5px',
};

interface IProps {
  style?: CSS.Properties;
}

const SwitchNames = ['ScoopAxis+', 'ScoopAxis-', 'SensorAxis+', 'SensorAxis-'];

interface IState {
  LimitSwitchOverride: boolean;
  WatchdogOverride: boolean;
  LimitSwitchStatus: boolean[];
}

let watchdogToggle = false;

class OverrideSwitches extends Component<IProps, IState> {
  static buildLimitSwitchCommand(LimitSwitch: boolean[]): number {
    let bitmask = '';
    bitmask += LimitSwitch[3] ? '1' : '0';
    bitmask += LimitSwitch[2] ? '1' : '0';
    bitmask += LimitSwitch[1] ? '1' : '0';
    bitmask += LimitSwitch[0] ? '1' : '0';
    // console.log(bitmask);
    const num = parseInt(bitmask, 2);
    // console.log(num);
    return num;
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      LimitSwitchOverride: false,
      WatchdogOverride: false,
      LimitSwitchStatus: [false, false, false, false],
    };
    this.toggleLimitSwitch = this.toggleLimitSwitch.bind(this);
    this.toggleWatchdog = this.toggleWatchdog.bind(this);
  }

  toggleLimitSwitch(index: number): void {
    if (this.state.LimitSwitchOverride) {
      const { LimitSwitchStatus } = this.state;
      LimitSwitchStatus[index] = !LimitSwitchStatus[index];
      this.setState({
        LimitSwitchStatus,
      });
      rovecomm.sendCommand('LimitSwitchOverride', OverrideSwitches.buildLimitSwitchCommand(LimitSwitchStatus));
    }
  }

  toggleWatchdog(): boolean {
    watchdogToggle = !watchdogToggle;
    this.setState({ WatchdogOverride: watchdogToggle });
    // console.log('Watchdog Override: ' + watchdogToggle);
    return watchdogToggle;
  }

  render(): JSX.Element {
    return (
      <div id="OverrideSwitches" style={this.props.style}>
        <div style={label}>Override Switches</div>
        <div style={container}>
          <div style={column}>
            <div style={row}>
              <div style={column}>
                {this.state.LimitSwitchStatus.map((value, index) => {
                  return (
                    <label key={index} htmlFor="LimitToggle">
                      <input
                        type="checkbox"
                        id="LimitToggle"
                        name="LimitToggle"
                        checked={value}
                        onChange={() => this.toggleLimitSwitch(index)}
                      />
                      {SwitchNames[index]}
                    </label>
                  );
                })}
                <button
                  style={ToggleButton}
                  onClick={() =>
                    this.setState((prevState) => ({ LimitSwitchOverride: !prevState.LimitSwitchOverride }))
                  }
                >
                  Limit Switch Toggle: {this.state.LimitSwitchOverride ? 'on' : 'off'}
                </button>
              </div>
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
