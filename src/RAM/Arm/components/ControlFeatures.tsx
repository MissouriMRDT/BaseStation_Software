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
  margin: "5px",
}
const button: CSS.Properties = {
  width: "35%",
  margin: "5px",
  fontSize: "16px",
  lineHeight: "24px",
  borderRadius: "20px",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  overridden: boolean
  tool: number
}

class ControlFeatures extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      overridden: false,
      tool: 0,
    }
  }

  limitOverride(): void {
    // we send 1 when false and 0 when true because we are about to toggle the bool
    rovecomm.sendCommand("LimitSwitchOverride", this.state.overridden ? 0 : 1)
    this.setState({ overridden: !this.state.overridden })
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
        </div>
      </div>
    )
  }
}

export default ControlFeatures
