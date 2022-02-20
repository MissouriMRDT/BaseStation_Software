import React, { Component } from "react"
import CSS from "csstype"
import { ChromePicker } from "react-color"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

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

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Lighting</div>
        <div style={container}>
          <ChromePicker color={this.state.color} onChangeComplete={(color: any) => this.colorChanged(color)} />
        </div>
      </div>
    )
  }
}

export default Lighting
