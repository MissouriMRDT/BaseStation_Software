import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"
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
const FILEPATH = path.join(__dirname, "../assets/TaskList.json")
let taskList: any

let timer: any = 0

if (fs.existsSync(FILEPATH)) {
  taskList = JSON.parse(fs.readFileSync(FILEPATH).toString())
}
// at some point work on being able to load a different file at runtime

function stopTimer(): void {
  clearInterval(timer)
  timer = 0
}

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

interface Task {
  title: string
  setTime: number
  difference: number
}

interface Parent extends Task {
  childTasks: Task[]
}

interface IProps {
  timer: any
}

interface IState {
  parentTask: Parent[]
  currentChild: number
  currentParent: number
  advOptionsOpen: boolean
  rmvAddOptionOpen: boolean
  timeSplitOpen: boolean
  currentParentTime: number
  currentChildTime: number
  endOfList: boolean
  isCounting: boolean
}

class Timer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      advOptionsOpen: false,
      rmvAddOptionOpen: false,
      timeSplitOpen: false,
      parentTask: taskList.ParentTasks,
      currentParent: 0,
      currentChild: 0,
      currentParentTime: 124,
      currentChildTime: 22,
      endOfList: false,
      isCounting: false,
    }
    this.startTimer = this.startTimer.bind(this)
    this.countDown = this.countDown.bind(this)
  }

  countDown(): void {
    let { currentParentTime, currentChildTime } = this.state
    currentChildTime++
    currentParentTime++
    this.setState({ currentChildTime, currentParentTime })
  }

  startTimer(): void {
    if (timer === 0) {
      timer = setInterval(this.countDown, 1000)
    }
  }

  loadNextTask(): void {
    let { endOfList, currentParent, currentChild, currentParentTime } = this.state
    if (currentChild === this.state.parentTask[currentParent].childTasks.length - 1) {
      if (currentParent === this.state.parentTask.length - 1) {
        endOfList = true
      } else {
        currentParent++
        currentParentTime = 0
        currentChild = 0
      }
    } else {
      currentChild++
    }
    this.setState({ currentChildTime: 0, currentParentTime, endOfList, currentParent, currentChild })
  }

  // currently there's no ability to save the differences for analysis after the app is closed
  // but there's the option to make that a saveable thing to the JSON
  saveJSON(): void {
    // prefer-const wants "parentTask" to be const, despite member variables being altered. Look at later
    const { parentTask } = this.state
    for (let ndx = 0; ndx < parentTask.length; ndx++) {
      parentTask[ndx].difference = 0
      for (let inner = 0; inner < parentTask[ndx].childTasks.length; inner++) {
        parentTask[ndx].childTasks[inner].difference = 0
      }
    }
    this.setState({ parentTask })
    fs.writeFileSync(FILEPATH, JSON.stringify(this.state.parentTask))
  }

  reset(): void {
    let { parentTask } = this.state
    parentTask = taskList.ParentTasks
    this.setState({ parentTask, currentParentTime: 0, currentChildTime: 0 })
    stopTimer()
  }

  handleStartStop(): void {
    let { isCounting } = this.state
    if (!isCounting) {
      this.startTimer()
      isCounting = true
    } else {
      stopTimer()
      isCounting = false
    }
    this.setState({ isCounting })
  }

  addInstance(time: string, parentName: string, childNameIn: string, parentNdx: number): void {
    const childName = childNameIn || ""
    const timeInSeconds = unpackInput(time)
    let { parentTask } = this.state
    if (childName) {
      const newTask: Task = {
        title: childName,
        setTime: timeInSeconds,
        difference: 0,
      }
      parentTask[parentNdx].childTasks = [...parentTask[parentNdx].childTasks, newTask]
    } else {
      const newTask: Parent = {
        title: parentName,
        setTime: timeInSeconds,
        difference: 0,
        childTasks: [],
      }
      parentTask = [...parentTask, newTask]
    }
    this.setState({ parentTask })
    this.saveJSON()
  }

  removeInstance(parentName: string, childId: number): void {
    const { parentTask } = this.state
    if (childId !== -1) {
      delete parentTask[parentName].childTasks[childId]
    } else {
      delete parentTask[parentName]
    }
    this.setState({ parentTask })
    this.saveJSON()
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
              current={this.state.currentParentTime}
              total={this.state.parentTask[this.state.currentParent].setTime}
              name="total"
            />
            <div style={{ ...timeRead, marginLeft: "1%", marginRight: "1%", marginTop: "-2.8%" }}>
              <p>{packOutput(this.state.currentParentTime)}</p>
              <p>-{packOutput(this.state.parentTask[this.state.currentParent].setTime)}</p>
            </div>
          </div>
          <div id="CurrentTaskContainer" style={{ ...column, marginTop: "-5%", width: "100%" }}>
            <p style={{ margin: "0px", fontSize: "23px", fontWeight: "bold" }}>
              {this.state.parentTask[this.state.currentParent].childTasks[this.state.currentChild].title}
            </p>
            <ProgressBar
              current={this.state.currentChildTime}
              total={this.state.parentTask[this.state.currentParent].childTasks[this.state.currentChild].setTime}
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
              <p>{packOutput(this.state.currentChildTime)}</p>
              <p>
                -
                {packOutput(
                  this.state.parentTask[this.state.currentParent].childTasks[this.state.currentChild].setTime
                )}
              </p>
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
              <button
                type="button"
                style={{ height: "50px", width: "100%", fontSize: "30px" }}
                onClick={() => this.handleStartStop()}
              >
                START/STOP
              </button>
            </div>
            <div style={{ ...column, flexGrow: 1 }}>
              <button type="button" style={{ height: "20px" }} onClick={() => this.setState({ advOptionsOpen: true })}>
                Advanced Options
              </button>
              <button type="button" style={{ height: "30px", fontSize: "23px" }} onClick={() => this.reset()}>
                RESET
              </button>
            </div>
            <div style={{ flexGrow: 3 }}>
              <button
                type="button"
                style={{ height: "50px", width: "100%", fontSize: "30px", marginLeft: "-10px" }}
                onClick={() => this.loadNextTask()}
              >
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
              <div>BIG LIST GOES HERE</div>
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
                {this.state.parentTask.map(task => {
                  return (
                    <div key={task.title}>
                      <p>{task.title}</p>
                      <button type="button">Trash Can Icon</button>
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
