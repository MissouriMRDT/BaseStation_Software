import React, { Component } from 'react';
import CSS from 'csstype';
import ReactTable from 'react-table-v6';
// import "../../node_modules/react-table-v6/react-table.css"
import { rovecomm, RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
import fs from 'fs';

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

interface IState {
  board: string;
  data: any;
  columns: any;
}

class PacketLogger extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: any) {
    super(props);
    this.state = {
      board: 'Core',
      data: [],
      columns: [
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
                overflow: 'none',
                textOverflow: 'ellipsis',
              }}
            >
              {data.value.join(', ')}
            </div>
          ),
        },
      ],
    };
    this.boardChange = this.boardChange.bind(this);
    this.addData = this.addData.bind(this);
    rovecomm.on(this.state.board, (data: any) => this.addData(data));
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value;
    rovecomm.removeAllListeners(this.state.board);
    rovecomm.on(board, (data: any) => this.addData(data));
    this.setState({
      board,
      data: [],
    });
  }

  addData(newData: any): void {
    this.setState((prevState) => ({ data: [newData].concat(prevState.data) }));
  }

  exportData(board: string): void {
    // Convert the data to CSV format
    const csvData = this.state.data.map((row: number) => Object.values(row).join(',')).join('\n');

    // ISO string will be formatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T:,Z to . (which covers to . for .csv)
    // Date format is consistent with the SensorData csv
    const timestamp = new Date().toISOString().replaceAll(/[:\-TZ]/g, '.');
    const EXPORT_FILE = `./PacketLogger/${board}-${timestamp}.csv`;

    if (!fs.existsSync('./PacketLogger')) {
      fs.mkdirSync('./PacketLogger');
    }

    // Write the CSV data to a file
    fs.writeFile(EXPORT_FILE, csvData, (err) => {
      if (err) throw err;
    });
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
            style={{ textAlign: 'center', margin: 'auto' }}
          />
          <button style={button} onClick={() => this.exportData(this.state.board)}>
            Export Data
          </button>
        </div>
      </div>
    );
  }
}

export default PacketLogger;
