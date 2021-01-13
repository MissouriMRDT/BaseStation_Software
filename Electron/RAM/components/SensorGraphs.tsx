import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import {
  XYPlot,
  XAxis,
  YAxis,
  HorizontalGridLines,
  LineSeries,
  DiscreteColorLegend,
} from "react-vis"

import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "640px",
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
      const imgData = canvas
        .toDataURL("image/png")
        .replace("image/png", "image/octet-stream")
      downloadURL(imgData)
      return null
    })
    .catch((error: any) => {
      console.error(error)
    })
}

interface IProps {}

interface IState {
  methane: { x: Date; y: number }[]
  co2: { x: Date; y: number }[]
  temperature: { x: Date; y: number }[]
  o2PP: { x: Date; y: number }[]
  o2Concentration: { x: Date; y: number }[]
  o2Pressure: { x: Date; y: number }[]
  sensor: string
}

class SensorGraphs extends Component<IProps, IState> {
  mounted = false

  constructor(props: IProps) {
    super(props)
    this.state = {
      methane: [
        { x: new Date(0), y: 0.0 },
        { x: new Date(1), y: 1.0 },
      ],
      co2: [
        { x: new Date(0), y: 0.1 },
        { x: new Date(1), y: 0.9 },
      ],
      temperature: [
        { x: new Date(0), y: 0.2 },
        { x: new Date(1), y: 0.8 },
      ],
      o2PP: [
        { x: new Date(0), y: 0.3 },
        { x: new Date(1), y: 0.7 },
      ],
      o2Concentration: [
        { x: new Date(0), y: 0.4 },
        { x: new Date(1), y: 0.6 },
      ],
      o2Pressure: [
        { x: new Date(0), y: 0.5 },
        { x: new Date(1), y: 0.5 },
      ],
      sensor: "All",
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.sensorChange = this.sensorChange.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
  }

  componentDidMount(): void {
    // Boolean for if component has mounted or not to determine if
    // this.state = {} or this.setState({}) should be used.
    this.mounted = true
  }

  sensorChange(event: { target: { value: string } }): void {
    this.setState({ sensor: event.target.value })
  }

  methane(data: any): void {
    const { methane } = this.state
    methane.push({ x: new Date(), y: data })
    this.setState({ methane })
  }

  co2(data: any): void {
    const { co2 } = this.state
    co2.push({ x: new Date(), y: data })
    this.setState({ co2 })
  }

  o2(data: any): void {
    const { temperature, o2PP, o2Concentration, o2Pressure } = this.state
    temperature.push({ x: new Date(), y: data })
    o2PP.push({ x: new Date(), y: data })
    o2Concentration.push({ x: new Date(), y: data })
    o2Pressure.push({ x: new Date(), y: data })
    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  render(): JSX.Element {
    console.log(this.state.sensor)
    return (
      <div id="canvas">
        <div style={label}>Sensor Graphs</div>
        <div style={container}>
          <div style={row}>
            <div style={selectbox}>
              <div style={h1Style}>Sensor:</div>
              <select
                value={this.state.sensor}
                onChange={e => this.sensorChange(e)}
                style={selector}
              >
                {[
                  "All",
                  "Methane",
                  "CO2",
                  "Temperature",
                  "O2PP",
                  "O2Concentration",
                  "O2Pressure",
                ].map(item => {
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
          <XYPlot style={{ margin: 10 }} width={620} height={480} xType="time">
            <HorizontalGridLines style={{ fill: "none" }} />
            {(this.state.sensor === "Methane" ||
              this.state.sensor === "All") && (
              <LineSeries data={this.state.methane} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "CO2" || this.state.sensor === "All") && (
              <LineSeries data={this.state.co2} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "Temperature" ||
              this.state.sensor === "All") && (
              <LineSeries
                data={this.state.temperature}
                style={{ fill: "none" }}
              />
            )}
            {(this.state.sensor === "O2PP" || this.state.sensor === "All") && (
              <LineSeries data={this.state.o2PP} style={{ fill: "none" }} />
            )}
            {(this.state.sensor === "O2Concentration" ||
              this.state.sensor === "All") && (
              <LineSeries
                data={this.state.o2Concentration}
                style={{ fill: "none" }}
              />
            )}
            {(this.state.sensor === "O2Pressure" ||
              this.state.sensor === "All") && (
              <LineSeries
                data={this.state.o2Pressure}
                style={{ fill: "none" }}
              />
            )}
            <XAxis />
            <YAxis />
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
