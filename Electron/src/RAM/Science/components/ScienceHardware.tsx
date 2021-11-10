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
  justifyContent: "center",
  marginTop: "5px",
  lineHeight: "25px",
}
const buttonRow: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "center",
  marginTop: "5px",
  marginBottom: "5px",
}
const leftGroup: CSS.Properties = {
  width: "50%",
  justifyContent: "center",
  margin: "auto",
}
const rightGroup: CSS.Properties = {
  width: "40%",
  justifyContent: "center",
  margin: "auto",
}

const testTube = 12

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
      tube: (((this.state.tube - 1) % testTube) + testTube) % testTube,
      buttonsDisabled: true,
    })
  }

  rotateRight(): void {
    if (this.state.buttonsDisabled) {
      return
    }
    rovecomm.sendCommand("GenevaIncrementPosition", [1])
    this.setState({
      tube: (((this.state.tube + 1) % testTube) + testTube) % testTube,
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
        <div style={label}>Science Hardware</div>
        <div style={container}>
          <div style={row}>
            <div style={leftGroup}>
              <div style={row}>Move Gantry to Test Tubes</div>
              <div style={buttonRow}>
                <button type="button">Grp 1</button>
                <button type="button">Grp 2</button>
                <button type="button">Grp 3</button>
              </div>
              <div style={row}>Move to Spare Scoop</div>
              <div style={buttonRow}>
                <button type="button">Test 1</button>
                <button type="button">Test 2</button>
                <button type="button">Test 2</button>
              </div>
            </div>
            <div style={rightGroup}>
              <div style={row}>Testing</div>
              <div style={row}>
                <button type="button">Grp 1</button>
                <button type="button">Grp 2</button>
              </div>
              <div style={row}>
                <button type="button">Grp 3</button>
                <button type="button">Drill</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Geneva
