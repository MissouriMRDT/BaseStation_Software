import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';

const path = require('path');
const fs = require('fs');

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

interface IProps {
  style?: CSS.Properties;
  sources: string[];
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {

  folder: string;

  static defaultProps = {
    style: {},
    sources: [
      'assets\\tmpVideo\\stream.m3u8',
      'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8',
      'https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8',
      'https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8',
    ],
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.folder = path.join(__dirname, '..\\assets\\tmpVideo\\');
    // fs.readdir(this.folder, (err: ErrnoException | null, files: string[]) => {
    //   if (err) throw err;
      
    //   for (const file of files) {
    //       fs.unlinkSync(path.join(this.folder, file));
    //   }
      
    // });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          {/* source will eventually be something like: assets\tmpVideo\stream.m3u8 */}
          {/* <CameraControls hlsUrl={path.join(this.folder, 'stream.m3u8')} /> */}
          <CameraControls sources={this.props.sources} startSource={0} />
          <CameraControls sources={this.props.sources} startSource={1} />
          <CameraControls sources={this.props.sources} startSource={2} />
          <CameraControls sources={this.props.sources} startSource={3} />
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
