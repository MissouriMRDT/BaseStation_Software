import React, { Component } from 'react';
import CSS from 'csstype';
import { XYPlot, VerticalGridLines, HorizontalGridLines, XAxis, YAxis, LineSeries, Crosshair } from 'react-vis';
// import html2canvas from 'html2canvas';
// import fs from 'fs';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
// import { windows } from '../../../Core/Window';

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
// const button: CSS.Properties = {
//   marginLeft: '15px',
//   width: '60px',
//   alignSelf: 'center',
// };

const overlay: CSS.Properties = {
  width: '200px',
  color: 'black',
};

// function downloadURL(imgData: string): void {
//   const filename = `./Screenshots/${new Date()
//     .toISOString()
//     // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
//     // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
//     .replaceAll(/[:\-TZ]/g, '.')}Raman.png`;

//   if (!fs.existsSync('./Screenshots')) {
//     fs.mkdirSync('./Screenshots');
//   }

//   const base64Image = imgData.replace('image/png', 'image/octet-stream').split(';base64,').pop();
//   if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
// }

// function saveImage(): void {
//   // Search through all the windows for Raman
//   let graph;
//   let thisWindow;
//   for (const win of Object.keys(windows)) {
//     if (windows[win].document.getElementById('Raman')) {
//       // When found, store the graph and the window it was in
//       thisWindow = windows[win];
//       graph = thisWindow.document.getElementById('Raman');
//       break;
//     }
//   }

//   // If the graph isn't found, throw an error
//   if (!graph) {
//     throw new Error("The element 'Raman' wasn't found");
//   }

//   // If the graph is found, convert its html into a canvas to be downloaded
//   html2canvas(graph, {
//     scrollX: 0,
//     scrollY: -thisWindow.scrollY - 38,
//   }) // We subtract 38 to make up for the 28 pixel top border and the -10 top margin
//     .then((canvas: any) => {
//       const imgData = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
//       downloadURL(imgData);
//       return null;
//     })
//     .catch((error: any) => {
//       console.error(error);
//     });
// }

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  /** array to store which packets have been recieved. There are 5 packets of ccd information since RoveComm has a limited packet size.
   *  We need to wait until we get all of them before we can do processing.
   */
  packetsRecieved: boolean[];

  data: number[];

  graphData: {
    x: number;
    y: number;
  }[];

  crosshairPos: number | null;

  enableLED: boolean;
}

// const LEDWavelength = 532; //nm

class Raman extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      packetsRecieved: [false, false, false, false, false],
      data: new Array(2048).fill(0).flat(),
      graphData: [{ x: 0, y: 0 }],

      crosshairPos: null,

      enableLED: false,
    };
    // this.exportData = this.exportData.bind(this);
    this.processReading = this.processReading.bind(this);
    this.onNearestX = this.onNearestX.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
    this.requestData = this.requestData.bind(this);

    rovecomm.on('CCDReading_Part1', (data: number[]) => this.processReading(1, 0, 500, data));
    rovecomm.on('CCDReading_Part2', (data: number[]) => this.processReading(2, 501, 1000, data));
    rovecomm.on('CCDReading_Part3', (data: number[]) => this.processReading(3, 1001, 1500, data));
    rovecomm.on('CCDReading_Part4', (data: number[]) => this.processReading(4, 1501, 2000, data));
    rovecomm.on('CCDReading_Part5', (data: number[]) => this.processReading(5, 2001, 2048, data));
  }

  processReading(packetID: number, startIndex: number, endIndex: number, data: number[]) {
    console.log(`processing Raman data of packet ID: ${packetID}`);
    this.setState(
      (prevState) => {
        const updatedPacketsRecieved = prevState.packetsRecieved;
        updatedPacketsRecieved[packetID - 1] = true; //the -1 term is because packetID index starts at 1
        const updatedData = prevState.data;
        for (let i = 0; i < endIndex - startIndex; i++) {
          updatedData[startIndex + i] = data[i];
        }

        return { packetsRecieved: updatedPacketsRecieved, data: updatedData };
      },
      () => {
        //if all the packets have been recieved, update graph
        let a = true;
        this.state.packetsRecieved.forEach((packetRecieved) => {
          a &&= packetRecieved;
        });

        if (a) this.updateGraphValues(); //TODO there's gotta be a better way, this is cursed
      }
    );
  }

  static sendLEDCommand(enable: boolean): number {
    let bitmask = '';
    bitmask += enable ? '1' : '0';
    bitmask += '0'; //for deprecated red LED
    console.log(bitmask);
    const num = parseInt(bitmask, 2);
    console.log(num);
    return num;
  }

  setLED(enable: boolean) {
    this.setState({ enableLED: enable }, () => {
      rovecomm.sendCommand('EnableLEDs', 'RamanSpectrometer', Raman.sendLEDCommand(enable));
    });
  }

  onMouseLeave(): void {
    this.setState({ crosshairPos: null });
  }

  onNearestX(index: number): void {
    this.setState({ crosshairPos: index });
  }

  updateGraphValues(): void {
    const data = this.state.data;

    //TODO do the map from ccd pixel to wavelength here, as well as normalizing intensity of wavelengths from ccd datasheet
    const dataToDisplay = data.map((value: number, index: number) => {
      return { x: 0 + index * 1, y: value };
    });

    this.setState({
      graphData: dataToDisplay,
    });
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
    this.setState(() => {
      return {
        //reset stored data to prepare for new aquisitions
        packetsRecieved: new Array(5).fill(false),
      };
    });

    rovecomm.sendCommand('RequestReading', 'RamanSpectrometer', 1);
    console.log(`Requesting Raman Reading`);
  }

  render(): JSX.Element {
    return (
      <div id="Raman" style={this.props.style}>
        <div style={label}>Raman</div>
        <div style={container}>
          <div style={componentBox}>
            <XYPlot
              id="plot"
              margin={{ top: 10, bottom: 50 }}
              width={window.document.documentElement.clientWidth - 50}
              height={300}
              yDomain={[0, 1023]} //TODO determine these values
              xDomain={[0, 2047]}
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
            <div style={row}>
              <div style={column}>
                <button onClick={() => this.setLED(!this.state.enableLED)}>
                  LED: {this.state.enableLED ? 'on' : 'off'}
                </button>
              </div>
              <div style={{ ...row, justifyContent: 'end' }}>
                <button
                  onClick={() => {
                    this.requestData(); //TODO make a counter variable or a text box to tweak the number of aquisitions
                  }}
                >
                  Request Reading
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Raman;
