import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"
import path from "path"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

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
  outline: "none",
}
const modal: CSS.Properties = {
  position: "absolute",
  width: "400px",
  margin: "10px 50px",
  zIndex: 1,
  backgroundColor: "rgba(255,255,255,0.9)",
  border: "2px solid #990000",
  textAlign: "center",
}
const modalButton: CSS.Properties = {
  width: "75px",
  lineHeight: "24px",
  color: "white",
  fontWeight: "bold",
  border: "none",
  margin: "10px",
}
const selector: CSS.Properties = {
  margin: "5px 20px",
  lineHeight: "24px",
  fontSize: "16px",
  WebkitAppearance: "none",
  MozAppearance: "none",
  padding: "5px",
}

const filepath = path.join(__dirname, "../assets/AngularPresets.json")

function getPosition(): void {
  // Unlike most telemetry, arm joint positions are only sent when requested
  rovecomm.sendCommand("RequestJointPositions", [1])
}

function toggleTelem(): void {
  // Some arm systems allow toggling an incoming stream of arm telemetry
  // When enabled, entering text into the textfields to set position will
  // become practically impossible
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

interface IProps {
  style?: CSS.Properties
}

interface IState {
  jointValues: Joint
  storedPositions: any
  selectedPosition: string
  addingPosition: boolean
  positionName: string
}

class Angular extends Component<IProps, IState> {
  constructor(props: IProps) {
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

    rovecomm.on("JointAngles", (data: any) => this.updatePosition(data))
  }

  componentDidMount(): void {
    /* When first built, we want to check the presets file if it exists
     * and import all preset positions. If presets exist, we should also
     * default selected position to the first position. If file does not
     * exist, it isn't created until we attempt to store data
     */
    if (fs.existsSync(filepath)) {
      const storedPositions = JSON.parse(fs.readFileSync(filepath).toString())
      if (Object.keys(storedPositions).length) {
        const selectedPosition = Object.keys(storedPositions)[0]
        this.setState({ storedPositions, selectedPosition })
      }
    }
  }

  setPosition(): void {
    /* This function should take the values of all the joints,
     * convert them from strings to floats (or empty string to 0)
     * and send the proper rovecomm packet
     */
    rovecomm.sendCommand(
      "ArmMoveToAngle",
      Object.values(this.state.jointValues).map(function (x: string) {
        return x ? parseFloat(x) : 0
      })
    )
  }

  updatePosition(data: any): void {
    /* Function to update displayed jointValues when a new position is recieved */
    const [J1, J2, J3, J4, J5, J6] = data
    const jointValues = { J1, J2, J3, J4, J5, J6 }
    this.setState({ jointValues })
  }

  store(): void {
    /* Adds the current position (or at least entered position) to the select box
     * and updates the json file
     */
    // If selectedPosition is still an empty string, this is a good time
    // to update it to a useful starting value
    let { selectedPosition } = this.state
    if (!selectedPosition) {
      selectedPosition = this.state.positionName
    }

    this.setState(
      {
        selectedPosition,
        addingPosition: false,
        positionName: "", // reset name in modal text box for next use
        storedPositions: {
          // Spread to ensure all currently stored positions are kept
          // but the newest position is added
          ...this.state.storedPositions,
          [this.state.positionName]: this.state.jointValues,
        },
      },
      () => {
        // function callback so that when setState has finished executing
        // we can properly update the json file. File is created if now if
        // it doesn't already exist
        fs.writeFile(filepath, JSON.stringify(this.state.storedPositions), err => {
          if (err) throw err
        })
      }
    )
  }

  recall(): void {
    /* Pulls selected stored position into the current jointValues
     * (if a position has been selected)
     */
    if (this.state.selectedPosition) {
      this.setState({
        jointValues: this.state.storedPositions[this.state.selectedPosition],
      })
    }
  }

  delete(): void {
    /* Deletes a selected stored position (if a position has been selected)
     * and properly updates the json file (see store() for more detailed comments)
     */
    const { storedPositions } = this.state
    delete storedPositions[this.state.selectedPosition]

    // Since the selectedPosition was just deleted, we want to grab a new
    // value. We grab the first key if one exists, or if not default to ""
    const selectedPosition = Object.keys(storedPositions).length ? Object.keys(storedPositions)[0] : ""

    this.setState(
      {
        storedPositions,
        selectedPosition,
      },
      () => {
        fs.writeFile(filepath, JSON.stringify(this.state.storedPositions), err => {
          if (err) throw err
        })
      }
    )
  }

  jointChange(event: { target: { value: string } }, joint: string): void {
    /* We only want positive floats, so filter out any other characters
     * but match returns an array of [match, index, input, groups]
     * or returns undefined if there is no match
     */
    // Regex filters for 0+ digits, 0 or 1 decimal, followed by 0+ more digits
    const cleansedValue = event.target.value.match(/^\d*\.?\d*/)

    let value = ""
    if (cleansedValue) {
      // Leading semicolon helps ensure [value] isn't taken as an index
      // but properly used for array destructuring
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
      <div style={this.props.style}>
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
            <button type="button" style={buttons} onClick={() => this.setState({ addingPosition: true })}>
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
              <h3>Please give this position a name</h3>
              <input
                type="text"
                value={this.state.positionName}
                onChange={e => this.setState({ positionName: e.target.value })}
                autoFocus={this.state.addingPosition}
              />
              <div style={row}>
                <button
                  type="button"
                  style={{ ...modalButton, backgroundColor: "red" }}
                  onClick={() =>
                    this.setState({
                      addingPosition: false,
                      positionName: "",
                    })
                  }
                >
                  Cancel
                </button>
                <button type="button" style={{ ...modalButton, backgroundColor: "green" }} onClick={this.store}>
                  Add
                </button>
              </div>
            </div>
          ) : null}
        </div>
      </div>
    )
  }
}

export default Angular
