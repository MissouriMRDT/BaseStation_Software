import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { rovecam } from '../../Core/RoveCam/RoveCam';

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
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  myID: string;
  roverID: string;
}
class CameraController extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      myID: 'basestation',
      roverID: 'rover',
    };
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Camera Controller</div>
        <div style={container}>
          <div style={row}>
            <button onClick={() => rovecam.call('rover')}>Call</button>
            <button
              onClick={() => {
                console.log(rovecam.conn);
              }}
            >
              test
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default CameraController;
