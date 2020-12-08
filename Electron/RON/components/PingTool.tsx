import React, { Component } from "react"
import CSS from "csstype"
import { exec } from "child_process"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import {
  DATAID,
  NetworkDevices,
} from "../../Core/RoveProtocol/RovecommManifest"

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
  fontFamily: "arial",
  width: "640px",
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
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  margin: "2.5px",
  justifyContent: "space-between",
}
const labelbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  margin: "2.5px",
  justifyContent: "space-between",
  textAlign: "center",
}
const auto: CSS.Properties = {
  width: "10%",
}
const name: CSS.Properties = {
  width: "50%",
}
const num: CSS.Properties = {
  width: "10%",
  textAlign: "center",
}
const but: CSS.Properties = {
  width: "10%",
}
const hid: CSS.Properties = {
  width: "10%",
  visibility: "hidden",
}

interface IProps {}

interface IState {
  devices: any
}

class PingTool extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    const devices = {}
    Object.keys(NetworkDevices).forEach(device => {
      devices[device] = { autoPing: false, ping: -1 }
    })
    Object.keys(DATAID).forEach(board => {
      devices[board] = { autoPing: false, ping: -1 }
    })
    this.state = {
      devices,
    }
    this.ICMP = this.ICMP.bind(this)
    this.Rove = this.Rove.bind(this)
    this.AutoPing = this.AutoPing.bind(this)
    this.AutoPingAll = this.AutoPingAll.bind(this)
    this.StartAutoPing = this.StartAutoPing.bind(this)
    this.StartAutoPing(1000)
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

    console.log(`Pinging ${ip}...`)
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

  Rove(device: string): void {
    console.log(`Rove not yet implemented. Detected ${device}`)
    this.setState({
      devices: {
        ...this.state.devices,
        [device]: {
          ...this.state.devices[device],
          ping: 0,
        },
      },
    })
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
  }

  StartAutoPing(interval: number): void {
    setInterval(() => {
      for (const device in this.state.devices) {
        if (this.state.devices[device].autoPing) {
          this.ICMP(device)
        }
      }
    }, interval)
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Ping Tool</div>
        <div style={container}>
          {[
            { category: "Network", list: NetworkDevices, rove: hid },
            { category: "Boards", list: DATAID, rove: but },
          ].map(item => {
            const { category, list, rove } = item
            return (
              <div key={category}>
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
                  return (
                    <div style={selectbox} key={device}>
                      <input
                        style={auto}
                        type="checkbox"
                        checked={this.state.devices[device].autoPing}
                        onChange={() => this.AutoPing(device)}
                      />
                      <div style={name}>{device}</div>
                      <div style={num}>{Ip.split(".")[3]}</div>
                      <div style={num}>{this.state.devices[device].ping}</div>
                      <button
                        type="button"
                        style={but}
                        onClick={() => this.ICMP(device)}
                      >
                        ICMP
                      </button>
                      <button
                        type="button"
                        style={rove}
                        onClick={() => this.Rove(device)}
                      >
                        Rove
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
