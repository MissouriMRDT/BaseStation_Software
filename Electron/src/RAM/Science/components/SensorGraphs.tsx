import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries, DiscreteColorLegend, Crosshair } from "react-vis"

import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

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
  console.log("Save")
  const input = document.getElementById("canvas")
  if (!input) {
    throw new Error("The element 'canvas' wasn't found")
  }
  html2canvas(input, {
    scrollX: 0,
    scrollY: -window.scrollY,
  })
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
  sensor: string
  crosshairValues: { x: Date; y: number }[]
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
      sensor: "All",
      crosshairValues: [],
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.sensorChange = this.sensorChange.bind(this)
    this.onNearestX = this.onNearestX.bind(this)
    this.onMouseLeave = this.onMouseLeave.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
  }

  onMouseLeave(): void {
    this.setState({ crosshairValues: [] })
  }

  onNearestX(value, { index }): void {
    this.setState({
      crosshairValues: [
        this.state.methane,
        this.state.co2,
        this.state.temperature,
        this.state.o2PP,
        this.state.o2Concentration,
        this.state.o2Pressure,
      ].map(d => d[index]),
    })
  }

  sensorChange(event: { target: { value: string } }): void {
    this.setState({ sensor: event.target.value })
  }

  methane(data: any): void {
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    const { methane } = this.state
    methane.push({ x: new Date(), y: data[0] })
    this.setState({ methane })
  }

  co2(data: any): void {
    const { co2 } = this.state
    co2.push({ x: new Date(), y: data[0] })
    this.setState({ co2 })
  }

  o2(data: any): void {
    const { temperature, o2PP, o2Concentration, o2Pressure } = this.state
    const [newTemperature, newO2PP, newO2Concentration, newO2Pressure] = data
    temperature.push({ x: new Date(), y: newTemperature })
    o2PP.push({ x: new Date(), y: newO2PP })
    o2Concentration.push({ x: new Date(), y: newO2Concentration })
    o2Pressure.push({ x: new Date(), y: newO2Pressure })
    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  crosshair(): JSX.Element | null {
    if (
      this.state.crosshairValues.length === 6 &&
      "x" in this.state.crosshairValues[0] &&
      "y" in this.state.crosshairValues[0]
    ) {
      return (
        <Crosshair values={this.state.crosshairValues}>
          <div style={overlay}>
            <h3>{this.state.crosshairValues[0].x.toTimeString().slice(0, 9)}</h3>
            <p>
              Methane:{" "}
              {this.state.crosshairValues[0].y.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{" "}
              ppm
            </p>
            <p>
              CO2:{" "}
              {this.state.crosshairValues[1].y.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{" "}
              ppm
            </p>
            <p>
              Temperature:{" "}
              {this.state.crosshairValues[2].y.toLocaleString(undefined, {
                minimumFractionDigits: 1,
                minimumIntegerDigits: 2,
              })}
              &#176;C
            </p>
            <p>
              O2PP:{" "}
              {this.state.crosshairValues[3].y.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{" "}
              ppm
            </p>
            <p>
              O2Concentration:{" "}
              {this.state.crosshairValues[4].y.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{" "}
              ppm
            </p>
            <p>
              O2Pressure:{" "}
              {this.state.crosshairValues[5].y.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{" "}
              ppm
            </p>
          </div>
        </Crosshair>
      )
    }
    return null
  }

  render(): JSX.Element {
    return (
      <div id="canvas" style={this.props.style}>
        <div style={label}>Sensor Graphs</div>
        <div style={container}>
          <div style={row}>
            <div style={selectbox}>
              <div style={h1Style}>Sensor:</div>
              <select value={this.state.sensor} onChange={e => this.sensorChange(e)} style={selector}>
                {["All", "Methane", "CO2", "Temperature", "O2PP", "O2Concentration", "O2Pressure"].map(item => {
                  return (
                    <option key={item} value={item}>
                      {item}
                    </option>
                  )
                })}
              </select>
            </div>
          </div>
          <DiscreteColorLegend
            style={{ fontSize: "16px", textAlign: "center" }}
            items={[
              { title: "Methane", strokeWidth: 6 },
              { title: "CO2", strokeWidth: 6 },
              { title: "Temperature", strokeWidth: 6 },
              { title: "O2PP", strokeWidth: 6 },
              { title: "O2Concentration", strokeWidth: 6 },
              { title: "O2Pressure", strokeWidth: 6 },
            ]}
            orientation="horizontal"
          />
          <XYPlot
            style={{ margin: 10 }}
            width={window.innerWidth - 50}
            height={300}
            xType="time"
            onMouseLeave={this.onMouseLeave}
          >
            <HorizontalGridLines style={{ fill: "none" }} />
            {(this.state.sensor === "Methane" || this.state.sensor === "All") && this.state.methane !== [] && (
              <LineSeries data={this.state.methane} style={{ fill: "none" }} onNearestX={this.onNearestX} />
            )}
            {(this.state.sensor === "CO2" || this.state.sensor === "All") && this.state.co2 !== [] && (
              <LineSeries data={this.state.co2} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "Temperature" || this.state.sensor === "All") && this.state.temperature !== [] && (
              <LineSeries data={this.state.temperature} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "O2PP" || this.state.sensor === "All") && this.state.o2PP !== [] && (
              <LineSeries data={this.state.o2PP} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "O2Concentration" || this.state.sensor === "All") &&
              this.state.o2Concentration !== [] && (
                <LineSeries data={this.state.o2Concentration} style={{ fill: "none" }} />
              )}
            {(this.state.sensor === "O2Pressure" || this.state.sensor === "All") && this.state.o2Pressure !== [] && (
              <LineSeries data={this.state.o2Pressure} style={{ fill: "none" }} />
            )}
            <XAxis />
            <YAxis />
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
