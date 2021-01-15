import React, { Component } from "react"
import CSS from "csstype"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "500px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const joints: CSS.Properties = {
  display: "grid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
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

interface Joint {
  J1: string
  J2: string
  J3: string
  J4: string
  J5: string
  J6: string
}

interface IProps {}

interface IState {
  jointValues: Joint
  storedPositions: any
  selectedPosition: string
  addingPosition: boolean
  positionName: string
}

class Angular extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      jointValues: {
        J1: "",
        J2: "",
        J3: "",
        J4: "",
        J5: "",
        J6: "",
      },
    }
  }

  jointChange(event: { target: { value: string } }, joint: string): void {
    // we only want positive floats, so filter out any other characters
    // but match returns an array of [match, index, input, groups]
    // or returns undefined if there is no match
    const cleansedValue = event.target.value.match(/^\d*\.?\d*/)
    let value = ""
    if (cleansedValue) {
      ;[value] = cleansedValue
    }
    this.setState({
      jointValues: {
        ...this.state.jointValues,
        [joint]: value,
      },
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Angular</div>
        <div style={container}>
          <div style={joints}>
            {Object.keys(this.state.jointValues).map(joint => {
              return (
                <div key={joint} style={row}>
                  <h1 style={h1Style}>{joint}</h1>
                  <input
                    type="text"
                    style={input}
                    value={this.state.jointValues[joint] || ""}
                    onChange={e => this.jointChange(e, joint)}
                  />
                </div>
              )
            })}
          </div>
        </div>
      </div>
    )
  }
}

export default Angular
