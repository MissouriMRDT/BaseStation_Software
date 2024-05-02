import React, { Component } from 'react';
import CSS from 'csstype';
import { XYPlot, VerticalGridLines, HorizontalGridLines, XAxis, YAxis, LineSeries, Crosshair } from 'react-vis';
import html2canvas from 'html2canvas';
import fs from 'fs';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { windows } from '../../../Core/Window';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  height: 'calc(100% - 40px)',
  padding: '5px',
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
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-around',
  alignContent: 'center',
  marginTop: '5px',
  width: '100%',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  justifyContent: 'space-around',
};
const componentBox: CSS.Properties = {
  margin: '3px 0 3px 0',
};

const overlay: CSS.Properties = {
  width: '200px',
  color: 'black',
};

// /** Will be merged with the row css if the Laser is off */
// const offIndicator: CSS.Properties = {
//   backgroundColor: '#FF0000',
//   lineHeight: '27px',
// };

// /** Will be merged with the row css if the Laser is off */
// const onIndicator: CSS.Properties = {
//   backgroundColor: '#00FF00',
//   lineHeight: '27px',
// };

function downloadURL(imgData: string): void {
  const filename = `./Screenshots/${new Date()
    .toISOString()
    // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
    .replaceAll(/[:\-TZ]/g, '.')}Reflectance.png`;

  if (!fs.existsSync('./Screenshots')) {
    fs.mkdirSync('./Screenshots');
  }

  const base64Image = imgData.replace('image/png', 'image/octet-stream').split(';base64,').pop();
  if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
}

function saveImage(): void {
  // Search through all the windows for Reflectance
  let graph;
  let thisWindow;
  for (const win of Object.keys(windows)) {
    if (windows[win].document.getElementById('Reflectance')) {
      // When found, store the graph and the window it was in
      thisWindow = windows[win];
      graph = thisWindow.document.getElementById('Reflectance');
      break;
    }
  }

  // If the graph isn't found, throw an error
  if (!graph) {
    throw new Error("The element 'Reflectance' wasn't found");
  }

  // If the graph is found, convert its html into a canvas to be downloaded
  html2canvas(graph, {
    scrollX: 0,
    scrollY: -thisWindow.scrollY - 38,
  }) // We subtract 38 to make up for the 28 pixel top border and the -10 top margin
    .then((canvas: any) => {
      const imgData = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
      downloadURL(imgData);
      return null;
    })
    .catch((error: any) => {
      console.error(error);
    });
}

interface IProps {
  style?: CSS.Properties;
}

const minWavelength = 340;
const maxWavelength = 850;
const maxintegrationTime = 60000;

interface IState {
  /** Holds which lasers are enabled */

  graphData: {
    x: number;
    y: number;
  }[];

  crosshairPos: number | null;
  integrationTime: number;
}

class Reflectance extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      graphData: [{ x: 0, y: 0 }],

      crosshairPos: null,
      integrationTime: 0,
    };
    this.onNearestX = this.onNearestX.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
    this.requestData = this.requestData.bind(this);
    this.integrationTimeChange = this.integrationTimeChange.bind(this);

    rovecomm.on('ReflectanceReading', (data: number[]) => this.updateGraphValues(data));
  }

  onMouseLeave(): void {
    this.setState({ crosshairPos: null });
  }

  onNearestX(index: number): void {
    this.setState({ crosshairPos: index });
  }

  updateGraphValues(data: number[]): void {
    const xScale = (maxWavelength - minWavelength) / 288;
    const tempGraph = [];
    for (let i = 1; i < data.length + 1; i++) {
      tempGraph.push({ x: i * xScale + minWavelength, y: data[i] });
    }
    this.setState({ graphData: tempGraph });
  }

  crosshair(): JSX.Element | null {
    const { crosshairPos } = this.state;

    // If we were able to find a reading at a time, then go ahead and display the crosshair
    // The heading will be that time as a string, and then if the key exists in crosshairValues
    // then we want to display its y value
    if (crosshairPos) {
      return (
        <Crosshair values={[this.state.graphData[crosshairPos]]}>
          <div style={overlay}>
            <h3 style={{ borderStyle: 'solid', width: '30%', textAlign: 'center', backgroundColor: 'white' }}>
              {this.state.graphData[crosshairPos].y}
            </h3>
          </div>
        </Crosshair>
      );
    }
    return null;
  }

  requestData(): void {
    rovecomm.sendCommand('RequestReflectanceReading', 'Instruments', this.state.integrationTime);
    console.log('requesting Reflectance');
  }

  integrationTimeChange(event: { target: { value: string } }): void {
    let integrationTime = parseInt(event.target.value);
    if (integrationTime < 0) {
      integrationTime = 0;
    } else if (integrationTime > maxintegrationTime) {
      integrationTime = maxintegrationTime;
    }
    this.setState({ integrationTime });
  }

  render(): JSX.Element {
    return (
      <div id="Reflectance" style={this.props.style}>
        <div style={label}>Reflectance</div>
        <div style={container}>
          <div style={componentBox}>
            <XYPlot
              id="plot"
              margin={{ top: 10, bottom: 50 }}
              width={window.document.documentElement.clientWidth - 50}
              height={300}
              yDomain={[0, 255]}
              xDomain={[minWavelength, maxWavelength]}
            >
              <VerticalGridLines style={{ fill: 'none' }} />
              <HorizontalGridLines style={{ fill: 'none' }} />
              <LineSeries
                data={this.state.graphData}
                style={{ fill: 'none' }}
                strokeWidth="2"
                color="blue"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index)}
              />
              <XAxis />
              <YAxis />
              {this.crosshair()}
            </XYPlot>
            <div style={{ ...row }}>
              <div style={{ ...row, justifyContent: 'center' }}>
                <div style={{ ...column, margin: '0 100px 0 100px' }}>
                  <button
                    onClick={() => {
                      this.requestData();
                    }}
                  >
                    Request Reading
                  </button>
                </div>
                <div>
                  Integration Time (ms):
                  <input
                    type="text"
                    style={{ marginLeft: '5px' }}
                    value={this.state.integrationTime || ''}
                    onChange={this.integrationTimeChange}
                  />
                </div>
                <div style={{ ...column, margin: '0 100px 0 100px' }}>
                  <button onClick={saveImage}>Export Graph</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Reflectance;
