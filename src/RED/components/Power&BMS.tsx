import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm, RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
import { ColorStyleConverter } from '../../Core/ColorConverter';
import { BitmaskUnpack } from '../../Core/BitmaskUnpack';
import { read } from 'fs';

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
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};
const readoutContainter: CSS.Properties = {
  height: 'auto',
  flexWrap: 'wrap',
  marginBottom: '10px',
};
const readout: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-between',
  fontFamily: 'arial',
};
const btnArray: CSS.Properties = {
  display: 'flex',
  justifyContent: 'center',
  gap: '5px',
};
const cellReadoutContainer: CSS.Properties = {
  display: 'grid',
  gridTemplateColumns: 'auto auto auto',
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
function reboot(): void {
  rovecomm.sendCommand('Reboot', 'PMS', 1);
}

function EStop(): void {
  rovecomm.sendCommand('EStop', 'PMS', 1);
}

function suicide(): void {
  rovecomm.sendCommand('Suicide', 'PMS', 1);
}

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  boardTelemetry: any;
  auxCurrent: number;
  packCurrent: number;
  packVoltage: number;
  cellVoltages: number[];
}

class Power extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    // eslint-disable-next-line @typescript-eslint/no-shadow
    const { PMS } = RovecommManifest;
    const boardTelemetry: Record<string, any> = {};
    Object.keys(PMS.Commands).forEach((Bus: string) => {
      if (Bus === 'SetBus') {
        // Check if the command is 'SetBus'
        boardTelemetry[Bus] = {};
        PMS.Commands[Bus].comments.split(', ').forEach((component: any) => {
          const componentName = component.split(' ')[0]; // Extract the first part of the comment
          const cleanedComponentName = componentName.replace(/^\[|\]$/g, ''); // Remove [ from the beginning and ] from the end
          if (
            !cleanedComponentName.toLowerCase().includes('enable') &&
            !cleanedComponentName.toLowerCase().includes('disable')
          ) {
            boardTelemetry[Bus][cleanedComponentName] = { enabled: true, value: 0 };
          }
        });
      }
    });

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
    rovecomm.on('PackCurrent', (data: number) => this.setState({ packCurrent: data }));
    rovecomm.on('AuxCurrent', (data: number) => this.setState({ auxCurrent: data }));
    rovecomm.on('PackVoltage', (data: number) => this.setState({ packVoltage: data }));
    rovecomm.on('CellVoltage', (data: number[]) => this.setState({ cellVoltages: data }));
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
        <div style={label}>PMS</div>
        <div style={mainContainer}>
          <div style={{ ...column, ...readoutContainter }}>
            {Object.keys(this.state.boardTelemetry).map((board: string) => {
              return (
                <div key={board} style={{ ...column }}>
                  {Object.keys(this.state.boardTelemetry[board]).map((bus: string) => {
                    const { value } = this.state.boardTelemetry[board][bus];
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
          <div
            style={{
              ...row,
              ...btnArray,
            }}
          >
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
            <button type="button" onClick={() => reboot()} style={{ cursor: 'pointer' }}>
              REBOOT
            </button>
            <button type="button" onClick={() => EStop()} style={{ cursor: 'pointer' }}>
              E-STOP
            </button>
            <button type="button" onClick={() => suicide()} style={{ cursor: 'pointer' }}>
              SUICIDE
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
            <div style={readout}>
              <h3 style={textPad}>Total Pack Current</h3>
              <h3 style={textPad}>{`${this.state.packCurrent} A`}</h3>
            </div>
            <div style={ColorStyleConverter(this.state.packVoltage, 15, 21.6, 25, 0, 120, readout)}>
              <h3 style={textPad}>Total Pack Voltage</h3>
              <h3 style={textPad}>{`${this.state.packVoltage.toLocaleString(undefined)} V`}</h3>
            </div>
          </div>
          <div style={{ ...row, width: '100%' }}>
            <div style={{ ...cellReadoutContainer, width: '100%' }}>
              {this.state.cellVoltages.map((cell, i) => {
                return (
                  <div key={i} style={ColorStyleConverter(i, 2.5, 3.1, 4.2, 0, 120, readout)}>
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>{`${i.toLocaleString(undefined)} V`}</h3>
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
