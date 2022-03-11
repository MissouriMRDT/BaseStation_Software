import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

const header: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "16px",
  lineHeight: "22px",
  width: "35%",
}
const value: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "16px",
  lineHeight: "22px",
  width: "10%",
  textAlign: "center",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  height: "calc(100% - 38px)",
  marginBottom: "5px",
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
  justifyContent: "space-around",
  alignItems: "center",
}
const button: CSS.Properties = {
  width: "35%",
  margin: "5px",
  fontSize: "16px",
  lineHeight: "24px",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  overridden: boolean
  tool: number
  laserOn: boolean
  sendInterval: NodeJS.Timeout
}

class ControlFeatures extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      overridden: false,
      tool: 0,
      laserOn: false,
      sendInterval: setInterval(() => rovecomm.sendCommand("Lasers", this.state.laserOn ? [1] : [0]), 1000),
    }
  }

  /** Called by React when the component is destroyed
   *  We want to stop sending the lasers command when the arm isn't being controlled.
   */
  componentWillUnmount() {
    clearInterval(this.state.sendInterval)
  }

  limitOverride(): void {
    // we send 1 when false and 0 when true because we are about to toggle the bool
    rovecomm.sendCommand("LimitSwitchOverride", this.state.overridden ? 0 : 1)
    this.setState({ overridden: !this.state.overridden })
  }

  toggleLasers(): void {
    this.setState({ laserOn: !this.state.laserOn })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Control Features</div>
        <div style={container}>
          <div style={row}>
            <div style={header}>Limit Override: </div>
            <button type="button" onClick={this.limitOverride} style={button}>
              All Joints
            </button>
          </div>
          <div style={row}>
            <div style={header}>Tool: </div>
            <div style={value}>{this.state.tool}</div>
          </div>
          <div style={row}>
            <input
              type="checkbox"
              id="LaserToggle"
              name="LaserToggle"
              checked={this.state.laserOn}
              onChange={() => this.toggleLasers()}
            />
            <label style={{ marginLeft: "5px" }} htmlFor="LaserToggle">
              Laser Power
            </label>
          </div>
        </div>
      </div>
    )
  }
}

export default ControlFeatures
