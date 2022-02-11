import React, { Component } from "react"
import CSS from "csstype"

const header: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "16px",
  lineHeight: "22px",
  width: "35%",
}
const value: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "16px",
  lineHeight: "22px",
  width: "10%",
  textAlign: "center",
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
  alignItems: "center",
  margin: "5px",
}
const slider: CSS.Properties = {
  background: "#990000",
  width: "40%",
  WebkitAppearance: "none",
  appearance: "none",
  height: "6px",
  outline: "none",
}

// While it is bad practice to export something other than a const
// exporting this is far easier and more accessible than passing it through
// the props chain. Disabled linter for just this line.
// eslint-disable-next-line import/no-mutable-exports
export let controlMultipliers: any = {}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  controlMultipliers: {
    Base: number
    Elbow: number
    Wrist: number
    Gripper: number
    Nipper: number
  }
}

class ControlMultipliers extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      controlMultipliers: {
        Base: 500,
        Elbow: 500,
        Wrist: 500,
        Gripper: 500,
        Nipper: 500,
      },
    }
    controlMultipliers = this.state.controlMultipliers
  }

  sliderChange(event: { target: { value: string } }, multiplier: string): void {
    /* When the slider changes, update the cooresponding multiplier in state.
     * We use state multiplier for display reasons, but then write that to
     * an exported variable to be used by the rest of the Arm system.
     */
    this.setState(
      {
        controlMultipliers: {
          ...this.state.controlMultipliers,
          [multiplier]: parseInt(event.target.value, 10),
        },
      },
      () => {
        controlMultipliers = this.state.controlMultipliers
      }
    )
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Control Multipliers</div>
        <div style={container}>
          {Object.keys(this.state.controlMultipliers).map(multipliers => {
            return (
              <div key={multipliers} style={row}>
                <div style={header}>{multipliers} Control Multiplier</div>
                <div style={value}>{this.state.controlMultipliers[multipliers]}</div>
                <input
                  type="range"
                  min="1"
                  max="1000"
                  value={this.state.controlMultipliers[multipliers]}
                  style={slider}
                  onChange={e => this.sliderChange(e, multipliers)}
                />
              </div>
            )
          })}
        </div>
      </div>
    )
  }
}

export default ControlMultipliers
