import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"

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
  display: "flex",
  flexDirection: "row",
  border: "1px solid black",
  margin: "5px",
  lineHeight: ".1",
  padding: "0 5px 0 5px",
}

enum FeatureType {
  COLOR,
  CLEAVE,
  FORM,
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
  colorList.sort()
  return [...new Set(colorList)]
}

const COLORMASTER: string[] = populateColors()

function populateForms(): string[] {
  let formList: string[] = []
  MINARR.map(mineral => {
    mineral.forms.map(form => {
      formList.push(form)
    })
  })
  formList.sort()
  return [...new Set(formList)]
}

const FORMMASTER: string[] = populateForms()

function populateCleaves(): string[] {
  let cleaveList: string[] = []
  MINARR.map(mineral => {
    mineral.cleaveAndLuster.map(cleave => {
      cleaveList.push(cleave)
    })
  })
  cleaveList.sort()
  return [...new Set(cleaveList)]
}

const CLEAVEMASTER: string[] = populateCleaves()

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

var outputCompare = (out1: Output, out2: Output) => {
  if (out1.ConfidenceScore > out2.ConfidenceScore) {
    return -1
  }
  if (out1.ConfidenceScore < out2.ConfidenceScore) {
    return 1
  }
  return 0
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
      availColors: JSON.parse(JSON.stringify(COLORMASTER)),
      s_Colors: [],
      availForms: JSON.parse(JSON.stringify(FORMMASTER)),
      s_Forms: [],
      availCleave: JSON.parse(JSON.stringify(CLEAVEMASTER)),
      s_Cleave: [],
      searchField: "WIP; Feature frozen",
      outputArr: [],
      selectedOutput: 0,
    }
    this.handleFeatureSubmit = this.handleFeatureSubmit.bind(this)
    this.handleInputChange = this.handleInputChange.bind(this)
    this.handleFeatureSelect = this.handleFeatureSelect.bind(this)
    this.compareSelections = this.compareSelections.bind(this)
  }

  reloadOptions(): void {
    this.setState(
      {
        availCleave: JSON.parse(JSON.stringify(CLEAVEMASTER)),
        availForms: JSON.parse(JSON.stringify(FORMMASTER)),
        availColors: JSON.parse(JSON.stringify(COLORMASTER)),
      },
      () => this.compareSelections()
    )
  }

  compareSelections() {
    let { s_Colors, s_Forms, s_Cleave } = this.state
    let possRock: Output[] = []
    let selectedMins: Minerals[] = []
    if (s_Colors.length >= 0 || s_Forms.length >= 0 || s_Cleave.length >= 0) {
      MINARR.forEach(mineral => {
        let hit: boolean = false
        s_Colors.forEach(color => {
          if (mineral.colors.indexOf(color) >= 0) {
            hit = true
          }
        })
        s_Forms.forEach(form => {
          if (mineral.forms.indexOf(form) >= 0) {
            hit = true
          }
        })
        s_Cleave.forEach(cleave => {
          if (mineral.cleaveAndLuster.indexOf(cleave) >= 0) {
            hit = true
          }
        })
        if (hit) {
          selectedMins.push(mineral)
        }
      })
      selectedMins = [...new Set(selectedMins)]
      this.cullImpossibles(selectedMins)
    }
    ROCKARR.forEach(rock => {
      let confScore: number = 0
      selectedMins.forEach(mineral => {
        if (rock.ComMinerals.indexOf(mineral.name) >= 0) {
          confScore += 1
        }
        if (rock.ReqMinerals.indexOf(mineral.name) >= 0) {
          confScore += 2
        }
      })
      if (confScore > 0) {
        possRock.push({ Rock: rock, ConfidenceScore: confScore })
      }
    })
    possRock = [...new Set(possRock)]
    possRock.sort(outputCompare)
    this.setState({ outputArr: possRock })
  }

  cullImpossibles(possibleMins: Minerals[]): void {
    let { availCleave, availColors, availForms } = this.state

    availCleave.forEach(cleave => {
      let isPoss: number = 0
      possibleMins.forEach(mineral => {
        if (mineral.cleaveAndLuster.indexOf(cleave) >= 0) {
          isPoss += 1
        }
      })
      if (isPoss === 0) {
        availCleave.splice(availCleave.indexOf(cleave), 1)
      }
    })
    availColors.forEach(color => {
      let isPoss: number = 0
      possibleMins.forEach(mineral => {
        if (mineral.colors.indexOf(color) >= 0) {
          isPoss += 1
        }
      })
      if (isPoss === 0) {
        availColors.splice(availColors.indexOf(color), 1)
      }
    })
    availForms.forEach(form => {
      let isPoss: number = 0
      possibleMins.forEach(mineral => {
        if (mineral.forms.indexOf(form) >= 0) {
          isPoss += 1
        }
      })
      if (isPoss === 0) {
        availForms.splice(availForms.indexOf(form), 1)
      }
    })
    this.setState({ availCleave, availColors, availForms })
  }

  handleFeatureSelect(event: any): void {
    if (event.target.id === "ColorList") {
      let { s_Colors, availColors } = this.state
      availColors.splice(availColors.indexOf(event.target.value), 1)
      s_Colors.push(event.target.value)
      this.setState({ s_Colors, availColors }, () => this.compareSelections())
    }
    if (event.target.id === "CleaveList") {
      let { s_Cleave, availCleave } = this.state
      availCleave.splice(availCleave.indexOf(event.target.value), 1)
      s_Cleave.push(event.target.value)
      this.setState({ s_Cleave, availCleave }, () => this.compareSelections())
    }
    if (event.target.id === "FormList") {
      let { s_Forms, availForms } = this.state
      availForms.splice(availForms.indexOf(event.target.value), 1)
      s_Forms.push(event.target.value)
      this.setState({ s_Forms, availForms }, () => this.compareSelections())
    }
  }

  handleFeatureSubmit(event: any) {
    event.preventDefault()
  }

  handleInputChange(event: any) {
    event.preventDefault()
    this.setState({ searchField: event.target.value })
  }

  removeSelectedFeature(type: FeatureType, index: number): void {
    switch (type) {
      case FeatureType.COLOR:
        let { availColors, s_Colors } = this.state
        availColors.push(s_Colors[index])
        availColors.sort()
        s_Colors.splice(index, 1)
        this.setState({ s_Colors, availColors }, () => this.reloadOptions())
        break
      case FeatureType.CLEAVE:
        let { availCleave, s_Cleave } = this.state
        availCleave.push(s_Cleave[index])
        availCleave.sort()
        s_Cleave.splice(index, 1)
        this.setState({ s_Cleave, availCleave }, () => this.reloadOptions())
        break
      case FeatureType.FORM:
        let { availForms, s_Forms } = this.state
        availForms.push(s_Forms[index])
        availForms.sort()
        s_Forms.splice(index, 1)
        this.setState({ s_Forms, availForms }, () => this.reloadOptions())
        break
    }
  }

  clearSelectedFeatures(): void {
    this.setState(
      {
        s_Colors: [],
        s_Cleave: [],
        s_Forms: [],
      },
      () => this.reloadOptions()
    )
  }

  possibleRocks(): JSX.Element | void {
    if (this.state.outputArr.length > 0) {
      return (
        <div>
          {this.state.outputArr[this.state.selectedOutput].Rock.name}:
          {this.state.outputArr[this.state.selectedOutput].Rock.description}
        </div>
      )
    }
    /*this.state.selectedOutput > 0 && this.state.outputArr.length > 0
      ? () => {
          return (
            <button type={"button"} onClick={() => this.setState({ selectedOutput: this.state.selectedOutput - 1 })}>
              ^^^
            </button>
          )
        }
      : null*/
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Rock Lookup</div>
        <div style={container}>
          {/*<div style={row}>
            <form onSubmit={e => this.handleFeatureSubmit(e)}>
              <input
                type="text"
                name="newTag"
                onChange={e => {
                  this.handleInputChange(e)
                }}
              />
              <input type="submit" value={this.state.searchField} />
            </form>
              </div>*/}
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
              {this.state.s_Colors.map((tag: string, index: number) => {
                return (
                  <div style={tagStyle}>
                    <p>{tag}</p>
                    <p
                      onClick={() => this.removeSelectedFeature(FeatureType.COLOR, index)}
                      style={{ marginLeft: "10px", color: "red" }}
                    >
                      x
                    </p>
                  </div>
                )
              })}
              {this.state.s_Cleave.map((tag: string, index: number) => {
                return (
                  <div style={tagStyle}>
                    <p>{tag}</p>
                    <p
                      onClick={() => this.removeSelectedFeature(FeatureType.CLEAVE, index)}
                      style={{ marginLeft: "10px", color: "red" }}
                    >
                      x
                    </p>
                  </div>
                )
              })}
              {this.state.s_Forms.map((tag: string, index: number) => {
                return (
                  <div style={tagStyle}>
                    <p>{tag}</p>
                    <p
                      onClick={() => this.removeSelectedFeature(FeatureType.FORM, index)}
                      style={{ marginLeft: "10px", color: "red" }}
                    >
                      x
                    </p>
                  </div>
                )
              })}
            </div>
            <div style={column}>
              <button onClick={() => this.clearSelectedFeatures()}>Clear</button>
            </div>
          </div>

          <div style={column}>
            <p>Possible Minerals:</p>
            <div style={{ display: "flex", flexWrap: "wrap", flexDirection: "column" }}>
              {/*this.state.outputArr.sort(outputCompare).map((output: Output) => {
                return (
                  <div title={output.Rock.description} style={{ ...column }}>
                    <p>
                      {output.Rock.name} - {output.ConfidenceScore}
                    </p>
                  </div>
                )
              })*/}

              <button
                type={"button"}
                onClick={() =>
                  this.setState({
                    selectedOutput:
                      this.state.selectedOutput > 0 ? this.state.selectedOutput - 1 : this.state.selectedOutput,
                  })
                }
              >
                ^^^
              </button>
              {this.possibleRocks()}
              <button type={"button"} onClick={() => this.setState({ selectedOutput: this.state.selectedOutput + 1 })}>
                vvv
              </button>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default RockLookUp
