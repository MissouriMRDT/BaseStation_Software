import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm, RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
import { ColorStyleConverter } from '../../Core/ColorConverter';
import { BitmaskUnpack } from '../../Core/BitmaskUnpack';

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
const textPad: CSS.Properties = {
  paddingLeft: '10px',
  paddingRight: '10px',
};
const mainContainer: CSS.Properties = {
  display: 'flex',
  flexWrap: 'wrap',
  flexDirection: 'column',
  flexGrow: 1,
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  fontSize: '10px',
  lineHeight: '10px',
};
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 2,
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 2,
};
const readoutContainter: CSS.Properties = {
  height: '301px',
  flexWrap: 'wrap',
  margin: '1px',
  marginBottom: '175px',
};
const readout: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-between',
  fontFamily: 'arial',
};
const btnArray: CSS.Properties = {
  display: 'grid',
  gridTemplateColumns: '105px 130px 105px',
  justifyContent: 'center',
  gridTemplateRows: '25px',
};
const cellReadoutContainer: CSS.Properties = {
  display: 'grid',
  gridTemplateColumns: 'auto auto auto auto',
};
const btnStyle: CSS.Properties = {
  width: '70px',
  cursor: 'pointer',
};

/**
 * The specific function of turnOffReboot() originates from how the control boards
 * on the rover are programmed. If turnOffReboot() gets passed "0", the rover turns off.
 * If "time" is any positive number, the rover just powercycles for that many seconds.
 * It's been determined that 5 seconds is long enough for a standard powercycle as any longer
 * is mostly redundent and would cut into the allowed time during competition, and any shorter
 * is probably not enough time. ¯\_(ツ)_/¯
 * @param time if time = 0, Rover turns off. Otherwise it power cycles for 'time' seconds
 */
function turnOffReboot(time: number): void {
  rovecomm.sendCommand('BMSStop', 'BMS', [time]);
}

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  boardTelemetry: any;
  batteryTelemetry: any;
}

class Power extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    // eslint-disable-next-line @typescript-eslint/no-shadow
    const { Power, BMS } = RovecommManifest;
    const boardTelemetry: Record<string, any> = {};
    const batteryTelemetry: Record<string, any> = {};
    Object.keys(Power.Commands).forEach((Bus: string) => {
      if (Bus === 'SetBus') {
        // Check if the command is 'SetBus'
        boardTelemetry[Bus] = {};
        Power.Commands[Bus].comments.split(', ').forEach((component: any) => {
          const componentName = component.split(' ')[0]; // Extract the first part of the comment
          if (!componentName.toLowerCase().includes('enable') && !componentName.toLowerCase().includes('disable')) {
            boardTelemetry[Bus][componentName] = { enabled: true, value: 0 };
          }
        });
      }
    });

    Object.keys(BMS.Telemetry).forEach((measGroup: string) => {
      batteryTelemetry[measGroup] = {};
      const tmpList = BMS.Telemetry[measGroup].comments.split(', ');
      if (tmpList.length > 1) {
        tmpList.forEach((cell: any) => {
          batteryTelemetry[measGroup][cell] = { value: 0 };
        });
      } else {
        batteryTelemetry[measGroup] = { value: 0 };
      }
    });
    this.state = {
      boardTelemetry,
      batteryTelemetry,
    };

    /**
     * bit of awkwardness with the board telemetry listeners:
     * the code that populates the state array for the board telemetry use the "Commands"
     * object from the manifest because the "comments" subobject don't have any duplicates.
     * The "Telemetry" object contains duplicates because there are two separate data types
     * that relate to each physical component. Because the populator pulls from "Commands,"
     * though, the name of the object in the State array is the name of the Command object.
     *
     * As of 2/21/22, changes are planned to accommodate the splitting of the power board in
     * addition to the fact that some of the boards should not have toggle buttons as they
     * should never toggle (nor will they since those specific boards aren't listening for
     * toggle commands).
     *
     * # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     *   IF YOU ARE EXPERIENCING ERRORS RELATING TO ROVECOMM
     *   MAKE SURE THE LISTENER IS LISTENING FOR TELEMETRY AND
     *   IS APPLYING THAT VALUE TO THE NAME FROM THE COMMANDS OBJECT
     * # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     */
    // Later, add functionality to have EnableBus and DisableBus buttons for the boards that can toggle
    rovecomm.on('SetBus', (data: number[]) => this.boardListenHandlerTog(data, 'SetBus'));
    rovecomm.on('BusCurrent', (data: number[]) => this.boardListenHandlerAmp(data, 'SetBus'));

    // Add Reboot, EStop, and Suicide button
    rovecomm.on('PackCurrent', (data: number[]) => this.batteryListenHandler(data, 'PackCurrent'));
    rovecomm.on('PackVoltage', (data: number[]) => this.batteryListenHandler(data, 'PackVoltage'));
    rovecomm.on('PackTemp', (data: number[]) => this.batteryListenHandler(data, 'PackTemp'));
    rovecomm.on('CellVoltage', (data: number[]) => this.batteryListenHandler(data, 'CellVoltage'));
    // Add Error Handling notification
  }

  /**
   * @desc takes amperage data from rovecomm and applies those values to the corresponding state object in boardTelemetry
   * @param data contains an array of floats that correspond to electrical current measurements
   * @param partList the list of components determined by the state object
   */
  boardListenHandlerAmp(data: number[], partList: string): void {
    const { boardTelemetry } = this.state;
    console.log(boardTelemetry);
    console.log(boardTelemetry[partList]);
    Object.keys(boardTelemetry[partList]).forEach((part: string, index: number) => {
      boardTelemetry[partList][part].value = data[index];
    });
    // The setState is kept until the end because of how the priority of state changes are handled.
    // ### This reason remains the same for all functions with a setState at the end. ###
    this.setState({ boardTelemetry });
  }

  /**
   * @desc takes toggle data from rovecomm and applies those values to the corresponding state object in boardTelemetry
   * @param data contains a single bitmasked number inside of an arra
   * @param partList this list of componenets dtermined by the state object
   */
  boardListenHandlerTog(data: number[], partList: string): void {
    const { boardTelemetry } = this.state;
    const bitmask = BitmaskUnpack(data[0], Object.keys(boardTelemetry[partList]).length);
    Object.keys(boardTelemetry[partList])
      .reverse() // Reverse to keep correct order for bitmap command
      .forEach((part: string, index: number) => {
        boardTelemetry[partList][part].enabled = Boolean(Number(bitmask[index]));
      });
    this.setState({ boardTelemetry });
  }

  /**
   * @desc takes voltage data from rovecomm and applies those values to the corresponding state object in batteryTelemetry
   * @param data is an array of voltage values. If the array size is >1, it assigns those values to the battery cell objects
   * @param part name of the object to assign values to
   */
  batteryListenHandler(data: number[], part: string): void {
    const { batteryTelemetry } = this.state;
    if (data.length > 1) {
      Object.keys(batteryTelemetry[part]).forEach((cell: any, index: number) => {
        batteryTelemetry[part][cell].value = data[index];
      });
    } else {
      batteryTelemetry[part].value = data[0];
    }
    this.setState({ batteryTelemetry });
  }

  /**
   * @desc gets called by each Enable/Disable button that changes a single value. After the state object is changed, packCommand() gets called immediately after.
   * @param board the object that the bus object is a child of
   * @param bus the object that is getting toggled by the Enable/Disable button
   */
  // buttonToggle(board: string, bus: string): void {
  //   const { boardTelemetry } = this.state;
  //   boardTelemetry[board][bus].enabled = !this.state.boardTelemetry[board][bus].enabled;
  //   this.setState({ boardTelemetry }, () => this.packCommand(board));
  // }

  /**
   * To simultaneously simplify code and to assure ALL the motors get enabled in case the
   * signal connection is spotty, the allMotorToggle button was split into two staticly
   * defined buttons
   * @desc Takes true or false and attempts to apply that to every motor object. After reassigning the state objects, it sends a bitmasked command to the rover
   * @param button True or false depending on which button is pressed
   */
  // allMotorToggle(button: boolean): void {
  //   const { boardTelemetry } = this.state;
  //   Object.keys(boardTelemetry.SetBus).forEach((motor: string) => {
  //     boardTelemetry.SetBus[motor].enabled = button;
  //   });
  //   this.setState({ boardTelemetry }, () => this.packCommand('SetBus'));
  // }

  /**
   * @desc gets called any time a bus needs to be toggled. Takes the array of booleans and translates it to a bitmasked integer which then gets sent to the relevant board.
   * @param board corresponds to the board that is being sent the toggle command
   */
  // packCommand(board: string): void {
  //   const { boardTelemetry } = this.state;
  //   let newBitMask = '';
  //   Object.keys(boardTelemetry[board])
  //     .reverse() // Reverse to keep correct order for bitmap command
  //     .forEach((bus) => {
  //       newBitMask += boardTelemetry[board][bus].enabled ? '1' : '0';
  //     });
  //   rovecomm.sendCommand(board, [parseInt(newBitMask, 2)]);
  // }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Power and BMS</div>
        <div style={mainContainer}>
          <div style={{ ...column, ...readoutContainter }}>
            {Object.keys(this.state.boardTelemetry).map((board: string) => {
              return (
                <div key={board} style={{ ...column }}>
                  {Object.keys(this.state.boardTelemetry[board]).map((bus: string) => {
                    const { enabled, value } = this.state.boardTelemetry[board][bus];
                    return (
                      <div key={bus} style={row}>
                        {/* <button type="button" onClick={() => this.buttonToggle(board, bus)} style={btnStyle}>
                          {enabled ? 'Enabled' : 'Disabled'}
                        </button> */}
                        <div style={ColorStyleConverter(value, 0, 7, 15, 120, 0, readout)}>
                          <h3 style={textPad}>{bus}</h3>
                          <h3 style={textPad}>
                            {`${value.toLocaleString(undefined, {
                              minimumFractionDigits: 1,
                              maximumFractionDigits: 1,
                              minimumIntegerDigits: 2,
                            })} A`}
                          </h3>
                        </div>
                      </div>
                    );
                  })}
                </div>
              );
            })}
          </div>
          <div style={{ ...row, ...btnArray, gridTemplateColumns: 'auto auto' }}>
            {/* <button
              type="button"
              onClick={() => {
                this.allMotorToggle(true);
              }}
              style={{ cursor: 'pointer' }}
            >
              Enable All Motors
            </button> */}
            {/* <button
              type="button"
              onClick={() => {
                this.allMotorToggle(false);
              }}
              style={{ cursor: 'pointer' }}
            >
              Disable All Motors
            </button> */}
            <button type="button" onClick={() => turnOffReboot(5)} style={{ cursor: 'pointer' }}>
              REBOOT
            </button>
            <button type="button" onClick={() => turnOffReboot(0)} style={{ cursor: 'pointer' }}>
              SHUT DOWN
            </button>
          </div>
          <h3
            style={{
              alignSelf: 'center',
              fontSize: '16px',
              fontFamily: 'arial',
              marginTop: '-1px',
              marginBottom: '2px',
            }}
          >
            -------------------------------------------------
          </h3>
          <div style={{ ...row, width: '100%' }}>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.PackTemp.value, 30, 75, 115, 120, 0, readout)}>
              <h3 style={textPad}>Battery Temperature</h3>
              <h3 style={textPad}>{this.state.batteryTelemetry.PackTemp.value.toLocaleString(undefined)}°</h3>
            </div>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.PackTemp.value, 0, 15, 160, 120, 0, readout)}>
              <h3 style={textPad}>Total Pack Current</h3>
              <h3 style={textPad}>{`${this.state.batteryTelemetry.PackCurrent.value.toLocaleString(undefined)} A`}</h3>
            </div>
            <div
              style={ColorStyleConverter(this.state.batteryTelemetry.PackVoltage.value, 15, 21.6, 25, 0, 120, readout)}
            >
              <h3 style={textPad}>Total Pack Voltage</h3>
              <h3 style={textPad}>{`${this.state.batteryTelemetry.PackVoltage.value.toLocaleString(undefined)} V`}</h3>
            </div>
          </div>
          <div style={{ ...row, width: '100%' }}>
            <div style={{ ...cellReadoutContainer, width: '100%' }}>
              {Object.keys(this.state.batteryTelemetry.CellVoltage).map((cell) => {
                const { value } = this.state.batteryTelemetry.CellVoltage[cell];
                return (
                  <div key={cell} style={ColorStyleConverter(value, 2.5, 3.1, 4.2, 0, 120, readout)}>
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>{`${value.toLocaleString(undefined)} V`}</h3>
                  </div>
                );
              })}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Power;
