import React, { Component } from "react"

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
        <h1>This is RAM</h1>
      </div>
    )
  }
}

export default RAM
