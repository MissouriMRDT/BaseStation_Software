import React, { Component } from "react"
import CSS from "csstype"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  flexGrow: 1,
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}

interface IProps {}

interface IState {}

class Autonomy extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return <div style={column}>Autonomy</div>
  }
}
export default Autonomy
