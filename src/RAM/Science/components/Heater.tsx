import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { BitmaskUnpack } from '../../../Core/BitmaskUnpack';

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
  height: '30px',
  marginTop: '5px',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  justifyContent: 'space-around',
};
const blockLabel: CSS.Properties = {
  width: '100%',
  justifyContent: 'space-around',
  alignSelf: 'center',
  marginLeft: '5px',
  fontWeight: 'bold',
};

const button: CSS.Properties = {
  width: '150px',
  height: '25px',
  alignSelf: 'center',
  margin: '0 5px 0 5px',
};

/** Will be merged with the row css if the block is off */
const offIndicator: CSS.Properties = {
  backgroundColor: '#FF0000',
};

/** Will be merged with the row css if the block is off */
const onIndicator: CSS.Properties = {
  backgroundColor: '#00FF00',
};

type HeaterBlock = {
  /** The current temperature of the block in Celsius */
  temp: number;
  /** True if the heater block is turned on */
  isOn: boolean;
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  blocks: HeaterBlock[];
  UVPowered: boolean;
  WhiteLightPowered: boolean;
  targetTemperature: number[];
}

class Heater extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  static buildLightCommand(UV: boolean, White: boolean): number {
    let bitmask = '';
    bitmask += UV ? '1' : '0';
    bitmask += White ? '1' : '0';
    return parseInt(bitmask, 2);
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      UVPowered: false,
      WhiteLightPowered: false,
      blocks: new Array(12)
        .fill(0)
        .map(() => ({ temp: -1, isOn: false }))
        .flat(),
      targetTemperature: [20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20],
    };
    this.updateTemps = this.updateTemps.bind(this);
    this.toggleBlock = this.toggleBlock.bind(this);
    this.updateEnabled = this.updateEnabled.bind(this);
    this.setAllBlocks = this.setAllBlocks.bind(this);
    this.toggleWhiteLight = this.toggleWhiteLight.bind(this);
    this.toggleUV = this.toggleUV.bind(this);
    this.setTargetTemperature = this.setTargetTemperature.bind(this);
    rovecomm.on('ThermoValues', (data: any) => this.updateTemps(data));
    rovecomm.on('HeaterEnabled', (data: any) => this.updateEnabled(data));
  }

  /**
   * Sends the command to set all blocks to either on or off
   * Does **not** update the state of the component. The board will send when it has turned off
   * @param powered true to turn all on, false to turn all off
   */
  setAllBlocks(powered: boolean): void {
    let bitmask = powered ? '1' : '0';
    bitmask = bitmask.repeat(this.state.blocks.length);
    rovecomm.sendCommand('HeaterToggle', [parseInt(bitmask, 2)]);
  }

  setTargetTemperature(index: number, event: { target: { value: string } }): void {
    let setTemp: number = parseInt(event.target.value, 10);
    this.setState((prevState) => {
      const targetTemperature = [...prevState.targetTemperature];
      if (setTemp < 0 || Number.isNaN(setTemp)) {
        setTemp = 0;
      } else if (setTemp > 105) {
        setTemp = 105;
      }
      targetTemperature[index] = setTemp;
      console.log(targetTemperature);
      return { targetTemperature };
    });
  }

  toggleWhiteLight(): void {
    this.setState(
      (prevState) => ({
        WhiteLightPowered: !prevState.WhiteLightPowered,
      }),
      () => {
        rovecomm.sendCommand('Lights', [Heater.buildLightCommand(this.state.UVPowered, this.state.WhiteLightPowered)]);
      }
    );
  }

  toggleUV(): void {
    this.setState(
      (prevState) => ({ UVPowered: !prevState.UVPowered }),
      () => {
        rovecomm.sendCommand('Lights', [Heater.buildLightCommand(this.state.UVPowered, this.state.WhiteLightPowered)]);
      }
    );
  }

  updateEnabled(data: number[]): void {
    const { blocks } = this.state;
    console.log(data);
    const bitmask = BitmaskUnpack(data[0], blocks.length);
    for (let i = 0; i < blocks.length; i++) {
      // subtracted from the length since we have to reverse the order of the block
      blocks[i].isOn = Boolean(Number(bitmask[i])).valueOf();
    }
    console.log(bitmask);
    this.setState({ blocks });
  }

  /**
   * Updates the temperatures shown on the component
   * @param temps a three value array of temps in degrees C
   */
  updateTemps(temps: number[]): void {
    const { blocks } = this.state;

    for (let i = 0; i < this.state.blocks.length; i++) {
      blocks[i].temp = temps[i];
      // console.log(blocks[i].temp);
    }

    this.setState({ blocks });
  }

  /**
   * Sends the command to toggle one block
   * Does **not** update the state of the component. The board will send when it has turned off
   * @param index 0-based index of the block to toggle
   */
  toggleBlock(index: number): void {
    const blocks: HeaterBlock[] = JSON.parse(JSON.stringify(this.state.blocks)); // Make a new array with copied data in order to not change state
    blocks[index].isOn = !blocks[index].isOn;

    let bitmask = '';
    for (let i = 0; i < blocks.length; i++) {
      // Reverse for-loop since the order of the blocks is reversed on the board
      bitmask += blocks[i].isOn ? '1' : '0';
    }
    rovecomm.sendCommand('HeaterToggle', [parseInt(bitmask, 2)]);
  }

  render(): JSX.Element {
    return (
      <div id="Heater" style={this.props.style}>
        <div style={label}>Science Hardware</div>
        <div style={container}>
          <div style={column}>
            {this.state.blocks.map((block, index) => {
              return (
                <div key={index} style={{ ...row, ...(block.isOn ? onIndicator : offIndicator) }}>
                  <p style={blockLabel}>Block {index + 1}: </p>
                  <button style={button} onClick={() => this.toggleBlock(index)}>
                    {block.temp.toFixed(2)}&#176; C
                  </button>
                  <input
                    type="text"
                    value={this.state.targetTemperature[index]}
                    style={{ ...button, width: '15%' }}
                    onChange={(event) => this.setTargetTemperature(index, event)}
                  />
                  <p style={{ alignSelf: 'center', marginRight: '5px', marginLeft: '2px' }}>&#176;C</p>
                </div>
              );
            })}
            <div style={row}>
              <button
                style={button}
                onClick={() => rovecomm.sendCommand('HeaterSetTemp', this.state.targetTemperature)}
              >
                Set Temperature
              </button>
              <button style={button} onClick={() => this.setAllBlocks(false)}>
                Disable All
              </button>
            </div>
            <div style={row}>
              <div>
                <label htmlFor="WhiteCheck">
                  <input
                    type="checkbox"
                    id="WhiteCheck"
                    name="WhiteCheck"
                    onChange={() => this.toggleWhiteLight()}
                    checked={this.state.WhiteLightPowered}
                  />
                  White Light
                </label>
              </div>
              <div>
                <label htmlFor="UVCheck">
                  <input
                    type="checkbox"
                    id="UVCheck"
                    name="UVCheck"
                    onChange={() => this.toggleUV()}
                    checked={this.state.UVPowered}
                  />
                  UV Light
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Heater;
