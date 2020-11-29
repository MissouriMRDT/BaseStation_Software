import React, { Component } from "react"
import CustomPackets from "./components/CustomPackets"

interface IProps {}

interface IState {}

class RoverOverviewOfNetwork extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div>
        <CustomPackets />
      </div>
    )
  }
}

export default RoverOverviewOfNetwork
