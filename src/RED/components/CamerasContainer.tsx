/* eslint-disable @typescript-eslint/naming-convention */
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

interface IProps {
  style?: CSS.Properties;

  // TODO cam amount does nothing
  camAmount: number;
  canvasWidth: number;
  canvasHeight: number;
}

interface IState {}

class CamerasContainer extends Component<IProps, IState> {
  outSources: string[];

  cameraIPs: string[];

  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.outSources = [
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

    // basestation ip
    // 192.168.100.10
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}> Camera Controls </div>
        <div style={container}>
          <CameraControls
            style={videoStyle}
            sources={this.outSources}
            startSource={0}
            canvasWidth={this.props.canvasWidth}
            canvasHeight={this.props.canvasHeight}
          />
          <CameraControls
            style={videoStyle}
            sources={this.outSources}
            startSource={1}
            canvasWidth={this.props.canvasWidth}
            canvasHeight={this.props.canvasHeight}
          />
          <CameraControls
            style={videoStyle}
            sources={this.outSources}
            startSource={2}
            canvasWidth={this.props.canvasWidth}
            canvasHeight={this.props.canvasHeight}
          />
          <CameraControls
            style={videoStyle}
            sources={this.outSources}
            startSource={3}
            canvasWidth={this.props.canvasWidth}
            canvasHeight={this.props.canvasHeight}
          />
        </div>
      </div>
    );
  }
}

export default CamerasContainer;
