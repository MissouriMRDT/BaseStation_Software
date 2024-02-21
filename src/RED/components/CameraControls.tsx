import React, { Component } from 'react';
import CSS from 'csstype';

import Hls from 'hls.js';

const controlContainer: CSS.Properties = {
  display: 'grid',
  width: '250px',
  marginLeft: '5px',
  gridTemplateColumns: '12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%',
  cursor: 'pointer',
};

interface IProps {
  style?: CSS.Properties;
  // eslint-disable-next-line @typescript-eslint/ban-types
  hlsUrl: string;
}

interface IState {}

// this is the ffmpeg command (IP may need to be changed):

// ffmpeg -flags low_delay^
//  -f mpegts -i udp://169.254.144.138:1234^
//  -f mp4 -listen 1 -movflags frag_keyframe+empty_moov http://localhost:2234

// I know, this code is all jank, I just want it up on the git repository.

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {

  static defaultProps = {};

  player: any;

  hlsUrl: string;

  constructor(props: IProps) {
    super(props);
    this.state = {};
    this.hlsUrl = props.hlsUrl;
    // props.sources[0].src = props.passedFileSource;
  }

  componentDidMount() {
    // create video Player
    const video = this.player;
    const hls = new Hls();

    hls.loadSource(this.hlsUrl);
    hls.attachMedia(video);
    hls.on(Hls.Events.MANIFEST_PARSED, function () {
      video.play();
    });
  }

  componentWillUnmount() {
    // if (this.player) {
    //   this.player.dispose();
    // }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div data-vjs-player>
          <video
            className="videoCanvas"
            ref={(player) => (this.player = player)}
            autoPlay={true}
            style={{ width: '320px' }}
          ></video>
        </div>
        <div style={controlContainer}>
          <button type="button" style={{ cursor: 'pointer' }}>
            1
          </button>
          <button type="button">2</button>
          <button>3</button>
          <button>4</button>
          <button>5</button>
          <button>6</button>
          <button>7</button>
          <button>8</button>
        </div>
        <button
          type="button"
          onClick={() => {
            console.log('Clicked 1234');
          }}
        >
          1234
        </button>
      </div>
    );
  }
}

export default CameraControls;
