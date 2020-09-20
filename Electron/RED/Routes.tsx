/* eslint react/jsx-props-no-spreading: off */
import React from "react"
import { Switch, Route } from "react-router-dom"
import routes from "./constants/routes.json"
import GPS from "./components/GPS"

export default function Routes() {
  return (
    <Switch>
      <Route path={routes.HOME} component={GPS} />
    </Switch>
  )
}
