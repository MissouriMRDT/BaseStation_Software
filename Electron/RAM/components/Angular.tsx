import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "500px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const joints: CSS.Properties = {
  display: "grid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
  margin: "10px 0px",
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
  justifyContent: "space-around",
  margin: "0px 10px",
}
const input: CSS.Properties = {
  width: "75%",
}
const buttons: CSS.Properties = {
  width: "30%",
  margin: "5px",
  fontSize: "14px",
  lineHeight: "24px",
  borderRadius: "20px",
}
const modal: CSS.Properties = {
  position: "absolute",
  width: "400px",
  margin: "10px 50px",
  zIndex: 1,
  backgroundColor: "rgba(255,255,255,0.9)",
  border: "2px solid #990000",
  textAlign: "center",
  borderRadius: "25px",
}
const selector: CSS.Properties = {
  margin: "5px 20px",
  lineHeight: "24px",
  fontSize: "16px",
  WebkitAppearance: "none",
  MozAppearance: "none",
  padding: "5px",
}

function getPosition(): void {
  rovecomm.sendCommand("RequestJointPositions", [1])
}

function toggleTelem(): void {
  rovecomm.sendCommand("TogglePositionTelem", [1])
}

interface Joint {
  J1: string
  J2: string
  J3: string
  J4: string
  J5: string
  J6: string
}

interface IProps {}

interface IState {
  jointValues: Joint
  storedPositions: any
  selectedPosition: string
  addingPosition: boolean
  positionName: string
}

class Angular extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      jointValues: {
        J1: "",
        J2: "",
        J3: "",
        J4: "",
        J5: "",
        J6: "",
      },
      storedPositions: {},
      selectedPosition: "",
      addingPosition: false,
      positionName: "",
    }
    this.store = this.store.bind(this)
    this.setPosition = this.setPosition.bind(this)
    this.recall = this.recall.bind(this)
    this.delete = this.delete.bind(this)

    rovecomm.on("ArmAngles", (data: any) => this.updatePosition(data))
  }

  setPosition(): void {
    rovecomm.sendCommand(
      "ArmToAngle",
      Object.values(this.state.jointValues).map(function (x: string) {
        return x ? parseFloat(x) : 0
      })
    )
  }

  updatePosition(data: any): void {
    const [J1, J2, J3, J4, J5, J6] = data
    const jointValues = { J1, J2, J3, J4, J5, J6 }
    this.setState({ jointValues })
  }

  store(): void {
    let { selectedPosition } = this.state
    if (!selectedPosition) {
      selectedPosition = this.state.positionName
    }

    this.setState(
      {
        selectedPosition,
        addingPosition: false,
        positionName: "",
        storedPositions: {
          ...this.state.storedPositions,
          [this.state.positionName]: this.state.jointValues,
        },
      },
    )
  }

  recall(): void {
    this.setState({
      jointValues: this.state.storedPositions[
        String(this.state.selectedPosition)
      ],
    })
  }

  delete(): void {
    const { storedPositions } = this.state
    delete storedPositions[this.state.selectedPosition]
    const selectedPosition = Object.keys(storedPositions).length
      ? Object.keys(storedPositions)[0]
      : ""
    this.setState(
      {
        storedPositions,
        selectedPosition,
      },
    )
  }

  jointChange(event: { target: { value: string } }, joint: string): void {
    // we only want positive floats, so filter out any other characters
    // but match returns an array of [match, index, input, groups]
    // or returns undefined if there is no match
    const cleansedValue = event.target.value.match(/^\d*\.?\d*/)
    let value = ""
    if (cleansedValue) {
      ;[value] = cleansedValue
    }
    this.setState({
      jointValues: {
        ...this.state.jointValues,
        [joint]: value,
      },
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Angular</div>
        <div style={container}>
          <div style={joints}>
            {Object.keys(this.state.jointValues).map(joint => {
              return (
                <div key={joint} style={row}>
                  <h1 style={h1Style}>{joint}</h1>
                  <input
                    type="text"
                    style={input}
                    value={this.state.jointValues[joint] || ""}
                    onChange={e => this.jointChange(e, joint)}
                  />
                </div>
              )
            })}
          </div>
          <div style={row}>
            <button type="button" style={buttons} onClick={getPosition}>
              Get Position
            </button>
            <button type="button" style={buttons} onClick={this.setPosition}>
              Set Position
            </button>
            <button type="button" style={buttons} onClick={toggleTelem}>
              Toggle Auto Telem
            </button>
          </div>
          <select
            value={this.state.selectedPosition}
            onChange={e => this.setState({ selectedPosition: e.target.value })}
            size={5}
            style={selector}
          >
            {Object.keys(this.state.storedPositions).map((position: any) => {
              return (
                <option key={position} value={position}>
                  {position}
                </option>
              )
            })}
          </select>
          <div style={row}>
            <button
              type="button"
              style={buttons}
              onClick={() => this.setState({ addingPosition: true })}
            >
              Store
            </button>
            <button type="button" style={buttons} onClick={this.recall}>
              Recall
            </button>
            <button type="button" style={buttons} onClick={this.delete}>
              Delete
            </button>
          </div>
          {this.state.addingPosition ? (
            <div style={modal}>
            </div>
          ) : null}
        </div>
      </div>
    )
  }
}

export default Angular
