import React, { Component } from "react"
import CSS from "csstype"
import Cameras from "../Core/components/Cameras"
import Map from "../RED/components/Map"
import ThreeDRover from "../Core/components/ThreeDRover"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}
const submod: CSS.Properties = {
  height: "calc(100% - 30px)",
  width: "100%",
  flexGrow: 1,
}

interface IProps {
  style?: CSS.Properties
  rowcol: string
  onMerge?: (display: string) => void
  displayed?: string
}

interface IState {
  storedWaypoints: any
  currentCoords: { lat: number; lon: number }
  display: any
  displayed: string
}

class RoverImageryDisplay extends Component<IProps, IState> {
  waypointsInstance: any

  buttons = (
    <div style={row}>
      <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickCamera()}>
        Camera
      </button>
      <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickMap()}>
        Map
      </button>
      <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickThreeDRover()}>
        3D Rover
      </button>
      <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onMerge()}>
        Merge
      </button>
      <button type="button" style={{ flexGrow: 1 }} onClick={() => this.onClickSplit()}>
        Split
      </button>
    </div>
  )

  constructor(props: IProps) {
    super(props)
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      display: this.buttons,
      displayed: this.props.displayed ? this.props.displayed : "none",
    }
    this.merge = this.merge.bind(this)
  }

  componentDidMount() {
    switch (this.state.displayed) {
      case "Camera":
        this.onClickCamera()
        break
      case "Map":
        this.onClickMap()
        break
      case "3D Rover":
        this.onClickThreeDRover()
        break
      default:
        this.setState({ display: this.buttons, displayed: "none" })
    }
  }

  onClickCamera(cam = 1) {
    this.setState({
      display: (
        <div style={{ flexGrow: 1, height: "100%" }}>
          {this.buttons}
          <Cameras defaultCamera={cam} style={submod} />
        </div>
      ),
      displayed: "Camera",
    })
  }

  onClickThreeDRover() {
    this.setState({
      display: (
        <div style={{ flexGrow: 1, height: "100%" }}>
          {this.buttons}
          <ThreeDRover style={submod} zoom={30} />
        </div>
      ),
      displayed: "3D Rover",
    })
  }

  onClickMap() {
    this.setState({
      display: (
        <div style={{ flexGrow: 1, height: "100%" }}>
          {this.buttons}
          <Map
            style={submod}
            storedWaypoints={this.state.storedWaypoints}
            currentCoords={this.state.currentCoords}
            name="RIDmap"
          />
        </div>
      ),
      displayed: "Map",
    })
  }

  onClickSplit() {
    const RIDStyle = this.props.rowcol === "row" ? row : column
    const nextStyle = this.props.rowcol === "row" ? "column" : "row"
    this.setState({
      display: (
        <div style={{ ...RIDStyle, width: "100%", height: "100%" }}>
          <RoverImageryDisplay
            rowcol={nextStyle}
            style={{ height: "100%", width: "100%", border: "2px solid", borderColor: "#990000" }}
            onMerge={this.merge}
            displayed={this.state.displayed}
          />
          <RoverImageryDisplay
            rowcol={nextStyle}
            style={{ height: "100%", width: "100%", border: "2px solid", borderColor: "#990000" }}
            onMerge={this.merge}
          />
        </div>
      ),
    })
  }

  onMerge() {
    if (typeof this.props.onMerge === "function") {
      this.props.onMerge(this.state.displayed)
    }
  }

  merge(display: string) {
    switch (display) {
      case "Camera":
        this.onClickCamera()
        break
      case "Map":
        this.onClickMap()
        break
      case "3D Rover":
        this.onClickThreeDRover()
        break
      default:
        this.setState({ display: this.buttons, displayed: "none" })
    }
  }

  render(): JSX.Element {
    return <div style={this.props.style}>{this.state.display}</div>
  }
}
export default RoverImageryDisplay
