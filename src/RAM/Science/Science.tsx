import React, { Component } from 'react';
import CSS from 'csstype';
// import SensorData from './components/SensorData';
// import SensorGraphs from './components/SensorGraphs';
// import Heater from './components/Heater';
// import Cameras from '../../Core/components/Cameras';
// import RockLookUp from './components/rocklookup';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import Reflectance from './components/Reflectance';
import EnvironmentalData from './components/EnvironmentalData';
import Raman from './components/Raman';
import ScienceGraphs from './components/ScienceGraphs';
import FTIR from './components/FTIR';
import CamerasContainer from '../../RED/components/CamerasContainer';
import CameraControls from '../../RED/components/CameraControls';
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
    rovecomm.sendCommand(
      'ScoopAxis_OpenLoop',
      'ScienceActuation',
      controllerInputs.ScoopAxis_OpenLoop * scoopMotorMultiplier
    );
  }

  if ('SensorAxis_OpenLoop' in controllerInputs) {
    rovecomm.sendCommand(
      'SensorAxis_OpenLoop',
      'ScienceActuation',
      controllerInputs.SensorAxis_OpenLoop * sensorMotorMultiplier
    );
  }

  if ('AugerUp' in controllerInputs && 'AugerDown' in controllerInputs) {
    if (controllerInputs.AugerUp === 1) {
      rovecomm.sendCommand('Auger', 'ScienceActuation', augerMotorMultiplier);
    } else if (controllerInputs.AugerDown === 1) {
      rovecomm.sendCommand('Auger', 'ScienceActuation', -augerMotorMultiplier);
    } else {
      rovecomm.sendCommand('Auger', 'ScienceActuation', 0);
    }
  }

  if ('ProboscisPlus' in controllerInputs && 'ProboscisMinus' in controllerInputs) {
    if (controllerInputs.ProboscisPlus === 1) {
      rovecomm.sendCommand('Proboscis', 'ScienceActuation', proboscisMotorMultiplier);
    } else if (controllerInputs.ProboscisMinus === 1) {
      rovecomm.sendCommand('Proboscis', 'ScienceActuation', -proboscisMotorMultiplier);
    } else {
      rovecomm.sendCommand('Proboscis', 'ScienceActuation', 0);
    }
  }

  if ('MicroscopePlus' in controllerInputs && 'MicroscopeMinus' in controllerInputs) {
    if (controllerInputs.MicroscopePlus === 1 && microscopePosition < 180) {
      microscopePosition += 5;
      rovecomm.sendCommand('Microscope', 'ScienceActuation', microscopePosition);
    } else if (controllerInputs.MicroscopeMinus === 1 && microscopePosition > 0) {
      microscopePosition -= 5;
      rovecomm.sendCommand('Microscope', 'ScienceActuation', microscopePosition);
    }
  }
}

interface IProps {}

interface IState {
  selectedTab: string;
}

class Science extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      selectedTab: 'raman', // Default tab is 'raman'
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
        {selectedTab === 'raman' && <Raman />}
        {selectedTab === 'reflectance' && <Reflectance />}
        {/* {selectedTab === 'FTIR' && <FTIR />} */}
        <div style={{ ...row, justifyContent: 'center', marginTop: '10px' }}>
          <button style={button} onClick={() => this.handleTabChange('raman')}>
            Raman
          </button>
          <button style={button} onClick={() => this.handleTabChange('reflectance')}>
            Reflectance
          </button>
          {/* <button style={button} onClick={() => this.handleTabChange('FTIR')}>
            FTIR
          </button> */}
        </div>
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '100%' }}>
            <EnvironmentalData />
          </div>
        </div>
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <CamerasContainer camAmount={7} canvasWidth={640} canvasHeight={480} />
          </div>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <CameraControls canvasWidth={640} canvasHeight={480} startSource={7} labelName={'Camera'}></CameraControls>
          </div>
        </div>
        <div style={{ ...column }}>
          <ControlScheme configs={['Science']} />
        </div>
        <div style={{ ...row }}>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <ScienceGraphs />
          </div>
          <div style={{ ...column, marginRight: '2.5px', width: '50%' }}>
            <ScienceGraphs />
          </div>
        </div>
      </div>
    );
  }
}
export default Science;
