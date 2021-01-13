import React, { Component } from "react"
import CSS from "csstype"
import { exec } from "child_process"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { DATAID, NetworkDevices } from "../../Core/RoveProtocol/RovecommManifest"
import { ColorConverter } from "../../Core/ColorConverter"

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

interface IProps {}

interface IState {
  ping: any
}

// For colorConverter
const min = 0
const cutoff = 45
const max = 100
const greenHue = 120
const redHue = 360

class PingMap extends Component<IProps, IState> {
  canvasRef: any

  constructor(props: IProps) {
    super(props)
    const ping = {}
    Object.keys(NetworkDevices).forEach(device => {
      ping[device] = -1
    })
    Object.keys(DATAID).forEach(board => {
      ping[board] = -1
    })
    this.state = {
      ping,
    }
    this.ICMP = this.ICMP.bind(this)
    this.StartAutoPing = this.StartAutoPing.bind(this)
    this.StartAutoPing(1000)

    this.canvasRef = React.createRef()
  }

  componentDidMount(): void {
    this.updatePingMap()
  }

  updatePingMap(): void {
    let text

    const canvas = this.canvasRef.current
    const context = canvas.getContext("2d")
    context.clearRect(0, 0, canvas.width, canvas.height)

    const centerW = context.canvas.width / 2

    context.font = "12px Arial"
    context.textBaseline = "bottom"

    text = "Basestation Switch"
    context.textAlign = "center"
    context.fillText(text, centerW, 30)
    context.beginPath()
    context.arc(centerW, 50, 20, 0, 2 * Math.PI)
    context.fillStyle =
      this.state.ping.BasestationSwitch === -1
        ? "white"
        : ColorConverter(this.state.ping.BasestationSwitch, min, cutoff, max, greenHue, redHue)
    context.fill()
    context.stroke()

    context.beginPath()
    context.moveTo(centerW, 70)
    context.lineTo(centerW - 40, 110)
    context.moveTo(centerW, 70)
    context.lineTo(centerW + 40, 110)
    context.stroke()

    context.rect(centerW - 60, 110, 40, 40)
    context.stroke()
    context.fillStyle =
      this.state.ping.Basestation5GHzRocket === -1
        ? "white"
        : ColorConverter(this.state.ping.Basestation5GHzRocket, min, cutoff, max, greenHue, redHue)
    context.fill()
    text = "BaseStation 5GHz Rocket"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 65, 130)

    context.rect(centerW + 20, 110, 40, 40)
    context.stroke()
    context.fillStyle =
      this.state.ping.Basestation900MHzRocket === -1
        ? "white"
        : ColorConverter(this.state.ping.Basestation900MHzRocket, min, cutoff, max, greenHue, redHue)
    context.fill()
    text = "BaseStation 900MHz Rocket"
    context.textBaseline = "middle"
    context.textAlign = "left"
    context.fillStyle = "black"
    context.fillText(text, centerW + 65, 130)

    context.beginPath()
    context.moveTo(centerW - 40, 150)
    for (let i = 0; i < 150; i++) {
      const x = 20 - 20 * Math.sin(i * 2 * Math.PI * (2 / 150))
      context.lineTo(x + centerW - 60, i + 150)
    }
    context.stroke()

    context.beginPath()
    context.moveTo(centerW + 40, 150)
    for (let i = 0; i < 150; i++) {
      const x = 20 - 20 * Math.sin(i * 2 * Math.PI * (1 / 150))
      context.lineTo(x + centerW + 20, i + 150)
    }
    context.stroke()

    context.beginPath()
    context.arc(centerW - 40, 320, 20, 0, 2 * Math.PI)
    context.fillStyle =
      this.state.ping.Rover5GHzRocket === -1
        ? "white"
        : ColorConverter(this.state.ping.Rover5GHzRocket, min, cutoff, max, greenHue, redHue)
    context.fill()
    context.stroke()
    text = "Rover 5GHz Rocket"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 65, 320)

    context.beginPath()
    context.arc(centerW + 40, 320, 20, 0, 2 * Math.PI)
    context.fillStyle =
      this.state.ping.Rover900MHzRocket === -1
        ? "white"
        : ColorConverter(this.state.ping.Rover900MHzRocket, min, cutoff, max, greenHue, redHue)
    context.fill()
    context.stroke()
    text = "Rover 900MHz Rocket"
    context.textBaseline = "middle"
    context.textAlign = "left"
    context.fillStyle = "black"
    context.fillText(text, centerW + 65, 320)

    context.beginPath()
    context.moveTo(centerW - 40, 340)
    context.lineTo(centerW, 380)
    context.moveTo(centerW + 40, 340)
    context.lineTo(centerW, 380)
    context.stroke()

    const boards = Object.keys(DATAID)
    const locations = [
      { x: centerW - 20, y: 540 },
      { x: centerW + 20, y: 540 },
      { x: centerW - 60, y: 530 },
      { x: centerW + 60, y: 530 },
      { x: centerW - 95, y: 515 },
      { x: centerW + 95, y: 515 },
      { x: centerW - 130, y: 495 },
      { x: centerW + 130, y: 495 },
      { x: centerW - 155, y: 460 },
      { x: centerW + 155, y: 460 },
      { x: centerW - 170, y: 420 },
      { x: centerW + 170, y: 420 },
      { x: centerW - 150, y: 380 },
      { x: centerW + 150, y: 380 },
    ]
    for (let i = 0; i < locations.length && i < boards.length; i++) {
      const coords = locations[i]
      const board = boards[i]
      context.fillStyle =
        this.state.ping[board] === -1
          ? "white"
          : ColorConverter(this.state.ping[board], min, cutoff, max, greenHue, redHue)
      context.beginPath()
      context.moveTo(centerW, 400)
      context.lineTo(coords.x, coords.y)
      context.stroke()
      context.moveTo(20 * Math.cos((1 / 6) * Math.PI) + coords.x, 20 * Math.sin((1 / 6) * Math.PI) + coords.y)
      context.lineTo(20 * Math.cos((5 / 6) * Math.PI) + coords.x, 20 * Math.sin((5 / 6) * Math.PI) + coords.y)
      context.lineTo(20 * Math.cos((9 / 6) * Math.PI) + coords.x, 20 * Math.sin((9 / 6) * Math.PI) + coords.y)
      context.lineTo(20 * Math.cos((1 / 6) * Math.PI) + coords.x, 20 * Math.sin((1 / 6) * Math.PI) + coords.y)
      context.stroke()
      context.fill()
      context.fillStyle = "black"
      context.textAlign = "center"
      context.fillText(board, coords.x, coords.y + 17)
    }
    context.beginPath()
    context.fillStyle =
      this.state.ping.GrandStream === -1
        ? "white"
        : ColorConverter(this.state.ping.GrandStream, min, cutoff, max, greenHue, redHue)
    context.rect(centerW - 20, 380, 40, 40)
    context.stroke()
    context.fill()
    text = "Router"
    context.textBaseline = "middle"
    context.fillStyle = "black"
    context.textAlign = "center"
    context.fillText(text, centerW, 400)
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
      if (!error && !stderr && stdout.indexOf("unreachable") === -1) {
        // On Windows, failed ping is not necessarily an error
        // Windows Ex. 'Reply from 8.8.8.8: bytes=32 time=26ms TTL=110'
        // Unix Ex. '64 bytes from 8.8.8.8: icmp_seq=0 ttl=110 time=387.477 ms'
        const start = stdout.indexOf("time=") + 5
        const end = stdout.indexOf("ms")
        delay = Math.round(parseFloat(stdout.substring(start, end)))
      }
      this.setState({
        ping: {
          ...this.state.ping,
          [device]: delay,
        },
      })
    })
  }

  StartAutoPing(interval: number): void {
    setInterval(() => {
      for (const device in this.state.ping) {
        if (Object.prototype.hasOwnProperty.call(this.state.ping, device)) {
          this.ICMP(device)
        }
      }
      this.updatePingMap()
    }, interval)
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Ping Tool</div>
        <div style={container}>
          <canvas ref={this.canvasRef} width="640" height="640" />
        </div>
      </div>
    )
  }
}

export default PingMap
