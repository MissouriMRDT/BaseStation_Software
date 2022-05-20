import React, { Component } from "react"
import CSS from "csstype"
import html2canvas from "html2canvas"
import fs from "fs"

import { windows } from "../Window"
import no_cam_img from "../../../assets/no_cam_img.png"

const cameraNumList = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
const cam_ips = [
  no_cam_img,
  `http://192.168.1.141:1181/stream.mjpg`,
  `http://192.168.1.141:1182/stream.mjpg`,
  `http://192.168.1.141:1183/stream.mjpg`,
  `http://192.168.1.141:1184/stream.mjpg`,
  `http://192.168.1.142:1181/stream.mjpg`,
  `http://192.168.1.142:1182/stream.mjpg`,
  `http://192.168.1.142:1183/stream.mjpg`,
  `http://192.168.1.142:1184/stream.mjpg`,
  `http://192.168.1.139:1181/stream.mjpg`,
  `http://192.168.1.139:1182/stream.mjpg`,
]

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
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}

const cam: CSS.Properties = {
  height: "calc(100% - 60px)",
}

interface IProps {
  defaultCamera: number
  style?: CSS.Properties
}

interface IState {
  currentCamera: number
  rotation: number
  style: CSS.Properties
  id: string
}

class Cameras extends Component<IProps, IState> {
  // Static id to create unique camera identifiers
  static id = 0

  constructor(props: IProps) {
    super(props)
    this.state = {
      currentCamera: this.props.defaultCamera,
      rotation: 0,
      style: {},
      id: `Camera ${Cameras.id}`,
    }
    Cameras.id += 1
  }

  rotate(): void {
    // Rotate will rotate clockwise by 90 degrees and then set up the other proper
    // transformations necessary to make the feed fit in the box
    let { rotation } = this.state
    rotation = (rotation + 90) % 360

    let width = 0
    let height = 0

    for (const win of Object.keys(windows)) {
      // To find the size of the camera feed, we must loop through all the different
      // windows documents and look for the camera id
      if (windows[win].document.getElementById(this.state.id)) {
        width = windows[win].document.getElementById(this.state.id).clientWidth
        height = windows[win].document.getElementById(this.state.id).clientHeight
        break
      }
    }

    // Compute ratios of height : width and width : height
    const Xfactor = height / width
    const Yfactor = width / height

    // Create a style: transformation string where we rotate the proper number of deg
    let transform = `rotate(${rotation}deg)`
    // And if the camera is sideways, properly scale down the width & height to fit
    if (rotation === 90 || rotation === 270) {
      transform += `scaleX(${Xfactor}) scaleY(${Yfactor})`
    }

    this.setState({ rotation, style: { transform } })
  }

  saveImage(): void {
    // Initial handler to start saving an image of the camera feed

    // Look through all of the window documents for the current camera feed
    let camera
    let thisWindow
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        thisWindow = windows[win]
        camera = thisWindow.document.getElementById(this.state.id)
        break
      }
    }

    if (!camera) {
      throw new Error(`The element '${camera}' wasn't found`)
    }

    // Then use package to turn the html into a canvas, which when complete calls downloadURL
    // to save it as an image
    // NOTE: This package seems to have issues with the dynamic nature of the camera feeds, and
    // is only able to return the html for the first few seconds of operation
    html2canvas(camera, {
      scrollX: 0,
      scrollY: -thisWindow.scrollY,
      useCORS: true,
      allowTaint: true,
    })
      .then(canvas => {
        const imgData = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream")
        this.downloadURL(imgData)
        return null
      })
      .catch(error => {
        console.error(error)
      })
  }

  downloadURL(imgData: string): void {
    // Takes in the octet-string of a camera feed and saves it as an image

    // The camera feed will be saved in the applications Screenshots folder with the filename
    // YYYY.MM.DD.HH.SS.sss.Cam{camNum}.png
    const filename = `./Screenshots/${new Date()
      .toISOString()
      // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
      // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
      .replaceAll(/[:\-TZ]/g, ".")}Cam${this.state.currentCamera}.png`

    // Create the screenshots directory if it doesn't exist
    if (!fs.existsSync("./Screenshots")) {
      fs.mkdirSync("./Screenshots")
    }

    // Encode the camera feed and write it to a file
    const base64Image = imgData.replace("image/png", "image/octet-stream").split(";base64,").pop()
    fs.writeFileSync(filename, base64Image!, { encoding: "base64" })
  }

  refresh(): void {
    //changes camera to 0 feed (default no camera), then changes back to current camera
    //uses setstate callback function to assure that the camera changes to 0 before changing it back
    let curCam = this.state.currentCamera
    this.setState({ currentCamera: 0 }, () => this.setState({ currentCamera: curCam }))
  }

  render(): JSX.Element {
    return (
      <div id="camera" style={this.props.style}>
        <div style={label}>Cameras</div>
        <div style={container}>
          <div style={column}>
            <div style={row}>
              {cameraNumList.map(num => {
                return (
                  <button
                    type="button"
                    key={num}
                    onClick={() => this.setState({ currentCamera: num })}
                    style={{
                      flexGrow: 1,
                      borderWidth: this.state.currentCamera == num ? "medium" : "thin",
                    }}
                  >
                    <h1 style={h1Style}>{num}</h1>
                  </button>
                )
              })}
            </div>
            <img
              src={cam_ips[this.state.currentCamera]}
              alt={`Camera ${this.state.currentCamera}`}
              style={{ ...cam, ...this.state.style }}
              id={this.state.id}
            />
            <div style={row}>
              <button type="button" onClick={() => this.saveImage()} style={{ width: "50%" }}>
                Screenshot
              </button>
              <button type="button" onClick={() => this.rotate()} style={{ width: "50%" }}>
                Rotate
              </button>
            </div>
            <div style={row}>
              <button type="button" onClick={() => this.setState({ currentCamera: 0 })} style={{ width: "50%" }}>
                Stop Listening
              </button>
              <button type="button" onClick={() => this.refresh()} style={{ width: "50%" }}>
                Refresh
              </button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Cameras
