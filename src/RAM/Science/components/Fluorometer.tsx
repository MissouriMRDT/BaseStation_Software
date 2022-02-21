import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import fs from "fs"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
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
  flexDirection: "row",
  flexGrow: 1,
  justifyContent: "space-around",
  alignContent: "center",
  marginTop: "5px",
  width: "100%",
}
const componentBox: CSS.Properties = {
  marginTop: "5px",
  marginBottom: "5px",
}
const button: CSS.Properties = {
  marginLeft: "15px",
  width: "60px",
  alignSelf: "center",
}

const controlButton: CSS.Properties = {
  //margin: "5px",
}

/** Will be merged with the row css if the Laser is off */
const offIndicator: CSS.Properties = {
  backgroundColor: "#FF0000",
}

/** Will be merged with the row css if the Laser is off */
const onIndicator: CSS.Properties = {
  backgroundColor: "#00FF00",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  /** Holds the last sent values of the diodes */
  DiodeValues: number[]
  /** Holds which lasers are enabled */
  LasersPowered: boolean[]
  /** If UV light is on */
  UVPowered: boolean
  /** If White Light is on */
  WhiteLightPowered: boolean
}

class Fluorometer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      DiodeValues: [0, 0, 0],
      LasersPowered: [false, false, false],
      UVPowered: false,
      WhiteLightPowered: false,
    }
    this.updateDiodeVals = this.updateDiodeVals.bind(this)
    this.toggleWhiteLight = this.toggleWhiteLight.bind(this)
    this.toggleUV = this.toggleUV.bind(this)
    this.buildLightCommand = this.buildLightCommand.bind(this)
    this.buildLaserCommand = this.buildLaserCommand.bind(this)
    this.exportData = this.exportData.bind(this)

    rovecomm.on("FluorometerData", (data: any) => this.updateDiodeVals(data))
  }

  /**
   * Updates the wavelengths received from the Rover.
   * @param data float array of length 3 with the new data
   */
  updateDiodeVals(data: number[]): void {
    this.setState({ DiodeValues: [data[0], data[1], data[2]] })
  }

  buildLightCommand(UV: boolean, White: boolean): number {
    let bitmask = ""
    bitmask += UV ? "1" : "0"
    bitmask += White ? "1" : "0"
    return parseInt(bitmask, 2)
  }

  buildLaserCommand(Lasers: boolean[]): number {
    let bitmask = ""
    bitmask += Lasers[0] ? "1" : "0"
    bitmask += Lasers[1] ? "1" : "0"
    bitmask += Lasers[2] ? "1" : "0"
    return parseInt(bitmask, 2)
  }

  toggleWhiteLight(): void {
    this.setState({ WhiteLightPowered: !this.state.WhiteLightPowered }, () => {
      rovecomm.sendCommand("Lights", [this.buildLightCommand(this.state.UVPowered, this.state.WhiteLightPowered)])
    })
  }

  toggleUV(): void {
    this.setState({ UVPowered: !this.state.UVPowered }, () => {
      rovecomm.sendCommand("Lights", [this.buildLightCommand(this.state.UVPowered, this.state.WhiteLightPowered)])
    })
  }

  toggleLaser(index: number): void {
    const { LasersPowered } = this.state
    LasersPowered[index] = !LasersPowered[index]
    rovecomm.sendCommand("FLasers", [this.buildLaserCommand(LasersPowered)])
    this.setState({ LasersPowered })
  }

  exportData(): void {
    // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
    // Date format is consistent with the SensorData csv
    const timestamp = new Date().toISOString().replaceAll(/[:\-TZ]/g, ".")
    const EXPORT_FILE = `./ScienceSaveFiles/Fluormeter-${timestamp}.csv`

    const { DiodeValues, LasersPowered, UVPowered, WhiteLightPowered } = this.state

    if (!fs.existsSync("./ScienceSaveFiles")) {
      fs.mkdirSync("./ScienceSaveFiles")
    }
    let csvText = "Laser1,Laser2,Laser3,UV Light,White Light,Diode1 (nm),Diode2 (nm),Diode3 (nm)\n"
    csvText += `${LasersPowered[0] ? "On" : "Off"},${LasersPowered[1] ? "On" : "Off"},${
      LasersPowered[2] ? "On" : "Off"
    },`
    csvText += `${UVPowered ? "On" : "Off"},${WhiteLightPowered ? "On" : "Off"},`
    csvText += `${DiodeValues[0]},${DiodeValues[1]},${DiodeValues[2]},\n`

    fs.writeFile(EXPORT_FILE, csvText, err => {
      if (err) throw err
    })
  }

  render(): JSX.Element {
    return (
      <div id="Flurometer" style={this.props.style}>
        <div style={label}>Fluorometer</div>
        <div style={container}>
          <div style={row}>
            <div>
              <input type="checkbox" id="WhiteCheck" name="WhiteCheck" onChange={() => this.toggleWhiteLight()} checked={this.state.WhiteLightPowered}/>
              <label htmlFor="WhiteCheck">White Light</label>
            </div>
            <div>
              <input type="checkbox" id="UVCheck" name="UVChech" onChange={() => this.toggleUV()} checked={this.state.UVPowered}/>
              <label htmlFor="UVCheck">UV Light</label>
            </div>
          </div>
          <div style={componentBox}>
            {this.state.DiodeValues.map((value, index) => {
              return (
                <div key={index} style={row}>
                  <label>
                    Diode {index + 1}: {value.toFixed(3)} nm
                  </label>
                </div>
              )
            })}
          </div>
          <div style={componentBox}>
            {this.state.LasersPowered.map((value, index) => {
              return (
                <div key={index} style={{ ...row, ...(value ? onIndicator : offIndicator) }}>
                  <label style={{ alignSelf: "center", fontWeight: "bold" }}> Laser {index + 1}: </label>
                  <button style={button} onClick={() => this.toggleLaser(index)}>
                    {value ? "Disable" : "Enable"}
                  </button>
                </div>
              )
            })}
          </div>
          <div style={componentBox}>
            <div style={row}>
              <button style={controlButton} onClick={() => this.exportData()}>
                Export
              </button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Fluorometer