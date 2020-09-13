import React, { Component } from "react"
import CSS from "csstype"
import rovecomm from "../RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  backgroundColor: "yellowgreen",
  right: 0,
  bottom: "0rem",
  padding: "0.5rem",
  fontFamily: "arial",
  fontSize: "0.5rem",
}
const container: CSS.Properties = {
  display: "grid",
  borderColor: "gray",
  borderWidth: "2px",
  borderStyle: "solid",
  grid: "repeat(2, 36px) / auto-flow dense",
}

class GPS extends Component {
  /*
    This class is meant to be called by the index page and handles the base
    logic for what is displayed
    */

  constructor(props: any) {
    super(props)
    this.state = {}
    console.log(rovecomm)
  }

  render(): JSX.Element {
    return (
      <div style={container}>
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
