import React, { Component } from 'react';
import CSS from 'csstype';
import { XYPlot, VerticalGridLines, HorizontalGridLines, XAxis, YAxis, LineSeries, Crosshair } from 'react-vis';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

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
const button: CSS.Properties = {
  marginLeft: '15px',
  width: '60px',
  alignSelf: 'center',
};

const overlay: CSS.Properties = {
  width: '200px',
  color: 'black',
};

/** Will be merged with the row css if the Laser is off */
const offIndicator: CSS.Properties = {
  backgroundColor: '#FF0000',
  lineHeight: '27px',
};

/** Will be merged with the row css if the Laser is off */
const onIndicator: CSS.Properties = {
  backgroundColor: '#00FF00',
  lineHeight: '27px',
};

interface IProps {
  style?: CSS.Properties;
}

const LEDNames = ['265nm', '275nm', '280nm', '310nm', '365nm'];

interface IState {
  /** Holds which lasers are enabled */
  LedStatus: boolean[];

  intensities: number[];

  maxIntensity: number;

  graphData: {
    x: number;
    y: number;
  }[];

  relExtrema: {
    mins: { x: number; intensity: number }[];
    maxs: { x: number; intensity: number }[];
  };

  crosshairPos: number | null;

  SHPeriod: number;
}

class Fluorometer extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  static buildLedCommand(LED: boolean[]): number {
    let bitmask = '';
    bitmask += LED[4] ? '1' : '0';
    bitmask += LED[3] ? '1' : '0';
    bitmask += LED[2] ? '1' : '0';
    bitmask += LED[1] ? '1' : '0';
    bitmask += LED[0] ? '1' : '0';
    console.log(bitmask);
    const num = parseInt(bitmask, 2);
    console.log(num);
    return num;
  }

  static rollingAverage(arr: number[], N: number): number[] {
    const result: number[] = [];

    for (let i = 0; i < arr.length; i++) {
      let sum = 0;
      let count = 0;

      for (let j = i; j > Math.max(i - N, -1); j--) {
        sum += arr[j];
        count += 1;
      }

      result.push(sum / count);
    }

    return result;
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      LedStatus: [false, false, false, false, false],
      intensities: new Array(3648).fill(0).flat(),
      graphData: [{ x: 0, y: 0 }],
      maxIntensity: 0,

      relExtrema: {
        mins: [],
        maxs: [],
      },

      crosshairPos: null,

      SHPeriod: 200,
    };
    this.updateDiodeVals = this.updateDiodeVals.bind(this);
    // this.exportData = this.exportData.bind(this);
    this.onNearestX = this.onNearestX.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
    this.calculateRelExtrema = this.calculateRelExtrema.bind(this);
    this.calcRelMins = this.calcRelMins.bind(this);
    this.calcRelMaxs = this.calcRelMaxs.bind(this);
    this.changeSHPeriod = this.changeSHPeriod.bind(this);
    this.requestData = this.requestData.bind(this);

    // Call updateDiodeValues with the new data and the index of that data
    rovecomm.on('FluorometerData1', (data: number[]) => this.updateDiodeVals(0, data));
    rovecomm.on('FluorometerData2', (data: number[]) => this.updateDiodeVals(500, data));
    rovecomm.on('FluorometerData3', (data: number[]) => this.updateDiodeVals(1000, data));
    rovecomm.on('FluorometerData4', (data: number[]) => this.updateDiodeVals(1500, data));
    rovecomm.on('FluorometerData5', (data: number[]) => this.updateDiodeVals(2000, data));
    rovecomm.on('FluorometerData6', (data: number[]) => this.updateDiodeVals(2500, data));
    rovecomm.on('FluorometerData7', (data: number[]) => this.updateDiodeVals(3000, data));
    rovecomm.on('FluorometerData8', (data: number[]) => this.updateDiodeVals(3500, data));
  }

  onMouseLeave(): void {
    this.setState({ crosshairPos: null });
  }

  onNearestX(index: number): void {
    this.setState({ crosshairPos: index });
  }

  calculateRelExtrema(arr: number[]): {
    mins: { x: number; intensity: number }[];
    maxs: { x: number; intensity: number }[];
  } {
    return {
      mins: this.calcRelMins(arr),
      maxs: this.calcRelMaxs(arr),
    };
  }

  // eslint-disable-next-line class-methods-use-this
  calcRelMins(arr: number[]): { x: number; intensity: number }[] {
    let peakI: number | undefined;
    const peaksI: number[] = arr.reduce((peaks: number[], _val, i) => {
      if (arr[i + 1] < arr[i]) {
        peakI = i + 1;
      } else if (arr[i + 1] > arr[i]) {
        if (peakI) {
          peaks.push(peakI);
          peakI = undefined;
        }
      }
      return peaks;
    }, []);
    return peaksI.map((val) => {
      return { x: 350 + val * 0.08121278, intensity: arr[val] };
    });
  }

  // eslint-disable-next-line class-methods-use-this
  calcRelMaxs(arr: number[]): { x: number; intensity: number }[] {
    let peakI: number;
    const peaksI: number[] = arr.reduce((peaks: number[], _val, i) => {
      if (arr[i + 1] > arr[i]) {
        peakI = i + 1;
      } else if (arr[i + 1] < arr[i]) {
        if (!Number.isNaN(peakI)) {
          peaks.push(peakI);
          peakI = NaN;
        }
      }
      return peaks;
    }, []);
    return peaksI.map((val) => {
      return { x: 350 + val * 0.08121278, intensity: arr[val] };
    });
  }

  /**
   * Updates the wavelengths received from the Rover.
   * @param index integer value that tells where this segment goes in data
   * @param dataInput uint16 array with the new data
   */
  updateDiodeVals(index: number, dataInput: number[]): void {
    this.setState(
      (prevState) => {
        const updatedInts = [
          ...prevState.intensities.slice(0, index),
          ...dataInput,
          ...prevState.intensities.slice(index + 500),
        ];
        // prevState.intensities.splice(index, index === 3500 ? 148 : 500, ...dataInput);
        return { intensities: updatedInts };
      },
      () => {
        this.updateGraphValues();
      }
    );
  }

  updateGraphValues(): void {
    const { intensities } = this.state;
    const avgd = Fluorometer.rollingAverage(intensities, 75);

    const maxIntensity = Math.max(...avgd);

    const normalizedData = avgd.map((value: number, ndx: number) => {
      return { x: 350 + ndx * 0.08121278, y: value / maxIntensity };
    });

    this.setState({
      graphData: normalizedData,
      maxIntensity,
      // relExtrema: this.calculateRelExtrema(avgd),
    });
  }

  toggleLed(index: number): void {
    const { LedStatus } = this.state;
    LedStatus[index] = !LedStatus[index];
    this.setState({
      LedStatus,
    });
    rovecomm.sendCommand('FluorometerLEDs', Fluorometer.buildLedCommand(LedStatus));
  }

  /*
  exportData(): void {
    // ISO string will be fromatted YYYY-MM-DDTHH:MM:SS:sssZ
    // this regex will convert all -,T,:,Z to . (which covers to . for .csv)
    // Date format is consistent with the SensorData csv
    const timestamp = new Date().toISOString().replaceAll(/[:\-TZ]/g, '.');
    const EXPORT_FILE = `./ScienceSaveFiles/Fluormeter-${timestamp}.csv`;

    const { DiodeValues, LedStatus } = this.state;

    if (!fs.existsSync('./ScienceSaveFiles')) {
      fs.mkdirSync('./ScienceSaveFiles');
    }
    let csvText = 'Laser1,Laser2,Laser3,Diode1 (nm),Diode2 (nm),Diode3 (nm)\n';
    csvText += `${LedStatus[0] ? 'On' : 'Off'},${LedStatus[1] ? 'On' : 'Off'},${LedStatus[2] ? 'On' : 'Off'},${
      LedStatus[3] ? 'On' : 'Off'
    },`;
    csvText += `${DiodeValues[0]},${DiodeValues[1]},${DiodeValues[2]},\n`;

    fs.writeFile(EXPORT_FILE, csvText, (err) => {
      if (err) throw err;
    });
  } */

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

  changeSHPeriod(event: { target: { value: string } }): void {
    const SHPeriod = parseInt(event.target.value, 10);
    this.setState({ SHPeriod });
  }

  requestData(): void {
    if (this.state.SHPeriod > 10000) {
      rovecomm.sendCommand('ReqFluorometer', 10000);
    } else if (this.state.SHPeriod < 20) {
      rovecomm.sendCommand('ReqFluorometer', 20);
    } else {
      rovecomm.sendCommand('ReqFluorometer', this.state.SHPeriod);
    }
  }

  render(): JSX.Element {
    return (
      <div id="Flurometer" style={this.props.style}>
        <div style={label}>Fluorometer</div>
        <div style={container}>
          <div style={componentBox}>
            <XYPlot
              margin={{ top: 10, bottom: 50 }}
              width={window.document.documentElement.clientWidth - 50}
              height={300}
              yDomain={[0, 1]}
            >
              <VerticalGridLines style={{ fill: 'none' }} />
              <HorizontalGridLines style={{ fill: 'none' }} />
              <LineSeries
                data={this.state.graphData}
                style={{ fill: 'none' }}
                strokeWidth="6"
                color="blue"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index)}
              />
              <XAxis />
              <YAxis />
              {this.crosshair()}
            </XYPlot>
            <div style={row}>
              <div style={column}>
                {this.state.LedStatus.map((value, index) => {
                  return (
                    <label key={index} htmlFor="LedToggle">
                      <input
                        type="checkbox"
                        id="LedToggle"
                        name="LedToggle"
                        checked={value}
                        onChange={() => this.toggleLed(index)}
                      />
                      {LEDNames[index]}
                    </label>
                  );
                })}
              </div>
              <input
                type="text"
                value={this.state.SHPeriod || ''}
                style={{ textAlign: 'center' }}
                onChange={this.changeSHPeriod}
              />
              <button
                onClick={() => {
                  this.requestData();
                }}
              >
                Request Reading
              </button>
              {/* <div style={column}>
                <p>Relative Mins</p>
                {this.state.relExtrema.mins.map((val, ndx) => {
                  return (
                    <p key={ndx} style={{ margin: '1px' }}>
                      x={val.x.toFixed(3)}, intensity={val.intensity.toFixed(3)}
                    </p>
                  );
                })}
              </div>
              <div style={column}>
                <p>Relative Maxs</p>
                {this.state.relExtrema.maxs.map((val, ndx) => {
                  return (
                    <p key={ndx} style={{ margin: '1px' }}>
                      x={val.x.toFixed(3)}, intensity={val.intensity.toFixed(3)}
                    </p>
                  );
                })}
              </div> */}
              <p>Max Intensity: {this.state.maxIntensity}</p>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Fluorometer;
