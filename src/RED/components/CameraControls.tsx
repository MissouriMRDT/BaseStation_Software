import React, { Component } from 'react';
import CSS from 'csstype';

import Hls from 'hls.js';
import { setSource } from 'video.js/dist/types/tech/middleware';

const controlContainer: CSS.Properties = {
  display: 'grid',
  width: '250px',
  marginLeft: '5px',
  gridTemplateColumns: '12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%',
  cursor: 'pointer',
};

const cameraSelectionContainer: CSS.Properties = {
  display: 'grid',
  width: '250px',
  marginLeft: '5px',
  gridTemplateColumns: '12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%',
  cursor: 'pointer',
};

const rotationContainer: CSS.Properties = {
  display: 'grid',
  width: '250px',
  marginLeft: '5px',
  gridTemplateColumns: '33.33% 33.33% 33.33%',
  cursor: 'pointer',
};

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
  sources: string[];
  startSource: number;
}

interface IState {
  rotationAngle: number;
  currentSource: number;
}

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {
  static defaultProps = {};

  player: any;

  hls: any;

  sources: string[];

  constructor(props: IProps) {
    super(props);
    this.state = { rotationAngle: 0, currentSource: props.startSource };
    this.sources = props.sources;
    // props.sources[0].src = props.passedFileSource;
  }

  componentDidMount() {
    this.hls = new Hls();
    this.setSource(this.state.currentSource);
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
    console.log(this.hls.latency);
  };

  setSource(newSource: number) {
    const video = this.player;
    this.hls.destroy();
    this.hls = new Hls({
      maxBufferLength: 1,
      maxLiveSyncPlaybackRate: 2,
      liveDurationInfinity: true,
      liveSyncDuration: 3,
    });
    console.log('maxBufferLength: ' + this.hls.config.maxBufferLength);
    this.setState({ currentSource: newSource });

    this.hls.loadSource(this.sources[newSource]);
    this.hls.attachMedia(video);

    this.hls.on(Hls.Events.MANIFEST_PARSED, function () {
      video.play();
    });
  }

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
            <button onClick={() => this.setSource(0)}>1</button>
            <button onClick={() => this.setSource(1)}>2</button>
            <button onClick={() => this.setSource(2)}>3</button>
            <button onClick={() => this.setSource(3)}>4</button>
            <button onClick={() => this.setSource(4)}>5</button>
            <button onClick={() => this.setSource(5)}>6</button>
            <button onClick={() => this.setSource(6)}>7</button>
            <button onClick={() => this.setSource(7)}>8</button>
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
