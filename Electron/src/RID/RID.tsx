import React, { Component } from "react"
import CSS from "csstype"
import Cameras from "../Core/components/Cameras"
import Map from "../RED/components/Map"
import CM3MS from "./components/CM3MS"

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

interface IProps {}

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
        <div style={{ flexGrow: "inherit" }}>
          {this.buttons}
          <Cameras defaultCamera={1} />
        </div>
      ),
    })
  }

  onClickMap() {
    this.setState({
      display: (
        <div style={{ flexGrow: "inherit" }}>
          {this.buttons}
          <Map storedWaypoints={this.state.storedWaypoints} currentCoords={this.state.currentCoords} name="RIDmap" />
        </div>
      ),
    })
  }

  onClickSplit() {
    this.setState({
      display: <div>{this.buttons}</div>,
    })
  }

  render(): JSX.Element {
    console.log(this)
    return <div>{this.state.display}</div>
  }
}
export default RoverImageryDisplay
