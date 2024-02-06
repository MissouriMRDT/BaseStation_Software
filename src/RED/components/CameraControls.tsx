import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import videojs from 'video.js';

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
  autoplay: boolean;
  controls: boolean;
  // eslint-disable-next-line @typescript-eslint/ban-types
  sources: object[];
}

interface IState {}

// this is the ffmpeg command (IP may need to be changed):

// ffmpeg -flags low_delay^
//  -f mpegts -i udp://169.254.144.138:1234^
//  -f mp4 -listen 1 -movflags frag_keyframe+empty_moov http://localhost:2234

// I know, this code is all jank, I just want it up on the git repository.

class CameraControls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
    autoplay: true,
    controls: true,
    sources: [{
      src: 'http://localhost:2234',
      // type: 'application/x-mpegURL'
      type: 'video/mp4'
    }]
  };

  player: any;

  videoNode: any;

  constructor(props: IProps) {
    super(props);
    this.state = {};
  }

  componentDidMount() {
    this.player = videojs(this.videoNode, this.props, () => {
      videojs.log('onPlayerReady', this);
    });
  }

  componentWillUnmount() {
    if (this.player) {
      this.player.dispose();
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Camera Controls</div>
        <div style={container}>
          <div data-vjs-player>
            <video
              ref={(node) => (this.videoNode = node)}
              className="video-js"
              style={{ backgroundColor: 'black', overflow: 'hidden' }}
            ></video>
          </div>
        </div>
      </div>
    );
  }
}

export default CameraControls;
