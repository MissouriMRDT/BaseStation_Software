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
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}

interface IProps {}

interface IState {
  devices: any
}

class RoverOverviewOfNetwork extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      devices: {},
    }
    this.updateDevices = this.updateDevices.bind(this)
  }

  updateDevices(devices: any) {
    this.setState({ devices })
  }

  render(): JSX.Element {
    return (
      <div style={row}>
        <div style={column}>
          <PingGraph devices={this.state.devices} />
          <PingMap devices={this.state.devices} />
          <PacketLogger />
        </div>
        <div style={column}>
          <CustomPackets />
          <PingTool onDevicesChange={this.updateDevices} />
        </div>
      </div>
    )
  }
}
export default RoverOverviewOfNetwork
