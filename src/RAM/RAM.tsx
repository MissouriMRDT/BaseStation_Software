import React, { Component } from 'react';
import CSS from 'csstype';
import Arm from './Arm/Arm';
import Autonomy from './Autonomy/Autonomy';
import Science from './Science/Science';
import { container } from '../Core/components/CssConstants';

const RON: CSS.Properties = {};
// function ColorToggle(theme: string): CSS.Properties {
//   if (theme === 'light') {
//     return LContainer;
//   }
//   return DContainer;
// }
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
  theme: string;
}

interface IState {
  displayed: string;
}

class RoverAttachmentManager extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      displayed: 'Arm',
    };
  }

  // setTheme(): void {
  //   let currentTheme: string;
  //   if (this.state.theme === 'light') {
  //     currentTheme = 'dark';
  //     this.setState({ theme: currentTheme });
  //     console.log('set state to dark mode');
  //   } else {
  //     currentTheme = 'light';
  //     this.setState({ theme: currentTheme });
  //     console.log('set state to light mode');
  //   }
  // }

  screenChange(screen: string): void {
    this.setState({
      displayed: screen,
    });
  }

  render(): JSX.Element {
    return (
      <div style={RON}>
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
        {this.state.displayed === 'Science' && <Science theme={this.props.theme}/>}
        {this.state.displayed === 'Autonomy' && (
          <Autonomy selectedWaypoint={this.props.selectedWaypoint} theme={this.props.theme} />
        )}
      </div>
    );
  }
}
export default RoverAttachmentManager;
