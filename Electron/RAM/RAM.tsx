import React, { Component } from "react"
import IK from "./components/IK"

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
        <IK />
      </div>
    )
  }
}

export default RAM
