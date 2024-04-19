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

const container: CSS.Properties = {
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  display: 'block',
};

interface IProps {
  style?: CSS.Properties;
  startSource: number;
  canvasWidth: number;
  canvasHeight: number;
  labelName: string;
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

  sources = [
    'ws://127.0.0.1:8082/cam',
    'ws://127.0.0.1:8084/cam',
    'ws://127.0.0.1:8086/cam',
    'ws://127.0.0.1:8088/cam',
    'ws://127.0.0.1:8090/cam',
    'ws://127.0.0.1:8092/cam',
    'ws://127.0.0.1:8094/cam',
    'ws://127.0.0.1:8096/cam',
  ];

  player: any;

  canvas!: HTMLCanvasElement | null;

  videoContainerRef!: HTMLDivElement | null;

  constructor(props: IProps) {
    super(props);
    this.state = { rotationAngle: 0, currentSource: props.startSource, width: 0 };

    this.src = this.sources[0];
    this.canvas = document.createElement('canvas');
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
    this.player = new JSMpeg.VideoElement(this.canvas, this.src, {
      canvas: this.canvas,
    });
  }

  render(): JSX.Element {
    const { rotationAngle } = this.state;
    const videoStyle: CSS.Properties = {
      width: '100%',
      height: '100%',
      transform: `rotate(${rotationAngle}deg)`,
      transformOrigin: 'center',
    };

    return (
      <div style={this.props.style}>
        <div style={this.props.labelName !== '' ? label : {}}> {this.props.labelName} </div>
        <div style={this.props.labelName !== '' ? container : {}}>
          <div style={videoContainerStyle} ref={(videoContainerRef) => (this.videoContainerRef = videoContainerRef)}>
            <div data-vjs-player>
              <canvas ref={(canvas) => (this.canvas = canvas)} style={videoStyle} width="640px" height="480px"></canvas>
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
