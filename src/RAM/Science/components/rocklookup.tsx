import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"
import { group } from "console"

const label: CSS.Properties = {
  color: "white",
  fontFamily: "arial",
  fontSize: "16px",
  position: "relative",
  marginTop: "-10px",
  top: "24px",
  left: "3px",
  zIndex: 1,
}

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  justifyContent: "center",
}

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "center",
  margin: "auto",
  //lineHeight: "25px",
  width: "98%"
}

const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  margin: "auto",
  lineHeight: "25px",
}

const featureColumn: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  margin: "auto",
  //lineHeight: "25px",
  width: "33%"
}

const tagList: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexWrap: "wrap",
}

const tagStyle: CSS.Properties = {
  border: "1px black"
}

const featurelist: string[] = [
  "Select",
  "Mineral",
  "Maphic",
  "Felsic",
  "Coarse",
  "Fine",

]

const colorlist: string[] = [
  "Blue",
  "Green",
  "White",
  "Grey",
  "Clear",
  "Orange",
  "Pink",
  "Black",
  "Tan",
  "Yellow",
  "Brown",
  "Red",
  "Purple",
]

/**
 * Read the rock definitions from the json file
 */
function fillFrom_R_Database(): Rocks[] {
  const FILEPATH = path.join(__dirname, "/assets/rockLookupAssets")
  if (fs.existsSync(FILEPATH.concat("/Rock Database"))) {
    let rockList = JSON.parse(fs.readFileSync(FILEPATH).toString()) // TODO
    /** TODO
     * Some kind of string parsing that takes all the values and splits them
     * according to the special characters in the excel sheet
     */
    return rockList
  }
  else {
    console.error("Failed loading Rock Table. File might not exist. Look in " + FILEPATH)
    return []
  }
}

function fillFrom_M_Database(): Minerals[] {
  const FILEPATH = path.join(__dirname, "/assets/rockLookupAssets")
  if (fs.existsSync(FILEPATH.concat("/Mineral Database"))) {
    let mineralList = JSON.parse(fs.readFileSync(FILEPATH).toString()) // TODO
    /** TODO
     * Some kind of string parsing that takes all the values and splits them
     * according to the special characters in the excel sheet
     */
    return mineralList
  }
  else {
    console.error("Failed loading Mineral Table. File might not exist. Look in " + FILEPATH)
    return []
  }
}

function populateColors(): string[] {
  let colorList = []
  MINARR.map(mineral => {
    mineral.colors.map(color => {
      colorList.push(color)
    })
  })
  return colorList
}

function populateForms(): string[] {
  let formList = []
  MINARR.map(mineral => {
    mineral.forms.map(form => {
      formList.push(form)
    })
  })
  return formList
}

function populateCleaves() : string[] {
  let cleaveList = []
  MINARR.map(mineral => {
    mineral.cleaveAndLuster.map(cleave => {
      cleaveList.push(cleave)
    })
  })
  return cleaveList
}

const MINARR: Minerals[] = /*fillFrom_M_Database()*/ [
  {
    name: "oaihg",
    forms: [
      "bigmama",
      "hooba"
    ],
    cleaveAndLuster: [
      "howdyouknow",
      "yourmom",
      "wasgey"
    ],
    colors: [
      "brown",
      "blue",
      "green"
    ]
  },
  {
    name: "oagdggswdsihg",
    forms: [
      "biegdeama",
      "hoobaggadga"
    ],
    cleaveAndLuster: [
      "howagagdyouknow",
      "youlkgbarmom",
      "waaghasgey"
    ],
    colors: [
      "shartruse",
      "cyan",
      "babypoogreen"
    ]
  }
]

const ROCKARR: Rocks[] = /*fillFrom_R_Database()*/ [
  {
    name: "hsveday",
    minerals: [
      "greenday",
      "coldplay",
      "owl city"
    ],
    description: "ITS A BOY"
  },
  {
    name: "blueaboodeeaboodie",
    minerals: [
      "MUZZIE",
      "RIOT",
      "WRLD"
    ],
    description: "I really like listening to really loud drum and bass"
  }
]
interface Minerals {
  name: string,
  forms: string[],
  cleaveAndLuster: string[],
  colors: string[]
}

interface Rocks {
  name: string,
  minerals: string[],
  description: string
}

interface Output {
  Rock: Rocks,
  ConfidenceScore: number
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  availColors: string[],
  /** Selected Colors */
  s_Colors: string[],
  availForms: string[],
  /** Selected Forms */
  s_Forms: string[],
  availCleave: string[],
  /** Selected Cleave */
  s_Cleave: string[],
  searchField: string,
  outputArr: Output[],
  selectedOutput: number
}

class RockLookUp extends Component<IProps, IState> {
  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      availColors: populateColors(),
      s_Colors: [],
      availForms: populateForms(),
      s_Forms: [],
      availCleave: populateCleaves(),
      s_Cleave: [],
      searchField: "WIP; Feature frozen",
      outputArr: [],
      selectedOutput: 0,
    }
    this.handleFeatureSubmit = this.handleFeatureSubmit.bind(this)
    this.handleInputChange = this.handleInputChange.bind(this)
    this.handleFeatureSelect = this.handleFeatureSelect.bind(this)
  }

  handleFeatureSelect(event: any): void {
    console.log(event)
    console.log(event.target.id)
    console.log(event.target.value)
    if (event.target.id === "ColorList") {
      let { s_Colors, availColors } = this.state
      availColors.splice(availColors.indexOf(event.target.value), 1)
      s_Colors.push(event.target.value)
      this.setState({s_Colors, availColors})
    }
    if (event.id === "CleaveList") {
      let { s_Cleave, availCleave } = this.state
      availCleave.splice(availCleave.indexOf(event.target.value), 1)
      s_Cleave.push(event.target.value)
      this.setState({s_Cleave, availCleave})
    }
    if (event.id === "FormList") {
      let { s_Forms, availForms } = this.state
      availForms.splice(availForms.indexOf(event.target.value), 1)
      s_Forms.push(event.target.value)
      this.setState({s_Forms, availForms})
    }
  }

  handleFeatureSubmit(event: any) {
    event.preventDefault()
  }

  handleInputChange(event: any) {
    event.preventDefault()
  }

  clearSelectedFeatures(): void {
    let {availColors, availCleave, availForms} = this.state
    let {s_Colors, s_Cleave, s_Forms} = this.state
    availColors.push.apply(availColors, s_Colors)
    availCleave.push.apply(availCleave, s_Cleave)
    availForms.push.apply(availForms, s_Forms)
    this.setState({
      s_Colors: [], s_Cleave: [], s_Forms: [], availColors, availCleave, availForms
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Rock Lookup</div>
        <div style={container}>
          <div style={row}>
            <form onSubmit={() => {this.handleFeatureSubmit} /* TODO */}>
              <input type="text" name="newTag" onChange={(e) => {this.handleInputChange(e)}}/>
              <input type="submit" value="Add"/>
            </form>
          </div>
          <div style={row}>
            <div style={featureColumn}>
              <p>Colors</p>
              <select size={10} id={"ColorList"} onChange={(event) => {this.handleFeatureSelect(event)}}>
                {this.state.availColors.map((colorName: string) => {
                  return (<option value={colorName}>{colorName}</option>)
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <p>Cleavage</p>
              <select size={10} id={"CleaveList"} onChange={(event) => {this.handleFeatureSelect(event)}}>
                {this.state.availCleave.map((cleaveName: string) => {
                  return (<option value={cleaveName}>{cleaveName}</option>)
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <p>Forms</p>
              <select size={10} id={"FormList"} onChange={(event) => {this.handleFeatureSelect(event)}}>
                {this.state.availForms.map((formName: string) => {
                  return (<option value={formName}>{formName}</option>)
                })}
              </select>
            </div>
          </div>
          <div style={column}>
            <div style={tagList}>
                {[...this.state.s_Colors, ...this.state.s_Forms, ...this.state.s_Cleave].map((tag: string) => {
                  return (<div style={tagStyle}>
                    <p>{tag}</p>
                    <p onClick={()=>{} /* TODO */}>x</p>
                  </div>)
                })}
            </div>
            <div style={column}>
              <button onClick={this.clearSelectedFeatures}>Clear</button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default RockLookUp
