import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { BitmaskUnpack } from '../../Core/BitmaskUnpack';

const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '12px',
};
const container: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  flexDirection: 'column',
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
// const row: CSS.Properties = {
//   display: 'flex',
//   flexDirection: 'row',
//   justifyContent: 'space-between',
// };
const buttonRow: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};

interface IProps {
  style?: CSS.Properties;
}

/** Number of simultaneous camera streams */
const NUM_CAMS = 4;
/** Number of cameras that can be attached to Jetson */
const MAX_CAMS = 8;

interface IState {
  /** Which cameras have been marked as able to stream */
  AvailableCams: boolean[];
  /** Cameras that are currently streaming starting at port 5000 */
  SelectedCams: number[];
}

/**
 * Send the Rovecomm command to switch a port to view a given camera.
 * Port is which feed will be switched.
 * The new feed will stream at port (5000 + portId)
 */
function switchFeed(portId: number, newCam: number): void {
  rovecomm.sendCommand('ChangeCameras', [portId, newCam]);
}

class CameraControls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      AvailableCams: new Array(MAX_CAMS).fill(false).flat(),
      SelectedCams: new Array(NUM_CAMS).fill(-1).flat(),
    };
    this.setAvailable = this.setAvailable.bind(this);
    this.setStreaming = this.setStreaming.bind(this);
    rovecomm.on('AvailableCameras', this.setAvailable);
    rovecomm.on('StreamingCameras', this.setStreaming);
  }

  setAvailable(data: number[]): void {
    const available = BitmaskUnpack(data[0], 8).split('').reverse().map(Number).map(Boolean);
    console.log(available);
    this.setState({ AvailableCams: available });
  }

  /** Tells Basestation which cameras the Jetson is currently streaming to which port */
  setStreaming(data: number[]): void {
    this.setState({ SelectedCams: data });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Camera Controls</div>
        <div style={container}>
          {/* This will be removed. Currently tests receiving rovecomm packets. */}
          <button onClick={() => this.setStreaming([5, 1, 1, 0])}>Test</button>
          {this.state.SelectedCams.map((val, ndx) => {
            return (
              <div key={`Cam${ndx}`} style={buttonRow}>
                <p>Port 500{ndx}</p>
                {this.state.AvailableCams.map((isAvail, avNdx) => {
                  return (
                    <button
                      type="button"
                      onClick={() => switchFeed(ndx, avNdx)}
                      key={avNdx}
                      style={{
                        flexGrow: 1,
                        borderWidth: val === avNdx ? 'medium' : 'thin',
                        backgroundColor: isAvail ? '#00FF00' : '#FF0000',
                      }}
                    >
                      <h1 style={h1Style}>{avNdx + 1}</h1>
                    </button>
                  );
                })}
              </div>
            );
          })}
        </div>
      </div>
    );
  }
}

export default CameraControls;
