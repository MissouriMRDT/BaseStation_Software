import React, { Component } from 'react';
import CSS from 'csstype';

const button: CSS.Properties = {
  width: '100%',
  margin: '2.5px',
  fontSize: '14px',
  lineHeight: '24px',
  backgroundColor: '#990000',
  color: 'white',
  borderRadius: '5px',
};

interface IProps {
  themeCallback: any;
}

interface IState {}

export class DarkModeToggle extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
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
    this.props.themeCallback();
    // window.location.reload();
  }

  render(): JSX.Element {
    return (
      <div>
        <button style={button} onClick={() => this.toggle()}>
          Toggle Light/Dark Mode
        </button>
      </div>
    );
  }
}

export default DarkModeToggle;
