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
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(1, 300px) / auto-flow dense",
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

class Console extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      ConsoleText: "",
    }
    this.Log()
  }

  Log(/* add some variables */): void {
    let text = this.state.ConsoleText
    text += "test" // set this to be that variable and time and newline
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

export default Console
