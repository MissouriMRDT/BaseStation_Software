import React, { Component } from "react"
import CSS from "csstype"
import { exec } from "child_process"
import { rovecomm, RovecommManifest, NetworkDevices } from "../../Core/RoveProtocol/Rovecomm"
import { ColorStyleConverter } from "../../Core/ColorConverter"
import { RONModuleWidth } from "./PingGraph"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  fontWeight: "bold",
  textAlign: "center",
  margin: "5px 0px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexWrap: "wrap",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
  alignItems: "center",
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
  justifyContent: "space-between",
}
let labelbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: `min(450px, ${RONModuleWidth}px)`,
  margin: "2.5px",
  justifyContent: "space-between",
  textAlign: "center",
}
const auto: CSS.Properties = {
  width: "10%",
}
const name: CSS.Properties = {
  width: "50%",
  display: "block",
  textOverflow: "ellipsis",
  overflow: "hidden",
  whiteSpace: "nowrap",
}
const num: CSS.Properties = {
  width: "10%",
  textAlign: "center",
  display: "block",
  textOverflow: "ellipsis",
  overflow: "hidden",
  whiteSpace: "nowrap",
}
const buttonText: CSS.Properties = {
  display: "block",
  textOverflow: "ellipsis",
  overflow: "hidden",
  whiteSpace: "nowrap",
}
const but: CSS.Properties = {
  width: "10%",
}
const hid: CSS.Properties = {
  width: "10%",
  visibility: "hidden",
}

interface IProps {
  style?: CSS.Properties
  onDevicesChange: (devices: any) => void
}

interface IState {
  devices: any
  pingInterval: any
}

// For colorConverter
const min = 0
const cutoff = 45
const max = 100
const greenHue = 120
const redHue = 360

class PingTool extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    const devices = {}
    Object.keys(NetworkDevices).forEach(device => {
      devices[device] = { autoPing: false, ping: -1 }
    })
    Object.keys(RovecommManifest).forEach(board => {
      devices[board] = { autoPing: false, ping: -1 }
    })
    this.state = {
      devices,
      pingInterval: setInterval(() => {
        this.props.onDevicesChange(this.state.devices)
        for (const device in this.state.devices) {
          if (this.state.devices[device].autoPing) {
            this.ICMP(device)
          }
        }
      }, 1000),
    }
    this.ICMP = this.ICMP.bind(this)
    this.Rove = this.Rove.bind(this)
    this.AutoPing = this.AutoPing.bind(this)
    this.AutoPingAll = this.AutoPingAll.bind(this)
  }

  componentWillUnmount() {
    clearInterval(this.state.pingInterval)
  }

  ICMP(device: string): void {
    // If device is not a network device, it must be a board
    let deviceInfo = NetworkDevices[device]
    if (deviceInfo === undefined) {
      deviceInfo = RovecommManifest[device]
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
        // They may also return "time<1ms", which we will treat as =1ms
        const start = stdout.indexOf("time") + 5
        const end = stdout.indexOf("ms")
        delay = Math.round(parseFloat(stdout.substring(start, end)))
      }
      this.setState({
        devices: {
          ...this.state.devices,
          [device]: {
            ...this.state.devices[device],
            ping: delay,
          },
        },
      })
    })
  }

  // eslint-disable-next-line class-methods-use-this
  Rove(device: string): void {
    rovecomm.ping(device)
  }

  AutoPing(device: string): void {
    const autoPing = !this.state.devices[device].autoPing
    this.setState({
      devices: {
        ...this.state.devices,
        [device]: {
          ...this.state.devices[device],
          autoPing,
        },
      },
    })
  }

  AutoPingAll(): void {
    const { devices } = this.state
    for (const device in devices) {
      if (Object.prototype.hasOwnProperty.call(devices, device)) {
        devices[device].autoPing = true
      }
    }
    this.setState({
      devices,
    })
    this.props.onDevicesChange(devices)
  }

  render(): JSX.Element {
    selectbox = { ...selectbox, width: `min(450px, ${RONModuleWidth - 15}px)` }
    labelbox = { ...selectbox, width: `min(450px, ${RONModuleWidth - 15}px)` }
    return (
      <div style={{ ...this.props.style, width: RONModuleWidth }}>
        <div style={label}>Ping Tool</div>
        <div style={container}>
          {[
            { category: "Network", list: NetworkDevices, rove: hid },
            { category: "Boards", list: RovecommManifest, rove: but },
          ].map(item => {
            const { category, list, rove } = item
            return (
              <div key={category} style={{ width: RONModuleWidth - 15 }}>
                <div style={h1Style}>{category}</div>
                <div style={labelbox}>
                  <div style={auto}>Auto</div>
                  <div style={name}>Name</div>
                  <div style={num}>IP</div>
                  <div style={num}>Ping</div>
                  <div style={hid}>ICMP</div>
                  <div style={hid}>Rove</div>
                </div>
                {Object.keys(list).map(device => {
                  const { Ip } = list[device]
                  const { ping } = this.state.devices[device]
                  return (
                    <div style={selectbox} key={device}>
                      <input
                        style={auto}
                        type="checkbox"
                        checked={this.state.devices[device].autoPing}
                        onChange={() => this.AutoPing(device)}
                      />
                      <div style={ColorStyleConverter(ping, min, cutoff, max, greenHue, redHue, name)}>{device}</div>
                      <div style={ColorStyleConverter(ping, min, cutoff, max, greenHue, redHue, num)}>
                        {Ip.split(".")[3]}
                      </div>
                      <div style={ColorStyleConverter(ping, min, cutoff, max, greenHue, redHue, num)}>
                        {this.state.devices[device].ping}
                      </div>
                      <button type="button" style={but} onClick={() => this.ICMP(device)}>
                        <div style={buttonText}>ICMP</div>
                      </button>
                      <button type="button" style={rove} onClick={() => this.Rove(device)}>
                        <div style={buttonText}>Rove</div>
                      </button>
                    </div>
                  )
                })}
              </div>
            )
          })}
          <button type="button" onClick={this.AutoPingAll}>
            Auto Ping All
          </button>
        </div>
      </div>
    )
  }
}

export default PingTool
