import React, { Component } from 'react';
import CSS from 'csstype';
import Cameras from '../Core/components/Cameras';
import Map from '../RED/components/Map';
import ThreeDRover from '../Core/components/ThreeDRover';
import Angular from '../RAM/Arm/components/Angular';
import ControlFeatures from '../RAM/Arm/components/ControlFeatures';
import IK from '../RAM/Arm/components/IK';
import Activity from '../RAM/Autonomy/components/Activity';
import StateDiagram from '../RAM/Autonomy/components/StateDiagram';
import Heater from '../RAM/Science/components/Heater';
import SensorData from '../RAM/Science/components/SensorData';
import SensorGraphs from '../RAM/Science/components/SensorGraphs';
import Drive from '../RED/components/Drive';
import Gimbal from '../RED/components/Gimbal';
import Lighting from '../RAM/Autonomy/components/Lighting';
import Log from '../RED/components/Log';
import Power from '../RED/components/Power&BMS';
import CustomPackets from '../RON/components/CustomPackets';
import PacketLogger from '../RON/components/PacketLogger';
// import PingGraph from '../RON/components/PingGraph';
import Fluorometer from '../RAM/Science/components/Fluorometer';
import RockLookUp from '../RAM/Science/components/rocklookup';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  marginRight: '5px',
};
const submod: CSS.Properties = {
  height: 'calc(100% - 30px)',
  width: '100%',
  flexGrow: 1,
};

interface IProps {
  style?: CSS.Properties;
  rowcol: string;
  onMerge?: (display: string) => void;
  store: (name: string, coords: { lat: number; lon: number }) => void;
  displayed?: string; // Initial conditition of what should be displayed
}

interface IState {
  storedWaypoints: {
    [key: string]: {
      onMap: boolean;
      color: string;
      name: string;
      latitude: number;
      longitude: number;
      displayRadius: number;
    };
  };
  currentCoords: { lat: number; lon: number };
  display: React.ReactNode;
  displayed: string; // Active condidition of what should be displayed
}

class RoverImageryDisplay extends Component<IProps, IState> {
  buttons: JSX.Element;

  static defaultProps = {
    style: {},
    onMerge: null,
    displayed: 'Camera',
  };

  constructor(props: Readonly<IProps> | IProps) {
    super(props);

    // Buttons is a recurring div that is displayed with every choice, and used in many
    // places, so is declared here as a constant
    this.buttons = (
      <div style={row}>
        <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickCamera()}>
          Camera
        </button>
        <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickMap()}>
          Map
        </button>
        <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickThreeDRover()}>
          3D Rover
        </button>
        <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onMerge()}>
          Merge
        </button>
        <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickSplit()}>
          Split
        </button>
        <select onChange={(e) => this.onSelect(e.target.value)} style={{ flexGrow: 1, textAlign: 'center' }}>
          {[
            'Angular',
            'ControlFeatures',
            'IK',
            'Activity',
            'Controls',
            'StateDiagram',
            'Heater',
            'SensorData',
            'SensorGraphs',
            'Drive',
            'Gimbal',
            'GPS',
            'Flourometer',
            'Lighting',
            'Log',
            'Power',
            'Waypoints',
            'CustomPackets',
            'PacketLogger',
            'PingGraph',
            'PingMap',
            'PingTool',
            'RockLookup',
          ].map((component) => {
            return (
              <option key={component} value={component}>
                {component}
              </option>
            );
          })}
        </select>
      </div>
    );

    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      display: <div style={{ ...this.props.style, backgroundColor: 'E0E0E0' }}>{this.buttons}</div>,
      // Set displayed to the passed in default if there is one, or default to "none"
      displayed: this.props.displayed ? this.props.displayed : 'none',
    };
    this.merge = this.merge.bind(this);
    this.onSelect = this.onSelect.bind(this);
  }

  componentDidMount() {
    // When the component does mount, process the current displayed property
    // so that adjustments are made if something was passed in
    switch (this.state.displayed) {
      case 'Camera':
        this.onClickCamera();
        break;
      case 'Map':
        this.onClickMap();
        break;
      case '3D Rover':
        this.onClickThreeDRover();
        break;
      default:
        this.setState({
          display: <div style={{ ...this.props.style, backgroundColor: 'E0E0E0' }}>{this.buttons}</div>,
          displayed: 'none',
        });
    }
  }

  onClickCamera(cam = 1) {
    // Inside the RID component, renders the controls and a camera feed (cam 1 by default)
    this.setState({
      display: (
        <div style={{ flexGrow: 1, height: '100%' }}>
          {this.buttons}
          <Cameras defaultCamera={cam} style={submod} />
        </div>
      ),
      displayed: 'Camera',
    });
  }

  onClickThreeDRover() {
    // Inside the RID component, renders the controls and a 3D rover graphic
    // NOTE: this component causes paint and three to update too often and all 3D models break
    this.setState({
      display: (
        <div style={{ flexGrow: 1, height: '100%' }}>
          {this.buttons}
          <ThreeDRover style={submod} zoom={30} />
        </div>
      ),
      displayed: '3D Rover',
    });
  }

  onClickMap() {
    // Inside the RID component, renders the controls and a map
    // NOTE: the map does not contain any of the active markers and may not work with localmaps
    this.setState((prevState) => ({
      display: (
        <div style={{ flexGrow: 1, height: '100%' }}>
          {this.buttons}
          <Map
            style={submod}
            storedWaypoints={prevState.storedWaypoints}
            currentCoords={prevState.currentCoords}
            store={(name: string, coords: { lat: number; lon: number }) => this.props.store(name, coords)}
            name="RIDmap"
          />
        </div>
      ),
      displayed: 'Map',
    }));
  }

  onClickSplit() {
    // Splits the current view in half, alternating first by width, then by height
    // Whatever the active display was before splitting will persist as the first component

    // Sets up styles such these next two RID components take the shape passed in
    // with a margin between components (whether it needs to be on the right or bottom)
    // and sets up the next style to be the opposite of the current
    const RIDStyle = this.props.rowcol === 'row' ? row : column;
    const margin = this.props.rowcol === 'row' ? { marginRight: '5px' } : { marginBottom: '5px' };
    const nextStyle = this.props.rowcol === 'row' ? 'column' : 'row';

    // Then we set the display to be a div containing two more RID components
    // The first RID component will take on the current display and has the additional margins,
    // while the second is completely raw
    // Also binds the onMerge event of the children with the parent's merge function to
    // allow us to deconstruct and merge back up
    this.setState((prevState) => ({
      display: (
        <div style={{ ...RIDStyle, width: '100%', height: '100%' }}>
          <RoverImageryDisplay
            rowcol={nextStyle}
            style={{ ...margin, height: '100%', width: '100%' }}
            onMerge={this.merge}
            displayed={prevState.displayed}
            store={() => {}}
          />
          <RoverImageryDisplay
            rowcol={nextStyle}
            style={{ height: '100%', width: '100%' }}
            onMerge={this.merge}
            store={() => {}}
          />
        </div>
      ),
    }));
  }

  onSelect(value: string): void {
    // console.log(this.state, value)
    let submodule: JSX.Element = <p>Error loading {value}</p>;
    switch (value) {
      case 'Angular':
        submodule = <Angular style={submod} />;
        break;
      case 'ControlFeatures':
        submodule = <ControlFeatures style={submod} gripperCallBack={undefined} />;
        break;
      case 'IK':
        console.log('IK');
        submodule = <IK style={submod} />;
        break;
      case 'Activity':
        submodule = <Activity style={submod} />;
        break;
      case 'Controls':
        // We cannot currently support controls since we don't have access to waypoints
        break;
      case 'StateDiagram':
        submodule = <StateDiagram style={submod} />;
        break;
      case 'Heater':
        submodule = <Heater style={submod} />;
        break;
      case 'SensorData':
        submodule = <SensorData style={submod} />;
        break;
      case 'SensorGraphs':
        submodule = <SensorGraphs style={submod} />;
        break;
      case 'Fluorometer':
        submodule = <Fluorometer style={submod} />;
        break;
      case 'Drive':
        submodule = <Drive style={submod} />;
        break;
      case 'Gimbal':
        submodule = <Gimbal style={submod} />;
        break;
      case 'GPS':
        // We cannot currently support GPS since we have no onCoordsChange handler
        break;
      case 'Lighting':
        submodule = <Lighting style={submod} />;
        break;
      case 'Log':
        submodule = <Log style={submod} />;
        break;
      case 'Power':
        submodule = <Power style={submod} />;
        break;
      case 'Waypoints':
        // We cannot currently support Waypoints since we have no ref, onWaypointChange, or currentCoords
        break;
      case 'CustomPackets':
        submodule = <CustomPackets style={submod} />;
        break;
      case 'PacketLogger':
        submodule = <PacketLogger style={submod} />;
        break;
      case 'PingGraph':
        // We cannot currently support PingGraph because we have no devices object
        break;
      case 'PingMap':
        // We cannot currently support PingGraph because we have no devices object
        break;
      case 'PingTool':
        // We cannot currently support PingTool because we have no onDevicesChange handler
        break;
      case 'RockLookup':
        submodule = <RockLookUp style={submod} />;
        break;
      default:
        break;
    }
    this.setState({
      displayed: value,
      display: (
        <div style={{ flexGrow: 1, height: '100%' }}>
          {this.buttons}
          {submodule}
        </div>
      ),
    });
  }

  onMerge() {
    // If there is a defined onMerge function, call it with the current displayed component
    // If there isn't, this is probably the top-most component, so we just fail silently
    if (typeof this.props.onMerge === 'function') {
      this.props.onMerge(this.state.displayed);
    }
  }

  merge(display: string) {
    // When merge is called, change from two RID components to the passed in component
    switch (display) {
      case 'Camera':
        this.onClickCamera();
        break;
      case 'Map':
        this.onClickMap();
        break;
      case '3D Rover':
        this.onClickThreeDRover();
        break;
      default:
        this.setState({ display: this.buttons, displayed: 'none' });
    }
  }

  render(): JSX.Element {
    // Since this.state.display is always changing but set to exactly what we want to display,
    // we can just render that directly into a div
    return <div style={this.props.style}>{this.state.display}</div>;
  }
}
export default RoverImageryDisplay;
