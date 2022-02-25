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
  width: "98%",
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
  width: "33%",
}

const tagList: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexWrap: "wrap",
}

const tagStyle: CSS.Properties = {
  border: "1px black",
}

const ROCKPATH = path.join(__dirname, "../assets/rockLookupAssets/RockDatabase.tsv")
/**
 * Read the rock definitions from the tsv file
 */
function fillFrom_R_Database(): Rocks[] {
  if (fs.existsSync(ROCKPATH)) {
    let rockTable: Rocks[] = []
    let filePull = fs
      .readFileSync(ROCKPATH)
      .toString()
      .split(/\r\n|\n\r|\n|\r/)
    filePull = filePull.slice(1, filePull.length) // gets rid of the sheet labels
    filePull.forEach(textLine => {
      const lineArr = textLine.split(/\t/)
      rockTable.push({
        name: lineArr[0],
        ReqMinerals: lineArr[1].split("; "),
        ComMinerals: lineArr[2].split("; "),
        description: lineArr[3],
      })
    })
    return rockTable
  } else {
    console.error("Failed loading Rock Table. File might not exist. Look in " + ROCKPATH)
    return []
  }
}

const MINPATH = path.join(__dirname, "../assets/rockLookupAssets/MineralDatabase.tsv")

function fillFrom_M_Database(): Minerals[] {
  if (fs.existsSync(MINPATH)) {
    let mineralTable: Minerals[] = []
    let filePull = fs
      .readFileSync(MINPATH)
      .toString()
      .split(/\r\n|\n\r|\n|\r/)
    filePull = filePull.slice(1, filePull.length) // gets rid of the sheet labels
    filePull.forEach(textLine => {
      const lineArr = textLine.split(/\t/)
      mineralTable.push({
        name: lineArr[0],
        forms: lineArr[1].split("; "),
        cleaveAndLuster: lineArr[2].split("; "),
        colors: lineArr[3].split("; "),
      })
    })
    return mineralTable
  } else {
    console.error("Failed loading Mineral Table. File might not exist. Look in " + MINPATH)
    return []
  }
}

const MINARR: Minerals[] = fillFrom_M_Database()
const ROCKARR: Rocks[] = fillFrom_R_Database()

function populateColors(): string[] {
  let colorList: string[] = []
  MINARR.map(mineral => {
    mineral.colors.map(color => {
      colorList.push(color)
    })
  })
  return [...new Set(colorList)]
}

function populateForms(): string[] {
  let formList: string[] = []
  MINARR.map(mineral => {
    mineral.forms.map(form => {
      formList.push(form)
    })
  })
  return [...new Set(formList)]
}

function populateCleaves(): string[] {
  let cleaveList: string[] = []
  MINARR.map(mineral => {
    mineral.cleaveAndLuster.map(cleave => {
      cleaveList.push(cleave)
    })
  })
  return [...new Set(cleaveList)]
}

interface Minerals {
  name: string
  forms: string[]
  cleaveAndLuster: string[]
  colors: string[]
}

interface Rocks {
  name: string
  ReqMinerals: string[]
  ComMinerals: string[]
  description: string
}

interface Output {
  Rock: Rocks
  ConfidenceScore: number
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  availColors: string[]
  /** Selected Colors */
  s_Colors: string[]
  availForms: string[]
  /** Selected Forms */
  s_Forms: string[]
  availCleave: string[]
  /** Selected Cleave */
  s_Cleave: string[]
  searchField: string
  outputArr: Output[]
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
/*
  compareSelections() {
    let { availColors, s_Colors, availForms, s_Forms, availCleave, s_Cleave } = this.state
    let numHits = 0
    let selectedMins = []
    MINARR.map(mineral => {
      s_Colors.map(color => {
        if (mineral.colors.indexOf(color) !== -1) {
          numHits++
        }
      })
      s_Forms.map(form => {
        if (mineral.forms.indexOf(form) !== -1) {
          numHits++
        }
      })
      s_Cleave.map(cleave => {
        if (mineral.cleaveAndLuster.indexOf(cleave) !== -1) {
          numHits++
        }
      })
      if (numHits) {
      }
    })
  }
*/
  handleFeatureSelect(event: any): void {
    if (event.target.id === "ColorList") {
      let { s_Colors, availColors } = this.state
      availColors.splice(availColors.indexOf(event.target.value), 1)
      s_Colors.push(event.target.value)
      this.setState({ s_Colors, availColors })
    }
    if (event.target.id === "CleaveList") {
      let { s_Cleave, availCleave } = this.state
      availCleave.splice(availCleave.indexOf(event.target.value), 1)
      s_Cleave.push(event.target.value)
      this.setState({ s_Cleave, availCleave })
    }
    if (event.target.id === "FormList") {
      let { s_Forms, availForms } = this.state
      availForms.splice(availForms.indexOf(event.target.value), 1)
      s_Forms.push(event.target.value)
      this.setState({ s_Forms, availForms })
    }
  }

  handleFeatureSubmit(event: any) {
    event.preventDefault()
  }

  handleInputChange(event: any) {
    event.preventDefault()
    this.setState({ searchField: event.target.value })
  }

  clearSelectedFeatures(): void {
    let { availColors, availCleave, availForms } = this.state
    let { s_Colors, s_Cleave, s_Forms } = this.state
    availColors.push.apply(availColors, s_Colors)
    availCleave.push.apply(availCleave, s_Cleave)
    availForms.push.apply(availForms, s_Forms)
    this.setState({
      s_Colors: [],
      s_Cleave: [],
      s_Forms: [],
      availColors,
      availCleave,
      availForms,
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Rock Lookup</div>
        <div style={container}>
          <div style={row}>
            <form
              onSubmit={
                () => {
                  this.handleFeatureSubmit
                } /* TODO */
              }
            >
              <input
                type="text"
                name="newTag"
                onChange={e => {
                  this.handleInputChange(e)
                }}
              />
              <input type="submit" value={this.state.searchField} />
            </form>
          </div>
          <div style={row}>
            <div style={featureColumn}>
              <p>Colors</p>
              <select
                size={10}
                id={"ColorList"}
                onChange={event => {
                  this.handleFeatureSelect(event)
                }}
              >
                {this.state.availColors.map((colorName: string) => {
                  return <option value={colorName}>{colorName}</option>
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <p>Cleavage</p>
              <select
                size={10}
                id={"CleaveList"}
                onChange={event => {
                  this.handleFeatureSelect(event)
                }}
              >
                {this.state.availCleave.map((cleaveName: string) => {
                  return <option value={cleaveName}>{cleaveName}</option>
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <p>Forms</p>
              <select
                size={10}
                id={"FormList"}
                onChange={event => {
                  this.handleFeatureSelect(event)
                }}
              >
                {this.state.availForms.map((formName: string) => {
                  return <option value={formName}>{formName}</option>
                })}
              </select>
            </div>
          </div>
          <div style={column}>
            <div style={tagList}>
              {[...this.state.s_Colors, ...this.state.s_Forms, ...this.state.s_Cleave].map((tag: string) => {
                return (
                  <div style={tagStyle}>
                    <p>{tag}</p>
                    <p onClick={() => {} /* TODO */}>x</p>
                  </div>
                )
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
