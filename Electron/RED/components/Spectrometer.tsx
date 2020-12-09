import React, { Component } from "react"
import CSS from "csstype"
import {
  XYPlot,
  XAxis,
  YAxis,
  HorizontalGridLines,
  LineSeries,
  DiscreteColorLegend,
} from "react-vis"

import {
  database,
  SpectrometerEntry,
  SpecDataEntry,
  TestInfoEntry,
} from "../../Core/Spectrometer/Database"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  justifyContent: "center",
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
  alignItems: "center",
  justifyContent: "center",
  margin: "10px",
}

interface IProps {}

interface IState {
  control: { x: number; y: number }[]
  experiment: { x: number; y: number }[]
  difference: { x: number; y: number }[]
  gathering: string
  counter: number
  integral: number
  // These are all calibrated constants and should be determined for each spectrometer
  A0: number
  B1: number
  B2: number
  B3: number
  B4: number
  B5: number
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
      integral: 0,
      A0: 2.98820453e2,
      B1: 2.402512393,
      B2: -9.234466848e-4,
      B3: -4.648308454e-6,
      B4: 2.090258796e-10,
      B5: 1.496304653e-11,
    }
    this.getControl = this.getControl.bind(this)
    this.getSpectra = this.getSpectra.bind(this)
    this.SpectrometerData = this.SpectrometerData.bind(this)
    this.calcWavelength = this.calcWavelength.bind(this)

    // Create database, if doesnt exist
    database.createAllTables()

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

  calcWavelength(pixel: number): number {
    return (
      this.state.A0 +
      this.state.B1 * pixel +
      this.state.B2 * pixel ** 2 +
      this.state.B3 * pixel ** 3 +
      this.state.B4 * pixel ** 4 +
      this.state.B5 * pixel ** 5
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
          control.push({ x: this.calcWavelength(i + offset), y: data[i] })
        }
        // Hack to correct x-axis immediately
        experiment = [
          { x: this.calcWavelength(0), y: 0 },
          { x: this.calcWavelength(1), y: 1 },
        ]
        difference = [
          { x: this.calcWavelength(0), y: 0 },
          { x: this.calcWavelength(1), y: 1 },
        ]
        break
      case "spectra":
        if (this.state.counter === 0) {
          experiment = []
          difference = []
          offset = 0
        }
        for (let i = 0; i < data.length; i++) {
          experiment.push({ x: this.calcWavelength(i + offset), y: data[i] })
          difference.push({
            x: this.calcWavelength(i + offset),
            y: experiment[i + offset].y - control[i + offset].y,
          })
        }

        if (this.state.counter) {
          // Insert this complete spectra
          Spectrometer.insertSpecData(difference)

          // Update local understanding of data
          this.getSpecTests()
        }
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
      integral: this.Integrate(difference),
    })
  }

  Integrate(data: { x: number; y: number }[]): number {
    let left: { x: number; y: number } = data[0]
    let right: { x: number; y: number } = data[1]
    let i: number
    let area = 0
    for (i = 1; i < this.state.difference.length; i++) {
      area += ((left.y + right.y) / 2) * (right.x - left.x)
      left = right
      right = this.state.difference[i]
    }

    return area
  }

  CompareIntegral(): string {
    let cutoff = 0

    for (const test in this.specTests) {
      if (Object.prototype.hasOwnProperty.call(this.specTests, test)) {
        cutoff += this.Integrate(this.specTests[test].data)
      }
    }

    cutoff /= this.specTests.length
    console.log(cutoff)

    if (this.state.integral > cutoff) {
      return "There is life in this sample"
    }
    return "There is not life in this sample"
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Spectrometer</div>
        <div style={container}>
          <DiscreteColorLegend
            style={{ height: "75px", fontSize: "20px", textAlign: "center" }}
            items={[
              { title: "Control", strokeWidth: 6 },
              { title: "Experiment", strokeWidth: 6 },
              { title: "Difference", strokeWidth: 6 },
            ]}
            orientation="horizontal"
          />
          <div style={{ textAlign: "center" }}>
            Integral: {this.state.integral}
          </div>
          <div style={{ textAlign: "center" }}>{this.CompareIntegral()}</div>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
            <HorizontalGridLines style={{ fill: "none" }} />
            <LineSeries data={this.state.control} style={{ fill: "none" }} />
            <LineSeries data={this.state.experiment} style={{ fill: "none" }} />
            <LineSeries data={this.state.difference} style={{ fill: "none" }} />
            <XAxis />
            <YAxis />
          </XYPlot>
          <div style={row}>
            <button
              type="button"
              style={{ width: "100px", marginRight: "10px" }}
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
      </div>
    )
  }
}

export default Spectrometer
