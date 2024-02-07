import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import videojs from 'video.js';
import 'video.js/dist/video-js.css';

const container: CSS.Properties = {
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  margin: '5px',
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

const controlContainer: CSS.Properties = {
  display: "grid",
  width: "250px",
  marginLeft: "5px",
  gridTemplateColumns: "12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%",
  cursor: "pointer"
}


interface IProps {
  style?: CSS.Properties;
  autoplay: boolean;
  controls: boolean;
  // eslint-disable-next-line @typescript-eslint/ban-types
  sources: { src: string; type: string }[];
  passedFileSource: string;
}

interface IState {}

// this is the ffmpeg command (IP may need to be changed):

// ffmpeg -flags low_delay^
//  -f mpegts -i udp://169.254.144.138:1234^
//  -f mp4 -listen 1 -movflags frag_keyframe+empty_moov http://localhost:2234

// I know, this code is all jank, I just want it up on the git repository.

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
    autoplay: true,
    controls: false,
    sources: [
      {
        // src is passed through CamerasController.tsx
        src: '',
        // type: 'application/x-mpegURL'
        type: 'video/mp4',
      },
    ],
  };

  player: any;

  videoNode: any;

  constructor(props: IProps) {
    super(props);
    this.state = {};
    props.sources[0].src = props.passedFileSource;
  }

  componentDidMount() {
    // create video Player
    this.player = videojs(this.videoNode, this.props, () => {
      videojs.log('onPlayerReady', this);
    });

    // liveTracker will consider itself "not live" if more than 2 seconds behind (default is 15 seconds)
    this.player.liveTracker.options.liveTolerance = 2;
    // force liveTracker to catch up when we start listening to stream
    this.player.liveTracker.seekToLiveEdge();
  }

  componentWillUnmount() {
    if (this.player) {
      this.player.dispose();
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div data-vjs-player>
          <video
            ref={(node) => (this.videoNode = node)}
            className="video-js"
            style={{ backgroundColor: "black", overflow: 'hidden', display: 'block', width: "250px", margin: "5px"}}
          ></video>

        </div>
        <div style={controlContainer}>
            <button type="button" style={{cursor: "pointer"}}>
              1
            </button>
            <button type="button">
              2
            </button>
            <button>
              3
            </button>
            <button>
              4
            </button>
            <button>
              5
            </button>
            <button>
              6
            </button>
            <button>
              7
            </button>
            <button>
              8
            </button>
          </div>
          <button type="button" onClick={() => {console.log("Clicked 1234")}}>1234</button>
      </div>
    );
  }
}

export default CameraControls;
