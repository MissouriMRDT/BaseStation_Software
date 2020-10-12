import React, { Component } from "react"
import CSS from "csstype"
import {
  XYPlot,
  XAxis,
  YAxis,
  HorizontalGridLines,
  LineSeries,
} from "react-vis"
import CSVReader from "react-csv-reader"
import { specData } from "./spec"
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
  height: "500px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
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

interface IProps {}

interface IState {
  spectrometerData: { x: number; y: number }[]
}

class Spectrometer extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      spectrometerData: specData,
    }

    rovecomm.on("SpectrometerData", (data: any) => this.SpectrometerData(data))
  }

  SpectrometerData(data: { x: number; y: number }[]): void {
    this.setState({
      spectrometerData: data,
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Spectrometer</div>
        <div style={container}>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
            <HorizontalGridLines />
            <LineSeries data={this.state.spectrometerData} />
            <XAxis />
            <YAxis />
          </XYPlot>
        </div>
      </div>
    )
  }
}

export default Spectrometer
