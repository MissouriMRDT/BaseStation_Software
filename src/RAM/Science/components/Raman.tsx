import React, { Component } from 'react';
import CSS from 'csstype';
import { XYPlot, VerticalGridLines, HorizontalGridLines, XAxis, YAxis, LineSeries, Crosshair } from 'react-vis';
import html2canvas from 'html2canvas';
import fs from 'fs';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { windows } from '../../../Core/Window';

const minWavelength = 532;
const maxWavelength = 650;
const maxintegrationTime = 60000;

const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};

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

const componentBox: CSS.Properties = {
  margin: '3px 0 3px 0',
};

const overlay: CSS.Properties = {
  width: '200px',
  color: 'black',
};

const buttonRow: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-evenly',
  alignItems: 'center',
};

const inputField: CSS.Properties = {
  marginLeft: '5px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  packetsRecieved: boolean[];
  data: number[];
  graphData: {
    x: number;
    y: number;
  }[];
  crosshairPos: number | null;
  integrationTime: number;
  minX: number;
  maxX: number;
  // baseline: boolean;
  // baselineData: number[];
  // deviationConstant: number;
  // filterType: number;
}

function downloadURL(imgData: string): void {
  const filename = `./Screenshots/${new Date().toISOString().replaceAll(/[:\-TZ]/g, '.')}Raman.png`;

  if (!fs.existsSync('./Screenshots')) {
    fs.mkdirSync('./Screenshots');
  }

  const base64Image = imgData.replace('image/png', 'image/octet-stream').split(';base64,').pop();
  if (base64Image) fs.writeFileSync(filename, base64Image, { encoding: 'base64' });
}

function saveImage(): void {
  let graph;
  let thisWindow;
  for (const win of Object.keys(windows)) {
    if (windows[win].document.getElementById('Raman')) {
      thisWindow = windows[win];
      graph = thisWindow.document.getElementById('Raman');
      break;
    }
  }

  if (!graph) {
    throw new Error("The element 'Raman' wasn't found");
  }

  html2canvas(graph, {
    scrollX: 0,
    scrollY: -thisWindow.scrollY - 38,
  })
    .then((canvas: any) => {
      const imgData = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');
      downloadURL(imgData);
      return null;
    })
    .catch((error: any) => {
      console.error(error);
    });
}

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
      integrationTime: 0,
      minX: this.wavelengthToWavenumber(minWavelength),
      maxX: Math.round(this.wavelengthToWavenumber(maxWavelength)),
      // baseline: false,
      // baselineData: new Array(2048).fill(1023).flat(),
      // deviationConstant: 0,
      // filterType: 0,
    };

    rovecomm.on('RamanReading_Part1', (data: number[]) => this.processReading(1, 0, 500, data));
    rovecomm.on('RamanReading_Part2', (data: number[]) => this.processReading(2, 500, 1000, data));
    rovecomm.on('RamanReading_Part3', (data: number[]) => this.processReading(3, 1000, 1500, data));
    rovecomm.on('RamanReading_Part4', (data: number[]) => this.processReading(4, 1500, 2000, data));
    rovecomm.on('RamanReading_Part5', (data: number[]) => this.processReading(5, 2000, 2048, data));
  }

  processReading(packetID: number, startIndex: number, endIndex: number, data: number[]) {
    this.setState(
      (prevState) => {
        const updatedPacketsRecieved = prevState.packetsRecieved;
        updatedPacketsRecieved[packetID - 1] = true;
        const updatedData = prevState.data;
        // const updatedData = this.state.baseline ? prevState.baselineData : prevState.data;
        for (let i = 0; i < endIndex - startIndex; i++) {
          updatedData[startIndex + i] = data[i];
        }
        // if (this.state.baseline) {
        //   console.log('Taking baseline');
        //   return {
        //     packetsRecieved: updatedPacketsRecieved,
        //     baselineData: updatedData,
        //     data: new Array(2048).fill(0).flat(),
        //   };
        // }
        console.log('Not taking baseline');
        // return { packetsRecieved: updatedPacketsRecieved, baselineData: this.state.baselineData, data: updatedData };
        return { packetsRecieved: updatedPacketsRecieved, data: updatedData };
      },
      () => {
        let a = true;
        this.state.packetsRecieved.forEach((packetRecieved) => {
          a &&= packetRecieved;
        });

        if (a) this.updateGraphValues();
      }
    );
  }

  exportDataCSV(): void {
    const csvData = this.state.graphData
      .map((data) => {
        return `${data.x},${data.y}`;
      })
      .join('\n');

    // ISO string will be formatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T:,Z to . (which covers to . for .csv)
    // Date format is consistent with the SensorData csv
    const timestamp = new Date().toISOString().replaceAll(/[:\-TZ]/g, '.');
    const EXPORT_FILE = `./RamanCSV/${timestamp}.csv`;

    if (!fs.existsSync('./RamanCSV')) {
      fs.mkdirSync('./RamanCSV');
    }

    // Write the CSV data to a file
    fs.writeFile(EXPORT_FILE, csvData, (err) => {
      if (err) throw err;
    });
  }

  onMouseLeave(): void {
    this.setState({ crosshairPos: null });
  }

  onNearestX(index: number): void {
    this.setState({ crosshairPos: index });
  }

  wavelengthToWavenumber(wavelength: number): number {
    return 10 ** 7 * (1 / minWavelength - 1 / wavelength);
  }

  takeBaseline(): void {
    this.setState(() => {
      return {
        packetsRecieved: new Array(5).fill(false),
        // baseline: true,
      };
    });

    rovecomm.sendCommand('RequestRamanReading', 'Instruments', this.state.integrationTime);
  }

  updateGraphValues(): void {
    // const filter = this.state.filterType;

    // if (filter === 0) {
    const data = this.state.data;
    const xScale = (maxWavelength - minWavelength) / 2048;
    const maxY = Math.max(...data);
    for (let i = 0; i < data.length; i++) {
      data[i] = data[i] - maxY;
    }
    const newMaxY = Math.max(...data);
    const newMinY = Math.min(...data);
    for (let i = 0; i < data.length; i++) {
      data[i] = (data[i] - newMinY) / (newMaxY - newMinY);
    }
    const dataToDisplay = data.map((value: number, index: number) => {
      return { x: this.wavelengthToWavenumber(index * xScale + minWavelength), y: value };
    });
    this.setState({
      graphData: dataToDisplay,
    });
    // }

    //   if (filter === 1) {
    //     const data = this.state.data;
    //     const baseline = this.state.baselineData;
    //     const deviationConstant = this.state.deviationConstant;
    //     const xScale = (maxWavelength - minWavelength) / 2048;

    //     // Inverse Data and Baseline
    //     const inverseData = data.map((value: number) => {
    //       return 1023 - value;
    //     });

    //     const inverseBaseline = baseline.map((value: number) => {
    //       return 1023 - value;
    //     });

    //     // Correct Data : (Data - Baseline)
    //     const correctedData = inverseData.map((value: number, index: number) => {
    //       return value - inverseBaseline[index];
    //     });

    //     // Find Corrected Data => Median
    //     const median = (() => {
    //       // Sort the array
    //       correctedData.sort((a, b) => a - b);

    //       // Calculate the median
    //       const middle = Math.floor(correctedData.length / 2);
    //       if (correctedData.length % 2 === 0) {
    //         // If the array length is even, return the average of the two middle numbers
    //         return (correctedData[middle - 1] + correctedData[middle]) / 2;
    //       }

    //       // If the array length is odd, return the middle number
    //       return correctedData[middle];
    //     })();

    //     // Find Corrected Data => Median Average Deviation (Median( |Data{} - Median| ))
    //     const medianAverageDeviation = (() => {
    //       // Calculate the corrected data minus the median
    //       const correctedDataMinusMedian = correctedData.map((value: number) => {
    //         return value - median;
    //       });

    //       // Calculate the absolute value of the corrected data minus the median
    //       const absoluteCorrectedDataMinusMedian = correctedDataMinusMedian.map((value: number) => {
    //         return Math.abs(value);
    //       });

    //       // Sort the array
    //       absoluteCorrectedDataMinusMedian.sort((a, b) => a - b);

    //       // Calculate the median
    //       const middle = Math.floor(absoluteCorrectedDataMinusMedian.length / 2);
    //       if (absoluteCorrectedDataMinusMedian.length % 2 === 0) {
    //         // If the array length is even, return the average of the two middle numbers
    //         return (absoluteCorrectedDataMinusMedian[middle - 1] + absoluteCorrectedDataMinusMedian[middle]) / 2;
    //       }

    //       // If the array length is odd, return the middle number
    //       return absoluteCorrectedDataMinusMedian[middle];
    //     })();

    //     // Find Corrected Data => Average
    //     const average = (() => {
    //       let sum = 0;
    //       correctedData.forEach((value: number) => {
    //         sum += value;
    //       });
    //       return sum / correctedData.length;
    //     })();

    //     // Find Corrected Data => Standard Deviation
    //     const standardDeviation = (() => {
    //       let sum = 0;
    //       correctedData.forEach((value: number) => {
    //         sum += Math.pow(value - average, 2);
    //       });
    //       return Math.sqrt(sum / correctedData.length);
    //     })();

    //     // IF: Corrected Data >= Average + Integer Multiple of Standard Deviation => Normalize and Plot
    //     // ELSE IF: Corrected Data >= Median + Integer Multiple of Median Average Deviation => Normalize and Plot
    //     // ELSE: Plot 0
    //     // Normalization: (x - min(x)) / (max(x) - min(x))
    //     const normalizedData = correctedData.map((value: number) => {
    //       if (
    //         value >= average + deviationConstant * standardDeviation ||
    //         value >= median + deviationConstant * medianAverageDeviation
    //       ) {
    //         return (value - Math.min(...correctedData)) / (Math.max(...correctedData) - Math.min(...correctedData));
    //       }

    //       return 0;
    //     });

    //     // Update the X Axis to Wave Number Scaling
    //     const dataToDisplay = normalizedData.map((value: number, index: number) => {
    //       return { x: index * xScale + minWavelength, y: value };
    //     });

    //     // Logging
    //     console.log('Data:', data);
    //     console.log('Baseline:', baseline);
    //     console.log('Deviation Constant:', deviationConstant);
    //     console.log('Inverse Data:', inverseData);
    //     console.log('Inverse Baseline:', inverseBaseline);
    //     console.log('Corrected Data:', correctedData);
    //     console.log('Median:', median);
    //     console.log('Median Average Deviation:', medianAverageDeviation);
    //     console.log('Average:', average);
    //     console.log('Standard Deviation:', standardDeviation);
    //     console.log('Normalized Data:', normalizedData);
    //     console.log('Data to Display:', dataToDisplay);

    //     // Update the graph data
    //     this.setState({
    //       graphData: dataToDisplay,
    //     });
  }

  crosshair(): JSX.Element | null {
    const { crosshairPos } = this.state;

    if (crosshairPos) {
      return (
        <Crosshair values={[this.state.graphData[crosshairPos]]}>
          <div style={overlay}>
            <h3>{this.state.graphData[crosshairPos].y}</h3>
          </div>
        </Crosshair>
      );
    }
    return null;
  }

  requestData(): void {
    this.setState(() => {
      return {
        packetsRecieved: new Array(5).fill(false),
        // baseline: false,
      };
    });

    rovecomm.sendCommand('RequestRamanReading', 'Instruments', this.state.integrationTime);
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
      <div id="Raman" style={this.props.style}>
        <div style={label}>Raman</div>
        <div style={container}>
          <div style={componentBox}>
            <XYPlot
              id="plot"
              margin={{ top: 10, bottom: 50 }}
              width={window.document.documentElement.clientWidth - 50}
              height={300}
              yDomain={[0, 1]}
              xDomain={[this.state.minX, this.state.maxX]}
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
            <div style={buttonRow}>
              <div>
                Integration Time (ms):
                <input
                  type="text"
                  style={inputField}
                  value={this.state.integrationTime || ''}
                  onChange={(e) => this.integrationTimeChange(e)}
                />
              </div>
              {/* <div>
                Deviation Constant
                <input
                  type="text"
                  style={inputField}
                  value={this.state.deviationConstant || ''}
                  onChange={(e) => this.setState({ deviationConstant: parseFloat(e.target.value) })}
                />
              </div> */}
              {/* <div>
                Filter Type
                <input
                  type="text"
                  style={inputField}
                  value={this.state.filterType || ''}
                  onChange={(e) => this.setState({ filterType: parseInt(e.target.value) })}
                />
              </div> */}
              <div>
                <button onClick={() => this.requestData()}>Request Reading</button>
              </div>
              {/* <div>
                <button onClick={() => this.takeBaseline()}>Request Baseline</button>
              </div> */}
            </div>
            <div style={buttonRow}>
              <div>
                Min X:
                <input
                  type="text"
                  style={inputField}
                  value={this.state.minX || ''}
                  onChange={(e) => this.setState({ minX: parseInt(e.target.value) })}
                />
              </div>
              <div>
                Max X:
                <input
                  type="text"
                  style={inputField}
                  value={this.state.maxX || ''}
                  onChange={(e) => this.setState({ maxX: parseInt(e.target.value) })}
                />
              </div>
              <div>
                <button
                  onClick={() =>
                    this.setState({
                      minX: this.wavelengthToWavenumber(minWavelength),
                      maxX: Math.round(this.wavelengthToWavenumber(maxWavelength)),
                    })
                  }
                >
                  Reset Graph
                </button>
              </div>
              <div>
                <button onClick={saveImage}>Export Graph to PNG</button>
                <button onClick={() => this.exportDataCSV()}>Export Data to CSV</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Raman;
