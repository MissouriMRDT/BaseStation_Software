import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { DataTypes, DATAID } from "../../Core/RoveProtocol/RovecommManifest"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "14px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
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
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "450px",
  justifyContent: "space-between",
  margin: "2.5px",
}
const selector: CSS.Properties = {
  width: "200px",
}
const textbox: CSS.Properties = {
  width: "50px",
  height: "14px",
}

interface IProps {}

interface IState {
  board: string
  Ip: string
  command: string
  dataId: number
  dataType: number
  value: number[]
  count: number
}

class CustomPackets extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      board: "Drive",
      Ip: "192.168.1.131",
      command: "DriveLeftRight",
      dataId: 1000,
      value: [0, 0],
      count: 2,
      dataType: DataTypes.INT16_T,
    }

    this.boardChange = this.boardChange.bind(this)
    this.commandChange = this.commandChange.bind(this)
    this.dataChange = this.dataChange.bind(this)
    this.sendCommand = this.sendCommand.bind(this)
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    const { Ip } = DATAID[board]
    const command = Object.keys(DATAID[board].Commands)[0]
    const { dataId, dataType, dataCount } = DATAID[board].Commands[command]
    const value = Array(dataCount).fill(0)
    this.setState({
      board,
      Ip,
      command,
      dataId,
      count: dataCount,
      value,
      dataType,
    })
  }

  commandChange(event: { target: { value: string } }): void {
    const { dataId, dataType, dataCount } = DATAID[this.state.board].Commands[
      event.target.value
    ]
    const value = Array(dataCount).fill(0)
    this.setState({
      command: event.target.value,
      dataId,
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
    return (
      <div>
        <div style={label}>Custom Packets</div>
        <div style={container}>
          {[
            {
              title: "Board",
              value: this.state.board,
              onChange: this.boardChange,
              list: DATAID,
            },
            {
              title: "Commands",
              value: this.state.command,
              onChange: this.commandChange,
              list: DATAID[this.state.board].Commands,
            },
          ].map(select => {
            const { title, value, onChange, list } = select
            return (
              <div style={selectbox} key={title}>
                <div style={h1Style}>{title}:</div>
                <select
                  name={title}
                  id={title}
                  value={value}
                  onChange={onChange}
                  style={selector}
                >
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
            <div style={h1Style}>Data ({DataTypes[this.state.dataType]}):</div>
            {[...Array(this.state.count).keys()].map(n => {
              return (
                <textarea
                  style={textbox}
                  key={n}
                  value={this.state.value[n]}
                  onChange={e => this.dataChange(e, n)}
                />
              )
            })}
          </div>
          <button
            type="button"
            onClick={this.sendCommand}
            style={{ margin: "2.5px" }}
          >
            Send
          </button>
        </div>
      </div>
    )
  }
}

export default CustomPackets
