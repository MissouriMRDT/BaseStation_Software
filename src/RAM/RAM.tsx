import React, { Component } from 'react';
import CSS from 'csstype';
import Arm from './Arm/Arm';
import Autonomy from './Autonomy/Autonomy';
import Science from './Science/Science';
import { LContainer, DContainer } from '../Core/components/CssConstants';

const RON: CSS.Properties = {
  height: '100%',
  width: '100%',
};
function ColorToggle(theme: string): CSS.Properties {
  if (theme === 'light') {
    return LContainer;
  }
  return DContainer;
}
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-around',
};
const buttons: CSS.Properties = {
  border: 'none',
  background: 'none',
  fontSize: '42px',
  fontFamily: 'times new roman',
  fontWeight: 'bold',
  outline: 'none',
};

interface IProps {
  selectedWaypoint: any;
}

interface IState {
  displayed: string;
  theme: string;
}

class RoverAttachmentManager extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      displayed: 'Arm',
      theme: 'light',
    };
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

  screenChange(screen: string): void {
    this.setState({
      displayed: screen,
    });
  }

  render(): JSX.Element {
    return (
      <div style={{ ...RON, ...ColorToggle(this.state.theme) }}>
        <div style={row}>
          {['Arm', 'Science', 'Autonomy'].map((screen) => {
            return (
              <button
                type="button"
                key={screen}
                onClick={() => this.screenChange(screen)}
                style={{
                  ...buttons,
                  color: this.state.displayed === screen ? '#990000' : 'gray',
                }}
              >
                {screen}
              </button>
            );
          })}
        </div>
        {this.state.displayed === 'Arm' && <Arm />}
        {this.state.displayed === 'Science' && <Science />}
        {this.state.displayed === 'Autonomy' && <Autonomy selectedWaypoint={this.props.selectedWaypoint} />}
      </div>
    );
  }
}
export default RoverAttachmentManager;
