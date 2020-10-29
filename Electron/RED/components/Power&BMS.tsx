import React, { Component } from "react"
import CSS from "csstype"
// import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const roAndBtnContainer: CSS.Properties = {
  // stands for "Readout and Button Container"; shortened for sanity.
  // Is a flexbox with each flex item containing columnated elements,
  // and location specifics will be determined by the "viewmodel"
  display: "flex",
  width: "auto",
  borderWidth: "thick",
  borderColor: "#ff5938",
  borderStyle: "solid",
  flexWrap: "nowrap",
  // next line is experimentation to get readout containers to scale
  // with window width like in the c# software
  flexBasis: "auto",
}
