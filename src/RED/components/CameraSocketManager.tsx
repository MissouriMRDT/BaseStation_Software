import path from 'path';
import React from 'react';
import { Component } from 'react';

import { Converter } from 'ffmpeg-stream';

async function startFFMPEG(input: string, output: string) {
  const converter = new Converter();

  converter.createInputFromFile(input, {
    f: 'mpegts',
    codec: 'mpeg1video',
  });

  converter.createOutputToFile(output, {
    f: 'mpegts',
    s: '480x320',
    'codec:v': 'mpeg1video',
    'b:v': '512k',
    maxrate: '524k',
    bf: '0',
  });

  // start processing
  const recursiveRetry = (numRetries: number) => {
    converter.run().catch((err: any) => {
      if (numRetries < 30) {
        if (numRetries < 15) setTimeout(recursiveRetry, 1000, numRetries + 1); // retry every 1s 15 times
        else if (numRetries < 10) setTimeout(recursiveRetry, 5000, numRetries + 1); // then retry every 5s 10 times
        else setTimeout(recursiveRetry, 10000, numRetries + 1); // then retry every 10s 5 times
      } else console.log('UDP Bind Repeatedly Failed on port ' + input + ' with error ' + err);
    });
  };
  recursiveRetry(0);
}

interface IProps {}

interface IState {}

class CameraSocketManager extends Component<IProps, IState> {
  inSources: string[];

  cameraIPs: string[];

  processStopper: AbortController = new AbortController();

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
      const child = require('child_process').fork(
        path.join(__dirname, '../assets/WebsocketRelay.js'),
        ['cam', 8081 + i * 2, 8082 + i * 2],
        { signal: this.processStopper.signal }
      );
      child.on('error', (m: any) => {
        //try to pass pid along with the signal from child to parent
        console.log('completed: ' + m);
        //killing child process when work signals it's done
        process.kill(m.pid);
      });
      startFFMPEG('udp://' + this.cameraIPs[i] + '?buffer_size=2000"&"fifo_size=1024', this.inSources[i]);
    }
  }

  componentWillUnmount(): void {
    this.processStopper.abort(); // doesn't work still idk
  }

  render(): JSX.Element {
    return <div>{/* <p>{path.join(__dirname, '../node_modules/ws/index.js')}</p> */}</div>;
  }
}
export default CameraSocketManager;
