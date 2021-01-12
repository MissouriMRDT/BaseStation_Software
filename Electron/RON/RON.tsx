import React, { Component } from "react"

interface IProps {}

interface IState {}

class RON extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div>
        <h1>This is RON</h1>
      </div>
    )
  }
}

export default RON
