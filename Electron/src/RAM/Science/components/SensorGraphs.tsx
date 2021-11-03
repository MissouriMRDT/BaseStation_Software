/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries, DiscreteColorLegend, Crosshair } from "react-vis"
import fs from "fs"

import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import { windows } from "../../../Core/Window"

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
  methane: { x: Date; y: number }[]
  co2: { x: Date; y: number }[]
  temperature: { x: Date; y: number }[]
  o2PP: { x: Date; y: number }[]
  o2Concentration: { x: Date; y: number }[]
  o2Pressure: { x: Date; y: number }[]
  no: { x: Date; y: number }[]
  n2o: { x: Date; y: number }[]
  normalized_methane: { x: Date; y: number }[]
  normalized_co2: { x: Date; y: number }[]
  normalized_temperature: { x: Date; y: number }[]
  normalized_o2PP: { x: Date; y: number }[]
  normalized_o2Concentration: { x: Date; y: number }[]
  normalized_o2Pressure: { x: Date; y: number }[]
  normalized_no: { x: Date; y: number }[]
  normalized_n2o: { x: Date; y: number }[]
  max_methane: number
  max_co2: number
  max_temperature: number
  max_o2PP: number
  max_o2Concentration: number
  max_o2Pressure: number
  max_no: number
  max_n2o: number
  min_methane: number
  min_co2: number
  min_temperature: number
  min_o2PP: number
  min_o2Concentration: number
  min_o2Pressure: number
  min_no: number
  min_n2o: number
  enabledSensors: Map<string, boolean>
}

class SensorGraphs extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      crosshairValues: new Map(),
      methane: [],
      co2: [],
      temperature: [],
      o2PP: [],
      o2Concentration: [],
      o2Pressure: [],
      no: [],
      n2o: [],
      normalized_methane: [],
      normalized_co2: [],
      normalized_temperature: [],
      normalized_o2PP: [],
      normalized_o2Concentration: [],
      normalized_o2Pressure: [],
      normalized_no: [],
      normalized_n2o: [],
      max_methane: -1,
      max_co2: -1,
      max_temperature: -1,
      max_o2PP: -1,
      max_o2Concentration: -1,
      max_o2Pressure: -1,
      max_no: -1,
      max_n2o: -1,
      min_methane: -1,
      min_co2: -1,
      min_temperature: -1,
      min_o2PP: -1,
      min_o2Concentration: -1,
      min_o2Pressure: -1,
      min_no: -1,
      min_n2o: -1,
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

  methane(data: any): void {
    // Data of 0 probably just means the sensors aren't working and risks causing div by 0 errors
    if (data[0] === 0) {
      return
    }
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    const { methane } = this.state
    let { normalized_methane, max_methane, min_methane } = this.state
    // If the max_methane value is -1 its unset, so update it with the incoming data
    if (max_methane === -1) {
      ;[max_methane] = data
    }
    // If the min_methane value is -1 its unset, so update it with the incoming data
    if (min_methane === -1) {
      ;[min_methane] = data
    }

    methane.push({ x: new Date(), y: data[0] })
    // Normalize the data to 0 to 1 by dividing its position in the range by the range
    normalized_methane.push({ x: new Date(), y: (data[0] - min_methane) / (max_methane - min_methane) })

    // If the newest datum was bigger than the max, readjust all past data to the new normalization
    if (data[0] >= max_methane) {
      normalized_methane = []
      for (const pairs of methane) {
        normalized_methane.push({ x: pairs.x, y: (pairs.y - min_methane) / (data[0] - min_methane) })
      }
      this.setState({ max_methane: data[0], normalized_methane })
    }
    // If the newest datum was smaller than the min, similarly readjust all past data to the new normalization
    if (data[0] <= min_methane) {
      normalized_methane = []
      for (const pairs of methane) {
        normalized_methane.push({ x: pairs.x, y: (pairs.y - data[0]) / (max_methane - data[0]) })
      }
      this.setState({ min_methane: data[0], normalized_methane })
    }

    this.setState({ methane })
  }

  co2(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { co2 } = this.state
    let { normalized_co2, max_co2, min_co2 } = this.state
    if (max_co2 === -1) {
      ;[max_co2] = data
    }
    if (min_co2 === -1) {
      ;[min_co2] = data
    }

    co2.push({ x: new Date(), y: data[0] })
    normalized_co2.push({ x: new Date(), y: data[0] / max_co2 })

    if (data[0] >= max_co2) {
      normalized_co2 = []
      for (const pairs of co2) {
        normalized_co2.push({ x: pairs.x, y: (pairs.y - min_co2) / (data[0] - min_co2) })
      }
      this.setState({ max_co2: data[0], normalized_co2 })
    }
    if (data[0] <= min_co2) {
      normalized_co2 = []
      for (const pairs of co2) {
        normalized_co2.push({ x: pairs.x, y: (pairs.y - data[0]) / (max_co2 - data[0]) })
      }
      this.setState({ min_co2: data[0], normalized_co2 })
    }

    this.setState({ co2 })
  }

  o2(data: any): void {
    if (data[0] === 0 || data[1] === 0 || data[2] === 0 || data[3] === 0) {
      return
    }
    const { temperature, o2PP, o2Concentration, o2Pressure } = this.state
    let { max_temperature, max_o2PP, max_o2Concentration, max_o2Pressure } = this.state
    let { min_temperature, min_o2PP, min_o2Concentration, min_o2Pressure } = this.state
    let { normalized_temperature, normalized_o2PP, normalized_o2Concentration, normalized_o2Pressure } = this.state
    const [newO2PP, newTemperature, newO2Concentration, newO2Pressure] = data
    if (max_temperature === -1 || max_o2PP === -1 || max_o2Concentration === -1 || max_o2Pressure === -1) {
      max_temperature = newTemperature
      max_o2PP = newO2PP
      max_o2Concentration = newO2Concentration
      max_o2Pressure = newO2Pressure
    }
    if (min_temperature === -1 || min_o2PP === -1 || min_o2Concentration === -1 || min_o2Pressure === -1) {
      min_temperature = newTemperature
      min_o2PP = newO2PP
      min_o2Concentration = newO2Concentration
      min_o2Pressure = newO2Pressure
    }

    temperature.push({ x: new Date(), y: newTemperature })
    o2PP.push({ x: new Date(), y: newO2PP })
    o2Concentration.push({ x: new Date(), y: newO2Concentration })
    o2Pressure.push({ x: new Date(), y: newO2Pressure })

    normalized_temperature.push({ x: new Date(), y: newTemperature / max_temperature })
    normalized_o2PP.push({ x: new Date(), y: newO2PP / max_o2PP })
    normalized_o2Concentration.push({ x: new Date(), y: newO2Concentration / max_o2Concentration })
    normalized_o2Pressure.push({ x: new Date(), y: newO2Pressure / max_o2Pressure })

    if (newTemperature >= max_temperature) {
      normalized_temperature = []
      for (const pairs of temperature) {
        normalized_temperature.push({ x: pairs.x, y: (pairs.y - min_temperature) / (newTemperature - min_temperature) })
      }
      this.setState({ max_temperature: newTemperature, normalized_temperature })
    }
    if (newTemperature <= min_temperature) {
      normalized_temperature = []
      for (const pairs of temperature) {
        normalized_temperature.push({ x: pairs.x, y: (pairs.y - newTemperature) / (max_temperature - newTemperature) })
      }
      this.setState({ min_temperature: newTemperature, normalized_temperature })
    }
    if (newO2PP >= max_o2PP) {
      normalized_o2PP = []
      for (const pairs of o2PP) {
        normalized_o2PP.push({ x: pairs.x, y: (pairs.y - min_o2PP) / (newO2PP - min_o2PP) })
      }
      this.setState({ max_o2PP: newO2PP, normalized_o2PP })
    }
    if (newO2PP <= min_o2PP) {
      normalized_o2PP = []
      for (const pairs of o2PP) {
        normalized_o2PP.push({ x: pairs.x, y: (pairs.y - newO2PP) / (max_o2PP - newO2PP) })
      }
      this.setState({ min_o2PP: newO2PP, normalized_o2PP })
    }
    if (newO2Concentration >= max_o2Concentration) {
      normalized_o2Concentration = []
      for (const pairs of o2Concentration) {
        normalized_o2Concentration.push({
          x: pairs.x,
          y: (pairs.y - min_o2Concentration) / (newO2Concentration - min_o2Concentration),
        })
      }
      this.setState({ max_o2Concentration: newO2Concentration, normalized_o2Concentration })
    }
    if (newO2Concentration <= min_o2Concentration) {
      normalized_o2Concentration = []
      for (const pairs of o2Concentration) {
        normalized_o2Concentration.push({
          x: pairs.x,
          y: (pairs.y - newO2Concentration) / (max_o2Concentration - newO2Concentration),
        })
      }
      this.setState({ min_o2Concentration: newO2Concentration, normalized_o2Concentration })
    }
    if (newO2Pressure >= max_o2Pressure) {
      normalized_o2Pressure = []
      for (const pairs of o2Pressure) {
        normalized_o2Pressure.push({ x: pairs.x, y: (pairs.y - min_o2Pressure) / (newO2Pressure - min_o2Pressure) })
      }
      this.setState({ max_o2Pressure: newO2Pressure, normalized_o2Pressure })
    }
    if (newO2Pressure <= min_o2Pressure) {
      normalized_o2Pressure = []
      for (const pairs of o2Pressure) {
        normalized_o2Pressure.push({ x: pairs.x, y: (pairs.y - newO2Pressure) / (max_o2Pressure - newO2Pressure) })
      }
      this.setState({ min_o2Pressure: newO2Pressure, normalized_o2Pressure })
    }

    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  no(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { no } = this.state
    let { normalized_no, min_no, max_no } = this.state
    if (max_no === -1) {
      ;[max_no] = data
    }
    if (min_no === -1) {
      ;[min_no] = data
    }

    no.push({ x: new Date(), y: data[0] })
    normalized_no.push({ x: new Date(), y: data[0] / max_no })

    if (data[0] >= max_no) {
      normalized_no = []
      for (const pairs of no) {
        normalized_no.push({ x: pairs.x, y: (pairs.y - min_no) / (data[0] - min_no) })
      }
      this.setState({ max_no: data[0], normalized_no })
    }
    if (data[0] <= min_no) {
      normalized_no = []
      for (const pairs of no) {
        normalized_no.push({ x: pairs.x, y: (pairs.y - data[0]) / (max_no - data[0]) })
      }
      this.setState({ min_no: data[0], normalized_no })
    }

    this.setState({ no })
  }

  n2o(data: any): void {
    if (data[0] === 0) {
      return
    }
    const { n2o } = this.state
    let { normalized_n2o, min_n2o, max_n2o } = this.state
    if (max_n2o === -1) {
      ;[max_n2o] = data
    }
    if (min_n2o === -1) {
      ;[min_n2o] = data
    }

    n2o.push({ x: new Date(), y: data[0] })
    normalized_n2o.push({ x: new Date(), y: data[0] / max_n2o })

    if (data[0] >= max_n2o) {
      normalized_n2o = []
      for (const pairs of n2o) {
        normalized_n2o.push({ x: pairs.x, y: (pairs.y - min_n2o) / (data[0] - min_n2o) })
      }
      this.setState({ max_n2o: data[0], normalized_n2o })
    }
    if (data[0] <= min_n2o) {
      normalized_n2o = []
      for (const pairs of n2o) {
        normalized_n2o.push({ x: pairs.x, y: (pairs.y - data[0]) / (max_n2o - data[0]) })
      }
      this.setState({ min_n2o: data[0], normalized_n2o })
    }

    n2o.push({ x: new Date(), y: data[0] })
    this.setState({ n2o })
  }

  clearData(): void {
    this.setState({
      methane: [],
      co2: [],
      temperature: [],
      o2PP: [],
      o2Concentration: [],
      o2Pressure: [],
      no: [],
      n2o: [],
      normalized_methane: [],
      normalized_co2: [],
      normalized_temperature: [],
      normalized_o2PP: [],
      normalized_o2Concentration: [],
      normalized_o2Pressure: [],
      normalized_no: [],
      normalized_n2o: [],
      max_methane: -1,
      max_co2: -1,
      max_temperature: -1,
      max_o2PP: -1,
      max_o2Concentration: -1,
      max_o2Pressure: -1,
      max_no: -1,
      max_n2o: -1,
      min_methane: -1,
      min_co2: -1,
      min_temperature: -1,
      min_o2PP: -1,
      min_o2Concentration: -1,
      min_o2Pressure: -1,
      min_no: -1,
      min_n2o: -1,
    })
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
            {crosshairValues.has("Methane") && (
              <p style={{ backgroundColor: "white" }}>
                Methane:{" "}
                {crosshairValues.get("Methane")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                %
              </p>
            )}
            {crosshairValues.has("CO2") && (
              <p style={{ backgroundColor: "white" }}>
                CO2:{" "}
                {crosshairValues.get("CO2")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {crosshairValues.has("Temperature") && (
              <p style={{ backgroundColor: "white" }}>
                Temperature:{" "}
                {crosshairValues.get("Temperature")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                &#176;C
              </p>
            )}
            {crosshairValues.has("O2PP") && (
              <p style={{ backgroundColor: "white" }}>
                O2PP:{" "}
                {crosshairValues.get("O2PP")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {crosshairValues.has("O2Concentration") && (
              <p style={{ backgroundColor: "white" }}>
                O2Concentration:{" "}
                {crosshairValues.get("O2Concentration")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {crosshairValues.has("O2Pressure") && (
              <p style={{ backgroundColor: "white" }}>
                O2Pressure:{" "}
                {crosshairValues.get("O2Pressure")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {crosshairValues.has("NO") && (
              <p style={{ backgroundColor: "white" }}>
                NO:{" "}
                {crosshairValues.get("NO")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {crosshairValues.has("N2O") && (
              <p style={{ backgroundColor: "white" }}>
                N2O:{" "}
                {crosshairValues.get("N2O")?.y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
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
                this.no([e.pageX])
              }}
            >
              TestNO
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
            <button type="button" onClick={this.clearData}>
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
            {this.state.enabledSensors.get("Methane") && this.state.methane !== [] && (
              <LineSeries
                data={this.state.normalized_methane}
                style={{ fill: "none" }}
                strokeWidth="6"
                color="#990000"
                onNearestX={(_datapoint: any, event: any) =>
                  this.onNearestX(event.index, this.state.methane, "Methane")
                }
              />
            )}
            {this.state.enabledSensors.get("CO2") && this.state.co2 !== [] && (
              <LineSeries
                data={this.state.normalized_co2}
                style={{ fill: "none" }}
                strokeWidth="6"
                strokeStyle="dashed"
                color="orange"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, this.state.co2, "CO2")}
              />
            )}
            {this.state.enabledSensors.get("Temperature") && this.state.temperature !== [] && (
              <LineSeries
                data={this.state.normalized_temperature}
                style={{ fill: "none" }}
                strokeWidth="6"
                color="yellow"
                onNearestX={(_datapoint: any, event: any) =>
                  this.onNearestX(event.index, this.state.temperature, "Temperature")
                }
              />
            )}
            {this.state.enabledSensors.get("O2PP") && this.state.o2PP !== [] && (
              <LineSeries
                data={this.state.normalized_o2PP}
                style={{ fill: "none" }}
                strokeWidth="6"
                strokeStyle="dashed"
                color="green"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, this.state.o2PP, "O2PP")}
              />
            )}
            {this.state.enabledSensors.get("O2Concentration") && this.state.o2Concentration !== [] && (
              <LineSeries
                data={this.state.normalized_o2Concentration}
                style={{ fill: "none" }}
                strokeWidth="6"
                color="blue"
                onNearestX={(_datapoint: any, event: any) =>
                  this.onNearestX(event.index, this.state.o2Concentration, "O2Concentration")
                }
              />
            )}
            {this.state.enabledSensors.get("O2Pressure") && this.state.o2Pressure !== [] && (
              <LineSeries
                data={this.state.normalized_o2Pressure}
                style={{ fill: "none" }}
                strokeWidth="6"
                strokeStyle="dashed"
                color="purple"
                onNearestX={(_datapoint: any, event: any) =>
                  this.onNearestX(event.index, this.state.o2Pressure, "O2Pressure")
                }
              />
            )}
            {this.state.enabledSensors.get("NO") && this.state.no !== [] && (
              <LineSeries
                data={this.state.normalized_no}
                style={{ fill: "none" }}
                strokeWidth="6"
                color="black"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, this.state.no, "NO")}
              />
            )}
            {this.state.enabledSensors.get("N2O") && this.state.n2o !== [] && (
              <LineSeries
                data={this.state.normalized_n2o}
                style={{ fill: "none" }}
                strokeWidth="6"
                strokeStyle="dashed"
                color="gray"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index, this.state.n2o, "N2O")}
              />
            )}
            <XAxis />
            <YAxis />
            {this.crosshair()}
          </XYPlot>
          <DiscreteColorLegend
            style={{ fontSize: "16px", textAlign: "center" }}
            items={[
              {
                title: "Methane",
                strokeWidth: 6,
                color: "#990000",
                disabled: !this.state.enabledSensors.get("Methane"),
              },
              {
                title: "CO2",
                strokeWidth: 6,
                strokeStyle: "dashed",
                color: "orange",
                disabled: !this.state.enabledSensors.get("CO2"),
              },
              {
                title: "Temperature",
                strokeWidth: 6,
                color: "yellow",
                disabled: !this.state.enabledSensors.get("Temperature"),
              },
              {
                title: "O2PP",
                strokeWidth: 6,
                strokeStyle: "dashed",
                color: "green",
                disabled: !this.state.enabledSensors.get("O2PP"),
              },
              {
                title: "O2Concentration",
                strokeWidth: 6,
                color: "blue",
                disabled: !this.state.enabledSensors.get("O2Concentration"),
              },
              {
                title: "O2Pressure",
                strokeWidth: 6,
                strokeStyle: "dashed",
                color: "purple",
                disabled: !this.state.enabledSensors.get("O2Pressure"),
              },
              { title: "NO", strokeWidth: 6, color: "black", disabled: !this.state.enabledSensors.get("NO") },
              {
                title: "N2O",
                strokeWidth: 6,
                strokeStyle: "dashed",
                color: "gray",
                disabled: !this.state.enabledSensors.get("N2O"),
              },
            ]}
            orientation="horizontal"
          />
        </div>
      </div>
    )
  }
}

export default SensorGraphs
