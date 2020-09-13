import React from 'react'
import { ConnectedRouter } from 'connected-react-router'
import { hot } from 'react-hot-loader/root'
import { History } from 'history'
import Routes from '../Routes'

type Props = {
  history: History
}

const Root = ({ history }: Props) => (
  <ConnectedRouter history={history}>
    <Routes />
  </ConnectedRouter>
)

export default hot(Root)
