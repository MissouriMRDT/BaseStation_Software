import React, { Component } from 'react';
import CSS from 'csstype';

import Hls from 'hls.js';

const cameraSelectionContainer: CSS.Properties = {
  display: 'grid',
  width: '100%',
  gridTemplateColumns: '12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%',
  cursor: 'pointer',
};

const rotationContainer: CSS.Properties = {
  display: 'grid',
  width: '100%',
  gridTemplateColumns: '33.33% 33.33% 33.33%',
  cursor: 'pointer',
};

const videoContainerStyle: CSS.Properties = {
  width: '100%',
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
  width: number;
}

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {
  static defaultProps = {};

  player: HTMLVideoElement | null = null;

  hls: any;

  sources: string[];

  videoContainerRef!: HTMLDivElement | null;

  constructor(props: IProps) {
    super(props);
    this.state = { rotationAngle: 0, currentSource: props.startSource, width: 0 };
    this.sources = props.sources;
    // props.sources[0].src = props.passedFileSource;
  }

  componentDidMount() {
    this.hls = new Hls();
    this.setSource(this.state.currentSource);
    this.updateWidth();
    window.addEventListener('resize', this.updateWidth);
  }

  componentWillUnmount() {
    // if (this.player) {
    //   this.player.dispose();
    // }
    window.removeEventListener('resize', this.updateWidth);
  }

  updateWidth = () => {
    if (this.player) {
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.videoContainerRef!.style.height = `${this.player.clientWidth}px`;
    }
  };

  rotateVideo = (angle: number) => {
    console.log(this.state.width);
    if (angle === 0) {
      this.setState({ rotationAngle: 0 });
    } else {
      this.setState((prevState) => ({
        rotationAngle: prevState.rotationAngle + angle,
      }));
    }
  };

  setSource(newSource: number) {
    const video = this.player;
    this.hls.destroy();
    this.hls = new Hls({
      maxBufferLength: 1,
      maxMaxBufferLength: 1,
      maxBufferSize: 100, // in bytes
      maxLiveSyncPlaybackRate: 2,
      liveDurationInfinity: true,
      liveSyncDurationCount: 1,
      liveMaxLatencyDurationCount: 2,
    });

    this.setState({ currentSource: newSource });

    this.hls.loadSource(this.sources[newSource]);
    this.hls.attachMedia(video);

    this.hls.on(Hls.Events.MANIFEST_PARSED, () => {
      const playPromise = video?.play();

      if (playPromise !== undefined) {
        playPromise
          .then((_) => {
            return;
          })
          .catch((error) => {});
      }
    });
  }

  render(): JSX.Element {
    const { rotationAngle } = this.state;
    const videoStyle = {
      width: '100%',
      transform: `rotate(${rotationAngle}deg)`,
      transformOrigin: 'center',
    };
    return (
      <div style={this.props.style}>
        <div>
          <div style={videoContainerStyle} ref={(videoContainerRef) => (this.videoContainerRef = videoContainerRef)}>
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
