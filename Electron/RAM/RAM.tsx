import React, { Component } from "react"
import ControlMultipliers from "./components/ControlMultipliers"

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
        <ControlMultipliers />
      </div>
    )
  }
}

export default RAM
