import React, { Component } from 'react';
import CSS from 'csstype';
import SensorData from './components/SensorData';
import SensorGraphs from './components/SensorGraphs';
import Heater from './components/Heater';
import Cameras from '../../Core/components/Cameras';
import RockLookUp from './components/rocklookup';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import Fluorometer from './components/Fluorometer';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};
const sensorMotorMultiplier = 500;
const scoopMotorMultiplier = 500;
const scoopIncrementMult = 5;

function science(): void {
  // Z actuation of the science system is controlled by the left up/down thumbstick
  if ('SensorAxis' in controllerInputs) {
    rovecomm.sendCommand('SensorAxis', [controllerInputs.SensorAxis * sensorMotorMultiplier]);
  }

  if ('XoopAxis' in controllerInputs) {
    rovecomm.sendCommand('XoopAxis', [controllerInputs.XoopAxis * scoopMotorMultiplier]);
  }

  if ('ZoopAxis' in controllerInputs) {
    rovecomm.sendCommand('ZoopAxis', [controllerInputs.ZoopAxis * scoopMotorMultiplier]);
  }

  // If both open and close scoop are pressed, close it
  if ('OpenScoop' in controllerInputs && 'CloseScoop' in controllerInputs) {
    if (controllerInputs.CloseScoop === 1) {
      rovecomm.sendCommand('ScoopGrabber', 1);
    } else if (controllerInputs.OpenScoop === 1) {
      rovecomm.sendCommand('ScoopGrabber', 0);
    }
  }

  if ('IncrementOpen' in controllerInputs && 'IncrementClose' in controllerInputs) {
    // Take the positive contribution from the open trigger and the negative contribution of the close trigger
    const IncrementAmt = controllerInputs.IncrementOpen - controllerInputs.IncrementClose;
    if (IncrementAmt !== 0) {
      rovecomm.sendCommand('IncrementalScoop', IncrementAmt * scoopIncrementMult);
    }
  }

  // Water controls are sent in one bitmasked value
  if ('Water1' in controllerInputs && 'Water2' in controllerInputs && 'Water3' in controllerInputs) {
    if ('WaterGroup1' in controllerInputs && 'WaterGroup2' in controllerInputs && 'WaterGroup3' in controllerInputs) {
      let water = '';
      if (controllerInputs.WaterGroup1 === 1) {
        water += controllerInputs.Water3;
        water += controllerInputs.Water2;
        water += controllerInputs.Water1;
      } else if (controllerInputs.WaterGroup2 === 1) {
        water += controllerInputs.Water3;
        water += controllerInputs.Water2;
        water += controllerInputs.Water1;
        water += '000';
      } else if (controllerInputs.WaterGroup3 === 1) {
        water += controllerInputs.Water3;
        water += controllerInputs.Water2;
        water += controllerInputs.Water1;
        water += '000000';
      } else {
        for (let i = 0; i < 3; i++) {
          water += controllerInputs.Water3;
        }
        for (let i = 0; i < 3; i++) {
          water += controllerInputs.Water2;
        }
        for (let i = 0; i < 3; i++) {
          water += controllerInputs.Water1;
        }
      }
      rovecomm.sendCommand('Water', parseInt(water, 2));
    }
  }
}

interface IProps {
  theme: string;
}

interface IState {}

class Science extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {};

    setInterval(() => science(), 100);
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <SensorGraphs theme={this.props.theme} />
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <SensorData theme={this.props.theme} />
            <Heater />
            <ControlScheme configs={['Science']} theme={this.props.theme} />
          </div>
          <RockLookUp style={{ marginLeft: '2.5px' }} />
        </div>
        <Cameras defaultCamera={7} theme={this.props.theme} />
      </div>
    );
  }
}
export default Science;
