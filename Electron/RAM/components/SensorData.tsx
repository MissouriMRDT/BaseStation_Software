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
const buttonRow: CSS.Properties = {
  display: "flex",
  alignItems: "center",
  justifyContent: "space-around",
  margin: "10px 50px",
}
const buttons: CSS.Properties = {
  lineHeight: "20px",
  fontSize: "16px",
  border: "none",
  backgroundColor: "#990000",
  borderRadius: "5px",
  color: "white",
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

  writeToFile: boolean
  sensorSaveFile: string | null
  fileWriteInterval: NodeJS.Timeout | null
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
      writeToFile: false,
      sensorSaveFile: null,
      fileWriteInterval: null,
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.fileWrite = this.fileWrite.bind(this)
    this.fileStart = this.fileStart.bind(this)
    this.fileStop = this.fileStop.bind(this)

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
    this.setState({ co2: data[0] })
  }

  o2(data: any): void {
    const [temperature, o2PP, o2Concentration, o2Pressure] = data
    this.setState({ temperature, o2PP, o2Concentration, o2Pressure })
  }

  fileStart(): void {
    const filestream = `./ScienceSaveFiles/${new Date()
      .toISOString()
      // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
      // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
      .replaceAll(/[:\-TZ]/g, ".")}csv`
    if (!fs.existsSync("./ScienceSaveFiles")) {
      fs.mkdirSync("./ScienceSaveFiles")
    }
    fs.open(filestream, "w", err => {
      if (err) throw err
    })
    fs.appendFile(filestream, "time,methane,co2,temp,o2PP,o2Concentration,o2Pressure\n", err => {
      if (err) throw err
    })
    const fileWriteInterval = setInterval(this.fileWrite, 1000)
    this.setState({
      sensorSaveFile: filestream,
      writeToFile: true,
      fileWriteInterval,
    })
  }

  fileStop(): void {
    if (this.state.fileWriteInterval) {
      clearInterval(this.state.fileWriteInterval)
    }
    this.setState({
      writeToFile: false,
      sensorSaveFile: null,
      fileWriteInterval: null,
    })
  }

  fileWrite(): void {
    if (this.state.writeToFile && this.state.sensorSaveFile) {
      fs.appendFile(
        this.state.sensorSaveFile,
        // time,methane,co2,temp,o2PP,o2Concentration,o2Pressure\n
        `${new Date().toLocaleDateString()},${this.state.methane},${this.state.co2},${this.state.temperature},${
          this.state.o2PP
        },${this.state.o2Concentration},${this.state.o2Pressure}\n`,
        err => {
          if (err) throw err
        }
      )
    }
  }

  render(): JSX.Element {
    return (
      <div id="canvas">
        <div style={label}>Sensor Data</div>
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
          <div style={buttonRow}>
            <div>Save Sensor Data</div>
            <button type="button" onClick={this.fileStart} style={buttons}>
              Start
            </button>
            <button type="button" onClick={this.fileStop} style={buttons}>
              Stop
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default SensorData
