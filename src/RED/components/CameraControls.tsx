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

const cameraSelectionContainer: CSS.Properties = {
  display: "grid",
  width: "250px",
  marginLeft: "5px",
  gridTemplateColumns: "12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%",
  cursor: "pointer"
}

const rotationContainer: CSS.Properties = {
  display: "grid",
  width: "250px",
  marginLeft: "5px",
  gridTemplateColumns: "33.33% 33.33% 33.33%",
  cursor: "pointer"
}

const videoContainerStyle: CSS.Properties = {
  width: '320px',
  height: '320px',
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  overflow: 'hidden',
};

interface IProps {
  style?: CSS.Properties;
  // eslint-disable-next-line @typescript-eslint/ban-types
  hlsUrl: string;
}

interface IState {
  rotationAngle: number;
}

// this is the ffmpeg command (IP may need to be changed):

// ffmpeg -flags low_delay^
//  -i "D:media\videos\Danger 5\S1 E1 - I Danced For Hitler.mkv"^
//  -f hls C:\Users\rfboe\Documents\GitHub\BaseStation_Software\assets\tmpVideo\stream.m3u8

// I know, this code is all jank, I just want it up on the git repository.

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {

  static defaultProps = {};

  player: any;

  hlsUrl: string;

  constructor(props: IProps) {
    super(props);
    this.state = {rotationAngle: 0,};
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


  rotateVideo = (angle: number) => {
    if (angle === 0) {
      this.setState({ rotationAngle: 0 });
    } else {
      this.setState((prevState) => ({
        rotationAngle: prevState.rotationAngle + angle,
      }));
    }
  };

  render(): JSX.Element {
    const { rotationAngle } = this.state;
    const videoStyle = {
      width: '320px',
      transform: `rotate(${rotationAngle}deg)`,
      transformOrigin: 'center',
    };
    return (
      <div style={this.props.style}>
        <div>
        <div style={videoContainerStyle}>
        <div data-vjs-player>
          <video
            className="videoCanvas"
            ref={(player) => (this.player = player)}
            autoPlay={true}
            style={videoStyle}
          ></video>
        </div>
        </div>
        <div style={cameraSelectionContainer}>
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
<div style={rotationContainer}>
          <button onClick={() => this.rotateVideo(0)}>Reset</button>
          <button onClick={() => this.rotateVideo(90)}>Rotate 90</button>
          <button onClick={() => this.rotateVideo(180)}>Rotate 180</button>
        </div>
        </div>
        
      </div>
    );
  }
}

export default CameraControls;
