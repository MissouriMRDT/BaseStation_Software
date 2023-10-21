import React, { Component } from 'react';
import CSS from 'csstype';
// import SensorData from './components/SensorData';
// import SensorGraphs from './components/SensorGraphs';
import Heater from './components/Heater';
// import Cameras from '../../Core/components/Cameras';
// import RockLookUp from './components/rocklookup';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import Fluorometer from './components/Fluorometer';
import ClosedLoopControls from './components/ClosedLoopControls';
import EncoderPositions from './components/EncoderPositions';

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
const microscopeMult = 500;

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
    const IncrementAmt = controllerInputs.IncrementClose - controllerInputs.IncrementOpen;
    if (IncrementAmt !== 0) {
      rovecomm.sendCommand('IncrementalScoop', IncrementAmt * scoopIncrementMult);
    }
  }

  if ('WaterLeft' in controllerInputs && 'WaterRight' in controllerInputs) {
    if (controllerInputs.WaterLeft === 1) {
      rovecomm.sendCommand('WaterSelector', [90]);
    } else if (controllerInputs.WaterRight === 1) {
      rovecomm.sendCommand('WaterSelector', [-90]);
    } else {
      rovecomm.sendCommand('WaterSelector', [0]);
    }
  }

  // if ('WaterPump' in controllerInputs) {
  //   if (controllerInputs.WaterPump === 1) {
  //     rovecomm.sendCommand('MicroscopeFocus', microscopeMult);
  //   }
  // }

  if ('MicroscopeFocusPlus' in controllerInputs && 'MicroscopeFocusMinus' in controllerInputs) {
    if (controllerInputs.MicroscopeFocusPlus === 1) {
      rovecomm.sendCommand('MicroscopeFocus', microscopeMult);
      console.log('on');
    } else if (controllerInputs.MicroscopeFocusMinus === 1) {
      rovecomm.sendCommand('MicroscopeFocus', [0]);
      console.log('off');
    }
  }

  if ('DropSample' in controllerInputs) {
    if (controllerInputs.DropSample === 1) {
      rovecomm.sendCommand('ScoopGrabber', 2);
    }
  }
}

interface IProps {}

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
        <Fluorometer />
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <Heater />
          </div>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <ControlScheme configs={['Science']} />
            <EncoderPositions style={{ width: '50%', marginRight: '2.5px', marginLeft: '2.5px' }} />
          </div>
        </div>

        <ClosedLoopControls />
      </div>
    );
  }
}
export default Science;
