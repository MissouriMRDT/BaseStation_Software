import React, { Component } from 'react';
import CSS from 'csstype';
import { XYPlot, XAxis, YAxis, HorizontalGridLines, LineSeries } from 'react-vis';
import { RovecommManifest } from '../../Core/RoveProtocol/Rovecomm';
import { DevicePingList } from './PingTool';

const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '18px',
  margin: '5px 0px',
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
  justifyContent: 'center',
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
const selectbox: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  margin: '2.5px',
  justifyContent: 'space-around',
};
const selector: CSS.Properties = {
  width: '200px',
};

const MAX_PING_DATA = 20;

interface IProps {
  devices: DevicePingList;
  style?: CSS.Properties;
}

interface IState {
  ping: {
    x: number;
    y: number;
  }[];
  board: string;
  interval: NodeJS.Timeout;
}

class PingGraph extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      ping: [],
      board: 'Core',
      interval: setInterval(() => this.update('Core'), 1000),
    };
    this.update = this.update.bind(this);
    this.boardChange = this.boardChange.bind(this);
  }

  componentWillUnmount() {
    clearInterval(this.state.interval);
  }

  update(device: string): void {
    let { ping } = this.state;
    // Delete old data if over limit, shifting x values left. The +1 is to account for the 0th index
    if (ping.length >= MAX_PING_DATA + 1)
      ping = ping.slice(1, MAX_PING_DATA + 1).map((item) => ({ ...item, x: item.x - 1 }));
    // Push new point to graph, default y to -1 if device isn't found or device has no ping
    if (device in this.props.devices) {
      ping.push({ x: ping.length, y: this.props.devices[device].ping });
    } else {
      ping.push({ x: ping.length, y: -1 });
    }
    this.setState({ ping });
  }

  boardChange(event: { target: { value: string } }): void {
    const board = event.target.value;
    clearInterval(this.state.interval);
    this.setState({
      board,
      ping: [],
      interval: setInterval(() => {
        this.update(board);
      }, 1000),
    });
  }

  render(): JSX.Element {
    return (
      <div style={{ ...this.props.style }}>
        <div style={label}>Ping Graph</div>
        <div style={container}>
          <div style={selectbox}>
            <div style={h1Style}>Board:</div>
            <select value={this.state.board} onChange={(e) => this.boardChange(e)} style={selector}>
              {Object.keys(RovecommManifest).map((item) => {
                return (
                  <option key={item} value={item}>
                    {item}
                  </option>
                );
              })}
            </select>
          </div>
          <XYPlot
            style={{ margin: 10 }}
            height={300}
            width={window.document.documentElement.clientWidth - 50}
            xDomain={[0, MAX_PING_DATA]}
            yDomain={[-2, 10]}
          >
            <HorizontalGridLines style={{ fill: 'none' }} />
            <LineSeries data={this.state.ping} style={{ fill: 'none' }} />
            <XAxis />
            <YAxis />
          </XYPlot>
        </div>
      </div>
    );
  }
}

export default PingGraph;
