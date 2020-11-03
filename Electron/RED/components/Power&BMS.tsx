/* eslint-disable prettier/prettier */
import React, { Component } from "react"
import CSS from "csstype"
import { PassThrough } from "stream"
// import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
// significant potential there will be a need for a mass container. If so, it'll
// be easy to organize all the containers in a 1 column centered grid.
const roAndBtnContainer: CSS.Properties = {
  // stands for "Readout and Button Container"; shortened for sanity.
  // intended to only contain the button and readout combo at top of feature.
  // Is a flexbox with each flex item containing columnated elements,
  // and location specifics will be determined by the "viewmodel"
  display: "flex",
  fontFamily: "arial",
  width: "auto",
  // next three lines will need to be moved to big container if one is made.
  borderWidth: "thick",
  borderColor: "#ff5938",
  borderStyle: "solid",
  flexWrap: "nowrap",
  // next line is experimentation to get readout containers to scale
  // with window width like in the c# software
  flexBasis: "auto",
}
const readoutDisplays: CSS.Properties = {
  fontSize: "10px",
  // color: "#30ff00", green; intended for container. Relocation likely
  // needs some way to position label on left and data on right
}
const btnArray: CSS.Properties = {
  // potential container for all the bus buttons and "reboot," "start log," etc.
  // not sure how to organize containers and objects. Bring up during work hours
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
  justifyContent: "center",
  fontFamily: "arial",
}
const buttonStyle: CSS.Properties = {
  // intended to be used both for readout buttons and bus array buttons
  fontSize: "9px",
  // textAlign: "center",
  width: "auto",
  // color: "#f5f5f5", very light gray; intended to color the button, not text
}
const totalPackContainer: CSS.Properties = {
  display: "grid",
  justifyContent: "space-evenly",
  gridTemplateColumns: "500px 500px", // placeholder number
  // color: "#30ff00", green; intended for "current"
  // color: "#ff1100", red; intended for "voltage"
}
const cellReadoutContainer: CSS.Properties = {
  // readoutDisplays will be used for text in this container
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
  // will likely need a way to keep elements confined to set columns
}

interface IProps {}

interface IState {
  // btnClicked: boolean
  motorLF: number
  motorLM: number
  motorLB: number
  motorRF: number
  motorRM: number
  motorRB: number
  motorExtra: number
  auxiliary: number
  comms: number
  logic: number
  actuation: number
  twelveVBoard: number
  batteryTemp: number
  ttlPackVolt: number
  ttlPackCurrent: number
  // unsure if each individual cell needs to be declared here.
  // same with bus button states
}

/*
class Power extends Component<IProps, Istate> {
  constructor(props: any) {
    super(props)
    this.state = {
      pass
    }
  }
  render(): JSX.Element {
    pass
  }
}
*/