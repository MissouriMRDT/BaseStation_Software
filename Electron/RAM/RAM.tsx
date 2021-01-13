import React, { Component } from "react"
import SensorGraphs from "./components/SensorGraphs"

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
        <SensorGraphs />
      </div>
    )
  }
}

export default RAM
