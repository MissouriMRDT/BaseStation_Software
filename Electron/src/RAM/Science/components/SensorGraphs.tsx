/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries, DiscreteColorLegend, Crosshair } from "react-vis"
import fs from "fs"

import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import { windows } from "../../../Core/Window"

/**
 * Type definition for a sensor.
 * To add a new sensor, define the default values in the clearData() function and
 * add a function to interpret the rovecomm packet.
 */
type Sensor = {
  /** The unit to display in the crosshair */
  units: string
  /**Color of the line on the graph for this sensor */
  graphLineColor: string
  /**Style of the line in the graph for this sensor */
  graphLineType: "solid" | "dashed"
  /**Keeps track of the sensor value at a given time */
  values: { x: Date; y: number }[]
  /**Values normalized between 0 and 1 */
  normalizedValues: { x: Date; y: number }[]
  max: number
  min: number
}

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  justifyContent: "center",
}
const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const buttonrow: CSS.Properties = {
  display: "flex",
  alignItems: "center",
  justifyContent: "space-around",
  margin: "10px",
}
const row: CSS.Properties = {
  display: "flex",
  alignItems: "center",
  justifyContent: "space-around",
  margin: "0px 10px 10px",
}
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "75%",
  margin: "2.5px",
  justifyContent: "space-around",
}
const selector: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  margin: "2.5px",
}
const overlay: CSS.Properties = {
  width: "200px",
  color: "black",
}

function downloadURL(imgData: string): void {
  const filename = `./Screenshots/${new Date()
    .toISOString()
    // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
    .replaceAll(/[:\-TZ]/g, ".")}SensorGraph.png`

  if (!fs.existsSync("./Screenshots")) {
    fs.mkdirSync("./Screenshots")
  }

  const base64Image = imgData.replace("image/png", "image/octet-stream").split(";base64,").pop()
  fs.writeFileSync(filename, base64Image!, { encoding: "base64" })
}

function saveImage(): void {
  // Search through all the windows for SensorGraphs
  let graph
  let thisWindow
  for (const win of Object.keys(windows)) {
    if (windows[win].document.getElementById("SensorGraph")) {
      // When found, store the graph and the window it was in
      thisWindow = windows[win]
      graph = thisWindow.document.getElementById("SensorGraph")
      break
    }
  }

  // If the graph isn't found, throw an error
  if (!graph) {
    throw new Error("The element 'SensorGraph' wasn't found")
  }

  // If the graph is found, convert its html into a canvas to be downloaded
  html2canvas(graph, {
    scrollX: 0,
    scrollY: -thisWindow.scrollY - 38,
  }) // We subtract 38 to make up for the 28 pixel top border and the -10 top margin
    .then((canvas: any) => {
      const imgData = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream")
      downloadURL(imgData)
      return null
    })
    .catch((error: any) => {
      console.error(error)
    })
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  crosshairValues: Map<string, { x: Date; y: number }>
  sensors: Map<string, Sensor>
  enabledSensors: Map<string, boolean>
  crosshairPos: Date | null
}

class SensorGraphs extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      crosshairValues: new Map(),
      sensors: this.getNewEmptyMap(),
      enabledSensors: new Map([
        ["Methane", false],
        ["CO2", false],
        ["Temperature", false],
        ["O2PP", false],
        ["O2Concentration", false],
        ["O2Pressure", false],
        ["NO", false],
        ["N2O", false],
      ]),
      crosshairPos: null,
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.no = this.no.bind(this)
    this.n2o = this.n2o.bind(this)
    this.sensorSelectionChanged = this.sensorSelectionChanged.bind(this)
    this.selectAll = this.selectAll.bind(this)
    this.deselectAll = this.deselectAll.bind(this)
    this.addData = this.addData.bind(this)
    this.onNearestX = this.onNearestX.bind(this)
    this.onMouseLeave = this.onMouseLeave.bind(this)
    this.getNewEmptyMap = this.getNewEmptyMap.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
    rovecomm.on("NO", (data: any) => this.no(data))
    rovecomm.on("N2O", (data: any) => this.n2o(data))
  }

  onMouseLeave(): void {
    // When the mouse exits the graph area, the crosshair should be cleared
    const { crosshairValues } = this.state
    crosshairValues.clear()
    this.setState({ crosshairValues })
  }

  onNearestX(index: number, list: Array<{ x: Date; y: number }>, listName: string): void {
    // When we hover over the graph area, find the closest x position of each line series
    // (using a built in function to react-vis) and then set that key-value pair
    // in crosshair values to be displayed
    const { crosshairValues } = this.state
    crosshairValues.set(listName, list[index])
    this.setState({ crosshairValues, crosshairPos: list[index].x })
  }

  /**
   * Called when a checkbox or radio box is clicked to change which sensors are active
   * @param sensorName the name of the sensor that was toggled. Must be a valid sensor in {this.state.enabledSensors}
   */
  sensorSelectionChanged(sensorName: string): void {
    const { enabledSensors } = this.state

    enabledSensors.set(sensorName, !enabledSensors.get(sensorName))

    this.setState({ enabledSensors })
  }

  /**
   * Turn on all concentration sensors' graph displays
   */
  selectAll(): void {
    const { enabledSensors } = this.state

    enabledSensors.forEach((_value, key) => {
      enabledSensors.set(key, true)
    })

    this.setState({ enabledSensors })
  }

  /**
   * Turn off all sensors' graph displays
   */
  deselectAll(): void {
    const { enabledSensors } = this.state

    //Can't clear map because we use the keys to render the input boxes
    enabledSensors.forEach((_value, key) => {
      enabledSensors.set(key, false)
    })

    this.setState({ enabledSensors })
  }

  /**
   * Adds a value to the sensor's values
   * @param sensor the name of the sensor to add the data to
   * @param newData the data to add to the sensor's values
   */
  addData(name: string, newData: number): void {
    // Data of 0 probably just means the sensors aren't working and risks causing div by 0 errors
    if (newData === 0) {
      rovecomm.emit("all", name + " Sensor sent 0. Discarding.")
      return
    }

    const { sensors } = this.state

    let sensor = sensors.get(name)
    if (!sensor) {
      return
    }

    //if the min and max are -1, they are unset and need to be updated
    if (sensor.max === -1) {
      sensor.max = newData
    }
    if (sensor.min === -1) {
      sensor.min = newData
    }

    sensor.values.push({ x: new Date(), y: newData })
    //Normalize data from 0 to 1 based on the minimum and maximum
    sensor.normalizedValues.push({ x: new Date(), y: (newData - sensor.min) / (sensor.max - sensor.min) })

    //renormalize entire array if newData is outside of current range
    if (newData > sensor.max) {
      sensor.normalizedValues = []
      for (const pairs of sensor.values) {
        sensor.normalizedValues.push({ x: pairs.x, y: (pairs.y - sensor.min) / (newData - sensor.min) })
      }
      sensor.max = newData
    }
    if (newData < sensor.min) {
      sensor.normalizedValues = []
      for (const pairs of sensor.values) {
        sensor.normalizedValues.push({ x: pairs.x, y: (pairs.y - newData) / (sensor.max - newData) })
      }
      sensor.min = newData
    }
    sensors.set(name, sensor!)
    this.setState({ sensors })
  }

  methane(data: any): void {
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    this.addData("Methane", data[0])
  }

  co2(data: any): void {
    this.addData("CO2", data[0])
  }

  o2(data: any): void {
    this.addData("O2PP", data[0])
    this.addData("Temperature", data[1])
    this.addData("O2Concentration", data[2])
    this.addData("O2Pressure", data[3])
  }

  no(data: any): void {
    this.addData("NO", data[0])
  }

  n2o(data: any): void {
    this.addData("N2O", data[0])
  }

  /**
   * There's no way to deep copy the data in a Map, so this function returns a new map with the empty sensors
   * @returns A new empty map of the default sensors
   */
  getNewEmptyMap(): Map<string, Sensor> {
    return new Map([
      [
        "Methane",
        {
          units: "%",
          graphLineColor: "#990000",
          graphLineType: "solid",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "CO2",
        {
          units: "ppm",
          graphLineColor: "orange",
          graphLineType: "dashed",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "Temperature",
        {
          units: "&#176;C",
          graphLineColor: "yellow",
          graphLineType: "solid",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "O2PP",
        {
          units: "mBar",
          graphLineColor: "green",
          graphLineType: "dashed",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "O2Concentration",
        {
          units: "ppm",
          graphLineColor: "blue",
          graphLineType: "solid",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "O2Pressure",
        {
          units: "mBar",
          graphLineColor: "purple",
          graphLineType: "dashed",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "NO",
        {
          units: "ppm",
          graphLineColor: "black",
          graphLineType: "solid",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
      [
        "N2O",
        {
          units: "ppm",
          graphLineColor: "gray",
          graphLineType: "dashed",
          values: [],
          normalizedValues: [],
          max: -1,
          min: -1,
        },
      ],
    ])
  }

  crosshair(): JSX.Element | null {
    // Return the desired crosshair element

    let { crosshairPos, crosshairValues } = this.state

    // If we were able to find a reading at a time, then go ahead and display the crosshair
    // The heading will be that time as a string, and then if the key exists in crosshairValues
    // then we want to display its y value
    if (crosshairPos) {
      return (
        <Crosshair values={[...crosshairValues.values()]}>
          <div style={overlay}>
            <h3 style={{ backgroundColor: "white" }}>{crosshairPos.toTimeString().slice(0, 9)}</h3>
            {[...this.state.sensors].map(([name, sensor]) => {
              return (
                crosshairValues.has(name) && (
                  <p style={{ backgroundColor: "white" }}>
                    {name}:{" "}
                    {crosshairValues.get(name)?.y.toLocaleString(undefined, {
                      minimumFractionDigits: 2,
                    })}{" "}
                    {sensor.units}
                  </p>
                )
              )
            })}
          </div>
        </Crosshair>
      )
    }
    return null
  }

  render(): JSX.Element {
    return (
      <div id="SensorGraph" style={this.props.style}>
        <div style={label}>Sensor Graphs</div>
        <div style={container}>
          <div style={buttonrow}>
            <button type="button" onClick={e => setInterval(() => this.addData("Methane", 100 * Math.random()), 2000)}>
              TestMethane
            </button>
            <button type="button" onClick={e => setInterval(() => this.addData("N2O", 100 * Math.random()), 2000)}>
              TestN2O
            </button>
            <button type="button" onClick={this.selectAll}>
              Select All
            </button>
            <button type="button" onClick={this.deselectAll}>
              Deselect All
            </button>
            <button type="button" onClick={saveImage}>
              Export Graph
            </button>
            <button type="button" onClick={_e => this.setState({ sensors: this.getNewEmptyMap() })}>
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
                )
              })}
            </div>
          </div>
          <XYPlot
            style={{ margin: 10 }}
            width={window.document.documentElement.clientWidth - 50}
            height={300}
            xType="time"
            onMouseLeave={this.onMouseLeave}
          >
            <HorizontalGridLines style={{ fill: "none" }} />
            {[...this.state.sensors].map(([name, sensor]) => {
              return (
                this.state.enabledSensors.get(name) &&
                sensor.values !== [] && (
                  <LineSeries
                    data={
                      //If there's more than one graph enabled, use the normalized values
                      [...this.state.enabledSensors.values()].filter(Boolean).length > 1
                        ? sensor.normalizedValues
                        : sensor.values
                    }
                    style={{ fill: "none" }}
                    strokeWidth="6"
                    color={sensor.graphLineColor}
                    strokeStyle={sensor.graphLineType}
                    onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, sensor.values, name)}
                  />
                )
              )
            })}
            <XAxis />
            {[...this.state.enabledSensors.values()].filter(Boolean).length > 1 ? null : <YAxis />}
            {this.crosshair()}
          </XYPlot>
          <DiscreteColorLegend
            style={{ fontSize: "16px", textAlign: "center" }}
            items={[...this.state.sensors].map(([name, sensor]) => {
              return {
                title: name,
                strokeWidth: 6,
                color: sensor.graphLineColor,
                strokeStyle: sensor.graphLineType,
                disabled: !this.state.enabledSensors.get(name),
              }
            })}
            orientation="horizontal"
          />
        </div>
      </div>
    )
  }
}

export default SensorGraphs
