import React, { Component } from "react"
import Angular from "./components/angular"

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
        <Angular />
      </div>
    )
  }
}

export default RAM
