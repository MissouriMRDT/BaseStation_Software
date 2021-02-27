import React, { Component } from "react"
import CSS from "csstype"
import ReactTable from "react-table-v6"
// import "../../node_modules/react-table-v6/react-table.css"
import { rovecomm, RovecommManifest } from "../../Core/RoveProtocol/Rovecomm"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "18px",
  margin: "5px 0px",
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
const selectbox: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  width: "75%",
  margin: "2.5px",
  justifyContent: "space-around",
}
const selector: CSS.Properties = {
  width: "200px",
}

interface IProps {
  style?: CSS.Properties
}

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
      data: [],
      columns: [
        { Header: "Name", accessor: "name", width: "100" },
        { Header: "Data Id", accessor: "dataId", width: "75" },
        { Header: "Time", accessor: "time", width: "100" },
        { Header: "Type", accessor: "dataType", width: "50" },
        { Header: "Count", accessor: "dataCount", width: "50" },
        {
          Header: "Data",
          accessor: "data",
          width: "fill",
          Cell: (data: any) => (
            <div
              style={{
                width: "125",
                overflow: "none",
                textOverflow: "ellipsis",
              }}
            >
              {data.value.join(", ")}
            </div>
          ),
        },
      ],
    }
    this.boardChange = this.boardChange.bind(this)
    this.addData = this.addData.bind(this)
    rovecomm.on(this.state.board, (data: any) => this.addData(data))
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value
    rovecomm.removeAllListeners(this.state.board)
    rovecomm.on(board, (data: any) => this.addData(data))
    this.setState({
      board,
      data: [],
    })
  }

  addData(newData: any): void {
    const data = [newData].concat(this.state.data)
    this.setState({ data })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Packet Logger</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select value={this.state.board} onChange={e => this.boardChange(e)} style={selector}>
              {Object.keys(RovecommManifest).map(item => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                )
              })}
            </select>
          </div>
          <ReactTable
            className="-striped"
            data={this.state.data}
            columns={this.state.columns}
            filterable
            defaultPageSize={10}
            resizable={false}
            showPageSizeOptions={false}
            style={{ textAlign: "center" }}
          />
        </div>
      </div>
    )
  }
}

export default PacketLogger
