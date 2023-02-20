import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { container, boxStyle } from '../../../Core/components/CssConstants';

const Container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  whiteSpace: 'pre-wrap',
  overflow: 'scroll',
  height: 'calc(100% - 40px)',
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
  theme: string;
}

interface IState {
  ActivityText: string;
  backgroundColor: string;
}

class Activity extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      ActivityText: '',
      backgroundColor: boxStyle(this.props.theme),
    };

    this.ReachedMarker = this.ReachedMarker.bind(this);
    this.Log = this.Log.bind(this);

    rovecomm.on('CurrentLog', (data: any) => this.Log(data));
    rovecomm.on('ReachedMarker', this.ReachedMarker);
  }

  componentDidUpdate(prevProps: Readonly<IProps>, prevState: Readonly<IState>, snapshot?: any): void {
    if (this.props.theme !== prevProps.theme) {
      this.setState({ backgroundColor: boxStyle(this.props.theme) });
    }
  }

  ReachedMarker(): void {
    this.setState({ backgroundColor: 'green' });
    this.Log('Reached waypoint!');
    const reachInterval = setInterval(() => {
      this.setState((prevState) => ({
        backgroundColor: prevState.backgroundColor === 'green' ? boxStyle(this.props.theme) : 'green',
      }));
    }, 250);

    setTimeout(() => {
      clearInterval(reachInterval);
      this.setState({ backgroundColor: boxStyle(this.props.theme) });
    }, 4000);
  }

  Log(data: string): void {
    this.setState((prevState) => ({
      ActivityText: `${prevState.ActivityText}${new Date().toLocaleTimeString()}: ${data} \n`,
    }));
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Autonomy Activity</div>
        <div style={{ ...Container, backgroundColor: this.state.backgroundColor }}>{this.state.ActivityText}</div>
      </div>
    );
  }
}

export default Activity;
