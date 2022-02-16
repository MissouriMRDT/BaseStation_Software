import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { ColorStyleConverter } from "../../Core/ColorConverter"
import { BitmaskUnpack } from "../../Core/BitmaskUnpack"
import path from "path"

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
const textPad: CSS.Properties = {
  paddingLeft: "10px",
  paddingRight: "10px",
}
const mainContainer: CSS.Properties = {
  display: "flex",
  flexWrap: "wrap",
  flexDirection: "column",
  flexGrow: 1,
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
  fontSize: "10px",
  lineHeight: "10px",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 2,
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 2,
}
const readoutContainter: CSS.Properties = {
  height: "401px",
  flexWrap: "wrap",
  margin: "1px",
}
const readout: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 1,
  justifyContent: "space-between",
  fontFamily: "arial",
}
const btnArray: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "105px 130px 105px",
  justifyContent: "center",
  gridTemplateRows: "25px",
}
const cellReadoutContainer: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
}
const btnStyle: CSS.Properties = {
  width: "70px",
  cursor: "pointer",
}

let RovecommManifest: any = {}
const FILEPATH = path.join(__dirname, "../assets/RovecommManifest.json")
if (fs.existsSync(FILEPATH)) {
  RovecommManifest = JSON.parse(fs.readFileSync(FILEPATH).toString()).RovecommManifest
}

/**
 * The specific function of turnOffReboot() originates from how the control boards
 * on the rover are programmed. If turnOffReboot() gets passed "0", the rover turns off.
 * If "time" is any positive number, the rover just powercycles for that many seconds.
 * It's been determined that 5 seconds is long enough for a standard powercycle as any longer
 * is mostly redundent and would cut into the allowed time during competition, and any shorter
 * is probably not enough time. ¯\_(ツ)_/¯
 * @param time if time = 0, Rover turns off. Otherwise it power cycles for 'time' seconds
 */
function turnOffReboot(time: number): void {
  rovecomm.sendCommand("BMSStop", [time])
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  boardTelemetry: any
  batteryTelemetry: any
}

class Power extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    const { Power, BMS } = RovecommManifest
    const boardTelemetry = {}
    const batteryTelemetry = {}
    Object.keys(Power.Commands).forEach((Bus: string, index: any) => {
      boardTelemetry[Bus] = {}
      Power.Commands[Bus].comments.split(", ").forEach((component: any) => {
        boardTelemetry[Bus][component] = { enabled: true, value: 0 }
      })
    })
    Object.keys(BMS.Telemetry).forEach((meas_group: string, index: any) => {
      batteryTelemetry[meas_group] = {}
      const tmpList = BMS.Telemetry[meas_group].comments.split(", ")
      if (tmpList.length > 1) {
        tmpList.forEach((cell: any) => {
          batteryTelemetry[meas_group][cell] = { value: 0 }
        })
      } else {
        batteryTelemetry[meas_group] = { value: 0 }
      }
    })
    this.state = {
      boardTelemetry,
      batteryTelemetry,
    }
    this.boardListenHandler = this.boardListenHandler.bind(this)
    this.batteryListenHandler = this.batteryListenHandler.bind(this)

    /**
     * bit of awkwardness with the board telemetry listeners:
     * the code that populates the state array for the board telemetry use the "Commands" object from the manifest
     * because the "comments" subobject don't have any duplicates. The "Telemetry" object contains duplicates
     * because there are two separate data types that relate to each physical component. Because the populator
     * pulls from "Commands," though, the name of the object in the State array is the name of the Command object.
     * # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     *   IF YOU ARE EXPERIENCING ERRORS RELATING TO ROVECOMM, MAKE SURE THE LISTENER IS LISTENING FOR TELEMETRY
     *   AND IS APPLYING THAT VALUE TO THE NAME FROM THE COMMANDS OBJECT
     * # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     */
    rovecomm.on("MotorBusEnabled", (data: number[]) => this.boardListenHandler(data, "MotorBusEnable"))
    rovecomm.on("MotorBusCurrent", (data: number[]) => this.boardListenHandler(data, "MotorBusEnable"))
    rovecomm.on("TwelveVActBusEnabled", (data: number[]) => this.boardListenHandler(data, "TwelveVActBusEnable"))
    rovecomm.on("TwelveVActBusCurrent", (data: number[]) => this.boardListenHandler(data, "TwelveVActBusEnable"))
    rovecomm.on("TwelveVLogicBusEnabled", (data: number[]) => this.boardListenHandler(data, "TwelveVLogicBusEnable"))
    rovecomm.on("TwelveVLogicBusCurrent", (data: number[]) => this.boardListenHandler(data, "TwelveVLogicBusEnable"))
    rovecomm.on("ThirtyVEnabled", (data: number[]) => this.boardListenHandler(data, "ThirtyVBusEnable"))
    rovecomm.on("ThirtyVCurrent", (data: number[]) => this.boardListenHandler(data, "ThirtyVBusEnable"))

    rovecomm.on("PackI_Meas", (data: number[]) => this.batteryListenHandler(data, "PackI_Meas"))
    rovecomm.on("PackV_Meas", (data: number[]) => this.batteryListenHandler(data, "PackV_Meas"))
    rovecomm.on("Temp_Meas", (data: number[]) => this.batteryListenHandler(data, "Temp_Meas"))
    rovecomm.on("CellV_Meas", (data: number[]) => this.batteryListenHandler(data, "CellV_Meas"))
    console.log(boardTelemetry)
  }

  /**
   * @desc takes data from rovecomm and applies those values to the state object
   * all rovecomm telemetry values are transported as arrays, even if only a single value
   * is transferred. IF a single value is transferred, the data is assumed to be a bitmask,
   * after which it is unpacked and has the resultant boolean values applied to the 'enabled'
   * properties of each component object. If there is more than one value, the data is assumed
   * to be the list of amperage floats, and assigned to the 'value' property.
   * @param data contains either a single bitmask number or a list of numbers; both inside of an array
   * @param partList the list of components determined by the state object
   */
  boardListenHandler(data: number[], partList: string): void {
    const { boardTelemetry } = this.state
    if (data.length > 1) {
      boardTelemetry[partList].forEach((part: string, index: number) => {
        boardTelemetry[partList][part].value = data[index]
      })
    } else {
      const bitmask = BitmaskUnpack(data[0], partList.length)
      boardTelemetry[partList].forEach((part: string, index: number) => {
        boardTelemetry[partList][part].enabled = Boolean(Number(bitmask[index]))
      })
    }
    // The setState is kept until the end because of how the priority of state changes are handled.
    // This reason remains the same for all the functions with a setState at the end.
    this.setState({ boardTelemetry })
  }

  batteryListenHandler(data: number[], part: string): void {
    const { batteryTelemetry } = this.state
    if (data.length > 1) {
      batteryTelemetry[part].forEach((cell: any, index: number) => {
        batteryTelemetry[part][cell].value = data[index]
      })
    } else {
      batteryTelemetry[part].value = data[0]
    }
    this.setState({ batteryTelemetry })
  }

  // except for the all motor buttons, buttonToggle() is called every time an enable/disable
  // button is pressed. When called, it's passed the name of a bus/device defined in the global
  // lists, and sets the state for that item to the opposite it was prior to the function call.
  // The ellipses function as the "spreading apart" operator that makes it so ONLY the specified
  // states get changed.
  buttonToggle(board: string, bus: string): void {
    let { boardTelemetry } = this.state
    boardTelemetry[board][bus].enabled = !this.state.boardTelemetry[board][bus].enabled
    this.setState(
      { boardTelemetry },
      // after the state of a given device is changed, packCommand() gets called to send the command(s)
      // corresponding with the toggled device to the rover, making it so only relevent commands are sent
      () => this.packCommand(board)
    )
  }

  allMotorToggle(button: boolean): void {
    let { boardTelemetry } = this.state
    // to simplify things, the all motor toggle was split into two buttons: an all enabled and an all disable.
    // Both buttons will pass a boolean to allMotorToggle() where it goes through each item in the constant
    // list and sets all their "enabled" states to true. Same if allMotorToggle() is passed false except it
    // sets all the states to false. Then the state changes all get thrown to the actual setState() and the
    // command is packed and sent.
    Object.keys(boardTelemetry.MotorBusEnable).forEach((motor: string) => {
      boardTelemetry.MotorBusEnable[motor].enabled = button ? true : false
    })
    // MOTORS[0] is used used mostly arbitrarily. allMotorToggle() changes the
    // state of all the motors at the same time, but even if only one motor had
    // changed state, the state of ALL the motors are sent as a SINGLE packet
    // by packCommand(), according to the groups defined by roveComm.
    this.setState({ boardTelemetry }, () => this.packCommand("MotorBusEnable"))
  }

  packCommand(board: string): void {
    const { boardTelemetry } = this.state
    let newBitMask = ""
    Object.keys(boardTelemetry[board]).forEach(bus => {
      newBitMask += boardTelemetry[board][bus].enabled ? "1" : "0"
    })
    rovecomm.sendCommand(board, [parseInt(newBitMask, 2)])
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Power and BMS</div>
        <div style={mainContainer}>
          <div style={{ ...column, ...readoutContainter }}>
            {Object.keys(this.state.boardTelemetry).map((board: string) => {
              return (
                <div key={board} style={{ ...column }}>
                  {Object.keys(this.state.boardTelemetry[board]).map((bus: string) => {
                    const { enabled, value } = this.state.boardTelemetry[board][bus]
                    return (
                      <div key={bus} style={row}>
                        <button type="button" onClick={() => this.buttonToggle(board, bus)} style={btnStyle}>
                          {enabled ? "Enabled" : "Disabled"}
                        </button>
                        <div style={ColorStyleConverter(value, 0, 7, 15, 120, 0, readout)}>
                          <h3 style={textPad}>{bus}</h3>
                          <h3 style={textPad}>
                            {`${value.toLocaleString(undefined, {
                              minimumFractionDigits: 1,
                              maximumFractionDigits: 1,
                              minimumIntegerDigits: 2,
                            })} A`}
                          </h3>
                        </div>
                      </div>
                    )
                  })}
                </div>
              )
            })}
          </div>
          <div style={{ ...row, ...btnArray, gridTemplateColumns: "auto auto" }}>
            <button
              type="button"
              onClick={() => {
                this.allMotorToggle(true)
              }}
              style={{ cursor: "pointer" }}
            >
              Enable All Motors
            </button>
            <button
              type="button"
              onClick={() => {
                this.allMotorToggle(false)
              }}
              style={{ cursor: "pointer" }}
            >
              Disable All Motors
            </button>
            <button type="button" onClick={() => turnOffReboot(5)} style={{ cursor: "pointer" }}>
              REBOOT
            </button>
            <button type="button" onClick={() => turnOffReboot(0)} style={{ cursor: "pointer" }}>
              SHUT DOWN
            </button>
          </div>
          <h3 style={{ alignSelf: "center", fontSize: "14px", fontFamily: "arial" }}>
            -------------------------------------------
          </h3>
          <div style={{ ...row, width: "100%" }}>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.Temp_Meas.value, 30, 75, 115, 120, 0, readout)}>
              <h3 style={textPad}>Battery Temperature</h3>
              <h3 style={textPad}>
                {this.state.batteryTelemetry.Temp_Meas.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                °
              </h3>
            </div>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.PackI_Meas.value, 0, 15, 20, 120, 0, readout)}>
              <h3 style={textPad}>Total Pack Current</h3>
              <h3 style={textPad}>
                {`${(this.state.batteryTelemetry.PackI_Meas.value / 1000).toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })} A`}
              </h3>
            </div>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.PackV_Meas.value, 22, 28, 33, 0, 120, readout)}>
              <h3 style={textPad}>Total Pack Voltage</h3>
              <h3 style={textPad}>
                {`${(this.state.batteryTelemetry.PackV_Meas.value / 1000).toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 2,
                  minimumIntegerDigits: 2,
                })} V`}
              </h3>
            </div>
          </div>
          <div style={{ ...row, width: "100%" }}>
            <div style={{ ...cellReadoutContainer, width: "100%" }}>
              {Object.keys(this.state.batteryTelemetry.CellV_Meas).map(cell => {
                const { value } = this.state.batteryTelemetry.CellV_Meas[cell]
                return (
                  <div key={cell} style={ColorStyleConverter(value, 2.5, 3.1, 4.2, 0, 120, readout)}>
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>
                      {`${(value / 1000).toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                        minimumIntegerDigits: 1,
                      })} V`}
                    </h3>
                  </div>
                )
              })}
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Power
