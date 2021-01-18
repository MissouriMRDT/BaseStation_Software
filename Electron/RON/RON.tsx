import React, { Component } from "react"
import CSS from "csstype"
import CustomPackets from "./components/CustomPackets"
import PingGraph from "./components/PingGraph"
import PingTool from "./components/PingTool"
import PacketLogger from "./components/PacketLogger"
import PingMap from "./components/PingMap"

const RON: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexWrap: "wrap",
  height: "100vh",
  alignContent: "flex-start",
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
      <div style={RON}>
        <CustomPackets />
        <PingTool />
        <PingGraph />
        <PacketLogger />
        <PingMap />
      </div>
    )
  }
}

export default RoverOverviewOfNetwork
