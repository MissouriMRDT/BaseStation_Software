import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"

import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

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
  justifyContent: "space-between",
  margin: "5px 25px",
}
const grid: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "repeat(2, 1fr)",
}

interface IProps {}

interface IState {
  methane: number
  co2: number
  temperature: number
  o2PP: number
  o2Concentration: number
  o2Pressure: number
}

class SensorData extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      methane: 0,
      co2: 0,
      temperature: 0,
      o2PP: 0,
      o2Concentration: 0,
      o2Pressure: 0,
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
  }

  methane(data: any): void {
    // the methane data packet is [methane concentration, temperature]
    // temperature is discarded since it is supplied from the O2 sensor as well
    this.setState({ methane: data[0] })
  }

  co2(data: any): void {
    this.setState({ co2: data })
  }

  o2(data: any): void {
    const [temperature, o2PP, o2Concentration, o2Pressure] = data
    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  render(): JSX.Element {
    return (
      <div id="canvas">
        <div style={label}>Sensor Graphs</div>
        <div style={container}>
          <div style={grid}>
            <div style={row}>
              <div>Methane Concentration:</div>
              <div>
                {this.state.methane.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </div>
            </div>
            <div style={row}>
              <div>CO2 Concentration:</div>
              <div>{this.state.co2.toLocaleString(undefined, { minimumFractionDigits: 2 })} ppm</div>
            </div>
            <div style={row}>
              <div>Temperature:</div>
              <div>
                {this.state.temperature.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                &#176;C{" "}
              </div>
            </div>
            <div style={row}>
              <div>O2 Partial Pressure:</div>
              <div>{this.state.o2PP.toLocaleString(undefined, { minimumFractionDigits: 2 })} mBar</div>
            </div>
            <div style={row}>
              <div>O2 Concentration:</div>
              <div>{this.state.o2Concentration.toLocaleString(undefined, { minimumFractionDigits: 2 })} ppm</div>
            </div>
            <div style={row}>
              <div>O2 Barometric Pressure:</div>
              <div>{this.state.o2Pressure.toLocaleString(undefined, { minimumFractionDigits: 2 })} mBar</div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default SensorData
