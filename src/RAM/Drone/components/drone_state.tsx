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
  width: `40%`,
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
  Navigating: 1,
  Cruising: 2,
  Searching: 3,
  Gate: 4,
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

  // componentDidMount() {
  //   this.updateDiagram([-1]);
  // }

  updateDiagram(mode: number): void {
    let text;

    const canvas = this.canvasRef.current;
    const context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);

    const centerW = context.canvas.width / 2;

    context.font = '12px Arial';

    text = 'Idle';
    context.rect(centerW - 64, 55, 160, 50);
    context.fillStyle = mode === stateEnum.Idle ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW, 80);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 105);
    context.lineTo(centerW, 125);
    context.stroke();

    text = 'Cruising';
    context.rect(centerW - 64, 125, 128, 50);
    context.fillStyle = mode === stateEnum.Cruising ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW, 150);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 175);
    context.lineTo(centerW, 195);
    context.stroke();

    text = 'Searching';
    context.rect(centerW - 9, 195, 128, 50);
    context.fillStyle = mode === stateEnum.Searching ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW, 220);
    context.stroke();

    text = 'Gate Flying';
    context.rect(centerW - 49, 245, 128, 50);
    context.fillStyle = mode === stateEnum.Gate ? 'green' : 'white';
    context.fill();
    context.textBaseline = 'middle';
    context.textAlign = 'left';
    context.fillStyle = 'black';
    context.fillText(text, centerW, 270);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 295);
    context.lineTo(centerW, 335);
    context.moveTo(centerW, 335);
    context.lineTo(centerW + 145, 335);
    context.moveTo(centerW + 145, 335);
    context.lineTo(centerW + 145, 135);
    context.moveTo(centerW + 145, 135);
    context.lineTo(centerW + 83, 135);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 64, 270);
    context.lineTo(centerW + 106, 270);
    context.moveTo(centerW + 106, 270);
    context.lineTo(centerW + 106, 127);
    context.moveTo(centerW + 106, 127);
    context.lineTo(centerW + 64, 127);
    context.stroke();
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drone State Diagram</div>
        <div style={container}>
          <canvas ref={this.canvasRef} width={window.document.documentElement.clientWidth / 4 + 15} height={400} />
        </div>
        <button type="button" onClick={() => this.updateDiagram(0)}>
          Force Update
        </button>
      </div>
    );
  }
}

export default DroneState;
