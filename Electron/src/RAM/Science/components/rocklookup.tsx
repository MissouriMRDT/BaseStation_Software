import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"
import { nullLiteral } from "@babel/types"


const h1Style: CSS.Properties = {}

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

const mainContainer: CSS.Properties = {
  display: "flex",
  width:"400px",
  fontFamily: "arial",
  margin: "5px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  height: "40px",
}

const mineralModal: CSS.Properties = {
  position: "absolute",
  zIndex:1,
  border:"2px solid #990000",
  backgroundColor:"white",
  width: "600px"
}

const columns: CSS.Properties = {
  /*float: "left",
  width: "28%",*/
  padding: "10px",
  /*height: "400px",*/
  columnWidth: "180px",
  columnCount: 3,
}

const rows: CSS.Properties = {
  content: "",
  display: "table",
  clear: "both",
}

const dropdown: CSS.Properties = {
  position: "absolute",
  zIndex:1,
  backgroundColor:"white",
}

const button: CSS.Properties = {
  width: "65px",
  border: "none",
  
}

const FILEPATH = path.join(__dirname, "//assets/rockList.json")

let rockList: any

if (fs.existsSync(FILEPATH)) {
  rockList = JSON.parse(fs.readFileSync(FILEPATH).toString())
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

interface MineralTab{
  colors: string[],
  isometric:string[],
  forms:string[],
  Id: number,
}

interface IProps {
}

interface IState {
  feature: number,
  selectedTab: number,
  MineralArr:MineralTab[],
  mineralModalOpen:boolean,
  mineralModalOpen2: boolean,
  featureopen: boolean,
  colorlist: string[],
  colorid: number,
}

class RockLookUp extends Component<IProps, IState> {
  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      selectedTab: -1,
      feature: 0,
      mineralModalOpen: false,
      mineralModalOpen2: false,
      featureopen: false,
      colorid: 0,
      colorlist: [],
      MineralArr: [
        {
          colors: ["Blue", "Green", "Black", "Pink"],
          isometric: ["Caltic", "Clinopyroxene", "Quartz"],
          forms: ["Monoclinic", "Triclinic"],
          Id: 101,
        }
      ]
    }
  }

  featureChange(event: {target: {value: string }}): void {
    this.setState({ feature: featurelist.indexOf(event.target.value) })
  }
/*
  colorchange(event: {target: {value: string }}): void {
    this.setState({ colorid: colorlist.indexOf(event.target.value) })
  }
  <h1 style={h1Style}>RockLookUp</h1>
*/


  mineralMenu3(tabNum: number): JSX.Element {
    const mineralobj=this.state.MineralArr[tabNum]
    return (
      <div>
        <p>Third Mineral Characteristics</p>
        <div style={columns}>
          <div key={mineralobj.Id}>
            <div >
              <p>Colors</p>
              <div>{mineralobj.colors.map(color =>{
                return(
                  <div>
                    <form>
                      <input type="checkbox"></input>
                      <label id={color}>{color}</label><br></br>
                    </form>
                  </div>
                )
              })}</div>
            </div>
            <div >
              <p>Isometric</p>
              <div>{mineralobj.isometric.map(isometric =>{
                return(
                  <div>
                    <form>
                      <input type="checkbox"></input>
                      <label id={isometric}>{isometric}</label><br></br>
                    </form>
                  </div>
                )
              })}</div>
            </div>
            <div>
              <p>Forms</p>
              <div>{mineralobj.forms.map(forms =>{
                return(
                  <div>
                    <form>
                      <input type="checkbox"></input>
                      <label id={forms}>{forms}</label><br></br>
                    </form>
                  </div>
                )
              })}</div>
            </div>
            
          </div>
        </div>
        <button type="button" onClick={()=>this.setState({selectedTab: -1 })}>
            Back
            </button>
      </div>  
    )
  }
  

mineralMenu2():JSX.Element{
  return(
    
    <div/*style={mineralModal}*/>
      <p>Second Mineral Characteristics</p>
      <div style={columns}>
        <div>
          <p>Colors</p>
          <form>
            <input type="checkbox"></input>
            <label>Blue</label><br></br>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Green</label><br></br>
          </form>
        </div>
        <div >
          <p>Isometric</p>
          <form>
            <input type="checkbox"></input>
            <label>Clinopyroxene</label>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Caltic</label>
          </form>
        </div>
        <div >
          <p>Forms</p>
          <form>
            <input type="checkbox"></input>
            <label>Monoclinic</label><br></br>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Triclinic</label>
          </form>
        </div>
        
      </div>
      <button type="button" onClick={()=>this.setState({mineralModalOpen2: false})}>
          Back
        </button>
        <button type={"button"} onClick={() => this.setState({ selectedTab: 0 })}>More</button>
        {this.state.selectedTab == -1 ? null : this.mineralMenu3(this.state.selectedTab)}
    </div>
  )
}


mineralMenu():JSX.Element{
  return(
    <div style={mineralModal}>
      <p>Mineral Characteristics</p>
      <div style={columns}>
        <div >
          <p>Colors</p>
          <form>
            <input type="checkbox"></input>
            <label>Blue</label><br></br>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Green</label><br></br>
          </form>
        </div>
        <div >
          <p>Isometric</p>
          <form>
            <input type="checkbox"></input>
            <label>Olivine</label>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Caltic</label>
          </form>
        </div>
        <div >
          <p>Crystal Forms</p>
          <form>
            <input type="checkbox"></input>
            <label>Monoclinic</label><br></br>
          </form>
          <form>
            <input type="checkbox"></input>
            <label>Triclinic</label>
          </form>
        </div>
        
      </div>
      <button type="button" onClick={()=>this.setState({mineralModalOpen: false})}>
          Back
        </button>
        <button type="button" onClick={()=>this.setState({mineralModalOpen2: true})}>More</button>
        {this.state.mineralModalOpen2 ? this.mineralMenu2():null}
    </div>
  )
}

mineraldropdown(): JSX.Element{
  return(
    <div style={dropdown}>
      <div>
        <button type="button" style={button} onClick={()=>this.setState({mineralModalOpen: true})}>Mineral</button>
      </div>
      <div>
        <button type="button" style={button} onClick={()=>this.setState({mineralModalOpen: true})}>Mafic</button>
      </div> 
      <div> 
        <button type="button" style={button} onClick={()=>this.setState({mineralModalOpen: true})}>Fine</button>
      </div>
      <div>  
        <button type="button" style={button} onClick={()=>this.setState({mineralModalOpen: true})}>Coarse</button>
      </div>
      <div>
        <button type="button" style={button} onClick={()=>this.setState({mineralModalOpen: true})}>Felsic</button>
      </div>
    </div>
  )
}

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Rock Lookup</div>
        <div style={mainContainer}>
          <button type="button" onClick={()=>this.setState({mineralModalOpen: true})}>Mineral characteristics</button>
          <button type="button" onClick={()=>this.setState({featureopen: true})}>Select Feature</button>
          <button type={"button"} onClick={() => this.setState({ selectedTab: 0 })}>color</button>
          <select 
            value={featurelist[this.state.feature]}
            onChange = {e => this.featureChange(e)}
            onClick={()=>this.state.feature>0? this.setState({mineralModalOpen:true}):null}
          >
            {featurelist.map(feature => {
              return(
                <option value={feature} key={feature}>
                  {feature}
                </option>
              )})
            }
          </select>
          
        {this.state.mineralModalOpen ? this.mineralMenu():null}
        {this.state.featureopen ? this.mineraldropdown():null}
        {this.state.selectedTab == -1 ? null : this.mineralMenu3(this.state.selectedTab)}
        </div>
      </div>
    )
  }
}

export default RockLookUp