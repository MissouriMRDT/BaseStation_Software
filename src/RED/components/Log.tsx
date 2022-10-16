import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { getCurrentTheme } from '../../Core/Components/DarkMode';

function Container(): CSS.Properties {
  let temp: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    temp = {
      display: 'grid',
      fontFamily: 'arial',
      height: '250px',
      borderTopWidth: '28px',
      borderColor: '#990000',
      borderBottomWidth: '2px',
      borderStyle: 'solid',
      whiteSpace: 'pre-wrap',
      overflow: 'scroll',
      padding: '5px',
      borderRadius: '5px',
      backgroundColor: '#333333',
      color: 'white',
    };
  } else {
    temp = {
      display: 'grid',
      fontFamily: 'arial',
      height: '250px',
      borderTopWidth: '28px',
      borderColor: '#990000',
      borderBottomWidth: '2px',
      borderStyle: 'solid',
      whiteSpace: 'pre-wrap',
      overflow: 'scroll',
      padding: '5px',
      borderRadius: '5px',
    };
  }
  return temp;
}
const container: CSS.Properties = Container();
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
  ConsoleText: string;
}

class Log extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      ConsoleText: '',
    };
    rovecomm.on('all', (data: any) => this.Log(data));
  }

  Log(data: string): void {
    this.setState((prevState) => ({
      ConsoleText: `${prevState.ConsoleText}${new Date().toLocaleTimeString()}: ${data} \n`,
    }));
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Console</div>
        <div style={container}>{this.state.ConsoleText}</div>
      </div>
    );
  }
}

export default Log;
