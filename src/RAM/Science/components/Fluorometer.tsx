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

const controlButton: CSS.Properties = {
  // margin: "5px",
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

interface IState {
  /** Holds which lasers are enabled */
  LedStatus: boolean[];

  data: {
    x: number;
    y: number;
  }[];

  crosshairPos: number | null;
}

class Fluorometer extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  static buildLedCommand(LED: boolean[]): number {
    let bitmask = '';
    bitmask += LED[0] ? '1' : '0';
    bitmask += LED[1] ? '1' : '0';
    bitmask += LED[2] ? '1' : '0';
    bitmask += LED[3] ? '1' : '0';
    return parseInt(bitmask, 2);
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      LedStatus: [false, false, false, false],
      data: [{ x: 0, y: 0 }],

      crosshairPos: null,
    };
    this.updateDiodeVals = this.updateDiodeVals.bind(this);
    // this.exportData = this.exportData.bind(this);
    this.onNearestX = this.onNearestX.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);

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

  /**
   * Updates the wavelengths received from the Rover.
   * @param index integer value that tells where this segment goes in data
   * @param dataInput float array of length 3 with the new data
   */
  // eslint-disable-next-line react/sort-comp
  updateDiodeVals(index: number, dataInput: number[]): void {
    const newData = dataInput.map((value: number, ndx: number) => {
      return { x: index + ndx, y: value };
    });

    this.setState((prevState) => ({ data: prevState.data.splice(index, index === 3500 ? 148 : 500, ...newData) }));
  }

  onMouseLeave(): void {
    this.setState({ crosshairPos: null });
  }

  onNearestX(index: number): void {
    this.setState({ crosshairPos: index });
  }

  toggleLed(index: number): void {
    const { LedStatus } = this.state;
    LedStatus[index] = !LedStatus[index];
    this.setState({
      LedStatus,
    });
    rovecomm.sendCommand('FlurometerLEDs', Fluorometer.buildLedCommand(LedStatus));
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
        <Crosshair values={[this.state.data[crosshairPos]]}>
          <div style={overlay}>
            <h3 style={{ borderStyle: 'solid', width: '30%', textAlign: 'center', backgroundColor: 'white' }}>
              {this.state.data[crosshairPos].y}
            </h3>
          </div>
        </Crosshair>
      );
    }
    return null;
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
            >
              <VerticalGridLines style={{ fill: 'none' }} />
              <HorizontalGridLines style={{ fill: 'none' }} />
              <LineSeries
                data={this.state.data}
                style={{ fill: 'none' }}
                strokeWidth="6"
                color="blue"
                onNearestX={(_datapoint: any, event: any) => this.onNearestX(event.index)}
              />
              <XAxis />
              <YAxis />
              {this.crosshair()}
            </XYPlot>
            {this.state.LedStatus.map((value, index) => {
              return (
                <label key={index} style={{ marginLeft: '5px' }} htmlFor="LedToggle">
                  <input
                    type="checkbox"
                    id="LedToggle"
                    name="LedToggle"
                    checked={value}
                    onChange={() => this.toggleLed(index)}
                  />
                  led#{index + 1}
                </label>
              );
            })}
            <button
              onClick={() => {
                rovecomm.sendCommand('ReqFluorometer', 1);
              }}
            >
              Request Reading
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default Fluorometer;
