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

const LEDNames = ['Laser', 'White LED'];

interface IState {
  LedStatus: boolean[];
  enableLED: boolean;
  enableLEDToggle: boolean;
  coolerToggle: number;
}

class LEDs extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      LedStatus: [false, false],
      enableLED: false,
      enableLEDToggle: false,
      coolerToggle: 0,
    };
    this.toggleCooler = this.toggleCooler.bind(this);
    this.toggleLed = this.toggleLed.bind(this);
  }

  static buildLedCommand(LED: boolean[]): number {
    let bitmask = '';

    bitmask += LED[1] ? '1' : '0';
    bitmask += LED[0] ? '1' : '0';
    console.log(bitmask);
    const num = parseInt(bitmask, 2);
    console.log(num);
    return num;
  }

  toggleLed(index: number): void {
    if (this.state.enableLEDToggle) {
      const { LedStatus } = this.state;
      LedStatus[index] = !LedStatus[index];
      this.setState({
        LedStatus,
      });
      rovecomm.sendCommand('EnableLEDs', 'Instruments', LEDs.buildLedCommand(LedStatus));
    }
  }

  toggleCooler(): void {
    this.setState(
      (prevState) => ({ coolerToggle: prevState.coolerToggle + 1 }),
      () => {
        console.log(this.state.coolerToggle % 2);
        rovecomm.sendCommand('EnableCooler', 'ScienceActuation', this.state.coolerToggle % 2);
      }
    );
  }

  render(): JSX.Element {
    return (
      <div id="LEDs" style={this.props.style}>
        <div style={label}>Environmental Data</div>
        <div style={{ ...container, width: '50%' }}>
          <div>
            {this.state.LedStatus.map((value, index) => {
              const uniqueKey = `ledToggle_${index}`;
              return (
                <label key={uniqueKey} htmlFor={uniqueKey}>
                  <input
                    type="checkbox"
                    id={uniqueKey}
                    name={uniqueKey}
                    checked={value}
                    onChange={() => this.toggleLed(index)}
                  />
                  {LEDNames[index]}
                </label>
              );
            })}
            <button onClick={() => this.setState((prevState) => ({ enableLEDToggle: !prevState.enableLEDToggle }))}>
              LED Toggle: {this.state.enableLEDToggle ? 'on' : 'off'}
            </button>
            <button onClick={this.toggleCooler}>Toggle Cooler: {this.state.coolerToggle % 2 ? 'on' : 'off'}</button>
          </div>
        </div>
      </div>
    );
  }
}

export default LEDs;
