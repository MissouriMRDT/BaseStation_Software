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
  padding: '5px',
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

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  packetType: string;
  packetStream: boolean;
  currentPacket: number;
  possiblePackets: number[];
}

class ClosedLoopControls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      packetType: 'Scoop',
      packetStream: false,
      currentPacket: 7,
      possiblePackets: [0, 1, 2, 3, 4, 5, 6, 7],
    };
    this.updatePacketStream = this.updatePacketStream.bind(this);
    this.changePacketType = this.changePacketType.bind(this);

    setInterval(() => this.sendPositionData(), 100);
  }

  updatePacketStream(): void {
    this.setState((prevState) => ({ packetStream: !prevState.packetStream }));
  }

  sendPositionData(): void {
    if (this.state.packetStream) {
      if (this.state.packetType === 'Scoop') {
        rovecomm.sendCommand('Auger', 'Science', this.state.currentPacket);
      } else if (this.state.packetType === 'Multiplexor') {
      }
      console.log(this.state.packetType, this.state.currentPacket);
    }
  }

  changePacketType(event: { target: { value: string } }): void {
    if (event.target.value === 'Scoop') {
      this.setState({ possiblePackets: [...Array(8)].map((_, index) => index) });
    } else if (event.target.value === 'Multiplexor') {
      this.setState({ possiblePackets: [...Array(13)].map((_, index) => index) });
    }
    this.setState({ packetType: event.target.value });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Closed Loop</div>
        <div style={container}>
          <div style={column}>
            <div style={row}>
              <select value={this.state.packetType} onChange={(e) => this.changePacketType(e)} style={{ flex: 1 }}>
                {['Scoop', 'Multiplexor'].map((packetSelect) => {
                  return (
                    <option value={packetSelect} key={packetSelect}>
                      {packetSelect}
                    </option>
                  );
                })}
              </select>
              <button onClick={() => this.updatePacketStream()} style={{ flex: 1 }}>
                {this.state.packetStream ? 'On' : 'Off'}
              </button>
            </div>
            <div style={row}>
              {this.state.possiblePackets.map((num) => {
                let buttonText: any = num;
                if (num === 0) {
                  if (this.state.packetType === 'Scoop') {
                    buttonText = 'Ground';
                  } else if (this.state.packetType === 'Multiplexor') {
                    buttonText = 'Calibrate';
                  }
                } else if (this.state.packetType === 'Scoop' && num === 7) {
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
                );
              })}
            </div>
            WARNING!! MAKE SURE TO CALIBRATE FIRST
          </div>
        </div>
      </div>
    );
  }
}

export default ClosedLoopControls;
