import React, { Component } from "react"
import CSS from "csstype"
import { ChromePicker } from "react-color"
import { rovecomm, RovecommManifest } from "../../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexDirection: "column",
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
}

const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
}

interface RGBColor {
  r: number
  g: number
  b: number
  a: number
}

interface IProps {
  style?: CSS.Properties
}
interface IState {
  color: RGBColor
}

class Lighting extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      color: { r: 255, g: 255, b: 255, a: 0 },
    }
  }

  colorChanged(newColor: any): void {
    const color = newColor.rgb
    this.setState({ color })
    rovecomm.sendCommand("LEDRGB", [color.r, color.g, color.b], true)
  }

  teleop(): void {
    rovecomm.sendCommand("StateDisplay", RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Teleop)
    console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Teleop)
  }

  autonomy(): void {
    rovecomm.sendCommand("StateDisplay", RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Autonomy)
    console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Autonomy)
  }

  reachedGoal(): void {
    rovecomm.sendCommand("StateDisplay", RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Reached_Goal)
    console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Reached_Goal)
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Lighting</div>
        <div style={container}>
          <div style={column}>
            <ChromePicker color={this.state.color} onChangeComplete={(color: any) => this.colorChanged(color)} />
            <div style={row}>
              <button onClick={() => this.teleop()}>Teleop</button>
              <button onClick={() => this.autonomy()}>Autonomy</button>
              <button onClick={() => this.reachedGoal()}>Reached Goal</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Lighting
