import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  alignItems: 'flex-start',
  height: '83.5%',
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
  style?: CSS.Properties;
}

const stateEnum = {
  Idle: 0,
  Cruising: 1,
  Searching: 2,
  Gate: 3,
};

interface IState {}

class DroneState extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  canvasRef: any;

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.canvasRef = React.createRef();
    rovecomm.on('droneStatus', (mode: number) => {
      this.updateDiagram(mode);
    });
  }

  componentDidMount() {
    this.updateDiagram(0);
  }

  updateDiagram(mode: number): void {
    let text;

    const canvas = this.canvasRef.current;
    const context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);

    const centerW = context.canvas.width / 2;

    context.font = '12px Arial';

    text = 'Idle';
    context.rect(centerW - 80, 25, 160, 50);
    context.fillStyle = mode === stateEnum.Idle ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 10, 50);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 75);
    context.lineTo(centerW, 95);
    context.stroke();

    text = 'Cruising';
    context.rect(centerW - 64, 95, 128, 50);
    context.fillStyle = mode === stateEnum.Cruising ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 23, 120);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 145);
    context.lineTo(centerW, 165);
    context.stroke();

    text = 'Searching';
    context.rect(centerW - 64, 165, 128, 50);
    context.fillStyle = mode === stateEnum.Searching ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 25, 190);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 215);
    context.lineTo(centerW, 235);
    context.stroke();

    text = 'Gate Flying';
    context.rect(centerW - 64, 235, 128, 50);
    context.fillStyle = mode === stateEnum.Gate ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 30, 260);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 285);
    context.lineTo(centerW, 315);
    context.moveTo(centerW, 315);
    context.lineTo(centerW + 140, 315);
    context.moveTo(centerW + 140, 315);
    context.lineTo(centerW + 140, 50);
    context.moveTo(centerW + 140, 50);
    context.lineTo(centerW + 82, 50);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 64, 270);
    context.lineTo(centerW + 106, 270);
    context.moveTo(centerW + 106, 270);
    context.lineTo(centerW + 106, 120);
    context.moveTo(centerW + 106, 120);
    context.lineTo(centerW + 64, 120);
    context.stroke();
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drone State Diagram</div>
        <div style={container}>
          <canvas ref={this.canvasRef} width={window.document.documentElement.clientWidth / 4 + 15} height={400} />
        </div>
      </div>
    );
  }
}

export default DroneState;
