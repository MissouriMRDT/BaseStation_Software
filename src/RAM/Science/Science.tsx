import React, { Component } from 'react';
import CSS from 'csstype';
// import SensorData from './components/SensorData';
// import SensorGraphs from './components/SensorGraphs';
import Heater from './components/Heater';
import Cameras from '../../Core/components/Cameras';
// import RockLookUp from './components/rocklookup';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import Fluorometer from './components/Fluorometer';
import ClosedLoopControls from './components/ClosedLoopControls';
import EncoderPositions from './components/EncoderPositions';
import OverrideSwitches from './components/OverrideSwitches';

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
const augerMotorMultiplier = 1000;
const proboscisMotorMultiplier = 500;
let microscopePosition = 0;

function science(): void {
  // Update controller inputs for these packets: EnvironmentalData (GRAPH)
  if ('ScoopAxis_OpenLoop' in controllerInputs) {
    rovecomm.sendCommand('ScoopAxis_OpenLoop', controllerInputs.ScoopAxis_OpenLoop * scoopMotorMultiplier);
  }

  if ('SensorAxis_OpenLoop' in controllerInputs) {
    rovecomm.sendCommand('SensorAxis_OpenLoop', controllerInputs.SensorAxis_OpenLoop * sensorMotorMultiplier);
  }

  if ('AugerUp' in controllerInputs) {
    rovecomm.sendCommand('Auger', augerMotorMultiplier);
  } else if ('AugerDown' in controllerInputs) {
    rovecomm.sendCommand('Auger', -augerMotorMultiplier);
  } else rovecomm.sendCommand('Auger', 0);

  if ('ProboscisPlus' in controllerInputs) {
    rovecomm.sendCommand('Proboscis', proboscisMotorMultiplier);
  } else if ('ProboscisMinus' in controllerInputs) {
    rovecomm.sendCommand('Proboscis', -proboscisMotorMultiplier);
  } else rovecomm.sendCommand('Proboscis', 0);

  if ('MicroscopePlus' in controllerInputs && microscopePosition < 180) {
    microscopePosition += 5;
  } else if ('MicroscopeMinus' in controllerInputs && microscopePosition > 0) {
    microscopePosition -= 5;
  }
  rovecomm.sendCommand('Microscope', microscopePosition);

  // 2023 Science System

  // if ('ScoopAxis_IncrementPosition' in controllerInputs) {
  //   rovecomm.sendCommand('ScoopAxis_IncrementPosition', [
  //     controllerInputs.ScoopAxis_IncrementPosition * scoopMotorMultiplier,
  //   ]);
  // }

  // if ('SensorAxis_IncrementPosition' in controllerInputs) {
  //   rovecomm.sendCommand('SensorAxis_IncrementPosition', [
  //     controllerInputs.SensorAxis_IncrementPosition * scoopMotorMultiplier,
  //   ]);
  // }

  // // If both open and close scoop are pressed, close it
  // if ('OpenScoop' in controllerInputs && 'CloseScoop' in controllerInputs) {
  //   if (controllerInputs.CloseScoop === 1) {
  //     rovecomm.sendCommand('LimitSwitchOverride', 1);
  //   } else if (controllerInputs.OpenScoop === 1) {
  //     rovecomm.sendCommand('LimitSwitchOverride', 0);
  //   }
  // }

  // if ('IncrementOpen' in controllerInputs && 'IncrementClose' in controllerInputs) {
  //   // Take the positive contribution from the open trigger and the negative contribution of the close trigger
  //   const IncrementAmt = controllerInputs.IncrementClose - controllerInputs.IncrementOpen;
  //   if (IncrementAmt !== 0) {
  //     rovecomm.sendCommand('Microscope', IncrementAmt * scoopIncrementMult);
  //   }
  // }

  // if ('WaterLeft' in controllerInputs && 'WaterRight' in controllerInputs) {
  //   if (controllerInputs.WaterLeft === 1) {
  //     rovecomm.sendCommand('SensorAxis_OpenLoop', [90]);
  //   } else if (controllerInputs.WaterRight === 1) {
  //     rovecomm.sendCommand('SensorAxis_OpenLoop', [-90]);
  //   } else {
  //     rovecomm.sendCommand('SensorAxis_OpenLoop', [0]);
  //   }
  // }

  // // if ('ScoopAxis_SetPosition' in controllerInputs) {
  // //   if (controllerInputs.ScoopAxis_SetPosition === 1) {
  // //     rovecomm.sendCommand('WatchdogOverride', microscopeMult);
  // //   }
  // // }

  // if ('MicroscopePlus' in controllerInputs && 'MicroscopeMinus' in controllerInputs) {
  //   if (controllerInputs.MicroscopePlus === 1) {
  //     rovecomm.sendCommand('WatchdogOverride', microscopeMult);
  //     console.log('on');
  //   } else if (controllerInputs.MicroscopeMinus === 1) {
  //     rovecomm.sendCommand('WatchdogOverride', [0]);
  //     console.log('off');
  //   }
  // }

  // if ('DropSample' in controllerInputs) {
  //   if (controllerInputs.DropSample === 1) {
  //     rovecomm.sendCommand('LimitSwitchOverride', 2);
  //   }
  // }
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
            <Cameras defaultCamera={8} />
          </div>
        </div>

        <ClosedLoopControls />
        <Cameras defaultCamera={7} />
        <OverrideSwitches />
      </div>
    );
  }
}
export default Science;
