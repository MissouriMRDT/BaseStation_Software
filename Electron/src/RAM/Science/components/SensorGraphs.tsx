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
  width: "200px",
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
}

class SensorGraphs extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      crosshairValues: new Map(),
      sensors: this.clearData(),
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
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.no = this.no.bind(this)
    this.n2o = this.n2o.bind(this)
    this.sensorSelectionChanged = this.sensorSelectionChanged.bind(this)
    this.selectAll = this.selectAll.bind(this)
    this.deselectAll = this.deselectAll.bind(this)
    this.onNearestX = this.onNearestX.bind(this)
    this.onMouseLeave = this.onMouseLeave.bind(this)
    this.clearData = this.clearData.bind(this)

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
    this.setState({ crosshairValues })
  }

  /**
   * Called when a checkbox or radio box is clicked to change which sensors are active
   * @param sensorName the name of the sensor that was toggled. Must be a valid sensor in {this.state.enabledSensors}
   */
  sensorSelectionChanged(sensorName: string): void {
    const { enabledSensors } = this.state

    if (sensorName === "Temperature" || sensorName === "O2Pressure") {
      //This will also deselect the temperature and o2pressure sensors.
      //The radio boxes don't allow deselection, so it'll get turned on again below
      this.deselectAll()
    } else {
      //If a concentration sensor is enabled, turn off the temperature & pressure sensors
      enabledSensors.set("Temperature", false)
      enabledSensors.set("O2Pressure", false)
    }
    enabledSensors.set(sensorName, !enabledSensors.get(sensorName))

    this.setState({ enabledSensors })
  }

  /**
   * Turn on all concentration sensors' graph displays
   */
  selectAll(): void {
    const { enabledSensors } = this.state

    enabledSensors.forEach((_value, key) => {
      if (key !== "O2Pressure" && key !== "Temperature") enabledSensors.set(key, true)
    })

    this.setState({ enabledSensors })
  }

  /**
   * Turn off all sensors' graph displays
   */
  deselectAll(): void {
    const { enabledSensors } = this.state

    //Can't clear here because we use this map to render the input boxes
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
    if (newData === 0) {
      return
    }

    const { sensors } = this.state
    if (!sensors.has(name)) {
      return
    }

    let sensor = sensors.get(name)

    if (sensor!.max === -1) {
      sensor!.max = newData
    }
    if (sensor!.min === -1) {
      sensor!.min = newData
    }

    sensor!.values.push({ x: new Date() })
  }

  methane(data: any): void {
    // Data of 0 probably just means the sensors aren't working and risks causing div by 0 errors
    if (data[0] === 0) {
      return
    }
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    const { sensors } = this.state
    //If the methane object doesn't exist, we can't write anything to it
    if (!sensors.has("Methane")) {
      return
    }
    let methane = sensors.get("Methane")

    // If the max_methane value is -1 its unset, so update it with the incoming data
    if (methane!.max === -1) {
      ;[methane!.max] = data
    }
    // If the min_methane value is -1 its unset, so update it with the incoming data
    if (methane!.min === -1) {
      ;[methane!.min] = data
    }

    methane?.values.push({ x: new Date(), y: data[0] })
    // Normalize the data to 0 to 1 by dividing its position in the range by the range
    methane?.normalizedValues.push({ x: new Date(), y: (data[0] - methane!.min) / (methane!.max - methane!.min) })

    // If the newest datum was bigger than the max, readjust all past data to the new normalization
    if (data[0] > methane!.max) {
      methane!.normalizedValues = []
      for (const pairs of methane!.values) {
        methane?.normalizedValues.push({ x: pairs.x, y: (pairs.y - methane.min) / (data[0] - methane.min) })
      }
      methane!.max = data[0]
    }
    // If the newest datum was smaller than the min, similarly readjust all past data to the new normalization
    if (data[0] < methane!.min) {
      methane!.normalizedValues = []
      for (const pairs of methane!.values) {
        methane?.normalizedValues.push({ x: pairs.x, y: (pairs.y - data[0]) / (methane.min - data[0]) })
      }
      methane!.min = data[0]
    }
    sensors.set("Methane", methane!)
    this.setState({ sensors })
  }

  co2(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { sensors } = this.state
    if (!sensors.has("CO2")) {
      return
    }
    let co2 = sensors.get("CO2")
    if (co2!.max === -1) {
      ;[co2!.max] = data
    }
    if (co2!.min === -1) {
      ;[co2!.min] = data
    }

    co2?.values.push({ x: new Date(), y: data[0] })
    co2?.normalizedValues.push({ x: new Date(), y: (data[0] - co2!.min) / (co2!.max - co2!.min) })

    if (data[0] > co2!.max) {
      co2!.normalizedValues = []
      for (const pairs of co2!.values) {
        co2!.normalizedValues.push({ x: pairs.x, y: (pairs.y - co2!.min) / (data[0] - co2!.min) })
      }
      co2!.max = data[0]
    }
    if (data[0] < co2!.min) {
      co2!.normalizedValues = []
      for (const pairs of co2!.values) {
        co2!.normalizedValues.push({ x: pairs.x, y: (pairs.y - data[0]) / (co2!.max - data[0]) })
      }
      co2!.min = data[0]
    }
    sensors.set("CO2", co2!)
    this.setState({ sensors })
  }

  o2(data: any): void {
    if (data[0] === 0 || data[1] === 0 || data[2] === 0 || data[3] === 0) {
      return
    }
    const { sensors } = this.state
    if (
      !sensors.has("Temperature") ||
      !sensors.has("O2PP") ||
      !sensors.has("O2Concentration") ||
      !sensors.has("O2Pressure")
    ) {
      return
    }
    let temperature = sensors.get("Temperature")
    let o2PP = sensors.get("O2PP")
    let o2Concentration = sensors.get("O2Concentration")
    let o2Pressure = sensors.get("o2Pressure")
    const [newO2PP, newTemperature, newO2Concentration, newO2Pressure] = data
    if (temperature!.max === -1 || o2PP!.max === -1 || o2Concentration!.max === -1 || o2Pressure!.max === -1) {
      temperature!.max = newTemperature
      o2PP!.max = newO2PP
      o2Concentration!.max = newO2Concentration
      o2Pressure!.max = newO2Pressure
    }
    if (temperature!.min === -1 || o2PP!.min === -1 || o2Concentration!.min === -1 || o2Pressure!.min === -1) {
      temperature!.min = newTemperature
      o2PP!.min = newO2PP
      o2Concentration!.min = newO2Concentration
      o2Pressure!.min = newO2Pressure
    }

    temperature!.values.push({ x: new Date(), y: newTemperature })
    o2PP!.values.push({ x: new Date(), y: newO2PP })
    o2Concentration!.values.push({ x: new Date(), y: newO2Concentration })
    o2Pressure!.values.push({ x: new Date(), y: newO2Pressure })

    temperature!.normalizedValues.push({
      x: new Date(),
      y: (newTemperature - temperature!.min) / (temperature!.max - temperature!.min),
    })
    o2PP!.normalizedValues.push({ x: new Date(), y: (newO2PP - o2PP!.min) / (o2PP!.max - o2PP!.min) })
    o2Concentration!.normalizedValues.push({
      x: new Date(),
      y: (newO2Concentration - o2Concentration!.min) / (o2Concentration!.max - o2Concentration!.min),
    })
    o2Pressure!.normalizedValues.push({
      x: new Date(),
      y: (newO2Pressure - o2Pressure!.min) / (o2Pressure!.max - o2Pressure!.min),
    })

    if (newTemperature > temperature!.max) {
      temperature!.normalizedValues = []
      for (const pairs of temperature!.values) {
        temperature!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - temperature!.min) / (newTemperature - temperature!.min),
        })
      }
      temperature!.max = newTemperature
    }
    if (newTemperature < temperature!.min) {
      temperature!.normalizedValues = []
      for (const pairs of temperature!.values) {
        temperature!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - newTemperature) / (temperature!.max - newTemperature),
        })
      }
      temperature!.min = newTemperature
    }
    if (newO2PP > o2PP!.max) {
      o2PP!.normalizedValues = []
      for (const pairs of o2PP!.values) {
        o2PP!.normalizedValues.push({ x: pairs.x, y: (pairs.y - o2PP!.min) / (newO2PP - o2PP!.min) })
      }
      o2PP!.max = newO2PP
    }
    if (newO2PP < o2PP!.min) {
      o2PP!.normalizedValues = []
      for (const pairs of o2PP!.values) {
        o2PP!.normalizedValues.push({ x: pairs.x, y: (pairs.y - newO2PP) / (o2PP!.max - newO2PP) })
      }
      o2PP!.min = newO2PP
    }
    if (newO2Concentration > o2Concentration!.max) {
      o2Concentration!.normalizedValues = []
      for (const pairs of o2Concentration!.values) {
        o2Concentration!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - o2Concentration!.min) / (newO2Concentration - o2Concentration!.min),
        })
      }
      o2Concentration!.max = newO2Concentration
    }
    if (newO2Concentration < o2Concentration!.min) {
      o2Concentration!.normalizedValues = []
      for (const pairs of o2Concentration!.values) {
        o2Concentration!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - newO2Concentration) / (o2Concentration!.max - newO2Concentration),
        })
      }
      o2Concentration!.min = newO2Concentration
    }
    if (newO2Pressure > o2Pressure!.max) {
      o2Pressure!.normalizedValues = []
      for (const pairs of o2Pressure!.values) {
        o2Pressure!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - o2Pressure!.min) / (newO2Pressure - o2Pressure!.min),
        })
      }
      o2Pressure!.max = newO2Pressure
    }
    if (newO2Pressure < o2Pressure!.min) {
      o2Pressure!.normalizedValues = []
      for (const pairs of o2Pressure!.values) {
        o2Pressure!.normalizedValues.push({
          x: pairs.x,
          y: (pairs.y - newO2Pressure) / (o2Pressure!.max - newO2Pressure),
        })
      }
      o2Pressure!.min = newO2Pressure
    }

    sensors.set("Temperature", temperature!)
    sensors.set("O2PP", o2PP!)
    sensors.set("O2Concentration", o2Concentration!)
    sensors.set("O2Pressure", o2Pressure!)
    this.setState({ sensors })
  }

  no(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { sensors } = this.state
    if (!sensors.has("NO")) {
      return
    }
    let no = sensors.get("NO")
    if (no!.max === -1) {
      ;[no!.max] = data
    }
    if (no!.min === -1) {
      ;[no!.min] = data
    }

    no!.values.push({ x: new Date(), y: data[0] })
    no!.normalizedValues.push({ x: new Date(), y: (data[0] - no!.min) / (no!.max - no!.min) })

    if (data[0] > no!.max) {
      no!.normalizedValues = []
      for (const pairs of no!.values) {
        no!.normalizedValues.push({ x: pairs.x, y: (pairs.y - no!.min) / (data[0] - no!.min) })
      }
      no!.max = data[0]
    }
    if (data[0] < no!.min) {
      no!.normalizedValues = []
      for (const pairs of no!.values) {
        no!.normalizedValues.push({ x: pairs.x, y: (pairs.y - data[0]) / (no!.max - data[0]) })
      }
      no!.min = data[0]
    }

    sensors.set("NO", no!)
    this.setState({ sensors })
  }

  n2o(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { sensors } = this.state
    if (!sensors.has("N2O")) {
      return
    }
    let n2o = sensors.get("N2O")
    if (n2o!.max === -1) {
      ;[n2o!.max] = data
    }
    if (n2o!.min === -1) {
      ;[n2o!.min] = data
    }

    n2o!.values.push({ x: new Date(), y: data[0] })
    n2o!.normalizedValues.push({ x: new Date(), y: (data[0] - n2o!.min) / (n2o!.max - n2o!.min) })

    if (data[0] > n2o!.max) {
      n2o!.normalizedValues = []
      for (const pairs of n2o!.values) {
        n2o!.normalizedValues.push({ x: pairs.x, y: (pairs.y - n2o!.min) / (data[0] - n2o!.min) })
      }
      n2o!.max = data[0]
    }
    if (data[0] < n2o!.min) {
      n2o!.normalizedValues = []
      for (const pairs of n2o!.values) {
        n2o!.normalizedValues.push({ x: pairs.x, y: (pairs.y - data[0]) / (n2o!.max - data[0]) })
      }
      n2o!.min = data[0]
    }

    sensors.set("N2O", n2o!)
    this.setState({ sensors })
  }

  clearData(): Map<string, Sensor> {
    let emptyData: Map<string, Sensor> = new Map([
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
          units: "ppm",
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
          units: "ppm",
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

    return emptyData
  }

  crosshair(): JSX.Element | null {
    // Return the desired crosshair element

    // We only want to return a crosshair element if there is a valid reading in the crosshair values
    // We would prefer CO2 cause it has the fastest update rate, then O2, then methane
    const { crosshairValues } = this.state
    let time
    if (crosshairValues.has("CO2")) {
      time = crosshairValues.get("CO2")?.x
    } else if (crosshairValues.has("O2PP")) {
      time = crosshairValues.get("O2PP")?.x
    } else if (crosshairValues.has("Methane")) {
      time = crosshairValues.get("Methane")?.x
    }

    // If we were able to find a reading at a time, then go ahead and display the crosshair
    // The heading will be that time as a string, and then if the key exists in crosshairValues
    // then we want to display its y value
    if (time) {
      return (
        <Crosshair values={[...crosshairValues.values()]}>
          <div style={overlay}>
            <h3 style={{ backgroundColor: "white" }}>{time?.toTimeString().slice(0, 9)}</h3>
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
            <button
              type="button"
              onClick={e => {
                this.methane([e.pageX])
              }}
            >
              TestMethane
            </button>
            <button
              type="button"
              onClick={e => {
                this.n2o([e.pageX])
              }}
            >
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
            <button type="button" onClick={_e => this.setState({ sensors: this.clearData() })}>
              Clear Data
            </button>
          </div>
          <div style={row}>
            <div style={selectbox}>
              {[...this.state.enabledSensors].map(([sensorName, val]) => {
                return (
                  <div key={undefined} style={selector}>
                    <input
                      type={sensorName === "Temperature" || sensorName === "O2Pressure" ? "radio" : "checkbox"}
                      id={sensorName}
                      name={sensorName}
                      checked={val}
                      onChange={() => this.sensorSelectionChanged(sensorName)}
                    />
                    <label>{sensorName}</label>
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
                    data={sensor.normalizedValues}
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
            <YAxis />
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
