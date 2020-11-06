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
  defaultState: number
}

interface IState {
  currentState: number
}

class ControlScheme extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      currentState: this.props.defaultState,
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>ControlScheme</div>
        <div style={container}>
          {[
            { title: "Drive", value: 1 },
            { title: "Main Gimbal", value: 2 },
          ].map(datum => {
            const { title, value } = datum
            return (
              <>
                <div>{title}</div>
                <button
                  type="button"
                  key={value}
                  onClick={() => this.setState({ currentState: value })}
                >
                  <h1 style={h1Style}>{value}</h1>
                </button></>
            )
          })}
        </div>
      </div>
    )
  }
}

export default ControlScheme
