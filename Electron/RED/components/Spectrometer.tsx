import React, { Component } from "react"
import html2canvas from "html2canvas"
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
  justifyContent: "space-around",
  margin: "10px",
}
const modal: CSS.Properties = {
  position: "absolute",
  marginTop: "-350px",
  width: "500px",
  zIndex: 1,
  backgroundColor: "rgba(255,255,255,0.9)",
  border: "2px solid #990000",
  textAlign: "center",
  borderRadius: "25px",
}
const modalButton: CSS.Properties = {
  width: "75px",
  lineHeight: "24px",
  color: "white",
  fontWeight: "bold",
  borderRadius: "10px",
  border: "none",
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

function insertSpecData(
  lifePresent: boolean,
  data: { x: number; y: number }[]
): void {
  database.insertData(
    new SpecDataEntry(
      1,
      new Date().toLocaleString(),
      lifePresent,
      integrate(data),
      data
    )
  )
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

  addToDatabase: boolean
  lifeBound: number
  noLifeBound: number
}

class Spectrometer extends Component<IProps, IState> {
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
      A0: 3.033554037e2,
      B1: 2.719323032,
      B2: -1.242349083e-3,
      B3: -7.181891333e-6,
      B4: 9.029223723e-9,
      B5: 2.829824434e-12,
      addToDatabase: false,
      lifeBound: 0,
      noLifeBound: 0,
    }
    this.getControl = this.getControl.bind(this)
    this.getSpectra = this.getSpectra.bind(this)
    this.spectrometerData = this.spectrometerData.bind(this)
    this.calcWavelength = this.calcWavelength.bind(this)

    // Create database, if doesnt exist
    database.createAllTables()

    this.getSpecTests()
    this.getTestInfos()
    this.getSpectrometers()

    rovecomm.on("SpectrometerData", (data: any) => this.spectrometerData(data))
  }

  componentDidMount(): void {
    // Boolean for if component has mounted or not to determine if
    // this.state = {} or this.setState({}) should be used.
    this.mounted = true
  }

  getControl(): void {
    rovecomm.resubscribe()
    this.setState({
      gathering: "control",
      counter: 0,
    })
    rovecomm.sendCommand("RunSpectrometer", [1])
  }

  getSpectra(): void {
    rovecomm.resubscribe()
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

    // Whenever we get more tests, we want to update our bounds being used
    // to predict if there is life in a sample or not

    // Create 2 counters and 2 accumulators
    let lifeCutoff = 0
    let lifeCount = 0
    let noLifeCutoff = 0
    let noLifeCount = 0

    for (const test in this.specTests) {
      if (Object.prototype.hasOwnProperty.call(this.specTests, test)) {
        // If our records show this sample to contain life, add it to the
        // variables calculating average integral of a sample with life
        if (this.specTests[test].life === true) {
          lifeCutoff += this.specTests[test].integral
          lifeCount += 1
        } else if (this.specTests[test].life === false) {
          noLifeCutoff += this.specTests[test].integral
          noLifeCount += 1
        }
      }
    }

    // Calculate the average integral of both life and noLife samples
    // and use that as the bound to predict life
    // Note, this method of determining the bound is subject to change.
    const lifeBound = lifeCutoff / lifeCount
    const noLifeBound = noLifeCutoff / noLifeCount

    if (this.mounted) {
      this.setState({
        lifeBound,
        noLifeBound,
      })
    } else {
      // You must mutate state directly when component is unmounted,
      // so use the spread operator to maintain other states
      // eslint-disable-next-line react/no-direct-mutation-state
      this.state = {
        ...this.state,
        lifeBound,
        noLifeBound,
      }
    }
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

  postSpectra(lifePresent: boolean): void {
    // Insert this complete spectra
    insertSpecData(lifePresent, this.state.difference)

    // Update local understanding of data
    this.getSpecTests()

    // And hide the modal
    this.setState({ addToDatabase: false })
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

  saveImage(): void {
    const input = document.getElementById("canvas")
    if (!input) {
      throw new Error("The element 'canvas' wasn't found")
    }
    html2canvas(input, {
      scrollX: 0,
      scrollY: -window.scrollY,
    })
      .then(canvas => {
        const imgData = canvas
          .toDataURL("image/png")
          .replace("image/png", "image/octet-stream")
        this.downloadURL(imgData)
        return null
      })
      .catch(error => {
        console.error(error)
      })
  }

  downloadURL(imgData: string): void {
    this.setState({
      gathering: "graphImage",
      counter: 0,
    })
    const a = document.createElement("a")
    a.href = imgData.replace("image/png", "image/octet-stream")
    a.download = "graph.png"
    a.click()
  }

  spectrometerData(data: number[]): void {
    let { control, experiment, difference } = this.state
    let offset = 144

    // This function will be called when any spectrometer data is collected,
    // so we need a method of determining whether it is a control or actual spectra
    // so we switch on a variable set when the command is sent to the rover
    switch (this.state.gathering) {
      case "control":
        // We also keep track of a counter of incoming packets, as the spectra
        // is too large to fit in one rovecomm packet and is split across 2
        // so if the counter is set to 0, this is the first packet and we want
        // to reset the control data list and there should be no offset
        // (The offset is set to 144 above to shift the second set of data's x values)
        if (this.state.counter === 0) {
          control = []
          offset = 0
        }

        for (let i = 0; i < data.length; i++) {
          // calcWavelength must be called on the index of each datum
          // as the spectrometer doesn't tell us the X but rather uses a precisely
          // calibrated function to determine it
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
        break
      default:
        break
    }

    // No matter which packet we recieved, we want to toggle our packet counter
    // such that if the first packet was recieved we know we are looking for the second
    // or if the second is recieved we are again ready to recieve a spectra pair
    const counter = this.state.counter === 0 ? 1 : 0
    this.setState({
      control,
      experiment,
      difference,
      counter,
      integral: integrate(difference), // update the integral calc for the new difference
    })
  }

  compareIntegral(): string {
    // Used to display predicted life in the sample
    // this shouldn't change even if our bound calculation changes
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
      <div id="canvas">
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
          <div style={{ textAlign: "center" }}>{this.compareIntegral()}</div>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
            <HorizontalGridLines style={{ fill: "none" }} />
            <LineSeries data={this.state.control} style={{ fill: "none" }} />
            <LineSeries data={this.state.experiment} style={{ fill: "none" }} />
            <LineSeries data={this.state.difference} style={{ fill: "none" }} />
            <XAxis />
            <YAxis />
          </XYPlot>
          <div style={row}>
            <button type="button" onClick={() => this.saveImage()}>
              Export Graph
            </button>
            <button type="button" onClick={this.getControl}>
              Grab Control
            </button>
            <button type="button" onClick={this.getSpectra}>
              Grab Spectra
            </button>
            <button
              type="button"
              onClick={() => this.setState({ addToDatabase: true })}
            >
              Add to Database
            </button>
            {this.state.addToDatabase ? (
              <div style={modal}>
                <h1>Was there life in this sample?</h1>
                <div style={row}>
                  <button
                    type="button"
                    style={{ ...modalButton, backgroundColor: "red" }}
                    onClick={() => this.postSpectra(false)}
                  >
                    NO
                  </button>
                  <button
                    type="button"
                    style={{ ...modalButton, backgroundColor: "green" }}
                    onClick={() => this.postSpectra(true)}
                  >
                    YES
                  </button>
                </div>
              </div>
            ) : null}
          </div>
        </div>
      </div>
    )
  }
}

export default Spectrometer
