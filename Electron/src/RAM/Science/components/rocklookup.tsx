import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"


const h1Style: CSS.Properties = {}

const mainContainer: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  margin: "5px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}

const FILEPATH = path.join(__dirname, "//assets/rockList.json")

let rockList: any

if (fs.existsSync(FILEPATH)) {
  rockList = JSON.parse(fs.readFileSync(FILEPATH).toString())
}


interface IProps {}

interface IState {
  startobj: { any: any }
  rocks: {}
}

class RockLookUp extends Component<IState, IProps> {
  constructor(props: any) {
    super(props)
    this.state = {
      startobj: { 0: 0 },
      rocks: rockList,
    }
  }

  //search()

  render(): JSX.Element {
    return (
      <div>
        <div style={mainContainer}>
          <h1 style={h1Style}>RockLookUp</h1>
          <button type={"button"} onClick={() => null} />
          <button style={{color:'red', backgroundColor:'black'}}>Search</button>
        </div>
      </div>
    )
  }
}
