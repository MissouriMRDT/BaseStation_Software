import React, { Component } from 'react';
import CSS from 'csstype';
import ReactTable from 'react-table-v6';
import { rovecomm, RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
// import WebSocket from 'ws'; // does not work in the browser client
import path from 'path';

const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '18px',
  margin: '5px 0px',
};
const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  alignItems: 'center',
  overflow: 'auto',
};
const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};
const button: CSS.Properties = {
  width: '50%',
  height: '30px',
  margin: '5px',
  fontSize: '16px',
};
const selectbox: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  width: '75%',
  margin: '2.5px',
  justifyContent: 'space-around',
};
const selector: CSS.Properties = {
  width: '200px',
};

interface IProps {
  style?: CSS.Properties;
}

interface TableEntry {
  name: string;
  dataId: number;
  time: string;
  dataType: string; // 'any' type in rovecomm
  dataCount: number; // 'any' type in rovecomm
  data: string | number[];
}

const PAGE_SIZE = 10;
const TABLE_MAX_PAGES = 3;

const tableSettings = [
  { Header: 'Name', accessor: 'name', width: '100' },
  { Header: 'Data Id', accessor: 'dataId', width: '75' },
  { Header: 'Time', accessor: 'time', width: '100' },
  { Header: 'Type', accessor: 'dataType', width: '50' },
  { Header: 'Count', accessor: 'dataCount', width: '50' },
  {
    Header: 'Data',
    accessor: 'data',
    width: 'fill',
    Cell: (data: any) => (
      <div
        style={{
          overflowX: 'scroll', // 'clip', for no scroll
          //textOverflow: 'ellipsis',
        }}
      >
        {Array.isArray(data.value) ? data.value.join(', ') : data.value}
      </div>
    ),
  },
];

interface IState {
  board: string;
  data: TableEntry[];
  columns: typeof tableSettings;
}

class PacketLogger extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  private excelStream?: WebSocket;

  //testing
  private interval1?: NodeJS.Timeout;

  private interval2?: NodeJS.Timeout;

  constructor(props: IProps) {
    super(props);
    this.state = {
      board: 'Core',
      data: [],
      columns: tableSettings,
    };
    this.boardChange = this.boardChange.bind(this);
    this.addData = this.addData.bind(this);
    rovecomm.on(this.state.board, (data: unknown) => this.addData(data as TableEntry));
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value;
    if (this.state.board === 'All') {
      Object.keys(RovecommManifest).map((boardName) => rovecomm.removeAllListeners(boardName));
    } else {
      rovecomm.removeAllListeners(this.state.board);
    }
    if (board === 'All') {
      Object.keys(RovecommManifest).map((boardName) =>
        rovecomm.on(boardName, (data: unknown) => this.addData(data as TableEntry))
      );
    } else {
      rovecomm.on(board, (data: unknown) => this.addData(data as TableEntry));
    }
    this.setState({
      board,
      data: [],
    });
  }

  addData(row: TableEntry): void {
    this.addTableRow(row);
    this.addExcelRow(row);
  }

  addTableRow(row: TableEntry): void {
    if (this.state.data.length >= PAGE_SIZE * TABLE_MAX_PAGES)
      this.setState((prevState) => ({ data: [row, ...prevState.data.slice(0, -1)] }));
    else this.setState((prevState) => ({ data: [row, ...prevState.data] }));
  }

  componentDidMount(): void {
    this.beginExcelStream();
    this.interval1 = setInterval(
      () =>
        this.addData({
          name: 'example1',
          dataId: 69,
          time: new Date().toLocaleTimeString(),
          dataType: 'FLOAT',
          dataCount: 6,
          data: [1, 2, 3, 4, 5, 6],
        }),
      200
    );
    this.interval2 = setInterval(
      () =>
        this.addData({
          name: 'example2',
          dataId: 420,
          time: new Date().toLocaleTimeString(),
          dataType: 'CHAR',
          dataCount: 16,
          data: 'Fuck California!',
        }),
      500
    );
  }

  componentWillUnmount(): void {
    this.closeExcelStream();
    if (this.interval1) clearInterval(this.interval1);
    if (this.interval2) clearInterval(this.interval2);
  }

  beginExcelStream(): void {
    if (this.excelStream) return;
    require('child_process').fork(path.join(__dirname, '../assets/ExcelStream.js'));
    // the process takes time to set up so we wait for a bit
    setTimeout(() => (this.excelStream = new WebSocket('ws://127.0.0.1:6969')), 5000);
  }

  closeExcelStream(): void {
    this.excelStream?.close();
  }

  addExcelRow(row: TableEntry): void {
    if (!this.excelStream) return;
    if (this.excelStream.readyState === WebSocket.OPEN) this.excelStream.send(JSON.stringify(row));
  }

  render(): JSX.Element {
    return (
      <div style={{ ...this.props.style }}>
        <div style={label}>Packet Logger</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select value={this.state.board} onChange={(e) => this.boardChange(e)} style={selector}>
              {Object.keys(RovecommManifest).map((item) => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                );
              })}
              <option value="All">All</option>
            </select>
          </div>
          <ReactTable
            className="-striped"
            data={this.state.data}
            columns={this.state.columns}
            filterable
            defaultPageSize={PAGE_SIZE}
            resizable={false}
            showPageSizeOptions={false}
            style={{ textAlign: 'center', width: '100%' }}
          />
          {/* <div>
            <label htmlFor="logXlsx">Log to .xlsx file?</label>
            <input type="checkbox" name="logXlsx" />
          </div> */}
        </div>
      </div>
    );
  }
}

export default PacketLogger;
