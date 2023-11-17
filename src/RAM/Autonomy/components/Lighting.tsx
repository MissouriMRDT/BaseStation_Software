import React, { ChangeEvent, Component } from 'react';
import CSS from 'csstype';
import { ChromePicker, ColorResult, RGBColor } from 'react-color';
import { RovecommManifest, rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexDirection: 'column',
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
};

const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};

const button: CSS.Properties = {
  margin: '5px',
};

interface IProps {
  style?: CSS.Properties;
}
interface IState {
  color: RGBColor;
  brightInput: string;
}

class Lighting extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  static teleop(): void {
    rovecomm.sendCommand('StateDisplay', RovecommManifest.Core.Enums.DISPLAYSTATE.Teleop);
    // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Teleop);
  }

  static autonomy(): void {
    rovecomm.sendCommand('StateDisplay', RovecommManifest.Core.Enums.DISPLAYSTATE.Autonomy);
    // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Autonomy);
  }

  static reachedGoal(): void {
    rovecomm.sendCommand('StateDisplay', RovecommManifest.Core.Enums.DISPLAYSTATE.Reached_Goal);
    // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Reached_Goal);
  }

  constructor(props: Readonly<IProps> | IProps) {
    super(props);
    this.state = {
      color: { r: 255, g: 255, b: 255, a: 0 },
      brightInput: '',
    };
  }

  handleEdit(event: ChangeEvent<HTMLInputElement>): void {
    this.setState({ brightInput: event.target.value });
  }

  colorChanged(newColor: ColorResult): void {
    const color = newColor.rgb;
    this.setState({ color });
    rovecomm.sendCommand('LEDRGB', [color.r, color.g, color.b]);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Lighting</div>
        <div style={container}>
          <div style={column}>
            <ChromePicker
              color={this.state.color}
              onChangeComplete={(color: ColorResult) => this.colorChanged(color)}
            />
            <div style={{ ...row, justifyContent: 'center' }}>
              <button style={button} onClick={Lighting.teleop}>
                Teleop
              </button>
              <button style={button} onClick={Lighting.autonomy}>
                Autonomy
              </button>
              <button style={button} onClick={Lighting.reachedGoal}>
                Goal
              </button>
            </div>
            <div style={{ ...row, justifyContent: 'center', marginBottom: '5px' }}>
              <input
                style={{ width: '50px' }}
                id="bright"
                value={this.state.brightInput}
                onChange={(e) => this.handleEdit(e)}
              />
              <button
                type="button"
                onClick={() => {
                  if (Number(this.state.brightInput) >= 0 && Number(this.state.brightInput) <= 124)
                    rovecomm.sendCommand('Brightness', this.state.brightInput);
                }}
              >
                Set Brightness
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Lighting;
