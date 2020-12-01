import React, { useRef, useState } from "react";
import useGamepads from "../hooks/useGamepads";
import GamepadSvg from "./GamepadSvg";
import { isEqual } from "lodash";
import fastDeepEqual from "fast-deep-equal";

export default function GamepadController() {
  const [gamepads, setGamepads] = useState({});
  // const gamepads = useRef({});
  useGamepads((newGamepads) => {
    // Detect changes, if so, re-render component
    // Go through buttons, find any pressed
    // Not good because it leaves buttons pressed, and doesn't
    // handle when button is "unpressed"
    // const buttonPressed =
    //   gamepads[0].buttons.filter((button) => button.pressed).length > 0;

    // Doesn't work, returns false positives, even for less-nested properties
    // const buttonPressed = isEqual(gamepads[0].buttons, newGamepads[0].buttons);

    // Doesn't work, will never equal true
    // const buttonPressed = gamepads === newGamepads

    // Doesn't work, returns false positives, even for less-nested properties
    // const buttonPressed = fastDeepEqual(gamepads[0], newGamepads[0]);
    // const buttonPressed = fastDeepEqual(gamepads[0].buttons, newGamepads[0].buttons);

    // if (buttonPressed) {
    setGamepads(newGamepads);
    // }
  });
  const calcDirectionVertical = (axe) => {
    // Up
    if (axe < -0.2) {
      return "up";
    }
    // Down
    if (axe > 0.2) {
      return "down";
    }
  };

  const calcDirectionHorizontal = (axe) => {
    // Left
    if (axe < -0.2) {
      return "left";
    }
    // Right
    if (axe > 0.2) {
      return "right";
    }
  };
  // console.log([
  //   calcDirectionHorizontal(gamepads[0].axes[0]),
  //   calcDirectionVertical(gamepads[0].axes[1])
  // ]);
  return (
    <div
      className="Gamepads"
      style={{ position: "fixed", bottom: 0, right: 0 }}
    >
      <small>Gamepads</small>
      {/* {gamepadDisplay} */}
      {gamepads && gamepads[0] && (
        <>
          <GamepadSvg
            directionUp={gamepads[0].buttons[12].pressed}
            directionDown={gamepads[0].buttons[13].pressed}
            directionLeft={gamepads[0].buttons[14].pressed}
            directionRight={gamepads[0].buttons[15].pressed}
            buttonDown={gamepads[0].buttons[0].pressed}
            buttonRight={gamepads[0].buttons[1].pressed}
            buttonLeft={gamepads[0].buttons[2].pressed}
            buttonUp={gamepads[0].buttons[3].pressed}
            select={gamepads[0].buttons[8].pressed}
            start={gamepads[0].buttons[9].pressed}
            analogLeft={
              gamepads[0].axes[0] > 0.3 ||
              gamepads[0].axes[0] < -0.3 ||
              gamepads[0].axes[1] > 0.3 ||
              gamepads[0].axes[1] < -0.3
            }
            analogRight={
              gamepads[0].axes[2] > 0.3 ||
              gamepads[0].axes[2] < -0.3 ||
              gamepads[0].axes[3] > 0.3 ||
              gamepads[0].axes[3] < -0.3
            }
            analogLeftDirection={[
              calcDirectionHorizontal(gamepads[0].axes[0]),
              calcDirectionVertical(gamepads[0].axes[1])
            ]}
            analogRightDirection={[
              calcDirectionHorizontal(gamepads[0].axes[2]),
              calcDirectionVertical(gamepads[0].axes[3])
            ]}
          />
          <h3>Player 1</h3>
        </>
      )}
    </div>
  );
}
