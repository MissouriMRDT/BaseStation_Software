import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';

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
  cams: CameraControls[];
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
    cams: [],
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          <CameraControls hlsUrl={'https://bitdash-a.akamaihd.net/content/sintel/hls/playlist.m3u8'} />
          {/* <CameraControls passedFileSource={'http://localhost:2235'} />
          <CameraControls passedFileSource={'http://localhost:2236'} />
          <CameraControls passedFileSource={'http://localhost:2237'} /> */}
          <button>test</button>
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
