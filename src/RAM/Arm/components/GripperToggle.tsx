import React, { Component } from 'react';
import CSS from 'csstype';

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

interface IProps {
  GripperCallback: any;
}

interface IState {
  GripperState: boolean;
}

class GripperToggle extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      GripperState: false,
    };
    this.toggle = this.toggle.bind(this);
  }

  // componentDidMount(): void {
  //   if (fs.existsSync(filepath)) {
  //     const storedTheme: string = JSON.parse(fs.readFileSync(filepath).toString());
  //     this.setState({
  //       currentTheme: storedTheme,
  //     });
  //   }
  // }

  toggle(): void {
    this.props.GripperCallback();
    this.setState((prevState) => ({
      GripperState: !prevState.GripperState
    }));
    // window.location.reload();
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Toggle Gripper</div>
        <div style={container}>
          <button onClick={() => this.toggle()}>{this.state.GripperState ? 'Gripper 2' : 'Gripper 1'}</button>
        </div>
      </div>
    );
  }
}

export default GripperToggle;
