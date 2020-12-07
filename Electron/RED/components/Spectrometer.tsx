import React, { Component } from "react"
import CSS from "csstype"
import {
  XYPlot,
  XAxis,
  YAxis,
  HorizontalGridLines,
  LineSeries,
} from "react-vis"

import {
  database,
  SpectrometerEntry,
  SpecDataEntry,
  TestInfoEntry,
} from "../../Core/Spectrometer/Database"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

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
  control: { x: number; y: number }[]
  experiment: { x: number; y: number }[]
  difference: { x: number; y: number }[]
  gathering: string
  counter: number
}

class Spectrometer extends Component<IProps, IState> {
  static insertSpecData(data: { x: number; y: number }[]): void {
    database.insertData(new SpecDataEntry(1, new Date().toLocaleString(), data))
  }

  specTests: SpecDataEntry[]

  testInfos: TestInfoEntry[]

  spectrometers: SpectrometerEntry[]

  constructor(props: any) {
    super(props)
    this.specTests = []
    this.testInfos = []
    this.spectrometers = []
    this.state = {
      control: [
        { x: 0, y: 0 },
        { x: 1, y: 1 },
      ],
      experiment: [
        { x: 0, y: 0 },
        { x: 1, y: 1 },
      ],
      difference: [
        { x: 0, y: 0 },
        { x: 1, y: 1 },
      ],
      gathering: "none",
      counter: 0,
    }
    this.getControl = this.getControl.bind(this)
    this.getSpectra = this.getSpectra.bind(this)
    this.SpectrometerData = this.SpectrometerData.bind(this)

    rovecomm.on("SpectrometerData", (data: any) => this.SpectrometerData(data))
  }

  getControl(): void {
    this.setState({
      gathering: "control",
      counter: 0,
    })
    rovecomm.sendCommand("RunSpectrometer", [1])
  }

  getSpectra(): void {
    this.setState({
      gathering: "spectra",
      counter: 0,
    })

    rovecomm.sendCommand("RunSpectrometer", [1])
  }

  getSpecTests(): void {
    database.retrieveAllTests((succ: boolean, data: SpecDataEntry[]) => {
      if (succ) {
        this.specTests = data
      }
    })
  }

  getTestInfos(): void {
    database.retrieveAllTestInfo((succ: boolean, data: TestInfoEntry[]) => {
      if (succ) {
        this.testInfos = data
      }
    })
  }

  getSpectrometers(): void {
    database.retrieveSpectrometers(
      (succ: boolean, data: SpectrometerEntry[]) => {
        if (succ) {
          this.spectrometers = data
        }
      }
    )
  }

  SpectrometerData(data: number[]): void {
    let { control, experiment, difference } = this.state
    let offset = 144
    switch (this.state.gathering) {
      case "control":
        if (this.state.counter === 0) {
          control = []
          offset = 0
        }
        for (let i = 0; i < data.length; i++) {
          control.push({ x: i + offset, y: data[i] })
        }
        break
      case "spectra":
        if (this.state.counter === 0) {
          experiment = []
          difference = []
          offset = 0
        }
        for (let i = 0; i < data.length; i++) {
          experiment.push({ x: i + offset, y: data[i] })
          difference.push({
            x: i + offset,
            y: experiment[i + offset].y - control[i + offset].y,
          })
        }

        // Insert this new data
        Spectrometer.insertSpecData(experiment)

        // Update local understanding of data
        this.getSpecTests()

        break
      default:
        break
    }

    const counter = this.state.counter === 0 ? 1 : 0
    this.setState({
      control,
      experiment,
      difference,
      counter,
    })
  }

  Integrate(): number {
    let left: { x: number; y: number } = this.state.difference[0]
    let right: { x: number; y: number } = this.state.difference[1]
    let i: number
    let area = 0
    for (i = 1; i < this.state.difference.length; i++) {
      area += ((left.y + right.y) / 2) * (right.x - left.x)
      left = right
      right = this.state.difference[i]
    }
    return area
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Spectrometer</div>
        <div style={container}>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
            <HorizontalGridLines stylee={{ fill: "none" }} />
            <LineSeries data={this.state.control} style={{ fill: "none" }} />
            <LineSeries data={this.state.experiment} style={{ fill: "none" }} />
            <LineSeries data={this.state.difference} style={{ fill: "none" }} />
            <XAxis />
            <YAxis />
          </XYPlot>
          <div style={{ margin: "0px 250px" }}>{this.Integrate()}</div>
          <button
            type="button"
            style={{ width: "100px" }}
            onClick={this.getControl}
          >
            Grab Control
          </button>
          <button
            type="button"
            style={{ width: "100px" }}
            onClick={this.getSpectra}
          >
            Grab Spectra
          </button>
        </div>
      </div>
    )
  }
}

export default Spectrometer
