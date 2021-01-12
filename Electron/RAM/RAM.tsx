import React, { Component } from "react"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"

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
        <Spectrometer />
        <SpectrometerViewer />
      </div>
    )
  }
}

export default RAM
