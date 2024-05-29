import React, { Component, createRef } from 'react';
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
  justifyContent: 'center',
  // width: `${window.document.documentElement.clientWidth / 4}`,
  // minWidth: `${window.document.documentElement.clientWidth / 4}`,
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

interface IState {
  svgContent: string;
}

const stateEnum: Record<string, number> = {
  Idle: 0,
  Navigating: 1,
  SearchPattern: 2,
  ApproachingMarker: 3,
  ApproachingObject: 4,
  VerifyingGPS: 5,
  VerifyingMarker: 6,
  VerifyingObject: 7,
  Avoidance: 8,
  Reversing: 9,
  Stuck: 10,
};

class StateDiagram extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  private containerRef = createRef<HTMLDivElement>();

  canvasRef: any;

  constructor(props: IProps) {
    super(props);
    this.state = {
      svgContent: '',
    };

    // this.canvasRef = React.createRef();
    this.updateSvgFill = this.updateSvgFill.bind(this);
    this.getDiagramFromSVG = this.getDiagramFromSVG.bind(this);
    rovecomm.on('CurrentState', (data: any) => this.updateSvgFill(data));
  }

  componentDidMount() {
    // this.updateStateDiagram([-1]);
    this.getDiagramFromSVG();
    this.updateSvgFill([0]);
  }

  getDiagramFromSVG() {
    fetch('../assets/AutonomySimpleStateMachine.svg')
      .then((response) => response.text())
      .then((data) => {
        this.setState({ svgContent: data });
        return;
      })
      .catch((error) => console.error('Error fetching SVG:', error));
  }

  updateSvgFill(data: any): void {
    console.log('Starting SVG update');
    const svgContent = this.state.svgContent;

    // Parse the SVG content
    const parser = new DOMParser();
    const svgDoc = parser.parseFromString(svgContent, 'image/svg+xml');

    // Update the fill attribute of <rect> elements
    const rects = svgDoc.querySelectorAll('rect');
    rects.forEach((rect) => {
      const parent = rect.parentElement;
      const sibling = parent?.nextElementSibling;

      let text = 'Idle';
      if (sibling) {
        const foreignObject = sibling.querySelector('foreignObject');
        if (foreignObject) {
          const outerDiv = foreignObject.querySelector('div');
          if (outerDiv) {
            const innerDiv = outerDiv.querySelector('div');
            if (innerDiv) {
              text = innerDiv?.textContent?.trim().replace(/\s+/g, '') ?? 'Idle';
            }
          }
        }
      }
      rect.setAttribute('fill', data[0] === stateEnum[text as keyof typeof stateEnum] ? '#00FF00' : 'white');
      console.log(
        text,
        ':',
        data[0] === stateEnum[text as keyof typeof stateEnum],
        ':',
        stateEnum[text as keyof typeof stateEnum],
        data[0]
      );
    });
    const serializer = new XMLSerializer();
    const updatedSvg = serializer.serializeToString(svgDoc);

    this.setState({ svgContent: updatedSvg });
  }

  // DEPRECIATED
  updateStateDiagram(data: any): void {
    let text;

    const canvas = this.canvasRef.current;
    const context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);

    const centerW = context.canvas.width / 2;

    context.font = '12px Arial';

    //State: Idle
    text = 'Idle';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 25, 50);
    context.beginPath();
    context.rect(centerW - 20, 30, 40, 40);
    context.fillStyle = data[0] === stateEnum[text] ? 'green' : 'white';
    context.fill();
    context.stroke();

    context.beginPath();
    context.moveTo(centerW, 70);
    context.lineTo(centerW, 110);
    context.stroke();

    // State: Navigating
    text = 'Navigating';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 25, 130);
    context.rect(centerW - 20, 110, 40, 40);
    context.stroke();
    context.fillStyle = data[0] === stateEnum[text] ? 'green' : 'white';
    context.fill();

    context.beginPath();
    context.moveTo(centerW + 10, 150);
    context.lineTo(centerW + 10, 190);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW - 10, 151);
    context.lineTo(centerW - 10, 169);
    context.lineTo(centerW - 44, 170);
    context.lineTo(centerW - 44, 189);
    context.lineTo(centerW - 40, 184);
    context.moveTo(centerW - 45, 189);
    context.lineTo(centerW - 50, 184);
    context.moveTo(centerW - 10, 151);
    context.lineTo(centerW - 5, 156);
    context.moveTo(centerW - 10, 151);
    context.lineTo(centerW - 15, 156);
    context.stroke();

    // State: Avoidance
    text = 'Avoidance';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 70, 210);
    context.rect(centerW - 65, 190, 40, 40);
    context.stroke();
    context.fillStyle = data[0] === stateEnum[text] ? 'green' : 'white';
    context.fill();

    context.beginPath();
    context.moveTo(centerW, 230);
    context.lineTo(centerW, 270);
    context.stroke();

    // State: Search Pattern
    text = 'SearchPattern';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW + 110, 210);
    context.rect(centerW - 10, 190, 40, 40);
    context.stroke();
    context.fillStyle = data[0] === stateEnum[text] ? 'green' : 'white';
    context.fill();

    context.beginPath();
    context.moveTo(centerW + 20, 230);
    context.lineTo(centerW + 20, 270);
    context.stroke();

    // State: Approaching Marker
    text = 'ApproachingMarker';
    context.textBaseline = 'middle';
    context.textAlign = 'right';
    context.fillStyle = 'black';
    context.fillText(text, centerW - 25, 290);
    context.rect(centerW - 20, 270, 40, 40);
    context.stroke();
    context.fillStyle = data[0] === stateEnum[text] ? 'green' : 'white';
    context.fill();

    context.beginPath();
    context.moveTo(centerW + 20, 290);
    context.lineTo(centerW + 50, 290);
    context.lineTo(centerW + 50, 280);
    context.lineTo(centerW + 30, 280);
    context.lineTo(centerW + 35, 285);
    context.moveTo(centerW + 30, 280);
    context.lineTo(centerW + 35, 275);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 20, 290);
    context.lineTo(centerW + 70, 290);
    context.lineTo(centerW + 70, 220);
    context.lineTo(centerW + 30, 220);
    context.lineTo(centerW + 35, 225);
    context.moveTo(centerW + 30, 220);
    context.lineTo(centerW + 35, 215);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 20, 290);
    context.lineTo(centerW + 115, 290);
    context.lineTo(centerW + 115, 60);
    context.lineTo(centerW + 20, 60);
    context.lineTo(centerW + 25, 65);
    context.moveTo(centerW + 20, 60);
    context.lineTo(centerW + 25, 55);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 30, 200);
    context.lineTo(centerW + 50, 200);
    context.lineTo(centerW + 50, 190);
    context.lineTo(centerW + 30, 190);
    context.lineTo(centerW + 35, 195);
    context.moveTo(centerW + 30, 190);
    context.lineTo(centerW + 35, 185);
    context.stroke();

    context.beginPath();
    context.moveTo(centerW + 20, 50);
    context.lineTo(centerW + 40, 50);
    context.lineTo(centerW + 40, 40);
    context.lineTo(centerW + 20, 40);
    context.lineTo(centerW + 25, 45);
    context.moveTo(centerW + 20, 40);
    context.lineTo(centerW + 25, 35);
    context.stroke();
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>State Diagram</div>
        <div style={container}>
          {/* <canvas ref={this.canvasRef} width={window.document.documentElement.clientWidth / 4 + 15} height={400} /> */}
          <div
            ref={this.containerRef}
            /*eslint-disable-next-line @typescript-eslint/naming-convention*/
            dangerouslySetInnerHTML={{ __html: this.state.svgContent }}
            // style={{ width: divWidth, height: divHeight, overflow: 'auto' }}
          ></div>
        </div>
      </div>
    );
  }
}

export default StateDiagram;
