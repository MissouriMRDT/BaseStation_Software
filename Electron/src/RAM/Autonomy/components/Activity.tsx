import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  whiteSpace: "pre-wrap",
  overflow: "scroll",
  height: "500px",
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
  ActivityText: string
}

class Activity extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      ActivityText: "",
    }
    rovecomm.on("AutonomyActivity", (data: any) => this.Log(data))
    rovecomm.on("CurrentLog", (data: any) => this.Log(data))
  }

  Log(data: string): void {
    let text = this.state.ActivityText
    text += `${data} \n` // set this to be that variable and time and newline
    this.setState({
      ActivityText: text,
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Autonomy Activity</div>
        <div style={container}>{this.state.ActivityText}</div>
      </div>
    )
  }
}

export default Activity
