import React, { Component } from 'react';
import CSS from 'csstype';

const images = require
  .context('../ScienceGraphs', false, /\.(jpg|png)$/)
  .keys()
  .map((image) => require(`../ScienceGraphs/${image.replace('./', '')}`).default);

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
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-around',
  alignContent: 'center',
  marginTop: '5px',
  width: '100%',
};

const imageStyle: CSS.Properties = {
  maxWidth: '80%',
  maxHeight: '100%',
  objectFit: 'contain',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  currentIndex: number;
}

class ScienceGraphs extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      currentIndex: 0,
    };
  }

  handlePrevClick = (): void => {
    this.setState((prevState) => ({
      currentIndex: (prevState.currentIndex - 1 + images.length) % images.length,
    }));
  };

  handleNextClick = (): void => {
    this.setState((prevState) => ({
      currentIndex: (prevState.currentIndex + 1) % images.length,
    }));
  };

  render(): JSX.Element {
    const { currentIndex } = this.state;

    return (
      <div id="ScienceGraphs" style={this.props.style}>
        <div style={label}>Science Graphs</div>
        <div style={container}>
          <div style={row}>
            <div style={{ ...row, justifyContent: 'center' }}>
              <button onClick={this.handlePrevClick}>&lt;</button>
              <img style={imageStyle} src={images[currentIndex]} alt={`image-${currentIndex}`} />
              <button onClick={this.handleNextClick}>&gt;</button>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ScienceGraphs;
