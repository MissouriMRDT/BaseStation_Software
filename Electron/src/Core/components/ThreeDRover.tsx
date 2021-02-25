import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import { rovecomm } from "../RoveProtocol/Rovecomm"
import STLViewer from "./STLViewer"

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
}

interface IState {
  IMUData: number[]
  masterDiv: any
}

class ThreeDRover extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      IMUData: [0, 0, 0],
      masterDiv: React.createRef(),
    }

    rovecomm.on("IMUData", (data: any) => this.IMUData(data))
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

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Rover</div>
        <div style={container} ref={this.state.masterDiv}>
          <STLViewer
            model={RoverFile}
            modelColor="#B92C2C"
            backgroundColor="#FFFFFF"
            rotate={false}
            rotation={this.state.IMUData}
            orbitControls
            width={300}
            height={150}
            zoom={20}
          />
        </div>
      </div>
    )
  }
}

export default ThreeDRover
