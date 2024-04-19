import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { ColorStyleConverter } from '../../Core/ColorConverter';

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

const mainContainer: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  gap: '5px',
  flexGrow: 1,
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
};
const btnArray: CSS.Properties = {
  display: 'flex',
  justifyContent: 'center',
  gap: '5px',
};
const readoutContainer: CSS.Properties = {
  width: '100%',
  display: 'grid',
  gridTemplateColumns: '1fr 1fr 1fr 200px',
  gap: '2px',
};
const busReadoutContainer: CSS.Properties = {
  width: '100%',
  display: 'grid',
  gridTemplateColumns: '2fr 1fr 200px',
  gridTemplateAreas: `'Motors non-aux-current pack'
                      'Core   non-aux-current pack'
                      'Aux    aux-current     pack'`,
  justifyItems: 'stretch',
  alignItems: 'stretch',
  rowGap: '5px',
  columnGap: '2px',
};
const readout: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '10pt',
  padding: '0.5em 1.2em',
  fontWeight: 'bold',
};
const cellReadout: CSS.Properties = {
  ...readout,
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
};
const packReadout: CSS.Properties = {
  ...cellReadout,
  gridRow: '1 / span 2',
  gridColumn: '-2 / -1',
};
const busReadout: CSS.Properties = {
  ...readout,
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  gap: '10px',
};
const busCurrentReadout: CSS.Properties = {
  ...cellReadout,
  backgroundColor: 'lightgray',
};

const MOTOR_ENABLE_BIT = 1 << 0;
const CORE_ENABLE_BIT = 1 << 1;
const AUX_ENABLE_BIT = 1 << 2;

const ENABLE_BITS = [MOTOR_ENABLE_BIT, CORE_ENABLE_BIT, AUX_ENABLE_BIT];

// power cycle everything (including network switch)
function reboot(): void {
  rovecomm.sendCommand('Reboot', 'PMS', 1);
}

// turn everything off except for network
function estop(): void {
  rovecomm.sendCommand('EStop', 'PMS', 1);
}

// turn everything off, cutting off communication
// NOTE: the only way to turn everything on again after suicide is releasing the physical E stop.
function suicide(): void {
  if (window.confirm('Are you sure you want to do this? You must pull the physical E Stop to turn the rover on again!'))
    rovecomm.sendCommand('Suicide', 'PMS', 1);
}

function enableBus(bitmask: number) {
  rovecomm.sendCommand('EnableBus', 'PMS', bitmask);
}

function disableBus(bitmask: number) {
  rovecomm.sendCommand('DisableBus', 'PMS', bitmask);
}

interface IProps {
  style?: CSS.Properties;
}

type BusStatus = 'enabled' | 'disabled' | 'pending';

interface IState {
  auxCurrent: number;
  packCurrent: number;
  packVoltage: number;
  cellVoltages: number[];
  busStatus: {
    Motors: BusStatus;
    Core: BusStatus;
    Aux: BusStatus;
  };

  // miscCurrents: number[]; // unused for now
}

type BusType = keyof IState['busStatus'];

class Power extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      auxCurrent: 0,
      packCurrent: 0,
      packVoltage: 0,
      cellVoltages: [0, 0, 0, 0, 0, 0],
      busStatus: {
        Motors: 'pending',
        Core: 'pending',
        Aux: 'pending',
      },
    };

    rovecomm.on('PackCurrent', (data: number) => this.setState({ packCurrent: data }));
    rovecomm.on('AuxCurrent', (data: number) => this.setState({ auxCurrent: data }));
    rovecomm.on('PackVoltage', (data: number) => this.setState({ packVoltage: data }));
    rovecomm.on('CellVoltage', (data: number[]) => this.setState({ cellVoltages: data }));

    rovecomm.on('BusStatus', (data: number) =>
      this.setState({
        busStatus: {
          Motors: data & MOTOR_ENABLE_BIT ? 'enabled' : 'disabled',
          Core: data & CORE_ENABLE_BIT ? 'enabled' : 'disabled',
          Aux: data & AUX_ENABLE_BIT ? 'enabled' : 'disabled',
        },
      })
    );
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>PMS</div>
        <div style={mainContainer}>
          <div style={busReadoutContainer}>
            {Object.keys(this.state.busStatus).map((bus, i) => {
              return (
                <div
                  key={bus}
                  style={{
                    ...busReadout,
                    gridArea: bus,
                    backgroundColor: this.state.busStatus[bus as BusType] === 'enabled' ? 'yellow' : 'lightgray',
                  }}
                >
                  <div>{bus}</div>
                  <button
                    style={{ cursor: this.state.busStatus[bus as BusType] === 'pending' ? 'progress' : 'pointer' }}
                    disabled={this.state.busStatus[bus as BusType] === 'pending'}
                    onClick={() => {
                      if (this.state.busStatus[bus as BusType] === 'disabled') {
                        enableBus(ENABLE_BITS[i]);
                      } else if (this.state.busStatus[bus as BusType] === 'enabled') {
                        disableBus(ENABLE_BITS[i]);
                      }
                      this.setState((prev) => {
                        const next: IState = { ...prev }; // shallow copy
                        next.busStatus[bus as BusType] = 'pending';
                        return next;
                      });
                    }}
                  >
                    {this.state.busStatus[bus as BusType] !== 'pending'
                      ? this.state.busStatus[bus as BusType] === 'disabled'
                        ? 'Enable'
                        : 'Disable'
                      : 'Pending...'}
                  </button>
                </div>
              );
            })}
            <div
              style={ColorStyleConverter(this.state.packCurrent - this.state.auxCurrent, 5, 30, 40, 120, 0, {
                ...busCurrentReadout,
                gridArea: 'non-aux-current',
              })}
            >
              <div>Core Current:</div>
              <div>{this.state.packCurrent - this.state.auxCurrent} A</div>
            </div>
            <div
              style={ColorStyleConverter(this.state.auxCurrent, 2, 10, 15, 120, 0, {
                ...busCurrentReadout,
                gridArea: 'aux-current',
              })}
            >
              <div>Aux Current:</div>
              <div>{this.state.auxCurrent} A</div>
            </div>
            <div
              style={ColorStyleConverter(this.state.packCurrent, 5, 40, 50, 120, 0, {
                ...busCurrentReadout,
                gridArea: 'pack',
              })}
            >
              <div>Pack Current:</div>
              <div>{this.state.packCurrent} A</div>
            </div>
          </div>
          <div style={readoutContainer}>
            {this.state.cellVoltages.map((voltage, i) => {
              return (
                // eslint-disable-next-line react/no-array-index-key
                <div key={i} style={ColorStyleConverter(voltage, 2.5, 3.1, 4.2, 0, 120, cellReadout)}>
                  <div>C{i}</div>
                  <div>{voltage} V</div>
                </div>
              );
            })}
            <div style={ColorStyleConverter(this.state.packVoltage, 15, 21.6, 25, 0, 120, packReadout)}>
              <div>Pack Voltage:</div>
              <div>{`${this.state.packVoltage} V`}</div>
            </div>
          </div>
          <hr style={{ borderTop: '2px dashed #990000', width: '100%' }} />
          <div style={btnArray}>
            <button type="button" onClick={() => reboot()} style={{ cursor: 'pointer' }}>
              REBOOT
            </button>
            <button type="button" onClick={() => estop()} style={{ cursor: 'pointer' }}>
              E-STOP
            </button>
            <button type="button" onClick={() => suicide()} style={{ cursor: 'pointer' }}>
              SUICIDE
            </button>
          </div>
          <hr style={{ borderTop: '2px dashed #990000', width: '100%' }} />
        </div>
      </div>
    );
  }
}

export default Power;
