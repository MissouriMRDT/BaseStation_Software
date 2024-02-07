import React, { Component } from 'react';
import CSS from 'csstype';
import CameraControls from './CameraControls';
import { createPortal } from 'react-dom';

const container: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  flexDirection: 'column',
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
          {/* All cameras currently listen to the same stream. TODO Set cameras to different streams */}
          <CameraControls passedFileSource={'http://localhost:2234'} />
          <CameraControls passedFileSource={'http://localhost:2234'} />
          <CameraControls passedFileSource={'http://localhost:2234'} />
          <CameraControls passedFileSource={'http://localhost:2234'} />
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
