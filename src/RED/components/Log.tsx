import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { DContainer, LContainer } from '../../Core/components/CssConstants';

function container(theme: string): CSS.Properties {
  if (theme === 'light') {
    return LContainer;
  }
  return DContainer;
}
const localContainer: CSS.Properties = {
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
  ConsoleText: string;
  theme: string;
}

class Log extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      ConsoleText: '',
      theme: 'light',
    };
    rovecomm.on('all', (data: any) => this.Log(data));
  }

  setTheme(): void {
    let currentTheme: string;
    if (this.state.theme === 'light') {
      currentTheme = 'dark';
      this.setState({ theme: currentTheme });
      console.log('set state to dark mode');
    } else {
      currentTheme = 'light';
      this.setState({ theme: currentTheme });
      console.log('set state to light mode');
    }
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
        <div style={{ ...container(this.state.theme), ...localContainer }}>{this.state.ConsoleText}</div>
      </div>
    );
  }
}

export default Log;
