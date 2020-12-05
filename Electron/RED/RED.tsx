import React, { Fragment } from "react"
import { render } from "react-dom"
import { AppContainer as ReactHotAppContainer } from "react-hot-loader"
import GPS from "./components/GPS"
import ControlScheme from "./components/ControlScheme"
import GamepadList from "./components/GamepadList"
//import ControlCenter from "./ControlCenter"

const AppContainer = process.env.PLAIN_HMR ? Fragment : ReactHotAppContainer

document.addEventListener("DOMContentLoaded", () => {
  // eslint-disable-next-line global-require
  // const Root = require('./containers/Root').default
  render(
    <AppContainer>
      <div>
        <GPS />
        <ControlScheme />
        <GamepadList />
      </div>
    </AppContainer>,
    document.getElementById("root")
  )
})
