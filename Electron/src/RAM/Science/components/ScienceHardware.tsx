import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"
import internal from "stream"

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

class ScienceHardware extends Component<IProps, IState> {
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
    //this.rotateLeft = this.rotateLeft.bind(this)
    //this.rotateRight = this.rotateRight.bind(this)
    //this.updatePosition = this.updatePosition.bind(this)
    //rovecomm.on("GenevaCurrentPosition", (data: any) => this.updatePosition(data))
  }

  /**
   * Toggles the power of a given heater block
   * @param index 0-based index of the block to toggle
   */
  toggleBlock(index: number): void {
    //TODO: Implement rovecomm
    //TODO: Maybe use 1-based index? Depends on rovecomm packet spec.
    const { blocks } = this.state
    blocks[index].isOn = !blocks[index].isOn
    this.setState({ blocks })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Science Hardware</div>
        <div style={container}>
          {this.state.blocks.map((block, index) => {
            return (
              <div style={row}>
                <label style={blockLabel} onClick={() => this.toggleBlock(index)}>
                  Block {index + 1}:{" "}
                </label>
                <button style={blockLabel}>{block.temp}&#176; C</button>
              </div>
            )
          })}
        </div>
      </div>
    )
  }
}

export default ScienceHardware
