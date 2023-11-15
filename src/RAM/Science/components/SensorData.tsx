import React, { Component } from 'react';
import CSS from 'csstype';
import fs from 'fs';

import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  justifyContent: 'center',
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
const row: CSS.Properties = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  marginLeft: '10px',
  marginRight: '10px',
};
const buttonRow: CSS.Properties = {
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-around',
  margin: '5px 50px',
};
const buttons: CSS.Properties = {
  lineHeight: '20px',
  fontSize: '16px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  ch3: number;
  temperature: number;
  co2: number;
  o2: number;
  no: number;
  no2: number;

  writeToFile: boolean;
  sensorSaveFile: string | null;
  fileWriteInterval: NodeJS.Timeout | null;
}

class SensorData extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: Readonly<IProps> | IProps) {
    super(props);
    this.state = {
      ch3: 0,
      temperature: 0,
      co2: 0,
      o2: 0,
      no: 0,
      no2: 0,
      writeToFile: false,
      sensorSaveFile: null,
      fileWriteInterval: null,
    };
    this.ch3 = this.ch3.bind(this);
    this.co2 = this.co2.bind(this);
    this.o2 = this.o2.bind(this);
    this.no2 = this.no2.bind(this);
    this.no = this.no.bind(this);
    this.fileWrite = this.fileWrite.bind(this);
    this.fileStart = this.fileStart.bind(this);
    this.fileStop = this.fileStop.bind(this);

    rovecomm.on('CH3', (data: any) => this.ch3(data));
    rovecomm.on('CO2', (data: any) => this.co2(data));
    rovecomm.on('O2', (data: any) => this.o2(data));
    rovecomm.on('NO', (data: any) => this.no(data));
    rovecomm.on('NO2', (data: any) => this.no2(data));
  }

  ch3(data: any): void {
    const [ch3, temperature] = data;
    this.setState({ ch3, temperature });
  }

  co2(data: any): void {
    this.setState({ co2: data[0] });
  }

  o2(data: any): void {
    this.setState({ o2: data[0] });
  }

  no2(data: any): void {
    this.setState({ no2: data[0] });
  }

  no(data: any): void {
    this.setState({ no: data[0] });
  }

  fileStart(): void {
    const filestream = `./ScienceSaveFiles/${new Date()
      .toISOString()
      // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
      // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
      .replaceAll(/[:\-TZ]/g, '.')}csv`;
    if (!fs.existsSync('./ScienceSaveFiles')) {
      fs.mkdirSync('./ScienceSaveFiles');
    }
    fs.open(filestream, 'w', (err) => {
      if (err) throw err;
    });
    fs.appendFile(filestream, 'time,ch3,temp,co2,o2,no,no2\n', (err) => {
      if (err) throw err;
    });
    const fileWriteInterval = setInterval(this.fileWrite, 1000);
    this.setState({
      sensorSaveFile: filestream,
      writeToFile: true,
      fileWriteInterval,
    });
  }

  fileStop(): void {
    if (this.state.fileWriteInterval) {
      clearInterval(this.state.fileWriteInterval);
    }
    this.setState({
      writeToFile: false,
      sensorSaveFile: null,
      fileWriteInterval: null,
    });
  }

  fileWrite(): void {
    if (this.state.writeToFile && this.state.sensorSaveFile) {
      fs.appendFile(
        this.state.sensorSaveFile,
        // time,ch3,temp,co2,o2,no,no2
        `${new Date().toLocaleDateString()},${this.state.ch3},${this.state.temperature},${this.state.co2},${
          this.state.o2
        },${this.state.no},${this.state.no2}\n`,
        (err) => {
          if (err) throw err;
        }
      );
    }
  }

  render(): JSX.Element {
    return (
      <div id="SensorData" style={this.props.style}>
        <div style={label}>Sensor Data</div>
        <div style={container}>
          <div style={{ ...row, marginTop: '3px' }}>
            <div>CH3 Concentration:</div>
            <div>
              {this.state.ch3.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{' '}
              ppm
            </div>
          </div>
          <div style={row}>
            <div>Temperature:</div>
            <div>
              {this.state.temperature.toLocaleString(undefined, {
                minimumFractionDigits: 1,
                minimumIntegerDigits: 2,
              })}
              &#176;C{' '}
            </div>
          </div>
          <div style={row}>
            <div>CO2 Concentration:</div>
            <div>
              {this.state.co2.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{' '}
              ppm
            </div>
          </div>
          <div style={row}>
            <div>O2 Concentration:</div>
            <div>
              {this.state.o2.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{' '}
              ppm
            </div>
          </div>
          <div style={row}>
            <div>NO Concentration:</div>
            <div>
              {this.state.no.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{' '}
              ppm
            </div>
          </div>
          <div style={row}>
            <div>NO2 Volume:</div>
            <div>
              {this.state.no2.toLocaleString(undefined, {
                minimumFractionDigits: 2,
              })}{' '}
              ppm
            </div>
          </div>
          <div style={row} />
          <div style={buttonRow}>
            <p>Save Sensor Data</p>
            <button type="button" onClick={this.fileStart} style={buttons}>
              Start
            </button>
            <button type="button" onClick={this.fileStop} style={buttons}>
              Stop
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default SensorData;
