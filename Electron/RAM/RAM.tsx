import React, { Component } from "react"
import SensorData from "./components/SensorData"

interface IProps {}

interface IState {}

class RAM extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div>
        <SensorData />
      </div>
    )
  }
}

export default RAM
