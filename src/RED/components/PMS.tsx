import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { ColorStyleConverter } from '../../Core/ColorConverter';
import Log from './Log';

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
const cellReadoutContainer: CSS.Properties = {
  width: '100%',
  display: 'grid',
  gridArea: 'cells',
  gridTemplateColumns: '1fr 1fr 1fr',
  gap: '2px',
};
const busReadoutContainer: CSS.Properties = {
  width: '100%',
  display: 'grid',
  gridTemplateColumns: '1fr 1fr 1fr',
  gridTemplateAreas: `'Motors non-aux-current pack-current'
                      'Core   non-aux-current pack-current'
                      'Aux    aux-current     pack-current'
                      'cells  cells           pack-voltage'`,
  rowGap: '5px',
  columnGap: '2px',
};
const miscReadoutContainer: CSS.Properties = {
  width: '100%',
  display: 'flex',
  justifyContent: 'space-evenly',
  alignItems: 'center',
};
const readout: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '12pt',
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
  gridArea: 'pack-voltage',
};
const busReadout: CSS.Properties = {
  ...readout,
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  gap: '10px',
};
const coreCurrentReadout: CSS.Properties = {
  ...cellReadout,
  gridArea: 'non-aux-current',
};
const auxCurrentReadout: CSS.Properties = {
  ...cellReadout,
  gridArea: 'aux-current',
};
const packCurrentReadout: CSS.Properties = {
  ...cellReadout,
  gridArea: 'pack-current',
};

const MOTOR_ENABLE_BIT = 1 << 0;
const CORE_ENABLE_BIT = 1 << 1;
const AUX_ENABLE_BIT = 1 << 2;

const ENABLE_BITS = [MOTOR_ENABLE_BIT, CORE_ENABLE_BIT, AUX_ENABLE_BIT];

const TELEMETRY_TIMEOUT = 10_000; // milliseconds

// power cycle everything (including network switch)
function reboot(): void {
  if (window.confirm('Are you sure you want to do this? It will take a few minutes to reboot the network switch.'))
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
  miscCurrents: number[];
  telemetryTimeoutId: NodeJS.Timeout | null;
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
      miscCurrents: [0, 0, 0],
      telemetryTimeoutId: null,
    };
    const onPmsTimeout = () => {
      console.log(`${new Date().toLocaleTimeString()} Warning: stopped receiving packets from PMS!`);
      this.setState({
        auxCurrent: 0,
        packCurrent: 0,
        packVoltage: 0,
        cellVoltages: [0, 0, 0, 0, 0, 0],
        busStatus: { Motors: 'pending', Core: 'pending', Aux: 'pending' },
        miscCurrents: [0, 0, 0],
        telemetryTimeoutId: null,
      });
    };
    const telemetryCallback = () => {
      if (this.state.telemetryTimeoutId !== null) {
        clearTimeout(this.state.telemetryTimeoutId);
      }
      this.setState({
        telemetryTimeoutId: setTimeout(onPmsTimeout, TELEMETRY_TIMEOUT),
      });
    };

    rovecomm.on('PackCurrent', (data: number) => {
      this.setState({ packCurrent: data });
      telemetryCallback();
    });
    rovecomm.on('AuxCurrent', (data: number) => {
      this.setState({ auxCurrent: data });
      telemetryCallback();
    });
    rovecomm.on('PackVoltage', (data: number) => {
      this.setState({ packVoltage: data });
      telemetryCallback();
    });
    rovecomm.on('CellVoltage', (data: number[]) => {
      this.setState({ cellVoltages: data });
      telemetryCallback();
    });

    rovecomm.on('BusStatus', (data: number) => {
      this.setState({
        busStatus: {
          Motors: data & MOTOR_ENABLE_BIT ? 'enabled' : 'disabled',
          Core: data & CORE_ENABLE_BIT ? 'enabled' : 'disabled',
          Aux: data & AUX_ENABLE_BIT ? 'enabled' : 'disabled',
        },
      });
      telemetryCallback();
    });

    rovecomm.on('MiscCurrent', (data: number[]) => {
      this.setState({ miscCurrents: data });
      telemetryCallback();
    });
  }

  coreCurrent() {
    return this.state.packCurrent - this.state.auxCurrent;
  }

  componentWillUnmount(): void {
    if (this.state.telemetryTimeoutId !== null) {
      clearTimeout(this.state.telemetryTimeoutId);
    }
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
            <div style={ColorStyleConverter(this.coreCurrent(), 0.5, 7.2, 30, 120, 0, coreCurrentReadout)}>
              <div>Core Current:</div>
              <div>{this.coreCurrent().toFixed(2)} A</div>
            </div>
            <div style={ColorStyleConverter(this.state.auxCurrent, 0.5, 5, 15, 120, 0, auxCurrentReadout)}>
              <div>Aux Current:</div>
              <div>{Number(this.state.auxCurrent).toFixed(2)} A</div>
            </div>
            <div style={ColorStyleConverter(this.state.packCurrent, 0.5, 9.5, 45, 120, 0, packCurrentReadout)}>
              <div>Pack Current:</div>
              <div>{Number(this.state.packCurrent).toFixed(2)} A</div>
            </div>
            <div style={cellReadoutContainer}>
              {this.state.cellVoltages.map((voltage, i) => {
                return (
                  // eslint-disable-next-line react/no-array-index-key
                  <div key={i} style={ColorStyleConverter(voltage, 2.5, 3.1, 4.2, 0, 120, cellReadout)}>
                    <div>{`C${i + 1}`}</div>
                    <div>{voltage.toFixed(2)} V</div>
                  </div>
                );
              })}
            </div>
            <div style={ColorStyleConverter(this.state.packVoltage, 15, 21.6, 25, 0, 120, packReadout)}>
              <div>Pack Voltage:</div>
              <div>{Number(this.state.packVoltage).toFixed(2)} V</div>
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
          <div style={miscReadoutContainer}>
            <div style={{ padding: '0.5em' }}>External Current Sensing</div>
            {this.state.miscCurrents.map((current, i) => {
              // eslint-disable-next-line react/no-array-index-key
              return <div key={i} style={readout}>{`C${i + 1}: ${current.toFixed(2)} A`}</div>;
            })}
          </div>
        </div>
      </div>
    );
  }
}

export default Power;
