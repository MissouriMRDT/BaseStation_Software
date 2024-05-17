import React, { Component } from 'react';
import CSS from 'csstype';
import JSMpeg from '@cycjimmy/jsmpeg-player';
import { windows } from '../../Core/Window';
// import fs from 'fs';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { dataSizes } from '../../Core/RoveProtocol/Rovecomm3';
// import { ContinuousColorLegend } from 'react-vis';

const cameraSelectionContainer: CSS.Properties = {
  display: 'grid',
  width: '100%',
  gridTemplateColumns: '12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5% 12.5%',
  cursor: 'pointer',
};

const rotationContainer: CSS.Properties = {
  display: 'grid',
  width: '100%',
  gridTemplateColumns: '25% 25% 25% 25%',
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
  gripperCam: number;
  cameraToggle: boolean;
}

interface IState {
  rotationAngle: number;
  currentSource: number;
  width: number;
  id: string;
  screenshotClicked: string;
}

// CameraControls: represents a single camera view w/ controls. should be contained under a CamerasContainer
class CameraControls extends Component<IProps, IState> {
  static defaultProps = {
    cameraToggle: false,
    gripperCam: 7,
  };

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

  static id = 0;

  player: any;

  canvas!: HTMLCanvasElement | null;

  videoContainerRef!: HTMLDivElement | null;

  // in seconds
  refreshInterval = 60;

  timeoutInterval: NodeJS.Timeout;

  constructor(props: IProps) {
    super(props);
    this.state = {
      rotationAngle: 0,
      currentSource: props.startSource,
      width: 0,
      id: `CameraControls_${CameraControls.id}`,
      screenshotClicked: 'initial',
    };

    this.src = this.sources[0];
    this.canvas = document.createElement('canvas');

    this.refreshSource = this.refreshSource.bind(this);
    this.takePano = this.takePano.bind(this);
    this.rotateGimbal90 = this.rotateGimbal90.bind(this);
    this.timeoutInterval = setInterval(this.refreshSource, this.refreshInterval * 1000);

    rovecomm.on('PictureTaken1', (data: number[]) => {
      if (data[0] === 1) {
        this.setState({ screenshotClicked: 'green' });
      }
    });
    rovecomm.on('PictureTaken2', (data: number[]) => {
      if (data[0] === 1) {
        this.setState({ screenshotClicked: 'green' });
      }
    });
  }

  rotateGimbal90(direction: boolean) {
    if (direction) {
      rovecomm.sendCommand('LeftMainGimbalIncrement', 'Core', [90, 0]);
    } else {
      rovecomm.sendCommand('LeftMainGimbalIncrement', 'Core', [-35, 0]);
    }
  }

  takePano() {
    const moveGimbalAndWait = (direction: boolean, delay: number) => {
      setTimeout(() => {
        this.rotateGimbal90(direction);
        const checkState = () => {
          if (this.state.screenshotClicked !== 'green') {
            setTimeout(checkState, 100); // Check every 100 milliseconds
          } else {
            // Move to the next step or complete the process
            this.saveImage(0); // Save the image
            if (direction) {
              moveGimbalAndWait(false, 2000); // Move to the next direction after 2 seconds
            }
          }
        };
        checkState(); // Start checking the state
      }, delay);
    };

    // Start the pano process by moving the gimbal to the initial position
    moveGimbalAndWait(false, 1000);
  }

  componentDidUpdate(prevProps: IProps) {
    if (prevProps.gripperCam !== this.props.gripperCam) {
      this.camToggle();
    }
  }

  camToggle() {
    if (this.props.cameraToggle) {
      console.log(this.props.gripperCam);
      this.setSource(this.props.gripperCam - 1);
    }
  }

  componentDidMount() {
    this.setSource(this.state.currentSource);
    this.updateWidth();
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        windows[win].addEventListener('resize', this.updateWidth);
        windows[win].addEventListener('focus', this.refreshSource);
      }
    }
  }

  componentWillUnmount() {
    // if (this.player) {
    //   this.player.dispose();
    // }
    clearInterval(this.timeoutInterval);
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        windows[win].removeEventListener('resize', this.updateWidth);
        windows[win].removeEventListener('focus', this.refreshSource);
      }
    }
    // if (this.player !== undefined) this.player.destroy();
  }

  updateWidth = () => {
    // if (this.player) {
    //   // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    //   // this.videoContainerRef!.style.height = `${this.videoContainerRef.clientWidth}px`;
    // }
  };

  rotateVideo = (angle: number) => {
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
    this.player = new JSMpeg.Player(this.src, {
      canvas: this.canvas,
      audio: false,
      preserveDrawingBuffer: true,
      pauseWhenHidden: false,
      videoBufferSize: 1024 * 1024 * 4,
    });
  }

  refreshSource() {
    console.log(this.state);
    this.setSource(this.state.currentSource);
  }

  saveImage(restartStream: number): void {
    this.setState({ screenshotClicked: 'red' });
    if (this.state.currentSource < 4) {
      const data = [this.state.currentSource, restartStream];
      rovecomm.sendCommand('TakePicture', 'Camera1', data);
    } else {
      const data = [this.state.currentSource - 4, restartStream];
      rovecomm.sendCommand('TakePicture', 'Camera2', data);
    }

    // Define a function to wait until screenshotClicked is changed to 'green' by rovecomm
    const waitForGreen = () => {
      if (this.state.screenshotClicked !== 'green') {
        setTimeout(waitForGreen, 100); // Check every 100 milliseconds
      } else {
        // Reset screenshotClicked after 3 seconds once it's changed to 'green'
        setTimeout(() => {
          this.setState({ screenshotClicked: 'initial' });
        }, 3000);
      }
    };

    waitForGreen(); // Start waiting

    // // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    // const image = this.canvas!.toDataURL('image/png').replace('image/png', 'image/octet-stream');
    // let filename = '';
    // if (pano !== '') {
    //   filename = `./Screenshots/${new Date().toISOString().replaceAll(/[:\-TZ]/g, '.')}Camera${pano}.png`;
    // } else {
    //   filename = `./Screenshots/${new Date().toISOString().replaceAll(/[:\-TZ]/g, '.')}Camera.png`;
    // }
    // console.log(filename);

    // if (!fs.existsSync('./Screenshots')) {
    //   fs.mkdirSync('./Screenshots');
    // }

    // const base64Image = image.replace('image/png', 'image/octet-stream').split(';base64,').pop();
    // if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
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
              <canvas ref={(canvas) => (this.canvas = canvas)} style={videoStyle}></canvas>
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
            <button onClick={() => this.rotateVideo(0)}>Reset Rotation</button>
            <button onClick={() => this.rotateVideo(90)}>Rotate 90</button>
            <button onClick={() => this.rotateVideo(180)}>Rotate 180</button>
            <button
              onClick={() => this.saveImage(1)}
              style={{
                backgroundColor: this.state.screenshotClicked,
              }}
            >
              Screenshot
            </button>
            <button onClick={() => this.takePano()}>Take Pano</button>
          </div>
        </div>
      </div>
    );
  }
}

export default CameraControls;
