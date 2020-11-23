import React, { Component } from "react"
import CSS from "csstype"
// import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

/* eslint max-classes-per-file: ["error", 2] */

const h1Style: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const grandContainer: CSS.Properties = {
  // display: "flex",
  // flexDirection: "column",
  display: "block",
  width: "640px",
  borderTopWidth: "28px",
  borderBottomWidth: "2px",
  borderColor: "rgb(153, 0, 0)", // MRDT red
  borderStyle: "solid",
  // gridRowStart: "placeholder",
}
const roAndBtnContainer: CSS.Properties = {
  // stands for "Readout and Button Container"; shortened for sanity
  display: "grid",
  width: "auto",
  gridTemplateColumns: "69px 1fr 69px 1fr",
  marginLeft: "2px",
  marginTop: "2px",
  marginBottom: "2px",
}
const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  backgroundColor: "#30ff00", // green
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "3px",
  paddingLeft: "3px",
  paddingRight: "3px",
  marginRight: "2px",
}
/* const btnArray: CSS.Properties = {
  // potential container for all the bus buttons and "reboot," "start log," etc.
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
  justifyContent: "center",
} */
const totalPackContainer: CSS.Properties = {
  display: "grid",
  justifyContent: "space-evenly",
  gridTemplateColumns: "500px 500px", // placeholder number
  // color: "#30ff00", green; intended for "current"
  // color: "#ff1100", red; intended for "voltage"
}
const cellReadoutContainer: CSS.Properties = {
  display: "grid",
  color: "#ff1100", // red
  gridTemplateColumns: "auto auto auto auto",
  // will likely need a way to keep elements confined to set columns
}

interface IProps {}

interface IState {
  motorLF: number
  motorLFButton: boolean
  motorLM: number
  motorLMButton: boolean
  motorLB: number
  motorLBButton: boolean
  motorRF: number
  motorRFButton: boolean
  motorRM: number
  motorRMButton: boolean
  motorRB: number
  motorRBButton: boolean
  motorExtra: number
  motorExtraButton: boolean
  auxiliary: number
  auxiliaryButton: boolean
  comms: number
  commsButton: boolean
  logic: number
  logicButton: boolean
  actuation: number
  actuationButton: boolean
  twelveVBoard: number
  twelveVBoardButton: boolean
  batteryTemp: number
  ttlPackVolt: number
  ttlPackCurrent: number
  bus15Button: boolean
  bus16Button: boolean
  bus17Button: boolean
  bus18Button: boolean
  bus19Button: boolean
  bus20Button: boolean
  bus21Button: boolean
  bus22Button: boolean
  bus23Button: boolean
  bus24Button: boolean
  bus25Button: boolean
  bus26Button: boolean
  allMtrsDisbled: boolean
  reboot: boolean
  shutDown: boolean
}

class Power extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      motorLF: 0.0,
      motorLFButton: false,
      motorLM: 0.0,
      motorLMButton: false,
      motorLB: 0.0,
      motorLBButton: false,
      motorRF: 0.0,
      motorRFButton: false,
      motorRM: 0.0,
      motorRMButton: false,
      motorRB: 0.0,
      motorRBButton: false,
      motorExtra: 0.0,
      motorExtraButton: false,
      auxiliary: 0.0,
      auxiliaryButton: false,
      comms: 0.0,
      commsButton: false,
      logic: 0.0,
      logicButton: false,
      actuation: 0.0,
      actuationButton: false,
      twelveVBoard: 0.0,
      twelveVBoardButton: false,
      batteryTemp: 0.0,
      bus15Button: false,
      bus16Button: false,
      bus17Button: false,
      bus18Button: false,
      bus19Button: false,
      bus20Button: false,
      bus21Button: false,
      bus22Button: false,
      bus23Button: false,
      bus24Button: false,
      bus25Button: false,
      bus26Button: false,
      allMtrsDisbled: false,
      reboot: false,
      shutDown: false,
      startLog: false,
      ttlPackVolt: 0.0,
      ttlPackCurrent: 0.0,
      cellOne: 0.0,
      cellTwo: 0.0,
      cellThree: 0.0,
      cellFour: 0.0,
      cellFive: 0.0,
      cellSix: 0.0,
      cellSeven: 0.0,
      cellEight: 0.0,
    }
    // rovecomm.on?
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={h1Style}>POWER AND BMS</div>
        <div style={grandContainer}>
          <div style={roAndBtnContainer}>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor LF</span>
              {this.state.motorLF.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Auxiliary</span>
              {this.state.auxiliary.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor LM</span>
              {this.state.motorLM.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Comms</span>
              {this.state.comms.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor LB</span>
              {this.state.motorLB.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Logic</span>
              {this.state.logic.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor RF</span>
              {this.state.motorRF.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Actuation</span>
              {this.state.actuation.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor RM</span>
              {this.state.motorRM.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Twelve V Board</span>
              {this.state.twelveVBoard.toFixed(1)} A
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor RB</span>
              {this.state.motorRB.toFixed(1)} A
            </div>
            <div />
            <div style={readoutDisplay}>
              <span>Battery Temp</span>
              {this.state.batteryTemp.toFixed(1)}°
            </div>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Enabled" : "Disabled"}
                </button>
              )}
            </ToggleButton>
            <div style={readoutDisplay}>
              <span>Motor Extra</span>
              {this.state.motorExtra.toFixed(1)} A
            </div>
          </div>
        </div>
      </div>
    )
  }
}

class ToggleButton extends Power {
  state = {
    on: false,
  }

  toggle = () => {
    this.setState({
      on: !this.state.on,
    })
  }

  render() {
    const { children } = this.props
    return children({
      on: this.state.on,
      toggle: this.toggle,
    })
  }
}

export default Power
