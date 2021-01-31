import React, { Component } from "react"
import CSS from "csstype"
import CustomPackets from "./components/CustomPackets"
import PingGraph from "./components/PingGraph"
import PingTool from "./components/PingTool"
import PacketLogger from "./components/PacketLogger"
import PingMap from "./components/PingMap"

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

interface IProps {}

interface IState {}

class RoverOverviewOfNetwork extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={row}>
        <div style={column}>
          <PingGraph />
          <PingMap />
          <PacketLogger />
        </div>
        <div style={column}>
          <CustomPackets />
          <PingTool />
        </div>
      </div>
    )
  }
}
export default RoverOverviewOfNetwork
