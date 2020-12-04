import React, { Fragment } from "react"
import { render } from "react-dom"
import { AppContainer as ReactHotAppContainer } from "react-hot-loader"
<<<<<<< HEAD
import GPS from "./components/GPS"
import ControlScheme from "./components/ControlScheme"
=======
>>>>>>> 117203f1325991b6a0f82ee5d2c6e540ce69f757
import ControlCenter from "./ControlCenter"

const AppContainer = process.env.PLAIN_HMR ? Fragment : ReactHotAppContainer

document.addEventListener("DOMContentLoaded", () => {
  // eslint-disable-next-line global-require
  // const Root = require('./containers/Root').default
  render(
    <AppContainer>
      <div>
        <GPS />
        <ControlScheme />
      </div>
    </AppContainer>,
    document.getElementById("root")
  )
})
