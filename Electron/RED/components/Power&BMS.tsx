/* eslint-disable prettier/prettier */
import React, { Component } from "react"
import CSS from "csstype"
import { PassThrough } from "stream"
// import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontSize: "12px",
}
const grandContainer: CSS.Properties = {
  fontFamily: "arial",
  display: "flex",
  flexDirection: "column",
  // width: "placeholder",
  borderWidth: "thick",
  borderColor: "rgb(153, 0, 0)",// MRDT red
  borderStyle: "solid",
  // gridRowStart: "placeholder",
}
const roAndBtnContainer: CSS.Properties = {
  // stands for "Readout and Button Container"; shortened for sanity.
  // intended to only contain the button and readout combo at top of feature.
  // Is a flexbox with each flex item containing columnated elements,
  // and location specifics will be determined by the "viewmodel"
  display: "grid",
  width: "auto",
  gridTemplateColumns: "75px 1fr 75px 1fr",
}
const readoutDisplays: CSS.Properties = {
  fontSize: "10px",
  color: "#30ff00",// green
  justifyContent: "space-between",
}
const label: CSS.Properties = {
  fontSize: "10px",
}
const btnArray: CSS.Properties = {
  // potential container for all the bus buttons and "reboot," "start log," etc.
  // not sure how to organize containers and objects. Bring up during work hours
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
  justifyContent: "center",
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
  color: "#ff1100",// red
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

class Power extends Component<IProps, Istate> {
  constructor(props: any) {
    super(props)
    this.state = {
      motorLF: 0.00,
      motorLM: 0.00,
      motorLB: 0.00,
      motorRF: 0.00,
      motorRM: 0.00,
      motorRB: 0.00,
      motorExtra: 0.00,
      auxiliary: 0.00,
      comms: 0.00,
      logic: 0.00,
      actuation: 0.00,
      twelveVNoard: 0.00,
      batteryTemp: 0.00,
      ttlPackVolt: 0.00,
      ttlPackCurrent: 0.00,
    }
    // rovecomm.on?
  }
  // eslint-disable-next-line @typescript-eslint/lines-between-class-members
  renderReadOutAmps(): JSX.Element {
    
  }

  renderReadOutVolts()

  render(): JSX.Element {
    return (
      <div>
        <div style={h1Style}>POWER AND BMS</div>
        <div style={grandContainer}>
          <div style={roAndBtnContainer}>
            <button style={buttonStyle}></button>
          </div>
          <div style={btnArray}></div>
          <span>----------------------------------------</span>
          <div style={totalPackContainer}></div>
          <div style={cellReadoutContainer}>
            {[
              { title: "Cell 1", value: this.state.cell1 },
            ]}
          </div>
        </div>
      </div>
    )
  }
}