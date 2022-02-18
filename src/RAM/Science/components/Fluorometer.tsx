import React, { Component } from "react"
import CSS from "csstype"

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
  flexGrow: 1,
  justifyContent: "center",
  margin: "auto",
  lineHeight: "30px",
  width: "100%",
}
const componentBox: CSS.Properties = {
  margin: "5px",
}
const button: CSS.Properties = {
  marginLeft: "15px",
  width: "60px",
}

const controlButton: CSS.Properties = {
  //margin: "5px",
}

/** Will be merged with the row css if the Laser is off */
const offIndicator: CSS.Properties = {
  backgroundColor: "#FF0000",
}

/** Will be merged with the row css if the Laser is off */
const onIndicator: CSS.Properties = {
  backgroundColor: "#00FF00",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  /** Holds the last sent values of the diodes */
  DiodeValues: number[]
  /** Holds which lasers are enabled */
  LasersPowered: boolean[]
  /** If UV light is on */
  UVPowered: boolean
  /** If White Light is powered */
  LightPowered: boolean
}

class Fluorometer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      DiodeValues: [0, 0, 0],
      LasersPowered: [false, false, false],
      UVPowered: false,
      LightPowered: false,
    }
  }

  render(): JSX.Element {
    return (
      <div id="Flurometer" style={this.props.style}>
        <div style={label}>Fluorometer</div>
        <div style={container}>
          <div style={componentBox}>
            {this.state.DiodeValues.map((value, index) => {
              return (
                <div style={row}>
                  <label>
                    Diode {index + 1}: {value} nm
                  </label>
                </div>
              )
            })}
          </div>
          <div style={componentBox}>
            {this.state.LasersPowered.map((value, index) => {
              return (
                <div style={{ ...row, ...(value ? onIndicator : offIndicator) }}>
                  <label> Laser {index + 1}: </label>
                  <button style={button}>{value ? "Disable" : "Enable"}</button>
                </div>
              )
            })}
          </div>
          <div style={componentBox}>
            <div style={row}>
              <button style={controlButton}>Enable All</button>
              <button style={controlButton}>Disable All</button>
              <button style={controlButton}>Export</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Fluorometer
