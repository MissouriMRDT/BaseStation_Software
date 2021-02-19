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
  height: "calc(100% - 40px)",
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
  marginTop: "5px",
  lineHeight: "25px",
}
const immutable: CSS.Properties = {
  backgroundColor: "#F0F0F0",
  width: "25px",
  fontSize: "18px",
  textAlign: "center",
}
const buttonRow: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-around",
  marginTop: "5px",
  marginBottom: "5px",
}
const button: CSS.Properties = {
  width: "60px",
  height: "60px",
  borderRadius: "10px",
  boxShadow: "3px 3px 3px gray",
  border: "none",
}
const disabledButton: CSS.Properties = {
  width: "60px",
  height: "60px",
  borderRadius: "10px",
  boxShadow: "3px 3px 3px gray inset",
  border: "none",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  tube: number
  buttonsDisabled: boolean
}

class Geneva extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      tube: 0,
      buttonsDisabled: false,
    }
    this.rotateLeft = this.rotateLeft.bind(this)
    this.rotateRight = this.rotateRight.bind(this)
    this.updatePosition = this.updatePosition.bind(this)
    rovecomm.on("GenevaCurrentPosition", (data: any) => this.updatePosition(data))
  }

  rotateLeft(): void {
    if (this.state.buttonsDisabled) {
      return
    }
    rovecomm.sendCommand("GenevaIncrementPosition", [-1])
    // Javascript doesn't have a mod operator, only a remainder operator
    // Since we want this value to wrap -1 to 7, we need mod, which can be
    // defined as ((n%m)+m)%m
    this.setState({
      tube: (((this.state.tube - 1) % 8) + 8) % 8,
      buttonsDisabled: true,
    })
  }

  rotateRight(): void {
    if (this.state.buttonsDisabled) {
      return
    }
    rovecomm.sendCommand("GenevaIncrementPosition", [1])
    this.setState({
      tube: (((this.state.tube + 1) % 8) + 8) % 8,
      buttonsDisabled: true,
    })
  }

  updatePosition(data: any): void {
    const buttonsDisabled = this.state.tube === data[0]
    this.setState({ buttonsDisabled })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Geneva Position</div>
        <div style={container}>
          <div style={row}>
            <div>Test Tube:</div>
            <div style={immutable}>{this.state.tube}</div>
          </div>
          <div style={buttonRow}>
            <button
              type="button"
              style={this.state.buttonsDisabled ? disabledButton : button}
              onClick={this.rotateLeft}
            >
              Rotate
              <br />
              Left
            </button>
            <button
              type="button"
              style={this.state.buttonsDisabled ? disabledButton : button}
              onClick={this.rotateRight}
            >
              Rotate
              <br />
              Right
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Geneva
