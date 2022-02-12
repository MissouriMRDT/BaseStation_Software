import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm, DataTypes, RovecommManifest } from "../../Core/RoveProtocol/Rovecomm"
import { RONModuleWidth } from "./PingGraph"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "14px",
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
let selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: `min(450px, ${RONModuleWidth}px)`,
  justifyContent: "space-between",
  margin: "2.5px",
}
const selector: CSS.Properties = {
  width: "200px",
}
const textbox: CSS.Properties = {
  width: "50px",
  height: "18px",
}
let textboxRow: CSS.Properties = {
  width: `min(450px, ${RONModuleWidth}px)`,
  display: "flex",
  flexDirection: "row",
  flexWrap: "wrap",
  justifyContent: "space-between",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  board: string
  command: string
  dataType: string
  value: number[]
  count: number
}

class CustomPackets extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      board: "Drive",
      command: "DriveLeftRight",
      value: [0, 0],
      count: 2,
      dataType: "INT16_T",
    }

    this.boardChange = this.boardChange.bind(this)
    this.commandChange = this.commandChange.bind(this)
    this.dataChange = this.dataChange.bind(this)
    this.sendCommand = this.sendCommand.bind(this)
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    const command = Object.keys(RovecommManifest[board].Commands)[0]
    const { dataType, dataCount } = RovecommManifest[board].Commands[command]
    const value = Array(dataCount).fill(0)
    this.setState({
      board,
      command,
      count: dataCount,
      value,
      dataType,
    })
  }

  commandChange(event: { target: { value: string } }): void {
    const { dataType, dataCount } = RovecommManifest[this.state.board].Commands[event.target.value]
    const value = Array(dataCount).fill(0)
    this.setState({
      command: event.target.value,
      count: dataCount,
      value,
      dataType,
    })
  }

  dataChange(event: { target: { value: string } }, num: number): void {
    const { value } = this.state
    value[num] = event.target.value ? parseInt(event.target.value, 10) : 0
    this.setState({
      value,
    })
  }

  sendCommand(): void {
    rovecomm.sendCommand(this.state.command, this.state.value)
  }

  render(): JSX.Element {
    selectbox = { ...selectbox, width: `min(450px, ${RONModuleWidth - 15}px)` }
    textboxRow = { ...textboxRow, width: `min(450px, ${RONModuleWidth - 15}px)` }
    return (
      <div style={{ ...this.props.style, width: RONModuleWidth }}>
        <div style={label}>Custom Packets</div>
        <div style={container}>
          {[
            {
              title: "Board",
              value: this.state.board,
              onChange: this.boardChange,
              list: RovecommManifest,
            },
            {
              title: "Commands",
              value: this.state.command,
              onChange: this.commandChange,
              list: RovecommManifest[this.state.board].Commands,
            },
          ].map(select => {
            const { title, value, onChange, list } = select
            return (
              <div style={selectbox} key={title}>
                <div style={h1Style}>{title}:</div>
                <select name={title} id={title} value={value} onChange={onChange} style={selector}>
                  {Object.keys(list).map(item => {
                    return (
                      <option key={item} value={item}>
                        {item}
                      </option>
                    )
                  })}
                </select>
              </div>
            )
          })}
          <div style={selectbox} key="Data">
            <div style={h1Style}>Data ({this.state.dataType}):</div>
            <div style={textboxRow}>
              {[...Array(this.state.count).keys()].map(n => {
                return (
                  <textarea style={textbox} key={n} value={this.state.value[n]} onChange={e => this.dataChange(e, n)} />
                )
              })}
            </div>
          </div>
          <button type="button" onClick={this.sendCommand} style={{ margin: "2.5px" }}>
            Send
          </button>
        </div>
      </div>
    )
  }
}

export default CustomPackets
