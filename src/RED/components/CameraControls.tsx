import React, { Component } from 'react';
import CSS from 'csstype';
import JSMpeg from '@cycjimmy/jsmpeg-player';
import { windows } from '../../Core/Window';
import html2canvas from 'html2canvas';
import fs from 'fs';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
// import { dataSizes } from '../../Core/RoveProtocol/Rovecomm3';
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
  width: 'fit-content',
};

interface IProps {
  style?: CSS.Properties;
  startSource: number;
  labelName: string;
  gripperCam: number;
  cameraToggle: boolean;
  defaultWidth: number;
}

interface IState {
  rotationAngle: number;
  currentSource: number;
  elementWidth: number;
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

  canvasParent!: HTMLDivElement | null;

  constructor(props: IProps) {
    super(props);
    this.state = {
      rotationAngle: 0,
      currentSource: props.startSource,
      id: `CameraControls_${CameraControls.id}`,
      screenshotClicked: 'initial',
      // in pixels
      elementWidth: this.props.defaultWidth,
    };

    this.src = this.sources[0];
    // this.canvas = document.createElement('canvas');

    this.refreshSource = this.refreshSource.bind(this);
    // this.takePano = this.takePano.bind(this);
    // this.rotateGimbal90 = this.rotateGimbal90.bind(this);
    this.timeoutInterval = setInterval(this.refreshSource, this.refreshInterval * 1000);
    this.rotateVideo = this.rotateVideo.bind(this);
    this.camToggle = this.camToggle.bind(this);
    this.setSource = this.setSource.bind(this);
    this.saveImage = this.saveImage.bind(this);
    this.refreshSource = this.refreshSource.bind(this);
    this.toggleStream = this.toggleStream.bind(this);
    this.setElementWidth = this.setElementWidth.bind(this);
    this.updateWidth = this.updateWidth.bind(this);
    this.waitForGreen = this.waitForGreen.bind(this);

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

  // rotateGimbal90(direction: boolean) {
  //   if (direction) {
  //     rovecomm.sendCommand('LeftMainGimbalIncrement', 'Core', [90, 0]);
  //   } else {
  //     rovecomm.sendCommand('LeftMainGimbalIncrement', 'Core', [-35, 0]);
  //   }
  // }

  // takePano() {
  //   const directions = [true, false, false, false, false, false];
  //   let delay = 1000; // Initial delay before first movement
  //   const numMovements = directions.length;

  //   for (let i = 0; i < numMovements; i++) {
  //     setTimeout(() => {
  //       this.rotateGimbal90(directions[i]);
  //       if (i === numMovements - 1) {
  //         this.waitForGreen(() => {
  //           this.saveImage(1); // Save the final image after the last movement
  //         });
  //       } else {
  //         this.waitForGreen(() => {
  //           this.saveImage(0); // Save images at intermediate stops
  //         });
  //       }
  //     }, delay);

  //     delay += 2000; // Increment delay for next movement
  //   }
  // }

  waitForGreen(callback: () => void) {
    const checkGreen = () => {
      if (this.state.screenshotClicked !== 'green') {
        setTimeout(checkGreen, 100); // Check every 100 milliseconds
      } else {
        // Once screenshotClicked is 'green', execute the callback
        callback();
      }
    };

    checkGreen(); // Start checking
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

  toggleStream(source: number, restartStream: number) {
    if (this.state.currentSource < 4) {
      rovecomm.sendCommand('ToggleStream1', 'Camera1', [source, restartStream]);
    } else {
      rovecomm.sendCommand('ToggleStream2', 'Camera2', [source - 4, restartStream]);
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
      this.setState({
        rotationAngle: this.state.rotationAngle + angle,
      });
    }
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    console.log(this.state.rotationAngle);
    this.canvas!.style.transform = 'rotate(' + (this.state.rotationAngle + angle) + 'deg)';
  };

  setSource(newSource: number) {
    this.setState({ currentSource: newSource });
    this.player?.destroy();
    this.player = null;
    this.src = this.sources[newSource];
    this.canvasParent?.children[0]?.remove();

    const newCanvas = document.createElement('canvas');
    newCanvas.style.width = '100%';
    newCanvas.style.transformOrigin = 'center';
    newCanvas.style.transform = 'rotate(' + this.state.rotationAngle + 'deg)';

    this.canvas = newCanvas;

    this.canvasParent?.appendChild(newCanvas);
    this.player = new JSMpeg.Player(this.src, {
      canvas: newCanvas,
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

  setElementWidth(newWidth: number) {
    this.setState({ elementWidth: Math.max(320, Math.min(newWidth, 1600)) });
  }

  downloadURL(imgData: string): void {
    // Takes in the octet-string of a camera feed and saves it as an image

    // The camera feed will be saved in the applications Screenshots folder with the filename
    // YYYY.MM.DD.HH.SS.sss.Cam{camNum}.png
    const filename = `./Screenshots/${new Date()
      .toISOString()
      // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
      // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
      .replaceAll(/[:\-TZ]/g, '.')}Cam${this.state.id}.png`;

    // Create the screenshots directory if it doesn't exist
    if (!fs.existsSync('./Screenshots')) {
      fs.mkdirSync('./Screenshots');
    }

    // Encode the camera feed and write it to a file
    const base64Image = imgData.replace('image/png', 'image/octet-stream').split(';base64,').pop();
    if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
  }

  captureCanvas(): void {
    // Initial handler to start saving an image of the camera feed

    // Look through all of the window documents for the current camera feed
    let camera;
    let thisWindow;
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById('camera-view')) {
        thisWindow = windows[win];
        camera = thisWindow.document.getElementById('camera-view');
        break;
      }
    }

    if (!camera) {
      throw new Error(`The element '${camera}' wasn't found`);
    }

    // Then use package to turn the html into a canvas, which when complete calls downloadURL
    // to save it as an image
    // NOTE: This package seems to have issues with the dynamic nature of the camera feeds, and
    // is only able to return the html for the first few seconds of operation
    html2canvas(camera, {
      scrollX: 0,
      scrollY: -thisWindow.scrollY,
      useCORS: true,
      allowTaint: true,
    })
      .then((canvas) => {
        const imgData = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
        this.downloadURL(imgData);
        return null;
      })
      .catch((error) => {
        console.error(error);
      });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={this.props.labelName !== '' ? label : {}}>
          {' '}
          <button
            // eslint-disable-next-line prettier/prettier
            onClick={() => this.setElementWidth(this.state.elementWidth + 160)}
          >
            {' '}
            +
          </button>
          <button
            // eslint-disable-next-line prettier/prettier
            onClick={() => this.setElementWidth(this.state.elementWidth - 160)}
          >
            {' '}
            -
          </button>{' '}
          {this.props.labelName}{' '}
        </div>
        <div style={container}>
          <div style={videoContainerStyle} ref={(videoContainerRef) => (this.videoContainerRef = videoContainerRef)}>
            <div
              data-vjs-player
              ref={(canvasParent) => (this.canvasParent = canvasParent)}
              style={{ width: this.state.elementWidth }}
              id="camera-view"
            >
              {/* <canvas ref={(canvas) => (this.canvas = canvas)} style={videoStyle}></canvas> */}
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
            <button onClick={() => this.captureCanvas()}>Capture Canvas</button>
            <button onClick={() => this.toggleStream(this.state.currentSource, 0)}>Stop Stream</button>
            <button onClick={() => this.toggleStream(this.state.currentSource, 1)}>Restart Stream</button>
            {/* <button onClick={() => this.takePano()}>Take Pano</button> */}
          </div>
        </div>
      </div>
    );
  }
}

export default CameraControls;
