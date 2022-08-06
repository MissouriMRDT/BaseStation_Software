import React, { Component } from 'react';
import CSS from 'csstype';
import { RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
import { ColorConverter } from '../../Core/ColorConverter';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  alignItems: 'center',
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
  devices: any;
  style?: CSS.Properties;
}

interface IState {
  lastWidth: number;
}

// For colorConverter
const min = 0;
const cutoff = 45;
const max = 100;
const greenHue = 120;
const redHue = 360;

class PingMap extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  canvasRef: any;

  constructor(props: IProps) {
    super(props);
    this.state = {
      lastWidth: 300,
    };
    this.updatePingMap = this.updatePingMap.bind(this);
    this.canvasRef = React.createRef();
  }

  componentDidUpdate(prevProps: IProps) {
    // Any time props are recieved that are different than the previous props, update the pingmap
    if (this.props.devices !== prevProps.devices || this.state.lastWidth !== 300) {
      this.updatePingMap();
    }
  }

  updatePingMap(): void {
    this.setState({ lastWidth: 300 });

    let text;

    const canvas = this.canvasRef.current;
    const context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);

    const centerW = context.canvas.width / 2;

    context.font = '12px Arial';
    context.textBaseline = 'bottom';

    text = 'Basestation Switch';
    context.textAlign = 'center';
    context.fillText(text, centerW, 30);
    context.beginPath();
    context.arc(centerW, 50, 20, 0, 2 * Math.PI);
    context.fillStyle =
      this.props.devices.BasestationSwitch.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.BasestationSwitch.ping, min, cutoff, max, greenHue, redHue);
    context.fill();
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 70);
    context.lineTo(centerW - 40, 110);
    context.moveTo(centerW, 70);
    context.lineTo(centerW + 40, 110);
    context.stroke();

    context.rect(centerW - 60, 110, 40, 40);
    context.stroke();
    context.fillStyle =
      this.props.devices.Basestation5GHzRocket.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.Basestation5GHzRocket.ping, min, cutoff, max, greenHue, redHue);
    context.fill();
    text = 'BaseStation 5GHz Rocket';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 65, 130);

    context.rect(centerW + 20, 110, 40, 40);
    context.stroke();
    context.fillStyle =
      this.props.devices.Basestation900MHzRocket.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.Basestation900MHzRocket.ping, min, cutoff, max, greenHue, redHue);
    context.fill();
    text = 'BaseStation 900MHz Rocket';
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW + 65, 130);

    context.beginPath();
    context.moveTo(centerW - 40, 150);
    for (let i = 0; i < 150; i++) {
      const x = 20 - 20 * Math.sin(i * 2 * Math.PI * (2 / 150));
      context.lineTo(x + centerW - 60, i + 150);
    }
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 40, 150);
    for (let i = 0; i < 150; i++) {
      const x = 20 - 20 * Math.sin(i * 2 * Math.PI * (1 / 150));
      context.lineTo(x + centerW + 20, i + 150);
    }
    context.stroke();

    context.beginPath();
    context.arc(centerW - 40, 320, 20, 0, 2 * Math.PI);
    context.fillStyle =
      this.props.devices.Rover5GHzRocket.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.Rover5GHzRocket.ping, min, cutoff, max, greenHue, redHue);
    context.fill();
    context.stroke();
    text = 'Rover 5GHz Rocket';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 65, 320);

    context.beginPath();
    context.arc(centerW + 40, 320, 20, 0, 2 * Math.PI);
    context.fillStyle =
      this.props.devices.Rover900MHzRocket.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.Rover900MHzRocket.ping, min, cutoff, max, greenHue, redHue);
    context.fill();
    context.stroke();
    text = 'Rover 900MHz Rocket';
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW + 65, 320);

    context.beginPath();
    context.moveTo(centerW - 40, 340);
    context.lineTo(centerW, 380);
    context.moveTo(centerW + 40, 340);
    context.lineTo(centerW, 380);
    context.stroke();

    const boards = Object.keys(RovecommManifest);
    const locations = [
      { x: centerW - 20, y: 540 },
      { x: centerW + 20, y: 540 },
      { x: centerW - 60, y: 530 },
      { x: centerW + 60, y: 530 },
      { x: centerW - 95, y: 515 },
      { x: centerW + 95, y: 515 },
      { x: centerW - 130, y: 495 },
      { x: centerW + 130, y: 495 },
      { x: centerW - 155, y: 460 },
      { x: centerW + 155, y: 460 },
      { x: centerW - 170, y: 420 },
      { x: centerW + 170, y: 420 },
      { x: centerW - 150, y: 380 },
      { x: centerW + 150, y: 380 },
      { x: centerW, y: 590 },
    ];
    for (let i = 0; i < locations.length && i < boards.length; i++) {
      const coords = locations[i];
      const board = boards[i];
      context.fillStyle =
        this.props.devices[board].ping === -1
          ? 'white'
          : ColorConverter(this.props.devices[board].ping, min, cutoff, max, greenHue, redHue);
      context.beginPath();
      context.moveTo(centerW, 400);
      context.lineTo(coords.x, coords.y);
      context.stroke();
      context.moveTo(20 * Math.cos((1 / 6) * Math.PI) + coords.x, 20 * Math.sin((1 / 6) * Math.PI) + coords.y);
      context.lineTo(20 * Math.cos((5 / 6) * Math.PI) + coords.x, 20 * Math.sin((5 / 6) * Math.PI) + coords.y);
      context.lineTo(20 * Math.cos((9 / 6) * Math.PI) + coords.x, 20 * Math.sin((9 / 6) * Math.PI) + coords.y);
      context.lineTo(20 * Math.cos((1 / 6) * Math.PI) + coords.x, 20 * Math.sin((1 / 6) * Math.PI) + coords.y);
      context.stroke();
      context.fill();
      context.fillStyle = 'black';
      context.textAlign = 'center';
      context.fillText(board, coords.x, coords.y + 17);
    }
    context.beginPath();
    context.fillStyle =
      this.props.devices.GrandStream.ping === -1
        ? 'white'
        : ColorConverter(this.props.devices.GrandStream.ping, min, cutoff, max, greenHue, redHue);
    context.rect(centerW - 20, 380, 40, 40);
    context.stroke();
    context.fill();
    text = 'Router';
    context.textBaseline = 'middle';
    context.fillStyle = 'black';
    context.textAlign = 'center';
    context.fillText(text, centerW, 400);
  }

  render(): JSX.Element {
    return (
      <div style={{ ...this.props.style }}>
        <div style={label}>Ping Map</div>
        <div style={container}>
          <canvas ref={this.canvasRef} height="640" width={window.document.documentElement.clientWidth / 2 - 20} />
        </div>
      </div>
    );
  }
}

export default PingMap;
