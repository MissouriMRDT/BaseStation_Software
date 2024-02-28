import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';

const path = require('path');
const { Converter } = require('ffmpeg-stream');
const { readdir, unlinkSync } = require('fs');

const container: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  gridTemplateColumns: 'auto auto',
  padding: '5px',
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

async function startFFMPEG(input: string, output: string) {
  const converter = new Converter();

  converter.createInputFromFile(input, {});
  converter.createOutputToFile(output, {
    f: 'hls',
    hls_flags: 'delete_segments',
    hls_time: '0.5',
    force_key_frames: 'expr:if(isnan(prev_forced_n),1,eq(n,prev_forced_n+8))',
    g: '8',
    segment_time: '0.5',
    probesize: '32',
    flags: 'low_delay',
    preset: 'ultrafast',
    tune: 'zerolatency',
  });

  // start processing
  await converter.run();
}

interface IProps {
  style?: CSS.Properties;
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {

  folder: string;

  sources: string[];

  cameraIPs: string[];

  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.folder = path.join(__dirname, '..\\assets\\tmpVideo\\');

    this.sources = [
      path.join(this.folder, 'stream.m3u8'),
      'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8',
      'https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8',
      'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8',
    ];

    this.cameraIPs = [
      '192.168.4.100:1181',
      '192.168.4.100:1182',
      '192.168.4.100:1183',
      '192.168.4.100:1184',
      '192.168.4.101:1181',
      '192.168.4.101:1182',
      '192.168.4.101:1183',
      '192.168.4.101:1184',
    ];
    // fs.readdir(this.folder, (err: ErrnoException | null, files: string[]) => {
    //   if (err) throw err;
    readdir(this.folder, (err: ErrnoException | null, files: string[]) => {
      if (err) throw err;

      for (const file of files) {
        unlinkSync(path.join(this.folder, file));
      }

      // cam streaming ips
      // 192.168.4.100:1181
      // 192.168.4.100:1182
      // 192.168.4.100:1183
      // 192.168.4.100:1184
      // 192.168.4.101:1181
      // 192.168.4.101:1182
      // 192.168.4.101:1183
      // 192.168.4.101:1184

      // basestation ip
      // 192.168.100.10
      // for (let i = 0; i < this.cameraIPs.length; i++) {
      //   startFFMPEG('udp://' + this.cameraIPs[i], path.join(this.folder, 'stream' + i + '.m3u8'));
      // }

      startFFMPEG('udp://169.254.144.138:1181', path.join(this.folder, 'stream.m3u8'));
    });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          {/* source will eventually be something like: assets\tmpVideo\stream.m3u8 */}
          {/* <CameraControls hlsUrl={path.join(this.folder, 'stream.m3u8')} /> */}
          <CameraControls sources={this.sources} startSource={0} />
          <CameraControls sources={this.sources} startSource={1} />
          <CameraControls sources={this.sources} startSource={2} />
          <CameraControls sources={this.sources} startSource={3} />
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
