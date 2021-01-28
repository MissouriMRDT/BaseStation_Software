import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  marginTop: "-3.5%",
  marginBottom: "1.25%",
  position: "relative",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const grandContainer: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  width: "640px",
  borderTopWidth: "28px",
  borderBottomWidth: "2px",
  borderColor: "rgb(153, 0, 0)", // MRDT red
  borderStyle: "solid",
  justifyContent: "center",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  fontFamily: "arial",
  alignContent: "flex-start",
  flexBasis: "auto",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  alignContent: "flex-start",
  flexBasis: "auto",
}
// stands for "Readout and Button Container"; shortened for sanity
const roAndBtnContainer: CSS.Properties = {
  display: "grid",
  width: "auto",
  gridTemplateColumns: "auto auto",
  marginLeft: "2px",
  marginTop: "2px",
  marginBottom: "2px",
}
const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  // backgroundColor: "#30ff00", // green
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "4px",
  paddingLeft: "3px",
  paddingRight: "3px",
  paddingBottom: "4px",
  marginRight: "2px",
}
const btnArray: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
  justifyContent: "center",
}
const totalPackContainer: CSS.Properties = {
  display: "grid",
  justifyContent: "space-evenly",
  gridTemplateColumns: "240px 240px 240px",
}
const cellReadoutContainer: CSS.Properties = {
  display: "grid",
  color: "black",
  gridTemplateColumns: "auto auto auto auto",
  marginLeft: "3px",
  marginBottom: "3px",
  marginRight: "3px",
}

interface IProps {}

interface IState {
  boardTelemetry: any
  batteryTelemetry: any
}

class Power extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      boardTelemetry: {
        "Drive LF": { enabled: false, value: 0 },
        "Drive LR": { enabled: false, value: 0 },
        "Drive RF": { enabled: false, value: 0 },
        "Drive RR": { enabled: false, value: 0 },
        "Steering LF": { value: 0 },
        "Steering LR": { value: 0 },
        "Steering RF": { value: 0 },
        "Steering RR": { value: 0 },
        "Spare Motor": { enabled: false, value: 0 },
        Gimbal: { enabled: false, value: 0 },
        Multimedia: { enabled: false, value: 0 },
        Autonomy: { enabled: false, value: 0 },
        Logic: { value: 0 },
        "12V": { enabled: false, value: 0 },
        Comms: { enabled: false, value: 0 },
        Auxiliary: { enabled: false, value: 0 },
        Drive: { enabled: false, value: 0 },
        Vacuum: { enabled: false, value: 0 },
        Nav: { enabled: false },
        Cameras: { enabled: false },
        Extra: { enabled: false },
      },
      batteryTelemetry: {
        Temp: { value: 0 },
        TotalPackCurrent: { value: 0 },
        TotalPackVoltage: { value: 0 },
        "Cell 1": { value: 0 },
        "Cell 2": { value: 0 },
        "Cell 3": { value: 0 },
        "Cell 4": { value: 0 },
        "Cell 5": { value: 0 },
        "Cell 6": { value: 0 },
        "Cell 7": { value: 0 },
        "Cell 8": { value: 0 },
      },
    }
    rovecomm.on("MotorBusCurrent", (data: number) => this.motorBusCurrents(data))
    rovecomm.on("MotorBusEnabled", (data: number) => this.motorBusEnabled(data))
    rovecomm.on("SteeringMotorCurrents", (data: number) => this.steeringMotorCurrents(data)) // potential removal
    rovecomm.on("12VActBusEnable", (data: number) => this.twelveVActBusEnable(data))
    rovecomm.on("12VLogicBusEnable", (data: number) => this.twelveVLogicBusEnable(data))
    rovecomm.on("30VBusEnabled", (data: number) => this.thirtyVBusEnabled(data))
    rovecomm.on("VacuumEnabled", (data: number) => this.vacuumEnabled(data))
    rovecomm.on("VacuumCurrent", (data: number) => this.vacuumCurrent(data))
    rovecomm.on("PackI_Meas", (data: number) => this.packCurrentMeas(data))
    rovecomm.on("PackV_Meas", (data: number) => this.packVoltageMeas(data))
    rovecomm.on("CellV_Meas", (data: number) => this.cellsVoltMeas(data))
    rovecomm.on("Temp_Meas", (data: number) => this.battTempMeas(data))
  }

  motorBusEnabled(data: number): void {
    const bitmask = data.toString(2)
    const motors = ["Drive LF", "Drive LR", "Drive FF", "Drive FR", "Spare Motor"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  motorBusCurrents(data: number): void {
    const motors = ["Drive LF", "Drive LR", "Drive FF", "Drive FR", "Spare Motor"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  steeringMotorCurrents(data: number): void {
    const motors = ["Steering LF", "Steering LR", "Steering RF", "Steering RR"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVActBusEnable(data: number): void {
    const bitmask = data.toString(2)
    const peripherals = ["Gimbal", "Multimedia", "Auxilliary"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < peripherals.length; i++) {
      boardTelemetry[peripherals[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVLogicBusEnable(data: number): void {
    const bitmask = data.toString(2)
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Navigation", "Cameras", "Extra"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVBusCurrent(data: number): void {
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Logic"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusEnabled(data: number): void {
    const bitmask = data.toString(2)
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusCurrents(data: number): void {
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  vacuumEnabled(data: number): void {
    const { boardTelemetry } = this.state
    boardTelemetry.Vacuum.enabled = data
    this.setState({ boardTelemetry })
  }

  vacuumCurrent(data: number): void {
    const { boardTelemetry } = this.state
    boardTelemetry.Vacuum.value = data
    this.setState({ boardTelemetry })
  }

  packCurrentMeas(data: number): void {
    const { batteryTelemetry } = this.state
    batteryTelemetry.TotalPackCurrent.value = data
    this.setState({ batteryTelemetry })
  }

  packVoltageMeas(data: number): void {
    const { batteryTelemetry } = this.state
    batteryTelemetry.TotalPackVoltage.value = data
    this.setState({ batteryTelemetry })
  }

  cellsVoltMeas(data: number): void {
    const cells = ["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"]
    const { batteryTelemetry } = this.state
    for (let i = 0; i < cells.length; i++) {
      batteryTelemetry[cells[i]].value = data[i]
    }
    this.setState({ batteryTelemetry })
  }

  battTempMeas(data: number): void {
    const { batteryTelemetry } = this.state
    batteryTelemetry.Temp.value = data
    this.setState({ batteryTelemetry })
  }

  buttonToggle(motor: string): void {
    this.setState({
      boardTelemetry: {
        ...this.state.boardTelemetry,
        [motor]: {
          ...this.state.boardTelemetry[motor],
          enabled: !this.state.boardTelemetry[motor].enabled,
        },
      },
    })
  }

  render(): JSX.Element {
    return (
      <div style={grandContainer}>
        <div style={h1Style}>POWER AND BMS</div>
        <div
          style={{ ...row, width: "100%", fontSize: "12px" }}
          /* meant to be a two column container that houses two two-column containers;
          the child containers have buttons on the left and readouts on the right */
        >
          <div
            style={{ ...column, padding: "1%", width: "50%" }}
            /* first column of buttons and readouts */
          >
            {[
              "Drive LF",
              "Drive LR",
              "Drive RF",
              "Drive RR",
              "Steering LF", // doesn't actually have enable
              "Steering LR", // doesn't actually have enable
              "Steering RF", // doesn't actually have enable
              "Steering RR", // doesn't actually have enable
              "Spare Motor",
            ].map(motor => {
              /* this and following map functions work to make the button onClick's, titles, 
              and electrical current details all callable by the same respective "name" or "ID" 
              stored in the above array */
              return (
                <div key={motor} style={{ ...row, height: "25px" }}>
                  {"enabled" in this.state.boardTelemetry[motor] ? (
                    <button
                      type="button"
                      onClick={() => this.buttonToggle(motor)}
                      style={{ width: "auto", height: "auto", alignContent: "center", marginRight: "8px" }}
                    >
                      {this.state.boardTelemetry[motor].enabled ? "Enabled" : "Disabled"}
                    </button>
                  ) : (
                    <div style={{ width: "73px" }} />
                  )}
                  <div style={{ ...row, flexGrow: 1, justifyContent: "space-between", paddingRight: "1%" }}>
                    <h3>{motor}</h3>
                    <h3>{this.state.boardTelemetry[motor].value}A</h3>
                  </div>
                </div>
              )
            })}
          </div>
          <div
            style={{ ...column, paddingRight: "1%" }}
            /* second column of buttons and readouts */
          >
            {[
              "Gimbal",
              "Multimedia",
              "Autonomy",
              "Logic", // doesn't actually have enable
              "12V",
              "Comms",
              "Auxiliary",
              "Drive",
              "Vacuum",
            ].map(part => {
              return (
                <div key={part} style={{ ...row }}>
                  {"enabled" in this.state.boardTelemetry[part] ? (
                    <button
                      type="button"
                      onClick={() => this.buttonToggle(part)}
                      style={{ width: "65px", height: "20px", alignSelf: "center", marginRight: "8px" }}
                    >
                      {this.state.boardTelemetry[part].enabled ? "Enabled" : "Disabled"}
                    </button>
                  ) : (
                    <div style={{ width: "73px" }} />
                  )}
                  <div style={{ ...row, flexGrow: 1, justifyContent: "space-between" }}>
                    <h3>{part}</h3>
                    <h3>{this.state.boardTelemetry[part].value}A</h3>
                  </div>
                </div>
              )
            })}
          </div>
        </div>
        <div style={btnArray}>
          {["Nav", "Cameras", "Extra"].map(peripheral => {
            return (
              <button type="button" key={peripheral} onClick={() => this.buttonToggle(peripheral)}>
                {this.state.boardTelemetry[peripheral].enabled ? `${peripheral} Enabled` : `${peripheral} Disabled`}
              </button>
            )
          })}
          <button type="button">All Motors</button>
          <button type="button">REBOOT</button>
          <button type="button">SHUT DOWN</button>
        </div>
        <h3>-------------------------------------</h3>
        <div style={totalPackContainer}>
          <div style={readoutDisplay}>
            <h3>Battery Temperature</h3>
            <h3>{this.state.batteryTelemetry.Temp.value}°</h3>
          </div>
          <div style={readoutDisplay}>
            <h3>Total Pack Current</h3>
            <h3>{this.state.batteryTelemetry.TotalPackCurrent.value}A</h3>
          </div>
          <div style={readoutDisplay}>
            <h3>Total Pack Voltage</h3>
            <h3>{this.state.batteryTelemetry.TotalPackVoltage.value}A</h3>
          </div>
          <div style={cellReadoutContainer}>
            {["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"].map(cell => {
              return (
                <div key={cell} style={readoutDisplay}>
                  <h3>{cell}</h3>
                  <h3>{this.state.batteryTelemetry[cell].value}V</h3>
                </div>
              )
            })}
          </div>
        </div>
      </div>
    )
  }
}

export default Power
