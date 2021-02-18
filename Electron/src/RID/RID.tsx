import React, { Component } from "react"
import CSS from "csstype"
import Cameras from "../Core/components/Cameras"
import Map from "../RED/components/Map"

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

/*
function onClickCamera(this: any): void {
  this.setState({
    display: (
      <div>
        {buttons}
        <Cameras defaultCamera={1}/>
      </div>
    )
  })
}
*/

interface IProps {
  style?: CSS.Properties
  rowcol: string
}

interface IState {
  storedWaypoints: any
  currentCoords: { lat: number; lon: number }
  display: any
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
      <button type="button" style={{ flexGrow: 1 }}>
        3D Rover
      </button>
      <button type="button" style={{ flexGrow: 1 }}>
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
    }
    this.onClickCamera = this.onClickCamera.bind(this)
  }

  onClickCamera() {
    console.log(this)
    this.setState({
      display: (
        <div style={{ flexGrow: 1 }}>
          {this.buttons}
          <Cameras defaultCamera={1} style={{ height: "100%", width: "100%", flexGrow: 1 }} />
        </div>
      ),
    })
  }

  onClickMap() {
    this.setState({
      display: (
        <div style={{ flexGrow: 1 }}>
          {this.buttons}
          <Map
            style={{ height: "100%", width: "100%", flexGrow: 1 }}
            storedWaypoints={this.state.storedWaypoints}
            currentCoords={this.state.currentCoords}
            name="RIDmap"
          />
        </div>
      ),
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
          />
          <RoverImageryDisplay
            rowcol={nextStyle}
            style={{ height: "100%", width: "100%", border: "2px solid", borderColor: "#990000" }}
          />
        </div>
      ),
    })
  }

  render(): JSX.Element {
    console.log(this)
    return <div style={this.props.style}>{this.state.display}</div>
  }
}
export default RoverImageryDisplay
