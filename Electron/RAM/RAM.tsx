import React, { Component } from "react"
import Geneva from "./components/Geneva"

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
        <Geneva />
      </div>
    )
  }
}

export default RAM
