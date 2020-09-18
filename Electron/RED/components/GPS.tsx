import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm, parse } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  backgroundColor: "white",
  right: 0,
  bottom: "0rem",
  padding: "0.5rem",
  fontFamily: "arial",
  fontSize: "0.5rem",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  width: "440px",
  borderTopWidth: "16px",
  borderColor: "darkred",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 36px) / auto-flow dense",
}
const label: CSS.Properties = {
  position: "absolute",
  top: "9px",
  fontSize: "12px",
  zIndex: 1,
  color: "white",
}

class GPS extends Component {
  /*
    This class is meant to be called by the index page and handles the base
    logic for what is displayed
    */

  constructor(props: any) {
    super(props)
    this.state = {}

    const listen = ["DriveLeftRight"]

    rovecomm.UDPSocket.on(
      "message",
      (msg: Uint8Array, rinfo: { address: string; port: number }) => {
        console.log(msg, "here", parse("dataId", msg))
        if (parse("dataId", msg) in listen) {
          switch (parse("dataId", msg)) {
            case "DriveLeftRight":
              console.log(msg)
              break
            default:
              console.log(msg)
              break
          }
        }
      }
    )
  }

  render(): JSX.Element {
    return (
      <div style={container}>
        <div style={label}>GPS</div>
        {[
          { title: "Fix Obtained", value: "False" },
          { title: "Satellite Count", value: "255" },
          { title: "Current Lat.", value: "0" },
          { title: "Lidar", value: "0.00" },
          { title: "Fix Quality", value: "255" },
          { title: "Odometer (Miles)", value: "0" },
          { title: "Current Lon.", value: "0" },
        ].map(datum => {
          const { title, value } = datum
          return (
            <div key={title}>
              <h1 style={h1Style}>
                {title}: {value}
              </h1>
            </div>
          )
        })}
      </div>
    )
  }
}

export default GPS
