import React, { Component } from 'react';
import CSS from 'csstype';

import no_cam_img from '../../../../assets/no_cam_img.png';

const CAM_IPS = [
    no_cam_img,
    'https://x.x.x.x:xxxx/steam.mjpg',
];

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  width: '33%',
  height: '100%',
};

const button: CSS.Properties = {
  fontFamily: 'arial',
  flexGrow: 1,
  margin: '5px',
  fontSize: '14px',
  lineHeight: '24px',
  borderWidth: '2px',
  height: '40px',
  marginTop: '10px',
  marginBottom: '10px'
};

const label: CSS.Properties = {
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};

const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  alignContent: 'center',
};

const cam: CSS.Properties = {
  height: 'calc(100% - 20px)',
  backgroundImage: no_cam_img,
};

interface IProps {
    style?: CSS.Properties;
}

interface IState {
  currentCamera: number;
}

class DroneCamera extends Component<IProps, IState> {
    
    static defaultProps = {
        style: {},
    }

    constructor(props: IProps) {
        super(props);
        this.state = {
          currentCamera: 0,
        };
    }

    refresh(): void {
        const curCam = this.state.currentCamera;
        this.setState({ currentCamera: 0 }, () => this.setState({ currentCamera: curCam }));
    }

    componentDidMount(): void {
      this.setState({ currentCamera: 1 });
    }

  render(): JSX.Element {
    return (
      <div style={{ width: '100%' }}>
        <div style={label}>Drone Camera</div>
          <div style={container}>
            <div style={{ ...column, }}>
              <div style={row}>
                <img
                  src={CAM_IPS[this.state.currentCamera]} 
                  alt={'Drone Camera'} 
                  style={{ ...cam, /*...this.state.style*/ }} 
                />
              </div>
              <div style={row}>
                <button type='button' style={button} onClick={this.refresh}>Refresh</button>
              </div>
          </div>
        </div>
      </div>
    )
  }
}

export default DroneCamera;
