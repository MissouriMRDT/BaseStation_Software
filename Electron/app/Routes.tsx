/* eslint react/jsx-props-no-spreading: off */
import React from 'react'
import { Switch, Route } from 'react-router-dom'
import routes from './constants/routes.json'
import App from './containers/App'
import Home from './components/Home'

export default function Routes() {
  return (
    <App>
      <Switch>
        <Route path={routes.HOME} component={Home} />
      </Switch>
    </App>
  )
}
