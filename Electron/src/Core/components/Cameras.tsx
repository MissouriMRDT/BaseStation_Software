import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
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
}

interface IState {
  currentCamera: number
  baseAddress: string[]
  rotation: number
}

class Cameras extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      currentCamera: this.props.defaultCamera,
      baseAddress: [
        "http://192.168.1.50:8080",
        "http://192.168.1.51:8080",
        "http://192.168.1.139:8081",
        "http://192.168.1.139:8082",
      ],
      rotation: 0,
    }
    // rovecomm.sendCommand(Packet(dataId, data), reliability)
  }

  ConstructAddress() {
    const index = Math.floor((this.state.currentCamera - 1) / 4)
    const camera = ((this.state.currentCamera - 1) % 4) + 1
    const addr = this.state.baseAddress[index]
    if (this.state.currentCamera === 9) {
      return this.state.baseAddress[2]
    }
    if (this.state.currentCamera === 10) {
      return this.state.baseAddress[3]
    }
    return `${addr}/${camera}/stream`
  }

  rotate(): void {
    this.setState({
      rotation: this.state.rotation + (90 % 360),
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
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
          {/*
            NOTE: This is a theory. If this image somehow exceeds a 
            certain dimension/width, whether it would be us increasing
            the resolution of the target computer, or adding styles,
            the entire app just fails to load without giving any
            obvious error.

            I'd check the electron-react-boilerplate repo to see
            if people are having issues with img elements
          */}
          <img
            id="camera"
            src={this.ConstructAddress()}
            alt={`Camera ${this.state.currentCamera}`}
            style={{ transform: `rotate(${this.state.rotation}deg)` }}
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
