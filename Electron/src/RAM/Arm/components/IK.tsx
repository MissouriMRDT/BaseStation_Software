import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
  width: "10%",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const axes: CSS.Properties = {
  display: "grid",
  gridRowStart: "2 & {}",
  grid: "repeat(3, 28px) / auto-flow dense",
  margin: "10px 0px",
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
  margin: "0px 10px",
}
const input: CSS.Properties = {
  width: "75%",
}
const buttons: CSS.Properties = {
  width: "40%",
  margin: "5px",
  fontSize: "14px",
  lineHeight: "24px",
  borderRadius: "20px",
}

function getPosition(): void {
  // Unlike most telemetry, arm joint positions are only sent when requested
  rovecomm.sendCommand("RequestAxesPositions", [1])
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  IKValues: any
}

class IK extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      IKValues: {
        X: "",
        Y: "",
        Z: "",
        Pitch: "",
        Yaw: "",
        Roll: "",
      },
    }
    this.setPosition = this.setPosition.bind(this)

    rovecomm.on("IKCoordinates", (data: any) => this.updatePosition(data))
  }

  setPosition(): void {
    /* This function should take the values of all the axes,
     * convert them from strings to floats (or empty string to 0)
     * and send the proper rovecomm packet
     */
    rovecomm.sendCommand(
      "ArmMoveIK",
      Object.values(this.state.IKValues).map(function (x: string) {
        return x ? parseFloat(x) : 0
      })
    )
  }

  updatePosition(data: any): void {
    /* Function to update displayed IKValues when a new position is recieved */
    const [X, Y, Z, Pitch, Yaw, Roll] = data
    const IKValues = { X, Y, Z, Pitch, Yaw, Roll }
    this.setState({ IKValues })
  }

  axisChange(event: { target: { value: string } }, axis: string): void {
    /* We only want floats, so filter out any other characters
     * but match returns an array of [match, index, input, groups]
     * or returns undefined if there is no match
     */
    // Regex filters for 0 or 1 negative sign, 0+ digits, 0 or 1 decimal, followed by 0+ more digits
    const cleansedValue = event.target.value.match(/^-?\d*\.?\d*/)

    let value = ""
    if (cleansedValue) {
      // Leading semicolon helps ensure [value] isn't taken as an index
      // but properly used for array destructuring
      ;[value] = cleansedValue
    }

    this.setState({
      IKValues: {
        ...this.state.IKValues,
        [axis]: value,
      },
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>IK</div>
        <div style={container}>
          <div style={axes}>
            {Object.keys(this.state.IKValues).map(axis => {
              return (
                <div key={axis} style={row}>
                  <h1 style={h1Style}>{axis}</h1>
                  <input
                    type="text"
                    style={input}
                    value={this.state.IKValues[axis] || ""}
                    onChange={e => this.axisChange(e, axis)}
                  />
                </div>
              )
            })}
          </div>
          <div style={row}>
            <button type="button" style={buttons} onClick={this.setPosition}>
              Set Absolute Position
            </button>
            <button type="button" style={buttons} onClick={getPosition}>
              Get Absolute Position
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default IK
