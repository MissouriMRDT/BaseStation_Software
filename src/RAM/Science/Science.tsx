import React, { Component } from 'react';
import CSS from 'csstype';
// import SensorData from './components/SensorData';
// import SensorGraphs from './components/SensorGraphs';
// import Heater from './components/Heater';
import Cameras from '../../Core/components/Cameras';
// import RockLookUp from './components/rocklookup';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
// import Fluorometer from './components/Fluorometer';
import EncoderPositions from './components/EncoderPositions';
import OverrideSwitches from './components/OverrideSwitches';
import EnvironmentalData from './components/EnvironmentalData';
import Raman from './components/Raman';
// import SensorGraphs from './components/SensorGraphs';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};
const button: CSS.Properties = {
  margin: '5px',
};
const sensorMotorMultiplier = 1000;
const scoopMotorMultiplier = 1000;
const augerMotorMultiplier = 1000;
const proboscisMotorMultiplier = 1000;
let microscopePosition = 0;

function science(): void {
  if ('ScoopAxis_OpenLoop' in controllerInputs) {
    rovecomm.sendCommand('ScoopAxis_OpenLoop', 'Science', controllerInputs.ScoopAxis_OpenLoop * scoopMotorMultiplier);
  }

  if ('SensorAxis_OpenLoop' in controllerInputs) {
    rovecomm.sendCommand(
      'SensorAxis_OpenLoop',
      'Science',
      controllerInputs.SensorAxis_OpenLoop * sensorMotorMultiplier
    );
  }

  if ('AugerUp' in controllerInputs && 'AugerDown' in controllerInputs) {
    if (controllerInputs.AugerUp === 1) {
      rovecomm.sendCommand('Auger', 'Science', augerMotorMultiplier);
    } else if (controllerInputs.AugerDown === 1) {
      rovecomm.sendCommand('Auger', 'Science', -augerMotorMultiplier);
    } else {
      rovecomm.sendCommand('Auger', 'Science', 0);
    }
  }

  if ('ProboscisPlus' in controllerInputs && 'ProboscisMinus' in controllerInputs) {
    if (controllerInputs.ProboscisPlus === 1) {
      rovecomm.sendCommand('Proboscis', 'Science', proboscisMotorMultiplier);
    } else if (controllerInputs.ProboscisMinus === 1) {
      rovecomm.sendCommand('Proboscis', 'Science', -proboscisMotorMultiplier);
    } else {
      rovecomm.sendCommand('Proboscis', 'Science', 0);
    }
  }

  if ('MicroscopePlus' in controllerInputs && 'MicroscopeMinus' in controllerInputs) {
    if (controllerInputs.MicroscopePlus === 1 && microscopePosition < 180) {
      microscopePosition += 5;
      rovecomm.sendCommand('Microscope', 'Science', microscopePosition);
    } else if (controllerInputs.MicroscopeMinus === 1 && microscopePosition > 0) {
      microscopePosition -= 5;
      rovecomm.sendCommand('Microscope', 'Science', microscopePosition);
    }
  }

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

interface IState {
  selectedTab: string;
}

class Science extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      selectedTab: 'environmental', // Default tab is 'environmental'
    };

    setInterval(() => science(), 100);
  }

  handleTabChange = (tab: string) => {
    this.setState({ selectedTab: tab });
  };

  render(): JSX.Element {
    const { selectedTab } = this.state;
    return (
      <div style={column}>
        {selectedTab === 'environmental' && <EnvironmentalData />}
        {selectedTab === 'raman' && <Raman />}
        <div style={{ ...row, justifyContent: 'center', marginTop: '10px' }}>
          <button style={button} onClick={() => this.handleTabChange('environmental')}>
            Environmental Data
          </button>
          <button style={button} onClick={() => this.handleTabChange('raman')}>
            Raman (NOT IMPLEMENTED)
          </button>
        </div>
        <div style={{ ...row }}>
          {/* <div style={{ ...column, marginRight: '2.5px', width: '50%' }}><Heater /></div> */}
          <div style={{ ...column, marginRight: '2.5px', width: '100%' }}>
            <ControlScheme configs={['Science']} />
          </div>
        </div>
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <EncoderPositions style={{ width: '80%', marginRight: '2.5px', marginLeft: '2.5px' }} />
            {/* <Cameras defaultCamera={8} /> */}
          </div>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <OverrideSwitches style={{ width: '100%' }} />
          </div>
        </div>
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '100%' }}>{/* <ClosedLoopControls /> */}</div>
        </div>
        <Cameras defaultCamera={7} />
      </div>
    );
  }
}
export default Science;
