import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import { RovecommManifest } from "../RoveProtocol/Rovecomm"
import { windows } from "../Window"
// import { rovecomm } from "../RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
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
  height: "calc(100% - 45px)",
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
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}

const cam: CSS.Properties = {
  height: "calc(100% - 60px)",
}

function downloadURL(imgData: string): void {
  const a = document.createElement("a")
  a.href = imgData.replace("image/png", "image/octet-stream")
  a.download = "camera.png"
  a.click()
}

function saveImage(): void {
  const input = document.getElementById("camera")
  if (!input) {
    throw new Error("The element 'camera' wasn't found")
  }
  html2canvas(input, {
    scrollX: 0,
    scrollY: -window.scrollY,
    letterRendering: 1,
    useCORS: true,
    allowTaint: false,
  })
    .then(canvas => {
      const imgData = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream")
      downloadURL(imgData)
      return null
    })
    .catch(error => {
      console.error(error)
    })
}

interface IProps {
  defaultCamera: number
  style?: CSS.Properties
  maxHeight?: number
}

interface IState {
  currentCamera: number
  cameraIps: string[]
  rotation: number
  style: CSS.Properties
  id: number
}

class Cameras extends Component<IProps, IState> {
  static id = 0

  constructor(props: IProps) {
    super(props)
    this.state = {
      currentCamera: this.props.defaultCamera,
      cameraIps: [RovecommManifest.Camera1.Ip, RovecommManifest.Camera2.Ip, RovecommManifest.Autonomy.Ip],
      rotation: 0,
      style: {},
      id: Cameras.id,
    }
    Cameras.id += 1
    // rovecomm.sendCommand(Packet(dataId, data), reliability)
  }

  ConstructAddress() {
    const index = Math.floor((this.state.currentCamera - 1) / 4)
    const camera = ((this.state.currentCamera - 1) % 4) + 1
    const ip = this.state.cameraIps[index]
    return `http://${ip}:8080/${camera}/stream`
  }

  rotate(): void {
    let style: CSS.Properties
    let { rotation } = this.state
    rotation = (rotation + 90) % 360

    let width = 0
    let height = 0

    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        width = windows[win].document.getElementById(this.state.id).clientWidth
        height = windows[win].document.getElementById(this.state.id).clientHeight
        break
      }
    }

    const Xfactor = height / width
    const Yfactor = width / height

    switch (rotation) {
      case 90:
        style = { transform: `rotate(90deg) scaleX(${Xfactor}) scaleY(${Yfactor})` }
        break
      case 180:
        style = { transform: "rotate(180deg)" }
        break
      case 270:
        style = { transform: `rotate(270deg) scaleX(${Xfactor}) scaleY(${Yfactor})` }
        break
      default:
        style = { transform: "" }
        break
    }

    this.setState({ rotation, style })
  }

  render(): JSX.Element {
    return (
      <div id="camera" style={{ ...this.props.style, maxHeight: `${this.props.maxHeight}px` }}>
        <div style={label}>Cameras</div>
        <div style={container}>
          <div style={row}>
            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(num => {
              return (
                <button
                  type="button"
                  key={num}
                  onClick={() => this.setState({ currentCamera: num })}
                  style={{ flexGrow: 1 }}
                >
                  <h1 style={h1Style}>{num}</h1>
                </button>
              )
            })}
          </div>
          <img
            src={this.ConstructAddress()}
            alt={`Camera ${this.state.currentCamera}`}
            style={{ ...cam, ...this.state.style }}
            id={this.state.id.toString()}
          />
          <div style={row}>
            <button type="button" onClick={() => saveImage()} style={{ flexGrow: 1 }}>
              Screenshot
            </button>
            <button type="button" onClick={() => this.rotate()} style={{ flexGrow: 1 }}>
              Rotate
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Cameras
