import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import ProgressBar from "../../Core/ProgressBar"

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
  fontFamily: "arial",
  textAlign: "center",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
}
const timeRead: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  marginTop: "0px",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  marginLeft: "auto",
  marginRight: "auto",
}
const advOptionsModal: CSS.Properties = {
  position: "absolute",
  zIndex: 1,
  border: "2px solid #990000",
  backgroundColor: "white",
}
const timeSplitModal: CSS.Properties = {
  position: "absolute",
  zIndex: 1,
  border: "2px solid #990000",
  backgroundColor: "white",
}
const rmvAddModal: CSS.Properties = {
  position: "absolute",
  zIndex: 1,
  border: "2px solid #990000",
  backgroundColor: "white",
}
const filepath = path.join(__dirname, "../assets/TaskList.json")

function unpackInput(time: string): number {
  const formattedTime = time.split(":")
  const hourToSeconds = 3600 * Number(formattedTime[0])
  const minToSeconds = 60 * Number(formattedTime[1])
  return Number(formattedTime[2]) + minToSeconds + hourToSeconds
}

function packOutput(time: number): string {
  const secondsToHours = Math.floor(time / 3600)
  const secondsToMinutes = Math.floor((time - secondsToHours * 3600) / 60)
  const remainSeconds = time - (secondsToHours * 3600 + secondsToMinutes * 60)
  return `${secondsToHours}:${secondsToMinutes.toLocaleString(undefined, {
    minimumIntegerDigits: 2,
  })}:${remainSeconds.toLocaleString(undefined, { minimumIntegerDigits: 2 })}`
}

interface ParentTask {
  index: number
  title: string
  setTime: number
  currentTime: number
}

interface ChildTask extends ParentTask {}

interface IProps {}

interface IState {
  parentTask: any
  timerInstance: any
  currentChild: number
  currentParent: number
  advOptionsOpen: boolean
  rmvAddOptionOpen: boolean
  timeSplitOpen: boolean
}

class Timer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      advOptionsOpen: false,
      rmvAddOptionOpen: false,
      timeSplitOpen: false,
      parentTask: {
        1: {
          title: "Parent Task",
          setTime: 300,
          currentTime: 195,
        },
      },
      timerInstance: {
        1: {
          title: "Default Instance",
          setTime: 30,
          currentTime: 10,
          difference: 0,
        },
      },
      currentChild: 1,
      currentParent: 1,
    }
  }

  addInstance(time: string, title: string): void {
    const timeInSeconds = unpackInput(time)
    let { timerInstance } = this.state
    let idNum: number
    for (idNum = 0; idNum <= timerInstance[idNum]; idNum++);
    timerInstance = {
      ...timerInstance,
      [idNum]: {
        title: { title },
        setTime: { time },
        currentTime: { timeInSeconds },
        difference: 0,
      },
    }
    this.setState({ timerInstance })
  }

  removeInstance(idNum: number): void {
    const { timerInstance } = this.state
    delete timerInstance[idNum]
    this.setState({ timerInstance })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Timer</div>
        <div style={{ ...container, ...column }}>
          <div id="TotalTimeContainer" style={{ ...column, marginTop: "-2.5%", width: "98%" }}>
            <p style={{ marginBottom: "0px", fontSize: "15px", marginTop: "10px", fontWeight: "bold" }}>
              {this.state.parentTask[this.state.currentParent].title}
            </p>
            <ProgressBar
              current={this.state.parentTask[this.state.currentParent].currentTime}
              total={this.state.parentTask[this.state.currentParent].setTime}
              name="total"
            />
            <div style={{ ...timeRead, marginLeft: "1%", marginRight: "1%", marginTop: "-2.8%" }}>
              <p>{packOutput(this.state.parentTask[this.state.currentParent].currentTime)}</p>
              <p>-{packOutput(this.state.parentTask[this.state.currentParent].setTime)}</p>
            </div>
          </div>
          <div id="CurrentTaskContainer" style={{ ...column, marginTop: "-5%", width: "100%" }}>
            <p style={{ margin: "0px", fontSize: "23px", fontWeight: "bold" }}>
              {this.state.timerInstance[this.state.currentChild].title}
            </p>
            <ProgressBar
              current={this.state.timerInstance[this.state.currentChild].currentTime}
              total={this.state.timerInstance[this.state.currentChild].setTime}
              name="other"
            />
            <div
              style={{
                ...timeRead,
                fontSize: "20px",
                marginTop: "-3.5%",
                marginLeft: "9%",
                marginRight: "9%",
                fontWeight: "bold",
              }}
            >
              <p>{packOutput(this.state.timerInstance[this.state.currentChild].currentTime)}</p>
              <p>-{packOutput(this.state.timerInstance[this.state.currentChild].setTime)}</p>
            </div>
          </div>
          <div id="NextTaskContainer" />
          <div
            id="StartStopContainer"
            style={{
              display: "flex",
              marginTop: "-3%",
              flexBasis: "auto",
            }}
          >
            <div style={{ flexGrow: 3 }}>
              <button type="button" style={{ height: "50px", width: "100%", fontSize: "30px" }}>
                START/STOP
              </button>
            </div>
            <div style={{ ...column, flexGrow: 1 }}>
              <button type="button" style={{ height: "20px" }} onClick={() => this.setState({ advOptionsOpen: true })}>
                Advanced Options
              </button>
              <button type="button" style={{ height: "30px", fontSize: "23px" }}>
                RESET
              </button>
            </div>
            <div style={{ flexGrow: 3 }}>
              <button type="button" style={{ height: "50px", width: "100%", fontSize: "30px", marginLeft: "-10px" }}>
                NEXT TASK
              </button>
            </div>
          </div>
          {this.state.advOptionsOpen ? (
            <div style={advOptionsModal}>
              <p>Advanced Options</p>
              <div>
                <button type="button">Prev Sub-Task</button>
                <button type="button" onClick={() => this.setState({ timeSplitOpen: true })}>
                  Open Split
                </button>
                <button type="button">Reset Task</button>
                <button type="button" onClick={() => this.setState({ rmvAddOptionOpen: true })}>
                  Edit List
                </button>
              </div>
              <div>
                <button type="button" onClick={() => this.setState({ advOptionsOpen: false })}>
                  back
                </button>
              </div>
            </div>
          ) : null}
          {this.state.timeSplitOpen ? (
            <div style={timeSplitModal}>
              <p>Times and Differences</p>
              <div>
                {[TASKLIST].map(task => {
                  return (
                    <div key={undefined}>
                      <p>{task}</p>
                    </div>
                  )
                })}
              </div>
              <div>
                <button type="button" onClick={() => this.setState({ timeSplitOpen: false })}>
                  back
                </button>
                <button
                  type="button"
                  onClick={() =>
                    this.setState({ timeSplitOpen: false, advOptionsOpen: false, rmvAddOptionOpen: false })
                  }
                >
                  close all
                </button>
              </div>
            </div>
          ) : null}
          {this.state.rmvAddOptionOpen ? (
            <div style={rmvAddModal}>
              <p>Edit Task List</p>
              <div>
                {[TASKLIST].map(task => {
                  return (
                    <div key={undefined}>
                      <p>{task}</p>
                      <button
                        type="button"
                        onClick={() => {
                          // placeholder for delete entry
                        }}
                      >
                        Trash Can Icon
                      </button>
                    </div>
                  )
                })}
              </div>
              <button type="button" onClick={() => this.setState({ rmvAddOptionOpen: false })}>
                back
              </button>
              <button
                type="button"
                onClick={() => this.setState({ rmvAddOptionOpen: false, advOptionsOpen: false, timeSplitOpen: false })}
              >
                close all
              </button>
            </div>
          ) : null}
        </div>
      </div>
    )
  }
}
export default Timer
