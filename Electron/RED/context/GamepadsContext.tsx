import React, { createContext, useEffect, useRef, useState } from "react";
import useInterval from "../hooks/useInterval";
const GamepadsContext = createContext();

const GamepadsProvider = ({ children }) => {
  const [gamepads, setGamepads] = useState({});
  const requestRef = useRef();

  var haveEvents = "ongamepadconnected" in window;

  const addGamepad = (gamepad) => {
    // console.log(
    //   "Gamepad connected at index %d: %s. %d buttons, %d axes.",
    //   gamepad.index,
    //   gamepad.id,
    //   gamepad.buttons.length,
    //   gamepad.axes.length
    // );

    setGamepads({
      ...gamepads,
      [gamepad.index]: {
        buttons: gamepad.buttons,
        id: gamepad.id,
        axes: gamepad.axes
      }
    });

    // Handle controller input before render
    // requestAnimationFrame(updateStatus);
  };

  /**
   * Adds game controllers during connection event listener
   * @param {object} e
   */
  const connectGamepadHandler = (e) => {
    addGamepad(e.gamepad);
    // console.log("connecting gamepads", e, e.gamepad);
  };

  /**
   * Finds all gamepads and adds them to context
   */
  const scanGamepads = () => {
    // Grab gamepads from browser API
    var detectedGamepads = navigator.getGamepads
      ? navigator.getGamepads()
      : navigator.webkitGetGamepads
      ? navigator.webkitGetGamepads()
      : [];

    // Loop through all detected controllers and add if not already in state
    for (var i = 0; i < detectedGamepads.length; i++) {
      if (detectedGamepads[i]) {
        addGamepad(detectedGamepads[i]);
      }
    }
  };

  // Add event listener for gamepad connecting
  useEffect(() => {
    window.addEventListener("gamepadconnected", connectGamepadHandler);

    return window.removeEventListener(
      "gamepadconnected",
      connectGamepadHandler
    );
  });

  // Update each gamepad's status on each "tick"
  const animate = (time) => {
    if (!haveEvents) scanGamepads();
    requestRef.current = requestAnimationFrame(animate);
  };

  useEffect(() => {
    requestRef.current = requestAnimationFrame(animate);
    return () => cancelAnimationFrame(requestRef.current);
  }, []);

  // Check for new gamepads regularly
  useInterval(() => {
    if (!haveEvents) scanGamepads();
  }, 1000);

  // console.log("component rendering", gamepads);

  return (
    <GamepadsContext.Provider value={{ gamepads, setGamepads }}>
      {children}
    </GamepadsContext.Provider>
  );
};

export { GamepadsProvider, GamepadsContext };
