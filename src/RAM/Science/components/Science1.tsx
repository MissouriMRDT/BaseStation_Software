import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  height: 'calc(100% - 40px)',
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
const componentBox: CSS.Properties = {
  margin: '3px 0 3px 0',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {}

class Science1 extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  render(): JSX.Element {
    return (
      <div id="Science1" style={this.props.style}>
        <div style={label}>Salt Water</div>
        <div style={container}>
          <div style={componentBox}>
            <img src={path.join(__dirname, '/RAM/Science/components/science1.png')} width="750px" />
          </div>
        </div>
      </div>
    );
  }
}

export default Science1;
