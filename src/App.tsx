import React, { Component } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./App.css";
import ControlCenter from "./RED/ControlCenter";

interface IProps {}
interface IState {
  storedWaypoints: object;
  currentCoords: { lat: number; lon: number };
  ronOpen: boolean;
  ramOpen: boolean;
  ridOpen: boolean;
  fourthHeight: number;
}

class App extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      ronOpen: false,
      ramOpen: false,
      ridOpen: false,
      fourthHeight: 1920 / 4 - 10,
    };
    this.updateWaypoints = this.updateWaypoints.bind(this);
    this.updateCoords = this.updateCoords.bind(this);
  }

  updateWaypoints(storedWaypoints: any): void {
    this.setState({
      storedWaypoints,
    });
  }

  updateCoords(lat: any, lon: any): void {
    this.setState({
      currentCoords: { lat, lon },
    });
  }

  render(): JSX.Element {
    return (
      <>
        <BrowserRouter>
          <Routes>
            <Route
              path="/"
              element={
                <ControlCenter/>
              }
            />
          </Routes>
        </BrowserRouter>
      </>
    );
  }
}

export default App;
