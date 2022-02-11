import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import { BitmaskUnpack } from "../../../Core/BitmaskUnpack"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  height: "calc(100% - 40px)",
  minWidth: "150px",
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
  flexGrow: 1,
  justifyContent: "center",
  margin: "auto",
  lineHeight: "25px",
  width: "100%",
}
const blockLabel: CSS.Properties = {
  margin: "auto",
}

const button: CSS.Properties = {
  width: "60px",
  margin: "auto",
}

/** Will be merged with the row css if the block is off */
const offIndicator: CSS.Properties = {
  backgroundColor: "#FF0000",
}

/** Will be merged with the row css if the block is off */
const onIndicator: CSS.Properties = {
  backgroundColor: "#00FF00",
}

type HeaterBlock = {
  /** The current temperature of the block in Celsius */
  temp: number
  /** True if the heater block is turned on */
  isOn: boolean
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  blocks: HeaterBlock[]
}

class Heater extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      blocks: [
        {
          temp: -1,
          isOn: false,
        },
        {
          temp: -1,
          isOn: false,
        },
        {
          temp: -1,
          isOn: false,
        },
      ],
    }
    this.updateTemps = this.updateTemps.bind(this)
    this.toggleBlock = this.toggleBlock.bind(this)
    this.updateEnabled = this.updateEnabled.bind(this)
    this.setAllBlocks = this.setAllBlocks.bind(this)
    rovecomm.on("ThermoValues", (data: any) => this.updateTemps(data))
    rovecomm.on("HeaterEnabled", (data: any) => this.updateEnabled(data))
  }

  updateEnabled(data: number): void {
    const { blocks } = this.state
    const bitmask = BitmaskUnpack(data, blocks.length)
    for (var i: number = 0; i < blocks.length; i++) {
      blocks[i].isOn = Boolean(Number(bitmask[i]))
    }
    this.setState({ blocks })
  }

  /**
   * Updates the temperatures shown on the component
   * @param temps a three value array of temps in degrees C
   */
  updateTemps(temps: number[]): void {
    const { blocks } = this.state

    for (let i: number = 0; i < this.state.blocks.length; i++) {
      blocks[i].temp = temps[i]
    }

    this.setState({ blocks })
  }

  /**
   * Sends the command to toggle one block
   * Does **not** update the state of the component. The board will send when it has turned off
   * @param index 0-based index of the block to toggle
   */
  toggleBlock(index: number): void {
    const blocks: HeaterBlock[] = JSON.parse(JSON.stringify(this.state.blocks)) //Make a new array with copied data in order to not change state
    blocks[index].isOn = !blocks[index].isOn

    let bitmask = ""
    blocks.forEach(block => {
      bitmask += block.isOn ? "1" : "0"
    })
    rovecomm.sendCommand("HeaterToggle", [parseInt(bitmask, 2)])
  }

  /**
   * Sends the command to set all blocks to either on or off
   * Does **not** update the state of the component. The board will send when it has turned off
   * @param powered true to turn all on, false to turn all off
   */
  setAllBlocks(powered: boolean): void {
    let bitmask = powered ? "1" : "0"
    bitmask = bitmask.repeat(this.state.blocks.length)
    rovecomm.sendCommand("HeaterToggle", [parseInt(bitmask, 2)])
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Science Hardware</div>
        <div style={container}>
          {this.state.blocks.map((block, index) => {
            return (
              <div style={{ ...row, ...(block.isOn ? onIndicator : offIndicator) }}>
                <label style={blockLabel}>Block {index + 1}: </label>
                <button style={button} onClick={() => this.toggleBlock(index)}>
                  {block.temp}&#176; C
                </button>
              </div>
            )
          })}
          <div style={row}>
            <button style={button} onClick={() => this.setAllBlocks(true)}>
              Enable All
            </button>
            <button style={button} onClick={() => this.setAllBlocks(false)}>
              Disable All
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Heater