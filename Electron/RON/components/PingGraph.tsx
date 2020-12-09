import React, { Component } from "react"
import CSS from "csstype"
import { exec } from "child_process"
import {
  XYPlot,
  XAxis,
  YAxis,
  HorizontalGridLines,
  LineSeries,
} from "react-vis"
import {
  DATAID,
  NetworkDevices,
} from "../../Core/RoveProtocol/RovecommManifest"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "640px",
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
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  margin: "2.5px",
  justifyContent: "space-around",
}
const selector: CSS.Properties = {
  width: "200px",
}

interface IProps {}

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
      interval: setInterval(() => this.ICMP("Drive"), 1000),
    }
    this.ICMP = this.ICMP.bind(this)
    this.boardChange = this.boardChange.bind(this)
  }

  ICMP(device: string): void {
    // If device is not a network device, it must be a board
    let deviceInfo = NetworkDevices[device]
    if (deviceInfo === undefined) {
      deviceInfo = DATAID[device]
    }
    const ip = deviceInfo.Ip

    // Ping command is slightly different for windows/unix
    let pingCommand = ""
    if (process.platform === "win32") {
      pingCommand = `ping -n 1 ${ip}`
    } else {
      pingCommand = `ping -c 1 ${ip}`
    }

    exec(pingCommand, (error, stdout, stderr) => {
      // -1 means not reachable
      let delay = -1
      if (error) {
        console.log(`error: ${error.message}`)
      } else if (stderr) {
        console.log(`stderr: ${stderr}`)
      } else if (stdout.indexOf("unreachable") === -1) {
        // On Windows, failed ping is not necessarily an error
        // Windows Ex. 'Reply from 8.8.8.8: bytes=32 time=26ms TTL=110'
        // Unix Ex. '64 bytes from 8.8.8.8: icmp_seq=0 ttl=110 time=387.477 ms'
        const start = stdout.indexOf("time=") + 5
        const end = stdout.indexOf("ms")
        delay = Math.round(parseFloat(stdout.substring(start, end)))
      }
      const { ping } = this.state
      if (ping.length < 10) {
        ping.push({ x: ping.length + 1, y: delay })
      } else {
        for (let i = 0; i < ping.length - 1; i++) {
          ping[i].y = ping[i + 1].y
        }
        ping[ping.length - 1].y = delay
      }
      this.setState({
        ping,
      })
    })
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    clearInterval(this.state.interval)
    this.setState({
      board,
      ping: [],
      interval: setInterval(() => {
        this.ICMP(board)
      }, 1000),
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Ping Graph</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select
              value={this.state.board}
              onChange={e => this.boardChange(e)}
              style={selector}
            >
              {Object.keys(DATAID).map(item => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                )
              })}
            </select>
          </div>
          <XYPlot style={{ margin: 10 }} width={620} height={480}>
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
