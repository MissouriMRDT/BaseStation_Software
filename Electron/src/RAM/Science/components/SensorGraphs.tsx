/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries, DiscreteColorLegend, Crosshair } from "react-vis"

import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import { windows } from "../../../Core/Window"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
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
const row: CSS.Properties = {
  display: "flex",
  alignItems: "center",
  justifyContent: "space-around",
  margin: "10px",
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
}
const overlay: CSS.Properties = {
  width: "200px",
  color: "black",
}

function downloadURL(imgData: string): void {
  const a = document.createElement("a")
  a.href = imgData.replace("image/png", "image/octet-stream")
  a.download = "graph.png"
  a.click()
}

function saveImage(): void {
  let graph
  let thisWindow
  for (const win of Object.keys(windows)) {
    if (windows[win].document.getElementById("SensorGraph")) {
      thisWindow = windows[win]
      graph = thisWindow.document.getElementById("SensorGraph")
      break
    }
  }

  if (!graph) {
    throw new Error("The element 'SensorGraph' wasn't found")
  }

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
  sensor: string
  crosshairValues: any
}

class SensorGraphs extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
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
      max_methane: 0,
      max_co2: 0,
      max_temperature: 0,
      max_o2PP: 0,
      max_o2Concentration: 0,
      max_o2Pressure: 0,
      max_no: 0,
      max_n2o: 0,
      sensor: "All",
      crosshairValues: {},
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.no = this.no.bind(this)
    this.n2o = this.n2o.bind(this)
    this.sensorChange = this.sensorChange.bind(this)
    this.onNearestX = this.onNearestX.bind(this)
    this.onMouseLeave = this.onMouseLeave.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
    rovecomm.on("NO", (data: any) => this.no(data))
    rovecomm.on("N2O", (data: any) => this.n2o(data))
  }

  onMouseLeave(): void {
    this.setState({ crosshairValues: [] })
  }

  onNearestX(value: any, { index }: any, list: any, listName: string): void {
    this.setState({
      crosshairValues: { ...this.state.crosshairValues, [listName]: list[index] },
    })
  }

  sensorChange(event: { target: { value: string } }): void {
    this.setState({ sensor: event.target.value })
  }

  methane(data: any): void {
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    const { methane, normalized_methane } = this.state
    methane.push({ x: new Date(), y: data[0] })
    normalized_methane.push({ x: new Date(), y: data[0] / this.state.max_methane })

    if (data[0] > this.state.max_methane) {
      for (const pairs of normalized_methane) {
        pairs.y *= this.state.max_methane / data[0]
      }
      this.setState({ max_methane: data[0], normalized_methane })
    }

    this.setState({ methane })
  }

  co2(data: any): void {
    const { co2, normalized_co2 } = this.state
    co2.push({ x: new Date(), y: data[0] })
    normalized_co2.push({ x: new Date(), y: data[0] / this.state.max_co2 })

    if (data[0] > this.state.max_co2) {
      for (const pairs of normalized_co2) {
        pairs.y *= this.state.max_co2 / data[0]
      }
      this.setState({ max_methane: data[0], normalized_co2 })
    }

    this.setState({ co2 })
  }

  o2(data: any): void {
    const { temperature, o2PP, o2Concentration, o2Pressure } = this.state
    const { normalized_temperature, normalized_o2PP, normalized_o2Concentration, normalized_o2Pressure } = this.state
    const [newTemperature, newO2PP, newO2Concentration, newO2Pressure] = data

    temperature.push({ x: new Date(), y: newTemperature })
    o2PP.push({ x: new Date(), y: newO2PP })
    o2Concentration.push({ x: new Date(), y: newO2Concentration })
    o2Pressure.push({ x: new Date(), y: newO2Pressure })

    normalized_temperature.push({ x: new Date(), y: newTemperature / this.state.max_temperature })
    normalized_o2PP.push({ x: new Date(), y: newO2PP / this.state.max_o2PP })
    normalized_o2Concentration.push({ x: new Date(), y: newO2Concentration / this.state.max_o2Concentration })
    normalized_o2Pressure.push({ x: new Date(), y: newO2Pressure / this.state.max_o2Pressure })

    if (newTemperature > this.state.max_temperature) {
      for (const pairs of normalized_temperature) {
        pairs.y *= this.state.max_temperature / newTemperature
      }
      this.setState({ max_temperature: newTemperature, normalized_temperature })
    }
    if (newO2PP > this.state.max_o2PP) {
      for (const pairs of normalized_o2PP) {
        pairs.y *= this.state.max_o2PP / newO2PP
      }
      this.setState({ max_o2PP: newO2PP, normalized_o2PP })
    }
    if (newO2Concentration > this.state.max_o2Concentration) {
      for (const pairs of normalized_o2Concentration) {
        pairs.y *= this.state.max_o2Concentration / newO2Concentration
      }
      this.setState({ max_o2Concentration: newO2Concentration, normalized_o2Concentration })
    }
    if (newO2Pressure > this.state.max_o2Pressure) {
      for (const pairs of normalized_o2Pressure) {
        pairs.y *= this.state.max_o2Pressure / newO2Pressure
      }
      this.setState({ max_o2Pressure: newO2Pressure, normalized_o2Pressure })
    }

    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  no(data: any): void {
    const { no, normalized_no } = this.state
    no.push({ x: new Date(), y: data[0] })
    normalized_no.push({ x: new Date(), y: data[0] / this.state.max_no })

    if (data[0] > this.state.max_no) {
      for (const pairs of normalized_no) {
        pairs.y *= this.state.max_no / data[0]
      }
      this.setState({ max_methane: data[0], normalized_no })
    }

    this.setState({ no })
  }

  n2o(data: any): void {
    const { n2o, normalized_n2o } = this.state
    normalized_n2o.push({ x: new Date(), y: data[0] / this.state.max_n2o })

    if (data[0] > this.state.max_no) {
      for (const pairs of normalized_n2o) {
        pairs.y *= this.state.max_no / data[0]
      }
      this.setState({ max_methane: data[0], normalized_n2o })
    }

    n2o.push({ x: new Date(), y: data[0] })
    this.setState({ n2o })
  }

  crosshair(): JSX.Element | null {
    let time
    for (const reading of this.state.crosshairValues) {
      if (reading !== undefined) {
        time = reading.x
        break
      }
    }
    console.log(this.state.crosshairValues)
    if (time) {
      return (
        <Crosshair values={this.state.crosshairValues}>
          <div style={overlay}>
            <h3>{time?.toTimeString().slice(0, 9)}</h3>
            {this.state.crosshairValues[0] !== undefined && (
              <p>
                Methane:{" "}
                {this.state.crosshairValues[0].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                %
              </p>
            )}
            {this.state.crosshairValues[1] !== undefined && (
              <p>
                CO2:{" "}
                {this.state.crosshairValues[1].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {this.state.crosshairValues[2] !== undefined && (
              <p>
                Temperature:{" "}
                {this.state.crosshairValues[2].y.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                &#176;C
              </p>
            )}
            {this.state.crosshairValues[3] !== undefined && (
              <p>
                O2PP:{" "}
                {this.state.crosshairValues[3].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {this.state.crosshairValues[4] !== undefined && (
              <p>
                O2Concentration:{" "}
                {this.state.crosshairValues[4].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {this.state.crosshairValues[5] !== undefined && (
              <p>
                O2Pressure:{" "}
                {this.state.crosshairValues[5].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {this.state.crosshairValues[6] !== undefined && (
              <p>
                NO:{" "}
                {this.state.crosshairValues[6].y.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </p>
            )}
            {this.state.crosshairValues[7] !== undefined && (
              <p>
                N2O:{" "}
                {this.state.crosshairValues[7].y.toLocaleString(undefined, {
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
          <div style={row}>
            <div style={selectbox}>
              <div style={h1Style}>Sensor:</div>
              <select value={this.state.sensor} onChange={e => this.sensorChange(e)} style={selector}>
                {["All", "Methane", "CO2", "Temperature", "O2PP", "O2Concentration", "O2Pressure", "NO", "N2O"].map(
                  item => {
                    return (
                      <option key={item} value={item}>
                        {item}
                      </option>
                    )
                  }
                )}
              </select>
            </div>
          </div>
          <DiscreteColorLegend
            style={{ fontSize: "16px", textAlign: "center" }}
            items={[
              { title: "Methane", strokeWidth: 6, color: "#990000" },
              { title: "CO2", strokeWidth: 6, strokeStyle: "dashed", color: "orange" },
              { title: "Temperature", strokeWidth: 6, color: "yellow" },
              { title: "O2PP", strokeWidth: 6, strokeStyle: "dashed", color: "green" },
              { title: "O2Concentration", strokeWidth: 6, color: "blue" },
              { title: "O2Pressure", strokeWidth: 6, strokeStyle: "dashed", color: "purple" },
              { title: "NO", strokeWidth: 6, color: "black" },
              { title: "N2O", strokeWidth: 6, strokeStyle: "dashed", color: "gray" },
            ]}
            orientation="horizontal"
          />
          <XYPlot
            style={{ margin: 10 }}
            width={window.document.documentElement.clientWidth - 50}
            height={300}
            xType="time"
            onMouseLeave={this.onMouseLeave}
          >
            <HorizontalGridLines style={{ fill: "none" }} />
            {(this.state.sensor === "Methane" || this.state.sensor === "All") && this.state.methane !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_methane : this.state.methane}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) =>
                  this.onNearestX(datapoint, event, this.state.methane, "Methane")
                }
              />
            )}
            {(this.state.sensor === "CO2" || this.state.sensor === "All") && this.state.co2 !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_co2 : this.state.co2}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) => this.onNearestX(datapoint, event, this.state.co2, "CO2")}
              />
            )}
            {(this.state.sensor === "Temperature" || this.state.sensor === "All") && this.state.temperature !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_temperature : this.state.temperature}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) =>
                  this.onNearestX(datapoint, event, this.state.temperature, "Temperature")
                }
              />
            )}
            {(this.state.sensor === "O2PP" || this.state.sensor === "All") && this.state.o2PP !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_o2PP : this.state.o2PP}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) => this.onNearestX(datapoint, event, this.state.o2PP, "O2PP")}
              />
            )}
            {(this.state.sensor === "O2Concentration" || this.state.sensor === "All") &&
              this.state.o2Concentration !== [] && (
                <LineSeries
                  data={
                    this.state.sensor === "All" ? this.state.normalized_o2Concentration : this.state.o2Concentration
                  }
                  style={{ fill: "none" }}
                  onNearestX={(datapoint: any, event: any) =>
                    this.onNearestX(datapoint, event, this.state.o2Concentration, "O2Concentration")
                  }
                />
              )}
            {(this.state.sensor === "O2Pressure" || this.state.sensor === "All") && this.state.o2Pressure !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_o2Pressure : this.state.o2Pressure}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) =>
                  this.onNearestX(datapoint, event, this.state.o2Pressure, "O2Pressure")
                }
              />
            )}
            {(this.state.sensor === "NO" || this.state.sensor === "All") && this.state.no !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_no : this.state.no}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) => this.onNearestX(datapoint, event, this.state.no, "NO")}
              />
            )}
            {(this.state.sensor === "N2O" || this.state.sensor === "All") && this.state.n2o !== [] && (
              <LineSeries
                data={this.state.sensor === "All" ? this.state.normalized_n2o : this.state.n2o}
                style={{ fill: "none" }}
                onNearestX={(datapoint: any, event: any) => this.onNearestX(datapoint, event, this.state.n2o, "N2O")}
              />
            )}
            <XAxis />
            {this.state.sensor !== "All" && <YAxis />}
            {this.crosshair()}
          </XYPlot>
          <div style={row}>
            <button type="button" onClick={saveImage}>
              Export Graph
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default SensorGraphs
