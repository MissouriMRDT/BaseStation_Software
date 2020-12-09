import React, { Component } from "react"
import CSS from "csstype"
import ReactTable from "react-table-v6"
import "../../node_modules/react-table-v6/react-table.css"
import { DATAID } from "../../Core/RoveProtocol/RovecommManifest"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
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
  padding: "5px",
  justifyContent: "center",
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
  margin: "2.5px",
  justifyContent: "space-around",
}
const selector: CSS.Properties = {
  width: "200px",
}

interface IProps {}

interface IState {
  board: string
  data: any
  columns: any
}

class PacketLogger extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      board: "Drive",
      data: [
        {
          name: "a",
          dataId: "a",
          time: Date.now().toString(),
          dataType: "0",
          dataCount: 1,
          data: [1, 2, 3],
        },
      ],
      columns: [
        { Header: "Name", accessor: "name" },
        { Header: "Data Id", accessor: "dataId" },
        { Header: "Time", accessor: "time" },
        { Header: "Type", accessor: "dataType" },
        { Header: "Count", accessor: "dataCount" },
        { Header: "Data", accessor: "data" },
      ],
    }
    this.boardChange = this.boardChange.bind(this)
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    rovecomm.off(this.state.board)
    rovecomm.on(board, (data: any) => this.addData(data))
    this.setState({
      board,
      data: [],
    })
  }

  addData(newData: any): void {
    const { data } = this.state
    data.push(newData)
    this.setState({ data })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Packet Logger</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select
              value={this.state.board}
              onChange={e => this.boardChange(e)}
              style={selector}
            >
              {Object.keys(DATAID).map(item => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                )
              })}
            </select>
          </div>
          <ReactTable
            data={this.state.data}
            columns={this.state.columns}
            className="-striped"
            filterable
            defaultPageSize={Math.floor((window.innerHeight * 0.75 - 200) / 46)}
            resizable={false}
            showPageSizeOptions={false}
          />
        </div>
      </div>
    )
  }
}

export default PacketLogger
