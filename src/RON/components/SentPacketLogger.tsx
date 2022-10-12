import React, { Component } from 'react';
import CSS from 'csstype';
import ReactTable from 'react-table-v6';
import fs from 'fs';
// import "../../node_modules/react-table-v6/react-table.css"
import { rovecomm, RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';

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

const buttons: CSS.Properties = {
  fontFamily: 'arial',
  lineHeight: '20px',
  fontSize: '16px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  board: string;
  data: any;
  columns: any;
}

class SentPacketLogger extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: any) {
    super(props);
    this.state = {
      board: 'Drive',
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
    // rovecomm.on(this.state.board, (data: any) => this.addData(data));
    rovecomm.on('BasestationCommand', (data: any) => this.addData(data));
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value;
    rovecomm.removeAllListeners(this.state.board);

    this.setState({
      board,
      data: [],
    });
  }

  addData(newData: any): void {
    this.setState((prevState) => ({ data: [newData].concat(prevState.data) }));
  }

  exportPacket(): void {
    fs.writeFile(
      'SentPacket.csv',
      this.state.data
        .map((temp: any) => {
          return `${temp.name}, ${temp.dataId}, ${temp.time}, ${temp.dataType}, ${temp.dataCount}, ${temp.data.join(
            ' '
          )}`;
        })
        .join('\n'),
      (err) => {
        if (err) console.log(err);
        else {
          console.log('File written successfully\n');
        }
      }
    );
  }

  render(): JSX.Element {
    return (
      <div style={{ ...this.props.style }}>
        <div style={label}>Sent Packet Logger</div>
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
            style={{ textAlign: 'center', margin: 'auto' }} // <button type="button" style={buttons} onClick={() => this.receivePackets()}></button>
          />
          <button type="button" style={buttons} onClick={() => this.exportPacket()}>
            Save Data
          </button>
        </div>
      </div>
    );
  }
}

export default SentPacketLogger;
