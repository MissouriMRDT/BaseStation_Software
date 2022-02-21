import React, { Component } from "react"
import CSS from "csstype"
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries } from "react-vis"
import { RovecommManifest } from "../../Core/RoveProtocol/Rovecomm"
import { windows } from "../../Core/Window"

// eslint-disable-next-line import/no-mutable-exports
export let RONModuleWidth: number = window.document.documentElement.clientWidth / 2 - 10

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
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
let selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: `min(450px, ${RONModuleWidth}px)`,
  margin: "2.5px",
  justifyContent: "space-around",
}
const selector: CSS.Properties = {
  width: "200px",
}

interface IProps {
  devices: any
  style?: CSS.Properties
}

interface IState {
  ping: {
    x: number
    y: number
  }[]
  board: string
  interval: any
}

class PingGraph extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      ping: [],
      board: "Drive",
      interval: setInterval(() => this.update("Drive"), 1000),
    }
    this.update = this.update.bind(this)
    this.boardChange = this.boardChange.bind(this)
  }

  componentWillUnmount() {
    clearInterval(this.state.interval)
  }

  update(device: string): void {
    const { ping } = this.state
    if (ping.length < 10) {
      if (device in this.props.devices && "ping" in this.props.devices[device] && this.props.devices[device].ping) {
        ping.push({ x: ping.length + 1, y: this.props.devices[device].ping })
      } else {
        ping.push({ x: ping.length + 1, y: -1 })
      }
    } else {
      for (let i = 0; i < ping.length - 1; i++) {
        ping[i].y = ping[i + 1].y
      }

      if (device in this.props.devices && "ping" in this.props.devices[device] && this.props.devices[device].ping) {
        ping[ping.length - 1].y = this.props.devices[device].ping
      } else {
        ping[ping.length - 1].y = -1
      }
    }
    this.setState({
      ping,
    })
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    clearInterval(this.state.interval)
    this.setState({
      board,
      ping: [],
      interval: setInterval(() => {
        this.update(board)
      }, 1000),
    })
  }

  render(): JSX.Element {
    selectbox = { ...selectbox, width: `min(450px, ${RONModuleWidth - 15}px)` }

    // At render time, we search for the RON window so that we can properly scale down the size of these
    // components if the width of this monitor is known
    if ("Rover Overview of Network" in windows) {
      RONModuleWidth = windows["Rover Overview of Network"].document.documentElement.clientWidth / 2 - 10
    }

    return (
      <div style={{ ...this.props.style, width: RONModuleWidth }}>
        <div style={label}>Ping Graph</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select value={this.state.board} onChange={e => this.boardChange(e)} style={selector}>
              {Object.keys(RovecommManifest).map(item => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                )
              })}
            </select>
          </div>
          <XYPlot style={{ margin: 10 }} width={RONModuleWidth - 10} height={300}>
            <HorizontalGridLines style={{ fill: "none" }} />
            <LineSeries data={this.state.ping} style={{ fill: "none" }} />
            <XAxis />
            <YAxis />
          </XYPlot>
        </div>
      </div>
    )
  }
}

export default PingGraph
