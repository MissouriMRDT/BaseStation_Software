import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

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
  height: "calc(100% - 50px)",
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

interface IProps {
  style?: CSS.Properties
}

interface IState {}

const stateEnum = {
  Idle: 0,
  Navigating: 1,
  SearchPattern: 2,
  ApproachingMarker: 3,
  ApproachingGate: 4,
  Avoidance: 5,
}

class StateDiagram extends Component<IProps, IState> {
  canvasRef: any

  constructor(props: IProps) {
    super(props)
    this.state = {}

    this.canvasRef = React.createRef()
    rovecomm.on("CurrentState", (data: any) => this.updateStateDiagram(data))
  }

  componentDidMount() {
    this.updateStateDiagram([-1])
  }

  updateStateDiagram(data: any): void {
    let text

    const canvas = this.canvasRef.current
    const context = canvas.getContext("2d")
    context.clearRect(0, 0, canvas.width, canvas.height)

    const centerW = context.canvas.width / 2

    context.font = "12px Arial"

    text = "Idle"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 25, 50)
    context.beginPath()
    context.rect(centerW - 20, 30, 40, 40)
    context.fillStyle = data[0] === stateEnum[text] ? "green" : "white"
    context.fill()
    context.stroke()

    context.beginPath()
    context.moveTo(centerW, 70)
    context.lineTo(centerW, 110)
    context.stroke()

    text = "Navigating"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 25, 130)
    context.rect(centerW - 20, 110, 40, 40)
    context.stroke()
    context.fillStyle = data[0] === stateEnum[text] ? "green" : "white"
    context.fill()

    context.beginPath()
    context.moveTo(centerW, 150)
    context.lineTo(centerW, 190)
    context.stroke()

    text = "SearchPattern"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 25, 210)
    context.rect(centerW - 20, 190, 40, 40)
    context.stroke()
    context.fillStyle = data[0] === stateEnum[text] ? "green" : "white"
    context.fill()

    context.beginPath()
    context.moveTo(centerW, 230)
    context.lineTo(centerW, 270)
    context.stroke()

    text = "ApproachingMarker"
    context.textBaseline = "middle"
    context.textAlign = "right"
    context.fillStyle = "black"
    context.fillText(text, centerW - 25, 290)
    context.rect(centerW - 20, 270, 40, 40)
    context.stroke()
    context.fillStyle = data[0] === stateEnum[text] ? "green" : "white"
    context.fill()

    context.beginPath()
    context.moveTo(centerW + 20, 290)
    context.lineTo(centerW + 40, 290)
    context.lineTo(centerW + 40, 280)
    context.lineTo(centerW + 20, 280)
    context.lineTo(centerW + 25, 285)
    context.moveTo(centerW + 20, 280)
    context.lineTo(centerW + 25, 275)
    context.stroke()

    context.beginPath()
    context.moveTo(centerW + 20, 290)
    context.lineTo(centerW + 60, 290)
    context.lineTo(centerW + 60, 220)
    context.lineTo(centerW + 20, 220)
    context.lineTo(centerW + 25, 225)
    context.moveTo(centerW + 20, 220)
    context.lineTo(centerW + 25, 215)
    context.stroke()

    context.beginPath()
    context.moveTo(centerW + 20, 290)
    context.lineTo(centerW + 80, 290)
    context.lineTo(centerW + 80, 60)
    context.lineTo(centerW + 20, 60)
    context.lineTo(centerW + 25, 65)
    context.moveTo(centerW + 20, 60)
    context.lineTo(centerW + 25, 55)
    context.stroke()

    context.beginPath()
    context.moveTo(centerW + 20, 210)
    context.lineTo(centerW + 40, 210)
    context.lineTo(centerW + 40, 200)
    context.lineTo(centerW + 20, 200)
    context.lineTo(centerW + 25, 205)
    context.moveTo(centerW + 20, 200)
    context.lineTo(centerW + 25, 195)
    context.stroke()

    context.beginPath()
    context.moveTo(centerW + 20, 50)
    context.lineTo(centerW + 40, 50)
    context.lineTo(centerW + 40, 40)
    context.lineTo(centerW + 20, 40)
    context.lineTo(centerW + 25, 45)
    context.moveTo(centerW + 20, 40)
    context.lineTo(centerW + 25, 35)
    context.stroke()
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>State Diagram</div>
        <div style={container}>
          <canvas ref={this.canvasRef} width={window.document.documentElement.clientWidth / 2 - 10} height={400} />
        </div>
      </div>
    )
  }
}

export default StateDiagram
