/* eslint-disable prettier/prettier */
import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

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

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px'
};

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-around',
  height: '30px',
  margin: '10px',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};

const packetNumList = [0, 1, 2, 3, 4, 5, 6, 7];

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  packetStream: boolean;
  currentPacket: number;
}

class ClosedLoopControls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props:IProps) {
    super(props);
    this.state = {
      packetStream: false,
      currentPacket: 7,
    }
    this.updatePacketStream = this.updatePacketStream.bind(this);

    setInterval(() => this.sendPositionData(), 100);
  }

  updatePacketStream() {
    this.setState((prevState) => ({packetStream: !prevState.packetStream}));
  }

  sendPositionData() {
    if (this.state.packetStream) {
      rovecomm.sendCommand('GotoPosition', this.state.currentPacket);
      console.log('packet value:', this.state.currentPacket);
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Closed Loop</div>
        <div style={container}>
          <div style={column}>
            <button onClick={() => this.updatePacketStream()}>{this.state.packetStream ? "On" : "Off"}</button>
            <div style={row}>
              {packetNumList.map((num) => {
                let buttonText: any = num;
                if (num === 0) {
                  buttonText = 'Ground';
                } else if (num === 7) {
                  buttonText = 'Calibrate';
                }
                // I know this is weird but people on science thought this wasn't verbose enough. Feel free to suggest something different.
                return (
                  <button
                    type="button" 
                    key={num}
                    onClick={() => this.setState({ currentPacket: num })}
                    style={{
                      flexGrow: 1,
                      borderWidth: this.state.currentPacket === num ? 'medium' : 'thin',
                    }}
                  >
                    {buttonText}
                  </button>
                )
              })}
            </div>
            WARNING!! MAKE SURE TO CALIBRATE FIRST
          </div>
        </div>
      </div>
    );
  }
}

export default ClosedLoopControls