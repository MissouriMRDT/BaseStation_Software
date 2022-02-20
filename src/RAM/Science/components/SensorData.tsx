import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"

import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  justifyContent: "center",
  height: "calc(100% - 40px)",
  padding: "5px",
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
  outline: "none",
}
const grid: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "repeat(2, 1fr)",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  methane: number
  temperature: number
  co2: number
  o2: number
  ch3: number
  no2: number

  writeToFile: boolean
  sensorSaveFile: string | null
  fileWriteInterval: NodeJS.Timeout | null
}

class SensorData extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      methane: 0,
      temperature: 0,
      co2: 0,
      o2: 0,
      ch3: 0,
      no2: 0,
      writeToFile: false,
      sensorSaveFile: null,
      fileWriteInterval: null,
    }
    this.methane = this.methane.bind(this)
    this.co2 = this.co2.bind(this)
    this.o2 = this.o2.bind(this)
    this.no2 = this.no2.bind(this)
    this.ch3 = this.ch3.bind(this)
    this.fileWrite = this.fileWrite.bind(this)
    this.fileStart = this.fileStart.bind(this)
    this.fileStop = this.fileStop.bind(this)

    rovecomm.on("Methane", (data: any) => this.methane(data))
    rovecomm.on("CO2", (data: any) => this.co2(data))
    rovecomm.on("O2", (data: any) => this.o2(data))
    // LIKELY CHANGE AS CH3 IS AN ION AND PROBABLY NOT WHAT WE'RE MEASURING
    rovecomm.on("CH3", (data: any) => this.ch3(data))
    rovecomm.on("NO2", (data: any) => this.no2(data))
  }

  methane(data: any): void {
    const [methane, temperature] = data
    this.setState({ methane, temperature })
  }

  co2(data: any): void {
    this.setState({ co2: data[0] })
  }

  o2(data: any): void {
    this.setState({ o2: data[0] })
  }

  no2(data: any): void {
    this.setState({ no2: data[0] })
  }

  ch3(data: any): void {
    this.setState({ ch3: data[0] })
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
    fs.appendFile(filestream, "time,methane,temp,co2,o2,ch3,no2\n", err => {
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
        // time,methane,temp,co2,o2,ch3,no2
        `${new Date().toLocaleDateString()},${this.state.methane},${this.state.temperature},${this.state.co2},${
          this.state.o2
        },${this.state.ch3},${this.state.no2}\n`,
        err => {
          if (err) throw err
        }
      )
    }
  }

  render(): JSX.Element {
    return (
      <div id={"SensorData"} style={this.props.style}>
        <div style={label}>Sensor Data</div>
        <div style={container}>
            <div style={row}>
              <div>Methane Concentration:</div>
              <div>
                {this.state.methane.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                %
              </div>
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
              <div>CO2 Concentration:</div>
              <div>
                {this.state.co2.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </div>
            </div>
            <div style={row}>
              <div>O2 Concentration:</div>
              <div>
                {this.state.o2.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </div>
            </div>
            <div style={row}>
              <div>CH3 Concentration:</div>
              <div>
                {this.state.ch3.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </div>
            </div>
            <div style={row}>
              <div>NO2 Volume:</div>
              <div>
                {this.state.no2.toLocaleString(undefined, {
                  minimumFractionDigits: 2,
                })}{" "}
                ppm
              </div>
            </div>
            <div style={row}></div>
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
