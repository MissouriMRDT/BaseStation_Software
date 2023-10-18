// eslint-disable-next-line max-classes-per-file
import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import fs from 'fs';

const container: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  flexDirection: 'column',
  padding: '5px',
};
const modal: CSS.Properties = {
  zIndex: 2,
  position: 'absolute',
  backgroundColor: 'white',
  borderStyle: 'solid',
  borderWidth: '2px',
  borderColor: 'black',
};
const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 2,
  color: 'white',
};
const readoutDisplay: CSS.Properties = {
  fontSize: '16px',
  fontFamily: 'arial',
  padding: '3px 3px 4.5px 0',
  marginRight: '2px',
  flex: 1,
};
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  margin: '2px',
};

// eslint-disable-next-line import/no-mutable-exports
export let controllerInputs: any = {};

// Input configuration file
const inputJSON = path.join(__dirname, '../assets/ControllerInput.json');
let CONTROLLERINPUT: any = {};
if (fs.existsSync(inputJSON)) {
  CONTROLLERINPUT = JSON.parse(fs.readFileSync(inputJSON).toString());
}

// Mappings between controller buttons' names and their indexes
const controllerJSON = path.join(__dirname, '../assets/Controller.json');
let CONTROLLER: any = {};
if (fs.existsSync(controllerJSON)) {
  CONTROLLER = JSON.parse(fs.readFileSync(controllerJSON).toString());
}

// passedScheme is the current scheme that is selected, pos is the position in the list of controllers that are connected
function controller(passedScheme: any, pos: any): any {
  controllerInputs = {};
  return setInterval(() => {
    // if navigator.getGampads()[pos] == flight stick
    let index: number;
    let deadZone = 0.075; // xbox one controller
    const controllerList = [];
    for (let i = 0; i < 4; i++) {
      if (navigator.getGamepads()[i] != null) {
        controllerList.push(navigator.getGamepads()[i]?.id);
      }
    }

    // goes through the list of currently connected controllers and looks to see if any of their names are "Logitech Ex..."
    // if there does exist an instance of that set FlighStickIndex to that Index, else set it to -1
    const FLIGHT_STICK_INDEX = controllerList.findIndex((element) => {
      if (element && element.indexOf('Logitech Extreme 3D') >= 0) return true;
      return false;
    });
    // This is the part that changes the current controller list to adjust for the FlightStick so that it never gets assigned as a xbox controller
    // if the FlightStickIndex is not -1 and it is less than the current index it will then assign it to that spot and "push" the xbox controller back a spot in the index
    switch (pos) {
      case 'Xbox 1':
        index = FLIGHT_STICK_INDEX !== -1 && FLIGHT_STICK_INDEX <= 0 ? 1 : 0;
        break;
      case 'Xbox 2':
        index = FLIGHT_STICK_INDEX !== -1 && FLIGHT_STICK_INDEX <= 1 ? 2 : 1;
        break;
      case 'Xbox 3':
        index = FLIGHT_STICK_INDEX !== -1 && FLIGHT_STICK_INDEX <= 2 ? 3 : 2;
        break;
      case 'Xbox 4':
        index = FLIGHT_STICK_INDEX !== -1 && FLIGHT_STICK_INDEX <= 3 ? 4 : 3;
        break;
      case 'Flight Stick': // Logitech Extreme 3D
        index = FLIGHT_STICK_INDEX;
        deadZone = 0.1;
        break;
      default:
        return;
    }
    // buttonType will either return if it is a button (such as a,b, start, etc) or if it is a axes of controller (such as left stick x axis)
    if (navigator.getGamepads()[index] != null && passedScheme !== '') {
      for (const button in CONTROLLERINPUT[passedScheme].bindings) {
        if (CONTROLLERINPUT[passedScheme].bindings[button].buttonType === 'button') {
          controllerInputs[button] =
            navigator.getGamepads()[index]?.buttons[ // Button indexes from the controller
              CONTROLLER[CONTROLLERINPUT[passedScheme].controller].button[ // object of buttons on this controller type
                CONTROLLERINPUT[passedScheme].bindings[button].button // the desired button from this control scheme
              ]
            ].value;
        } else {
          controllerInputs[button] =
            navigator.getGamepads()[index]?.axes[ // axes indexes from the controller
              CONTROLLER[CONTROLLERINPUT[passedScheme].controller].joystick[ // object of joystick on this controller type
                CONTROLLERINPUT[passedScheme].bindings[button].button // the desired "button" from this control scheme
              ]
            ];
          // this checks to see if the current value of the axes are under the deadzone and will set
          // it to zero to prevent unwanted inputs when the controller is not being used
          if (controllerInputs[button] >= -deadZone && controllerInputs[button] <= deadZone) {
            controllerInputs[button] = 0.0;
            // This inverts the input because it is opposite from what you would think (pushing forward would give you a negative value)
          } else {
            controllerInputs[button] *= -1;
          }
        }
      }
    }
    if (navigator.getGamepads()[index] == null && passedScheme !== '') {
      for (const button in CONTROLLERINPUT[passedScheme].bindings) {
        controllerInputs[button] = 0;
      }
    }
  }, 50);
}

interface IProps {
  style?: CSS.Properties;
  configs: string[];
}

interface IState {
  functionality: any;
  controlPreviewModal: boolean;
  image: string;
}

const xboxController = path.join(__dirname, '../assets/xboxController.png');
const flightStick = path.join(__dirname, '../assets/flightStick.png');

class ControlScheme extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: Readonly<IProps>) {
    super(props);
    this.state = {
      functionality: {
        Drive: {
          toggled: 'Off',
          scheme: 'TankDrive',
          controller: 'Xbox 1',
          interval: null,
        },
        MainGimbal: {
          toggled: 'Off',
          scheme: 'Gimbal',
          controller: 'Xbox 2',
          interval: null,
        },
        // Science and Arm are set to Xbox 3 so that they do not conflict with gimbal or drive as they might be on the same page
        Science: {
          toggled: 'Off',
          scheme: 'ScienceControls',
          controller: 'Xbox 3',
          interval: null,
        },
        Arm: {
          toggled: 'Off',
          scheme: 'ArmControls',
          controller: 'Xbox 3',
          interval: null,
        },
        ControlMultipliers: {
          toggled: 'Off',
          scheme: 'ControlMultipliers',
          controller: 'Xbox 4',
          interval: null,
        },
      },
      controlPreviewModal: false,
      image: xboxController,
    };
    this.schemeChange = this.schemeChange.bind(this);
    // detects if a controller disconnects
    window.addEventListener('gamepaddisconnected', function (e) {
      console.log('Gamepad disconnected from index %d: %s', e.gamepad.index, e.gamepad.id);
    });
  }
  // takes in the controllers scheme and the position in the array of controllers to determin which controller it is

  controllerChange(event: { target: { value: string } }, config: string): void {
    controllerInputs = {};
    let defaultScheme = '';
    let selectedImage = '';
    // determines which scheme to select at default so that the default is not one that otherwise couldnt be used (if there is any available)
    // (such as diagonal drive for xbox controller)
    for (const scheme in CONTROLLERINPUT) {
      if (
        CONTROLLERINPUT[scheme].config === config &&
        event.target.value.indexOf(CONTROLLERINPUT[scheme].controller) >= 0
      ) {
        selectedImage = CONTROLLERINPUT[scheme].controller === 'Xbox' ? xboxController : flightStick;
        defaultScheme = scheme;
        break;
      }
    }
    this.setState(
      (prevState) => ({
        functionality: {
          ...prevState.functionality,
          [config]: {
            ...prevState.functionality[config],
            controller: event.target.value,
            scheme: defaultScheme,
            interval: clearInterval(prevState.functionality[config].interval),
          },
        },
        image: selectedImage,
      }),
      // this is a callback for when the setState finished updating it clears the set interval,
      // so that after setState is finished it is able to create a new interval and assign it
      () => {
        if (this.state.functionality[config].toggled === 'On') {
          this.setState((prevState) => ({
            functionality: {
              ...prevState.functionality,
              [config]: {
                ...prevState.functionality[config],
                interval: controller(
                  prevState.functionality[config].scheme,
                  prevState.functionality[config].controller
                ),
              },
            },
            image: selectedImage,
          }));
        }
      }
    );
  }

  schemeChange(event: { target: { value: string } }, config: string): void {
    controllerInputs = {};
    this.setState(
      (prevState) => ({
        functionality: {
          ...prevState.functionality,
          [config]: {
            ...prevState.functionality[config],
            scheme: event.target.value,
            interval: clearInterval(prevState.functionality[config].interval),
          },
        },
      }),
      () => {
        if (this.state.functionality[config].toggled === 'On') {
          this.setState((prevState) => ({
            functionality: {
              ...prevState.functionality,
              [config]: {
                ...prevState.functionality[config],
                interval: controller(
                  prevState.functionality[config].scheme,
                  prevState.functionality[config].controller
                ),
              },
            },
          }));
        }
      }
    );
  }

  buttonToggle(config: string): void {
    controllerInputs = {};
    // if toggling on
    if (this.state.functionality[config].toggled === 'Off') {
      this.setState((prevState) => ({
        functionality: {
          ...prevState.functionality,
          [config]: {
            ...prevState.functionality[config],
            toggled: 'On',
            interval: controller(prevState.functionality[config].scheme, prevState.functionality[config].controller),
          },
        },
      }));
    }
    // if toggling off
    else if (this.state.functionality[config].toggled === 'On') {
      this.setState((prevState) => ({
        functionality: {
          ...prevState.functionality,
          [config]: {
            ...prevState.functionality[config],
            toggled: 'Off',
            interval: clearInterval(prevState.functionality[config].interval),
          },
        },
      }));
    }
  }

  // now operates as a windowed modal
  controlLayoutPreview(): JSX.Element {
    return (
      <div style={{ ...modal, ...row }}>
        {Object.keys(this.state.functionality).map((selectedController) => {
          return (
            <div key={selectedController}>
              {this.state.functionality[selectedController].toggled === 'On' &&
              CONTROLLERINPUT[this.state.functionality[selectedController].scheme] ? (
                <div style={{ ...row, alignItems: 'center', alignSelf: 'auto' }}>
                  <img src={this.state.image} alt={selectedController} />
                  <div style={{ flexDirection: 'column' }}>
                    <div>
                      {selectedController} controlled with {this.state.functionality[selectedController].controller}:
                    </div>
                    {Object.keys(CONTROLLERINPUT[this.state.functionality[selectedController].scheme].bindings).map(
                      (bind) => {
                        return (
                          <div key={bind}>
                            {bind}:{' '}
                            {CONTROLLERINPUT[this.state.functionality[selectedController].scheme].bindings[bind].button
                              ? CONTROLLERINPUT[this.state.functionality[selectedController].scheme].bindings[bind]
                                  .button
                              : CONTROLLERINPUT[this.state.functionality[selectedController].scheme].bindings
                                  .buttonIndex}
                          </div>
                        );
                      }
                    )}
                  </div>
                  <button
                    type="button"
                    onClick={() =>
                      this.setState((prevState) => ({ controlPreviewModal: !prevState.controlPreviewModal }))
                    }
                  >
                    back
                  </button>
                </div>
              ) : null}
            </div>
          );
        })}
      </div>
    );
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Control Scheme</div>
        <div style={container}>
          {this.props.configs.map((config) => {
            return (
              <div key={config} style={row}>
                <div style={readoutDisplay}>{config}</div>
                <select
                  value={this.state.functionality[config].controller}
                  onChange={(e) => this.controllerChange(e, config)}
                  style={{ flex: 1 }}
                >
                  {['Xbox 1', 'Xbox 2', 'Xbox 3', 'Xbox 4', 'Flight Stick'].map((controllerSelect) => {
                    return (
                      <option value={controllerSelect} key={controllerSelect}>
                        {controllerSelect}
                      </option>
                    );
                  })}
                </select>
                <select
                  value={this.state.functionality[config].scheme}
                  onChange={(e) => this.schemeChange(e, config)}
                  style={{ flex: 1 }}
                >
                  {Object.keys(CONTROLLERINPUT).map((scheme) => {
                    if (
                      CONTROLLERINPUT[scheme].config === config &&
                      this.state.functionality[config].controller.indexOf(CONTROLLERINPUT[scheme].controller) >= 0
                    )
                      return (
                        <option value={scheme} key={scheme}>
                          {scheme}
                        </option>
                      );
                    return null;
                  })}
                </select>
                <button style={{ zIndex: 1 }} type="button" onClick={() => this.buttonToggle(config)}>
                  {this.state.functionality[config].toggled}
                </button>
              </div>
            );
          })}
          <button
            type="button"
            onClick={() => this.setState((prevState) => ({ controlPreviewModal: !prevState.controlPreviewModal }))}
            style={{ width: '120px', alignSelf: 'center' }}
          >
            {this.state.controlPreviewModal ? 'Hide Controls' : 'Show Controls'}
          </button>
          {this.state.controlPreviewModal ? this.controlLayoutPreview() : null}
        </div>
      </div>
    );
  }
}

export default ControlScheme;
