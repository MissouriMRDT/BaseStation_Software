import React from 'react'
import { Link } from 'react-router-dom'
import routes from '../constants/routes.json'
import './Home.css'

export default function Home(): JSX.Element {
  return (
    <div className="container" data-tid="container">
      <h2>Home</h2>
      <Link to={routes.COUNTER}>to Counter</Link>
    </div>
  )
}
