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

function integrate(data: { x: number; y: number }[]): number {
  let left: { x: number; y: number } = data[0]
  let right: { x: number; y: number } = data[1]
  let i: number
  let area = 0
  for (i = 1; i < data.length; i++) {
    area += ((left.y + right.y) / 2) * (right.x - left.x)
    left = right
    right = data[i]
  }

  return area
}

interface IProps {}

interface IState {
  integral: number
  databaseSpectra: { x: number; y: number }[]
  lifeBound: number
  noLifeBound: number
}

class SpectrometerViewer extends Component<IProps, IState> {
  mounted = false

  specTests: SpecDataEntry[]

  testInfos: TestInfoEntry[]

  spectrometers: SpectrometerEntry[]

  constructor(props: IProps) {
    super(props)
    this.specTests = []
    this.testInfos = []
    this.spectrometers = []
    this.state = {
      databaseSpectra: [
        { x: 0, y: 0 },
        { x: 1, y: 1 },
      ],
      integral: 0,
      lifeBound: 0,
      noLifeBound: 0,
    }
    database.createAllTables()

    this.loadSpectra = this.loadSpectra.bind(this)
    this.getSpecTests()

    rovecomm.on("SpectrometerData", () => this.getSpecTests())
  }

  componentDidMount(): void {
    this.mounted = true
  }

  getSpecTests(): void {
    database.retrieveAllTests((succ: boolean, data: SpecDataEntry[]) => {
      if (succ) {
        this.specTests = data
      }
    })

    let lifeCutoff = 0
    let lifeCount = 0
    let noLifeCutoff = 0
    let noLifeCount = 0
    for (const test in this.specTests) {
      if (Object.prototype.hasOwnProperty.call(this.specTests, test)) {
        if (this.specTests[test].life === true) {
          lifeCutoff += this.specTests[test].integral
          lifeCount += 1
        } else if (this.specTests[test].life === false) {
          noLifeCutoff += this.specTests[test].integral
          noLifeCount += 1
        }
      }
    }

    if (this.mounted) {
      this.setState({
        lifeBound: lifeCutoff / lifeCount,
        noLifeBound: noLifeCutoff / noLifeCount,
      })
    } else {
      // You must mutate state directly when component is unmounted
      // eslint-disable-next-line react/no-direct-mutation-state
      this.state = {
        ...this.state,
        lifeBound: lifeCutoff / lifeCount,
        noLifeBound: noLifeCutoff / noLifeCount,
      }
    }
  }

  loadSpectra(event: { target: { value: string } }): void {
    const timestamp = event.target.value

    database.retrieveTestByTimestamp(
      timestamp,
      (succ: boolean, data: SpecDataEntry[]) => {
        if (succ) {
          // Load the spectra
          this.setState({
            databaseSpectra: data[0].data,
            integral: integrate(data[0].data),
          })
        }
      }
    )
  }

  compareIntegral(): string {
    if (this.state.integral > this.state.lifeBound) {
      return "There is life in this sample"
    } else if (this.state.integral < this.state.noLifeBound) {
      return "There is not life in this sample"
    } else {
      return "Cannot predict if there is life in this sample"
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Spectrometer Record Viewer</div>
        <div style={container}>
          <DiscreteColorLegend
            style={{ height: "75px", fontSize: "20px", textAlign: "center" }}
            items={[{ title: "Experiment", strokeWidth: 6 }]}
            orientation="horizontal"
          />
          <div style={{ textAlign: "center" }}>
            Integral: {this.state.integral}
          </div>
          <div style={{ textAlign: "center" }}>{this.compareIntegral()}</div>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
            <HorizontalGridLines style={{ fill: "none" }} />
            <LineSeries
              data={this.state.databaseSpectra}
              style={{ fill: "none" }}
            />
            <XAxis />
            <YAxis />
          </XYPlot>
          <div style={row}>
            <select onChange={this.loadSpectra}>
              {this.specTests.map(item => {
                return (
                  <option key={item.datetime} value={item.datetime}>
                    {item.datetime}
                  </option>
                )
              })}
            </select>
          </div>
        </div>
      </div>
    )
  }
}

export default SpectrometerViewer
