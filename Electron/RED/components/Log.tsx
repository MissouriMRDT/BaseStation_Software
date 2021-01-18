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
  width: "640px",
  height: "400px",
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

interface IProps {}

interface IState {
  ConsoleText: string
}

class Log extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      ConsoleText: "",
    }
    rovecomm.on("all", (data: any) => this.Log(data))
  }

  Log(data: string): void {
    let text = this.state.ConsoleText
    text += `${data} \n` // set this to be that variable and time and newline
    this.setState({
      ConsoleText: text,
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Console</div>
        <div style={container}>{this.state.ConsoleText}</div>
      </div>
    )
  }
}

export default Log
