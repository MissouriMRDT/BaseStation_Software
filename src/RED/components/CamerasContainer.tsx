import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';

const path = require('path');
const { Converter } = require("ffmpeg-stream")
const { readdir, unlinkSync } = require("fs")

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

async function startFFMPEG(input: string, output:string) {
  const converter = new Converter()
 
  converter.createInputFromFile(input, {});
  converter.createOutputToFile(output, {
    f: 'hls',
    hls_flags: 'delete_segments'
  });

  // start processing
  await converter.run()
}

interface IProps {
  style?: CSS.Properties;
  cams: CameraControls[];
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {

  folder: string;

  static defaultProps = {
    style: {},
    cams: [],
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.folder = path.join(__dirname, '..\\assets\\tmpVideo\\');
    readdir(this.folder, (err: ErrnoException | null, files: string[]) => {
      if (err) throw err;
      
      for (const file of files) {
        unlinkSync(path.join(this.folder, file));
      }
      
      startFFMPEG('D:media\\videos\\Danger 5\\S1 E1 - I Danced For Hitler.mkv', path.join(this.folder, 'stream.m3u8'));
    }, );
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          {/* source will eventually be something like: assets\tmpVideo\stream.m3u8 */}
          <CameraControls hlsUrl={path.join(this.folder, 'stream.m3u8')} />
          {/* <CameraControls hlsUrl={'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8'} />
          <CameraControls hlsUrl={'https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8'} />
          <CameraControls hlsUrl={'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8'} /> */}
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
