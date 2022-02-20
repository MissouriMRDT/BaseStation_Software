import React, { Component } from "react"
import CSS from "csstype"
import Cameras from "../../Core/components/Cameras"
import Controls from "./components/Controls"
import StateDiagram from "./components/StateDiagram"
import Activity from "./components/Activity"
import Lighting from "./components/Lighting"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
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
      <div style={{ ...column, padding: "5px" }}>
        <div style={{ ...row, justifyContent: "space-between" }}>
          <div style={{...column, flex: 1, }}>
            <Controls style={{ marginRight: "5px", flexWrap: "wrap" }} selectedWaypoint={this.props.selectedWaypoint} />
            <div style={{ ...row, marginRight: "5px", flexGrow: 1 }}>
              <Activity style={{ flex: 1, marginRight: "5px" }}/>
              <Lighting style={{ flexShrink: 1 }} />
            </div>
          </div>
          <div style={column}>
            <StateDiagram />
          </div>
        </div>
        <div style={row}>
          <Cameras defaultCamera={9} style={{ flex: 1, marginRight: "2.5px" }} />
          <Cameras defaultCamera={10} style={{ flex: 1, marginLeft: "2.5px" }} />
        </div>
      </div>
    )
  }
}
export default Autonomy
