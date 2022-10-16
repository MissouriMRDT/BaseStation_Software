import React, { Component } from 'react';
import CSS from 'csstype';
import fs from 'fs';
import path from 'path';

const button: CSS.Properties = {
  width: '100%',
  margin: '2.5px',
  fontSize: '14px',
  lineHeight: '24px',
  backgroundColor: '#990000',
  color: 'white',
  borderRadius: '5px',
};
const filepath = path.join(__dirname, '../assets/ThemeState.json');

interface IProps {}

interface IState {
  currentTheme: string;
}

export class DarkModeToggle extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    let storedTheme = 'light';
    if (fs.existsSync(filepath)) {
      storedTheme = JSON.parse(fs.readFileSync(filepath).toString());
      if (storedTheme === '') {
        fs.writeFile(filepath, JSON.stringify('light'), (err) => {
          if (err) throw err;
        });
        storedTheme = 'light';
      }
    }
    this.state = {
      currentTheme: storedTheme,
    };
    if (this.state.currentTheme === 'light') {
      document.body.style.backgroundColor = 'white';
    } else {
      document.body.style.backgroundColor = '#252525';
    }
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
    let currentTheme: string;
    if (this.state.currentTheme === 'light') {
      currentTheme = 'dark';
      fs.writeFile(filepath, JSON.stringify('dark'), (err) => {
        if (err) throw err;
      });
      this.setState({
        currentTheme,
      });
      document.body.style.backgroundColor = '#252525';
      console.log('set state to dark mode');
    } else {
      currentTheme = 'light';
      fs.writeFile(filepath, JSON.stringify('light'), (err) => {
        if (err) throw err;
      });
      this.setState({
        currentTheme,
      });
      document.body.style.backgroundColor = 'white';
      console.log('set state to light mode');
    }
    window.location.reload();
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

const props = {};
export function getCurrentTheme(): string {
  const currTheme = new DarkModeToggle(props);
  return currTheme.state.currentTheme;
}
