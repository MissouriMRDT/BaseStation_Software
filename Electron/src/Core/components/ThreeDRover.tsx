import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import { rovecomm } from "../RoveProtocol/Rovecomm"
import STLViewer from "./STLViewer"
import { windows } from "../Window"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
  alignItems: "center",
  height: "calc(100% - 47px)",
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

const RoverFile = path.join(__dirname, "../assets/Rover.stl")

interface IProps {
  style?: CSS.Properties
  zoom: number
}

interface IState {
  IMUData: number[]
  id: string
  width: number
  height: number
}

class ThreeDRover extends Component<IProps, IState> {
  static defaultProps = {
    zoom: 20,
  }

  static id = 0

  constructor(props: any) {
    super(props)
    this.state = {
      IMUData: [0, 0, 0],
      id: `3DRover_${ThreeDRover.id}`,
      width: 300,
      height: 150,
    }

    ThreeDRover.id += 1

    rovecomm.on("IMUData", (data: any) => this.IMUData(data))
  }

  componentDidMount() {
    this.findWidth()
  }

  IMUData(data: any) {
    // We discard the yaw of the rover because it makes it harder to tell if the
    // rotation of the rover is worriesome, which is the main point of the graphic
    // IMU data is in degrees but STL viewer needs radians
    const { IMUData } = this.state
    IMUData[0] = (data[0] / 360) * (2 * Math.PI)
    IMUData[2] = (data[2] / 360) * (2 * Math.PI)
    this.setState({ IMUData })
  }

  findWidth() {
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        if (
          this.state.width !== windows[win].document.getElementById(this.state.id).clientWidth - 10 ||
          this.state.height !== windows[win].document.getElementById(this.state.id).clientHeight - 12
        ) {
          windows[win].addEventListener("resize", () => this.findWidth())
          this.setState({
            width: windows[win].document.getElementById(this.state.id).clientWidth - 10,
            height: windows[win].document.getElementById(this.state.id).clientHeight - 12,
          })
        }
      }
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Rover</div>
        <div style={container} id={this.state.id}>
          <STLViewer
            model={RoverFile}
            modelColor="#B92C2C"
            backgroundColor="#FFFFFF"
            rotate={false}
            rotation={this.state.IMUData}
            orbitControls
            width={this.state.width}
            height={this.state.height}
            zoom={this.props.zoom}
          />
        </div>
      </div>
    )
  }
}

export default ThreeDRover
