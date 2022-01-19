import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import internal from "stream"
import { blockStatement } from "@babel/types"

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
  justifyContent: "center",
  margin: "auto",
  lineHeight: "25px",
  width: "100%",
}
const blockLabel: CSS.Properties = {
  margin: "auto",
}
const onIndicator: CSS.Properties = {
  height: "15px",
  width: "15px",
  backgroundColor: "#009900",
  borderRadius: "50%",
  display: "flex",
  margin: "auto",
}
const offIndicator: CSS.Properties = {
  height: "15px",
  width: "15px",
  backgroundColor: "#990000",
  borderRadius: "50%",
  display: "flex",
  margin: "auto",
}
const button: CSS.Properties = {
  width: "60px",
  margin: "auto",
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
          temp: 105,
          isOn: false,
        },
        {
          temp: 98,
          isOn: false,
        },
        {
          temp: 32,
          isOn: false,
        },
      ],
    }
    this.updateTemps = this.updateTemps.bind(this)
    this.toggleBlock = this.toggleBlock.bind(this)
    rovecomm.on("Thermo Values", (data: any) => this.updateTemps(data))
    // need to clarify data type for HeaterEnabled
    rovecomm.on("HeaterEnabled", (data: any) => this.updateEnabled(data))
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
   * Toggles the power of a given heater block
   * @param index 0-based index of the block to toggle
   */
  toggleBlock(index: number): void {
    //TODO: Implement rovecomm. ask about bitmask
    console.log("toggle block " + index)
    const { blocks } = this.state
    blocks[index].isOn = !blocks[index].isOn
    this.setState({ blocks: blocks })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Science Hardware</div>
        <div style={container}>
          {this.state.blocks.map((block, index) => {
            return (
              <div style={row}>
                <label style={blockLabel}>Block {index + 1}: </label>
                <button style={button} onClick={() => this.toggleBlock(index)}>
                  {block.temp}&#176; C
                </button>
                <span style={block.isOn ? onIndicator : offIndicator}></span>
              </div>
            )
          })}
        </div>
      </div>
    )
  }
}

export default Heater
