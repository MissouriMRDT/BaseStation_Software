/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from 'react';
import CSS from 'csstype';
import html2canvas from 'html2canvas';
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries, DiscreteColorLegend, Crosshair } from 'react-vis';
import fs from 'fs';

import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { windows } from '../../../Core/Window';

/**
 * Type definition for a sensor.
 * To add a new sensor, define the default values in the getNewEmptyMap() function and
 * add a function to interpret the rovecomm packet.
 */
type Sensor = {
  /** The unit to display in the crosshair */
  units: string;
  /** Color of the line in the graph for this sensor */
  graphLineColor: string;
  /** Style of the line in the graph for this sensor */
  graphLineType: 'solid' | 'dashed';
  /** Keeps track of the sensor value at a given time */
  values: { x: Date; y: number }[];
  /** Values normalized between 0 and 1 */
  normalizedValues: { x: Date; y: number }[];
  max: number;
  min: number;
};

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  justifyContent: 'center',
};
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
const buttonrow: CSS.Properties = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-around',
  margin: '10px',
};
const row: CSS.Properties = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-around',
  margin: '0px 10px 10px',
};
const selectbox: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  width: '75%',
  margin: '2.5px',
  justifyContent: 'space-around',
};
const selector: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  margin: '2.5px',
};
const overlay: CSS.Properties = {
  width: '200px',
  color: 'black',
};

function downloadURL(imgData: string): void {
  const filename = `./Screenshots/${new Date()
    .toISOString()
    // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
    .replaceAll(/[:\-TZ]/g, '.')}SensorGraph.png`;

  if (!fs.existsSync('./Screenshots')) {
    fs.mkdirSync('./Screenshots');
  }

  const base64Image = imgData.replace('image/png', 'image/octet-stream').split(';base64,').pop();
  if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
}

function saveImage(): void {
  // Search through all the windows for SensorGraphs
  let graph;
  let thisWindow;
  for (const win of Object.keys(windows)) {
    if (windows[win].document.getElementById('SensorGraph')) {
      // When found, store the graph and the window it was in
      thisWindow = windows[win];
      graph = thisWindow.document.getElementById('SensorGraph');
      break;
    }
  }

  // If the graph isn't found, throw an error
  if (!graph) {
    throw new Error("The element 'SensorGraph' wasn't found");
  }

  // If the graph is found, convert its html into a canvas to be downloaded
  html2canvas(graph, {
    scrollX: 0,
    scrollY: -thisWindow.scrollY - 38,
  }) // We subtract 38 to make up for the 28 pixel top border and the -10 top margin
    .then((canvas: any) => {
      const imgData = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
      downloadURL(imgData);
      return null;
    })
    .catch((error: any) => {
      console.error(error);
    });
}

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  /** Map of values to display in the crosshair. Keys are the name of the sensor.
   * Values are the nearest value of the graph on the x-axis to the mouse. */
  crosshairValues: Map<string, { x: Date; y: number }>;
  /** Map of the sensors, holds all recorded data sent from the rover.
   * Keys are the names of the sensor in PascalCase.
   * Values are Sensor objects which contain all rendering options and logged data. */
  sensors: Map<string, Sensor>;
  /** Keys are names of sensors in PascalCase,
   * values control whether the sensor shows on the graph or not */
  enabledSensors: Map<string, boolean>;
  /** The x-axis position to draw the crosshair at.
   * If null, the crosshair will not be drawn */
  crosshairPos: Date | null;
}

class SensorGraphs extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  /**
   * There's no way to easily deep copy the data in a Map, so this
   * function returns a new map with the empty sensors' options
   * @returns A new empty map of the default sensors
   */
  static getNewEmptyMap(): Map<string, Sensor> {
    return new Map([
      [
        'CH4',
        {
          units: 'ppm',
          graphLineColor: '#990000',
          graphLineType: 'solid',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        'CO2',
        {
          units: 'ppm',
          graphLineColor: 'orange',
          graphLineType: 'dashed',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        'Temperature',
        {
          units: '°C',
          graphLineColor: 'yellow',
          graphLineType: 'solid',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        'O2',
        {
          units: 'ppm',
          graphLineColor: 'blue',
          graphLineType: 'solid',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        'NH3',
        {
          units: 'ppm',
          graphLineColor: 'black',
          graphLineType: 'solid',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        'NO2',
        {
          units: 'ppm',
          graphLineColor: 'gray',
          graphLineType: 'dashed',
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
    ]);
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      crosshairValues: new Map(),
      sensors: SensorGraphs.getNewEmptyMap(),
      enabledSensors: new Map([
        ['CH4', false],
        ['CO2', false],
        ['Temperature', false],
        ['O2', false],
        ['NH3', false],
        ['NO2', false],
      ]),
      crosshairPos: null,
    };
    this.ch4 = this.ch4.bind(this);
    this.co2 = this.co2.bind(this);
    this.o2 = this.o2.bind(this);
    this.nh3 = this.nh3.bind(this);
    this.no2 = this.no2.bind(this);
    this.sensorSelectionChanged = this.sensorSelectionChanged.bind(this);
    this.selectAll = this.selectAll.bind(this);
    this.deselectAll = this.deselectAll.bind(this);
    this.addData = this.addData.bind(this);
    this.onNearestX = this.onNearestX.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);

    rovecomm.on('CH4', (data: any) => this.ch4(data));
    rovecomm.on('CO2', (data: any) => this.co2(data));
    rovecomm.on('O2', (data: any) => this.o2(data));
    rovecomm.on('NH3', (data: any) => this.nh3(data));
    rovecomm.on('NO2', (data: any) => this.no2(data));
  }

  /**
   * Clears the crosshair when the mouse leaves the graph area
   */
  onMouseLeave(): void {
    const { crosshairValues } = this.state;
    crosshairValues.clear();
    this.setState({ crosshairValues });
  }

  /**
   * Called for every enabled sensor when the mouse hovers over the graph
   * (using a built in function to react-vis) and then set that key-value pair
   * in crosshair values to be displayed
   *
   * @param index index of sensor's data to use
   * @param list sensor's data list
   * @param listName name of the sensor to set
   */
  onNearestX(index: number, list: Array<{ x: Date; y: number }>, listName: string): void {
    const { crosshairValues } = this.state;
    crosshairValues.set(listName, list[index]);
    this.setState({ crosshairValues, crosshairPos: list[index].x });
  }

  ch4(data: any): void {
    // the methane data packet is [methane concentration, temperature]
    this.addData('CH4', data[0]);
    this.addData('Temperature', data[1]);
  }

  co2(data: any): void {
    this.addData('CO2', data[0]);
  }

  o2(data: any): void {
    this.addData('O2', data[0]);
  }

  nh3(data: any): void {
    this.addData('nh3', data[0]);
  }

  no2(data: any): void {
    this.addData('NO2', data[0]);
  }

  /**
   * Adds a value to the sensor's values
   * Discards if data is 0
   * @param sensor the name of the sensor to add the data to
   * @param newData the data to add to the sensor's values
   */
  addData(name: string, newData: number): void {
    // Data of 0 risks causing div by 0 errors
    // log it to the Rovecomm console
    if (newData === 0) {
      rovecomm.emit('all', `${name} Sensor sent 0. Discarding.`);
      return;
    }

    const { sensors } = this.state;

    const sensor = sensors.get(name);
    if (!sensor) {
      return;
    }

    // if the min and max are -1, they are unset and need to be updated
    if (sensor.max === -1) {
      sensor.max = newData;
    }
    if (sensor.min === -1) {
      sensor.min = newData;
    }

    sensor.values.push({ x: new Date(), y: newData });
    // Normalize data from 0 to 1 based on the minimum and maximum
    sensor.normalizedValues.push({ x: new Date(), y: (newData - sensor.min) / (sensor.max - sensor.min) });

    // renormalize entire array if newData is outside of current range
    if (newData > sensor.max) {
      sensor.normalizedValues = [];
      for (const pairs of sensor.values) {
        sensor.normalizedValues.push({ x: pairs.x, y: (pairs.y - sensor.min) / (newData - sensor.min) });
      }
      sensor.max = newData;
    }
    if (newData < sensor.min) {
      sensor.normalizedValues = [];
      for (const pairs of sensor.values) {
        sensor.normalizedValues.push({ x: pairs.x, y: (pairs.y - newData) / (sensor.max - newData) });
      }
      sensor.min = newData;
    }
    sensors.set(name, sensor);
    this.setState({ sensors });
  }

  /**
   * Turn off all sensors' graph displays
   */
  deselectAll(): void {
    const { enabledSensors } = this.state;

    // Can't clear map because we use the keys to render the input boxes
    enabledSensors.forEach((_value, key) => {
      enabledSensors.set(key, false);
    });

    this.setState({ enabledSensors });
  }

  /**
   * Turn on all concentration sensors' graph displays
   */
  selectAll(): void {
    const { enabledSensors } = this.state;

    enabledSensors.forEach((_value, key) => {
      enabledSensors.set(key, true);
    });

    this.setState({ enabledSensors });
  }

  /**
   * Called when a checkbox or radio box is clicked to change which sensors are active
   * @param sensorName the name of the sensor that was toggled. Must be a valid sensor in {this.state.enabledSensors}
   */
  sensorSelectionChanged(sensorName: string): void {
    const { enabledSensors } = this.state;

    enabledSensors.set(sensorName, !enabledSensors.get(sensorName));

    this.setState({ enabledSensors });
  }

  /**
   * @returns The JSX element for the crosshair if the crosshair should render, null if it shouldn't render
   */
  crosshair(): JSX.Element | null {
    const { crosshairPos, crosshairValues } = this.state;

    // If we were able to find a reading at a time, then go ahead and display the crosshair
    // The heading will be that time as a string, and then if the key exists in crosshairValues
    // then we want to display its y value
    if (crosshairPos) {
      return (
        <Crosshair values={[...crosshairValues.values()]}>
          <div style={overlay}>
            <h3 style={{ backgroundColor: 'white' }}>{crosshairPos.toTimeString().slice(0, 9)}</h3>
            {[...this.state.sensors].map(([name, sensor]) => {
              return (
                crosshairValues.has(name) && (
                  <p style={{ backgroundColor: 'white' }}>
                    {name}:{' '}
                    {crosshairValues.get(name)?.y.toLocaleString(undefined, {
                      minimumFractionDigits: 2,
                    })}{' '}
                    {sensor.units}
                  </p>
                )
              );
            })}
          </div>
        </Crosshair>
      );
    }
    return null;
  }

  render(): JSX.Element {
    return (
      <div id="SensorGraph" style={this.props.style}>
        <div style={label}>Sensor Graphs</div>
        <div style={container}>
          <div style={buttonrow}>
            <button type="button" onClick={this.selectAll}>
              Select All
            </button>
            <button type="button" onClick={this.deselectAll}>
              Deselect All
            </button>
            <button type="button" onClick={saveImage}>
              Export Graph
            </button>
            <button type="button" onClick={() => this.setState({ sensors: SensorGraphs.getNewEmptyMap() })}>
              Clear Data
            </button>
          </div>
          <div style={row}>
            <div style={selectbox}>
              {[...this.state.enabledSensors].map(([sensorName, val]) => {
                return (
                  <div key={undefined} style={selector}>
                    <input
                      type="checkbox"
                      id={sensorName}
                      name={sensorName}
                      checked={val}
                      onChange={() => this.sensorSelectionChanged(sensorName)}
                    />
                    <label htmlFor={sensorName}>{sensorName}</label>
                  </div>
                );
              })}
            </div>
          </div>
          <XYPlot
            margin={{ left: 100, top: 10, bottom: 10 }}
            width={window.document.documentElement.clientWidth - 50}
            height={300}
            xType="time"
            onMouseLeave={this.onMouseLeave}
          >
            <HorizontalGridLines style={{ fill: 'none' }} />
            {[...this.state.sensors].map(([name, sensor]) => {
              return (
                this.state.enabledSensors.get(name) &&
                sensor.values.length > 0 (
                  <LineSeries
                    data={
                      // If there's more than one graph enabled, use the normalized values
                      // Gets the values from the enabledSensors, then filters them for truthy values
                      [...this.state.enabledSensors.values()].filter(Boolean).length > 1
                        ? sensor.normalizedValues
                        : sensor.values
                    }
                    style={{ fill: 'none' }}
                    strokeWidth="6"
                    color={sensor.graphLineColor}
                    strokeStyle={sensor.graphLineType}
                    onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, sensor.values, name)}
                  />
                )
              );
            })}
            <XAxis />
            {[...this.state.enabledSensors.values()].filter(Boolean).length > 1 ? null : <YAxis />}
            {this.crosshair()}
          </XYPlot>
          <DiscreteColorLegend
            style={{ fontSize: '16px', textAlign: 'center' }}
            items={[...this.state.sensors].map(([name, sensor]) => {
              return {
                title: name,
                strokeWidth: 6,
                color: sensor.graphLineColor,
                strokeStyle: sensor.graphLineType,
                disabled: !this.state.enabledSensors.get(name),
              };
            })}
            orientation="horizontal"
          />
        </div>
      </div>
    );
  }
}

export default SensorGraphs;
