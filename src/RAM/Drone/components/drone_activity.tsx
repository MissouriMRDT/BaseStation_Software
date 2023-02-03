import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  whiteSpace: 'pre-wrap',
  overflow: 'scroll',
  height: '283px',
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
  text: string;
  backgroundColor: string;
}

class DroneActivity extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      text: '',
      backgroundColor: '',
    };

    this.ReachedMarker = this.ReachedMarker.bind(this);
    this.Log = this.Log.bind(this);

    rovecomm.on('droneCurrentLog', (data: any) => this.Log(data));
    rovecomm.on('droneReachedMarker', this.ReachedMarker);
  }

  Log(data: string): void {
    this.setState((prevState) => ({
      text: `${prevState.text}${new Date().toLocaleTimeString()}: ${data} \n`,
    }));
  }

  ReachedMarker(): void {
    this.setState({ backgroundColor: 'green' });
    this.Log('Reached waypoint!');
    const reachInterval = setInterval(() => {
      this.setState((prevState) => ({ backgroundColor: prevState.backgroundColor === 'green' ? 'white' : 'green' }));
    }, 250);

    setTimeout(() => {
      clearInterval(reachInterval);
      this.setState({ backgroundColor: 'white' });
    }, 4000);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drone Activity</div>
        <div style={{ ...container, backgroundColor: this.state.backgroundColor }}>{this.state.text}</div>
      </div>
    );
  }
}

export default DroneActivity;
