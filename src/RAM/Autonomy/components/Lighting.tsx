import React, { Component } from 'react';
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
  pattern: number;
  message: string;
  configs: {
    bold: boolean;
    underline: boolean;
    strike: boolean;
    color: string;
    highlight: string;
  };
  showPatternSelect: boolean;
  showColorSelect: boolean;
  showHighlightSelect: boolean;
}

const colorRegex = /\\c(#[a-fA-F0-9]{6});/g;
const highlightRegex = /\\h(#[a-fA-F0-9]{6});/g;
const nonAsciiRegex = /[^\x20-\x7E]/g;

function sendTeleop() {
  rovecomm.sendCommand('StateDisplay', 'Core', RovecommManifest.Core.Enums.DISPLAYSTATE.Teleop);
  // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Teleop);
}

function sendAutonomy() {
  rovecomm.sendCommand('StateDisplay', 'Core', RovecommManifest.Core.Enums.DISPLAYSTATE.Autonomy);
  // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Autonomy);
}

function sendReachedGoal() {
  rovecomm.sendCommand('StateDisplay', 'Core', RovecommManifest.Core.Enums.DISPLAYSTATE.Reached_Goal);
  // console.log(RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Reached_Goal);
}

function sendText(message: string) {
  const payload = message.padEnd(256, '\0');
  rovecomm.sendCommand('LEDText', 'Core', payload);
  //console.log(payload);
}
function sendPattern(pattern: number) {
  const payload = Math.max(0, Math.min(pattern, 255)); // clamp to char range
  rovecomm.sendCommand('LEDPatterns', 'Core', payload);
  //console.log(payload);
}

class Lighting extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  private preview: React.RefObject<HTMLDivElement>;

  private textInput: React.RefObject<HTMLTextAreaElement>;

  constructor(props: IProps) {
    super(props);
    this.state = {
      color: { r: 255, g: 255, b: 255, a: 0 },
      brightInput: '',
      pattern: 0,
      message: '',
      configs: {
        bold: false,
        underline: false,
        strike: false,
        color: '0',
        highlight: '0',
      },
      showPatternSelect: false,
      showColorSelect: false,
      showHighlightSelect: false,
    };
    this.preview = React.createRef();
    this.textInput = React.createRef();
  }

  colorChanged(newColor: ColorResult): void {
    const color = newColor.rgb;
    this.setState({ color });
    rovecomm.sendCommand('LEDRGB', 'Core', [color.r, color.g, color.b]);
  }

  refocus(caretPos: number | null) {
    this.textInput.current?.focus();
    this.textInput.current?.setSelectionRange(caretPos, caretPos);
  }

  insertAtCursor(tag: string, after?: () => void) {
    let caretPos = this.textInput.current?.selectionStart;
    if (caretPos === null) caretPos = undefined;
    if (caretPos !== undefined)
      this.setState(
        {
          message: this.state.message.slice(0, caretPos) + tag + this.state.message.slice(caretPos),
        },
        after
      );
    else this.setState({ message: this.state.message + tag }, after);
  }

  insertCurry(tag: string) {
    const func = () => {
      let caretPos = this.textInput.current?.selectionStart;
      if (caretPos === null) caretPos = undefined;
      const suffix =
        (this.state.message.lastIndexOf(tag + '0', caretPos) ?? 0) >
        (this.state.message.lastIndexOf(tag + '1', caretPos) ?? 0)
          ? '1'
          : '0';
      if (caretPos !== undefined) this.insertAtCursor(tag + suffix, () => this.refocus(caretPos + tag.length + 1));
    };
    func.bind(this);
    return func; // a delicious batch of function curry
  }

  closeColorSelect() {
    this.setState({ showColorSelect: false });
    this.insertAtCursor(`\\c${this.state.configs.color};`);
    this.textInput.current?.focus();
  }

  closeHighlightSelect() {
    this.setState({ showHighlightSelect: false });
    this.insertAtCursor(`\\h${this.state.configs.highlight};`);
    this.textInput.current?.focus();
  }

  componentDidUpdate(_prevProps: Readonly<IProps>, prevState: Readonly<IState>): void {
    if (prevState.message !== this.state.message) {
      if (this.state.message.match(nonAsciiRegex)) {
        this.setState({ message: this.state.message.replaceAll(nonAsciiRegex, '') });
        return;
      }
      if (this.state.message.length > 256) {
        this.setState({ message: this.state.message.slice(0, 256) });
        return;
      }
      const messagePreview = this.state.message
        .replaceAll('\\b0', '<b>')
        .replaceAll('\\b1', '</b>')
        .replaceAll('\\u0', '<u>')
        .replaceAll('\\u1', '</u>')
        .replaceAll('\\s0', '<s>')
        .replaceAll('\\s1', '</s>')
        .replaceAll(colorRegex, '<span style="color:$1">')
        .replaceAll(/\\c0;/g, '<span style="color:white">')
        .replaceAll(highlightRegex, '<span style="background-color:$1">')
        .replaceAll(/\\h0;/g, '<span style="background-color:black">');
      if (this.preview.current) this.preview.current.innerHTML = messagePreview;
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Lighting</div>
        <div style={container}>
          <div style={column}>
            <ChromePicker
              color={this.state.color}
              onChangeComplete={(color) => this.colorChanged(color)}
              styles={{ default: { picker: { width: '100%' } } }}
            />
            <div style={{ ...row, justifyContent: 'center' }}>
              <button style={button} onClick={sendTeleop}>
                Teleop
              </button>
              <button style={button} onClick={sendAutonomy}>
                Autonomy
              </button>
              <button style={button} onClick={sendReachedGoal}>
                Goal
              </button>
            </div>
            <div style={{ ...row, justifyContent: 'center', marginBottom: '5px' }}>
              <input
                style={{ width: '50px' }}
                type="number"
                id="bright"
                value={this.state.brightInput}
                onChange={(e) => this.setState({ brightInput: e.target.value })}
              />
              <button
                type="button"
                onClick={() => {
                  if (Number(this.state.brightInput) >= 0 && Number(this.state.brightInput) <= 124)
                    rovecomm.sendCommand('Brightness', 'Core', this.state.brightInput);
                }}
              >
                Set Brightness
              </button>
            </div>
            <div style={{ ...row, justifyContent: 'center', marginBottom: '5px' }}>
              <div style={column}>
                <select
                  onChange={(e) =>
                    this.setState({
                      pattern: e.currentTarget.value !== 'Other' ? parseInt(e.currentTarget.value) : 0,
                      showPatternSelect: e.currentTarget.value === 'Other',
                    })
                  }
                >
                  {Object.keys(RovecommManifest.Core.Enums.PATTERNS).map((pattern, i) => (
                    <option key={pattern} value={i}>
                      {pattern}
                    </option>
                  ))}
                  <option>Other</option>
                </select>
                {this.state.showPatternSelect && (
                  <input
                    style={{ width: '50px' }}
                    type="number"
                    onChange={(e) => this.setState({ pattern: parseInt(e.currentTarget.value) })}
                  />
                )}
              </div>
              <button onClick={() => sendPattern(this.state.pattern)}>Set Pattern</button>
            </div>
            <div>
              <div>
                <button onClick={this.insertCurry('\\b')}>
                  <b>B</b>
                </button>
                <button onClick={this.insertCurry('\\u')}>
                  <u>U</u>
                </button>
                <button onClick={this.insertCurry('\\s')}>
                  <s>S</s>
                </button>
                <span style={{ position: 'relative' }}>
                  {this.state.showColorSelect && (
                    <div style={{ position: 'absolute', bottom: 15, left: 0, zIndex: 10 }}>
                      <ChromePicker
                        color={this.state.configs.color}
                        onChangeComplete={(color) => {
                          this.setState({
                            configs: { ...this.state.configs, color: color.hex.toUpperCase() },
                          });
                        }}
                      />
                      <button
                        onClick={() => {
                          this.setState({ configs: { ...this.state.configs, color: '0' } }, () =>
                            this.closeColorSelect()
                          );
                        }}
                      >
                        Clear Color
                      </button>
                    </div>
                  )}
                  <button
                    onClick={() => {
                      if (this.state.showColorSelect) this.closeColorSelect();
                      else this.setState({ showColorSelect: true, showHighlightSelect: false });
                    }}
                  >
                    <u style={{ color: this.state.configs.color === '0' ? 'black' : this.state.configs.color }}>A</u>
                  </button>
                </span>
                <span style={{ position: 'relative' }}>
                  {this.state.showHighlightSelect && (
                    <div style={{ position: 'absolute', bottom: 15, left: 0, zIndex: 10 }}>
                      <ChromePicker
                        color={this.state.configs.highlight}
                        onChangeComplete={(color) => {
                          this.setState({
                            configs: { ...this.state.configs, highlight: color.hex.toUpperCase() },
                          });
                        }}
                      />
                      <button
                        onClick={() => {
                          this.setState({ configs: { ...this.state.configs, highlight: '0' } }, () =>
                            this.closeHighlightSelect()
                          );
                        }}
                      >
                        Clear Highlight
                      </button>
                    </div>
                  )}
                  <button
                    onClick={() => {
                      if (this.state.showHighlightSelect) this.closeHighlightSelect();
                      else this.setState({ showHighlightSelect: true, showColorSelect: false });
                    }}
                  >
                    <u
                      style={{
                        backgroundColor:
                          this.state.configs.highlight === '0' ? 'rgba(0, 0, 0, 0.2)' : this.state.configs.highlight,
                      }}
                    >
                      A
                    </u>
                  </button>
                </span>
              </div>
              <textarea
                onChange={(e) => this.setState({ message: e.currentTarget.value })}
                rows={10}
                style={{ width: '100%', resize: 'none' }}
                value={this.state.message}
                maxLength={256}
                ref={this.textInput}
              ></textarea>
              <div style={{ padding: '2px' }}>
                <div>Preview:</div>
                <div
                  style={{
                    wordBreak: 'break-all',
                    whiteSpace: 'pre-wrap',
                    fontFamily: 'consolas',
                    backgroundColor: 'black',
                    color: 'white',
                  }}
                  ref={this.preview}
                />
                <button onClick={() => sendText(this.state.message)}>Set Message</button>
                <i style={{ float: 'right' }}>{this.state.message.length} of 256</i>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Lighting;
