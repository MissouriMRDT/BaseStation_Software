import React, { Component } from "react"
import CSS from "csstype"
import Cameras from "../../Core/components/Cameras"
import Controls from "./components/Controls"
import StateDiagram from "./components/StateDiagram"
import Activity from "./components/Activity"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  flexGrow: 1,
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  marginRight: "5px",
}

interface IProps {
  selectedWaypoint: any
}

interface IState {}

class Autonomy extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <div style={column}>
            <Controls selectedWaypoint={this.props.selectedWaypoint} />
          </div>
        </div>
        <div style={row}>
          <Cameras defaultCamera={9} style={{ flex: 1, marginRight: "5px" }} />
          <Cameras defaultCamera={10} style={{ flex: 1, marginLeft: "5px" }} />
        </div>
      </div>
    )
  }
}
export default Autonomy
