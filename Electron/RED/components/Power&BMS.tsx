import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
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
const redReadoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  backgroundColor: "#ff3526", // red
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "4px",
  paddingLeft: "3px",
  paddingRight: "3px",
  paddingBottom: "4px",
}
const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  backgroundColor: "#30ff00", // green
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
  gridTemplateColumns: "240px 240px",
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
  motorBusButtons: string
}

class Power extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      motorBusButtons: Array(16).join("0")
    }
    rovecomm.on("MotorBusEnabled", (pckt: number) => this.MotorButtons(pckt))
  }

  MotorButtons(pckt: number) {
    const binStr: string = pckt.toString(2)
    this.setState({
      motorBusButtons: binStr,
    })
  }

  /* render(): JSX.Element {
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
          <div style={btnArray}>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 15 Enabled" : "Bus 15 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 16 Enabled" : "Bus 16 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 17 Enabled" : "Bus 17 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 18 Enabled" : "Bus 18 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 19 Enabled" : "Bus 19 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 20 Enabled" : "Bus 20 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 21 Enabled" : "Bus 21 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 22 Enabled" : "Bus 22 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 23 Enabled" : "Bus 23 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 24 Enabled" : "Bus 24 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 25 Enabled" : "Bus 25 Disabled"}
                </button>
              )}
            </ToggleButton>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "Bus 26 Enabled" : "Bus 26 Disabled"}
                </button>
              )}
            </ToggleButton>
          </div>
          <div style={btnArray}>
            <button type="button">All Motors Disabled</button>
            <button type="button">REBOOT</button>
            <button type="button">SHUT DOWN</button> 
            <button type="button">START LOG</button>
          </div>
          <div style={btnArray}>
            -------------------------------------------------
          </div>
          <div style={totalPackContainer}>
            <div style={redReadoutDisplay}>
              <span>Total Pack Voltage</span>
              {this.state.ttlPackVolt.toFixed(1)} V
            </div>
            <div style={readoutDisplay}>
              <span>Total Pack Current</span>
              {this.state.ttlPackCurrent.toFixed(1)} A
            </div>
          </div>
          <div style={cellReadoutContainer}>
            <div style={redReadoutDisplay}>
              <span>Cell 1</span>
              {this.state.cellOne.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 2</span>
              {this.state.cellTwo.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 3</span>
              {this.state.cellThree.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 4</span>
              {this.state.cellFour.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 5</span>
              {this.state.cellFive.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 6</span>
              {this.state.cellSix.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 7</span>
              {this.state.cellSeven.toFixed(1)} V
            </div>
            <div style={redReadoutDisplay}>
              <span>Cell 8</span>
              {this.state.cellEight.toFixed(1)} V
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
*/ 
export default Power
