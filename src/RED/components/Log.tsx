import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  height: "250px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  whiteSpace: "pre-wrap",
  overflow: "scroll",
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

interface IProps {
  style?: CSS.Properties
}

interface IState {
  ConsoleText: string
}

class Log extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      ConsoleText: "",
    }
    rovecomm.on("all", (data: any) => this.Log(data))
  }

  Log(data: string): void {
    let text = this.state.ConsoleText
    text += `${new Date().toLocaleTimeString()}: ${data} \n`
    this.setState({
      ConsoleText: text,
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Console</div>
        <div style={container}>{this.state.ConsoleText}</div>
      </div>
    )
  }
}

export default Log
