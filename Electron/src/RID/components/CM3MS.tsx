import React, { Component } from "react"
import CSS from "csstype"
import { exec } from "child_process"
import { rovecomm, RovecommManifest, NetworkDevices } from "../../Core/RoveProtocol/Rovecomm"
import { ColorStyleConverter } from "../../Core/ColorConverter"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
  alignItems: "center",
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
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  margin: "2.5px",
  justifyContent: "space-between",
}
const labelbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  margin: "2.5px",
  justifyContent: "space-between",
  textAlign: "center",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {}

class CM3MS extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={row}>
        <button type="button" style={{ flexGrow: 1 }}>
          Camera
        </button>
        <button type="button" style={{ flexGrow: 1 }}>
          Map
        </button>
        <button type="button" style={{ flexGrow: 1 }}>
          3D Rover
        </button>
        <button type="button" style={{ flexGrow: 1 }}>
          Merge
        </button>
        <button type="button" style={{ flexGrow: 1 }}>
          Split
        </button>
      </div>
    )
  }
}

export default CM3MS
