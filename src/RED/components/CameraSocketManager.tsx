import path from 'path';
import React from 'react';
import { Component } from 'react';

const { Converter } = require('ffmpeg-stream');

async function startFFMPEG(input: string, output: string) {
  const converter = new Converter();

  converter.createInputFromFile(input, {
    f: 'mpegts',
    codec: 'mpeg1video',
  });

  converter.createOutputToFile(output, {
    f: 'mpegts',
    s: '320x240',
    'codec:v': 'mpeg1video',
    'b:v': '64k',
    maxrate: '128k',
    bf: '0',
  });

  // start processing
  try {
    await converter.run();
  } catch (e: any) {
    console.log('UDP Bind Failed on port ' + input + ' with error ' + e);
  }
}

interface IProps {}

interface IState {}

class CameraSocketManager extends Component<IProps, IState> {
  inSources: string[];

  cameraIPs: string[];

  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

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

    // basestation ip
    // 192.168.100.10

    for (let i = 0; i < this.cameraIPs.length; i++) {
      require('child_process').fork(path.join(__dirname, '../assets/WebsocketRelay.js'), [
        'cam',
        8081 + i * 2,
        8082 + i * 2,
      ]);
      startFFMPEG('udp://' + this.cameraIPs[i] + '?buffer_size=2000"&"fifo_size=1024', this.inSources[i]);
    }
  }

  render(): JSX.Element {
    return <div>{/* <p>{path.join(__dirname, '../node_modules/ws/index.js')}</p> */}</div>;
  }
}
export default CameraSocketManager;
