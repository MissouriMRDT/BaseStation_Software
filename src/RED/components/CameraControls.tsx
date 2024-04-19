import React, { Component } from 'react';
import CSS from 'csstype';
import JSMpeg from '@cycjimmy/jsmpeg-player';

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

  src: string;

  sources: string[];

  player: any;

  videoContainerRef!: HTMLDivElement | null;

  constructor(props: IProps) {
    super(props);
    this.state = { rotationAngle: 0, currentSource: props.startSource, width: 0 };

    this.sources = props.sources;
    this.src = this.sources[props.startSource];
  }

  componentDidMount() {
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
    // if (this.player) {
    //   // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    //   // this.videoContainerRef!.style.height = `${this.videoContainerRef.clientWidth}px`;
    // }
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
    this.setState({ currentSource: newSource });
    this.player?.destroy();
    this.src = this.sources[newSource];
    this.player = new JSMpeg.VideoElement(document.getElementById('video-canvas' + this.props.startSource), this.src, {
      canvas: document.getElementById('video-canvas' + this.props.startSource),
    });
  }

  render(): JSX.Element {
    const { rotationAngle } = this.state;
    const videoStyle = {
      width: '100%',
      height: '100%',
      transform: `rotate(${rotationAngle}deg)`,
      transformOrigin: 'center',
    };
    return (
      <div style={this.props.style}>
        <div>
          <div style={videoContainerStyle} ref={(videoContainerRef) => (this.videoContainerRef = videoContainerRef)}>
            <div data-vjs-player>
              <canvas
                id={'video-canvas' + this.props.startSource}
                style={videoStyle}
                width="640px"
                height="480px"
              ></canvas>
            </div>
          </div>
          <div style={cameraSelectionContainer}>
            {Array.from({ length: 8 }, (_, i) => (
              <button
                key={i}
                onClick={() => this.setSource(i)}
                style={{
                  backgroundColor: this.state.currentSource === i ? 'gray' : '#EFEFEF',
                  borderRadius: '2px',
                  border: '1px solid gray',
                }}
              >
                {i + 1}
              </button>
            ))}
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
