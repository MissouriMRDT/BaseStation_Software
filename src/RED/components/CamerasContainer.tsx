/* eslint-disable @typescript-eslint/naming-convention */
import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';

const path = require('path');
const { Converter } = require('ffmpeg-stream');
const { readdir, unlinkSync, existsSync, mkdirSync } = require('fs');

const container: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  gridTemplateColumns: 'auto auto',
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
const videoStyle: CSS.Properties = {
  border: '2px solid #990000',
};
async function startFFMPEG(input: string, output: string) {
  const converter = new Converter();

  converter.createInputFromFile(input, {
    f: 'mpegts',
    codec: 'mpeg1video',
  });
  converter.createOutputToFile(output, {
    f: 'mpegts',
    s: '320x240',
    // probesize: '32',
    // flags: 'low_delay',
    // preset: 'ultrafast',
    // tune: 'zerolatency',
    'codec:v': 'mpeg1video',
    'b:v': '1000k',
    bf: '0',
  });

  // start processing
  try {
    await converter.run();
  } catch (e: any) {
    console.log('UDP Bind Failed on port ' + input + ' with error ' + e);
  }
}

interface IProps {
  style?: CSS.Properties;
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {
  folder: string;

  sources: string[];

  inSources: string[];

  cameraIPs: string[];

  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.folder = path.join(__dirname, '..\\assets\\tmpVideo\\');
    if (!existsSync(this.folder)) {
      mkdirSync(this.folder);
    }

    this.inSources = [
      'http://127.0.0.1:8081/cam',
      'http://127.0.0.1:8083/cam',
      'http://127.0.0.1:8085/cam',
      'http://127.0.0.1:8087/cam',
      'http://127.0.0.1:8089/cam',
      'http://127.0.0.1:8091/cam',
      'http://127.0.0.1:8093/cam',
      'http://127.0.0.1:8095/cam',
    ];

    this.sources = [
      'ws://127.0.0.1:8082/cam',
      'ws://127.0.0.1:8084/cam',
      'ws://127.0.0.1:8086/cam',
      'ws://127.0.0.1:8088/cam',
      'ws://127.0.0.1:8090/cam',
      'ws://127.0.0.1:8092/cam',
      'ws://127.0.0.1:8094/cam',
      'ws://127.0.0.1:8096/cam',
    ];

    this.cameraIPs = [
      '192.168.4.100:1181',
      '192.168.4.100:1182',
      '192.168.4.100:1183',
      '192.168.4.100:1184',
      '192.168.4.101:1185',
      '192.168.4.101:1186',
      '192.168.4.101:1187',
      '192.168.4.101:1188',
    ];

    readdir(this.folder, (err: Error | null, files: string[]) => {
      if (err) throw err;

      for (const file of files) {
        unlinkSync(path.join(this.folder, file));
      }

      // basestation ip
      // 192.168.100.10

      for (let i = 0; i < this.cameraIPs.length; i++) {
        require('child_process').fork(String.raw`src\RED\components\WebsocketRelay.js`, [
          'cam',
          8081 + i * 2,
          8082 + i * 2,
        ]);
        startFFMPEG('udp://' + this.cameraIPs[i], this.inSources[i]);
      }
    });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          <CameraControls style={videoStyle} sources={this.sources} startSource={0} />
          <CameraControls style={videoStyle} sources={this.sources} startSource={1} />
          <CameraControls style={videoStyle} sources={this.sources} startSource={2} />
          <CameraControls style={videoStyle} sources={this.sources} startSource={3} />
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
