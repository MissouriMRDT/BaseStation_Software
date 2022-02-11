import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"
import ProgressBar from "../../Core/ProgressBar"
import TrashCanIcon from "../../../assets/icons/TrashCanIcon.png"

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
  zIndex: 2,
  border: "2px solid #990000",
  backgroundColor: "white",
}
const rmvAddModal: CSS.Properties = {
  position: "absolute",
  zIndex: 2,
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

// arrow buttons in the top corners to navigate main tasks

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
  const absTime = Math.abs(time)
  const secondsToHours = Math.floor(absTime / 3600)
  const secondsToMinutes = Math.floor((absTime - secondsToHours * 3600) / 60)
  const remainSeconds = absTime - (secondsToHours * 3600 + secondsToMinutes * 60)
  return `${secondsToHours}:${secondsToMinutes.toLocaleString(undefined, {
    minimumIntegerDigits: 2,
  })}:${remainSeconds.toLocaleString(undefined, { minimumIntegerDigits: 2 })}`
}

interface Task {
  title: string
  id: number
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
  chosenEdit: number
  selectedOption: number
  parentTask: Parent[]
  timeInput: string
  nameInput: string
  missionInputOpen: boolean
  taskInputOpen: boolean
  currentChild: number
  advOptionsOpen: boolean
  rmvAddOptionOpen: boolean
  timeSplitOpen: boolean
  currentParentTime: number
  currentChildTime: number
  endOfList: boolean
  isCounting: boolean
  delta: number
}

class Timer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      chosenEdit: 0,
      selectedOption: 100,
      nameInput: "",
      timeInput: "",
      missionInputOpen: false,
      taskInputOpen: false,
      advOptionsOpen: false,
      rmvAddOptionOpen: false,
      timeSplitOpen: false,
      parentTask: taskList.ParentTasks,
      currentChild: 0,
      currentParentTime: 22,
      currentChildTime: 22,
      endOfList: false,
      isCounting: false,
      delta: 0,
    }
    this.startTimer = this.startTimer.bind(this)
    this.countDown = this.countDown.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
  }

  handleChange(event: any, type: string): void {
    if (type === "name") {
      this.setState({ nameInput: event.target.value })
    } else if (type === "time") {
      this.setState({ timeInput: event.target.value })
    }
  }

  handleEdit(ID: number): void {
    const { parentTask } = this.state
    let { timeInput, chosenEdit } = this.state
    if (this.state.timeInput) {
      let childIndex = 0
      const newTime = unpackInput(timeInput)
      const parentIndex = this.findIndex(ID)
      const oldChild = parentTask[this.findIndex(ID)].childTasks.filter(i => i.id === ID)
      parentTask[parentIndex].setTime -= oldChild[0].setTime
      parentTask[parentIndex].setTime += newTime
      for (let i = 0; i < parentTask.length; i++) {
        if (parentTask[parentIndex].childTasks[i].id === ID) {
          childIndex = i
          break
        }
      }
      parentTask[parentIndex].childTasks[childIndex].setTime = newTime
      timeInput = ""
    }
    chosenEdit = 0
    this.setState({ parentTask, timeInput, chosenEdit })
    this.saveJSON()
  }

  findIndex(ID: number): number {
    for (let i = 0; i < this.state.parentTask.length; i++) {
      if (Math.floor(this.state.parentTask[i].id / 100) === Math.floor(ID / 100)) {
        return i
      }
    }
    return -1
  }

  handleSubmit(event: any): void {
    event.preventDefault()
    this.addInstance(this.state.timeInput, this.state.selectedOption, this.state.nameInput)
    this.setState({ timeInput: "", nameInput: "" })
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
    let { endOfList, currentChild, delta } = this.state
    if (currentChild === this.state.parentTask[this.findIndex(this.state.selectedOption)].childTasks.length - 1) {
      if (this.findIndex(this.state.selectedOption) === this.state.parentTask.length - 1) {
        endOfList = true
      }
    } else {
      delta +=
        this.state.parentTask[this.findIndex(this.state.selectedOption)].childTasks[currentChild].setTime -
        this.state.currentChildTime
      currentChild++
    }
    this.setState({ currentChildTime: 0, endOfList, currentChild, delta })
  }

  // currently there's no ability to save the differences for analysis after the app is closed
  // but there's the option to make that a saveable thing to the JSON
  saveJSON(): void {
    fs.writeFileSync(FILEPATH, JSON.stringify({ ParentTasks: this.state.parentTask }, null, 2))
  }

  reset(): void {
    let { currentParentTime } = this.state
    currentParentTime -= this.state.currentChildTime
    this.setState({ currentParentTime, currentChildTime: 0, isCounting: false })
    stopTimer()
  }

  resetParentTask(): void {
    this.setState({ currentParentTime: 0, currentChild: 0, currentChildTime: 0, isCounting: false, delta: 0 })
    stopTimer()
  }

  findNewID(): number {
    let ID = 100
    let doesExist = true
    while (doesExist) {
      doesExist = false
      for (let i = 0; i < this.state.parentTask.length; i++) {
        if (ID === this.state.parentTask[i].id) {
          doesExist = true
        }
      }
      ID += doesExist ? 100 : 0
    }
    return ID
  }

  previousSubTask(): void {
    let { currentChild } = this.state
    if (currentChild) {
      this.reset()
      currentChild -= 1
    }
    this.setState({ currentChild })
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

  addInstance(time: string, inputID: number, nameIn: string): void {
    const index = this.findIndex(inputID)
    const timeInSeconds = unpackInput(time)
    // eslint also wants this one made a const... not sure why as it does get changed but yeah
    const { parentTask } = this.state
    if (time) {
      const newTask: Task = {
        title: nameIn,
        id: inputID + parentTask[index].childTasks.length + 1,
        setTime: timeInSeconds,
        difference: 0,
      }
      parentTask[index].childTasks.push(newTask)
      parentTask[index].setTime += timeInSeconds
    } else {
      const newTask: Parent = {
        title: nameIn,
        id: this.findNewID(),
        setTime: 0,
        difference: 0,
        childTasks: [],
      }
      parentTask.push(newTask)
    }
    this.setState({ parentTask })
    this.saveJSON()
  }

  removeInstance(ID: number): void {
    this.reset()
    let { parentTask } = this.state
    if (ID % 100) {
      const index = this.findIndex(ID)
      console.log(ID)
      console.log(index)
      const oldChild = parentTask[index].childTasks.filter(i => i.id === ID)
      parentTask[index].setTime -= oldChild[0].setTime
      const newChildList = parentTask[index].childTasks.filter(i => i.id !== ID)
      parentTask[index].childTasks = newChildList
    } else {
      const newParentList = parentTask.filter(i => i.id !== ID)
      parentTask = newParentList
    }
    this.setState({ parentTask, currentChild: 0 })
    this.saveJSON()
  }

  advancedOptionsMenu(): JSX.Element {
    return (
      <div style={advOptionsModal}>
        <p>Advanced Options</p>
        <div>
          <button type="button" onClick={() => this.previousSubTask()}>
            Prev Task
          </button>
          <button type="button" onClick={() => this.setState({ timeSplitOpen: true })}>
            Open Split
          </button>
          <button type="button" onClick={() => this.setState({ rmvAddOptionOpen: true })}>
            Show/Edit List
          </button>
        </div>
        <div>
          <button type="button" onClick={() => this.setState({ advOptionsOpen: false })}>
            back
          </button>
        </div>
      </div>
    )
  }

  timeSplitMenu(): JSX.Element {
    return (
      <div style={timeSplitModal}>
        <p>Times and Differences</p>
        <div>BIG LIST GOES HERE</div>
        <div>
          <button type="button" onClick={() => this.setState({ timeSplitOpen: false })}>
            back
          </button>
          <button
            type="button"
            onClick={() => this.setState({ timeSplitOpen: false, advOptionsOpen: false, rmvAddOptionOpen: false })}
          >
            close all
          </button>
        </div>
      </div>
    )
  }

  rmvAddMenu(): JSX.Element {
    return (
      <div style={{ ...rmvAddModal }}>
        <p>Edit Task List</p>
        <form onSubmit={this.handleSubmit}>
          <div>
            <button type="button" onClick={() => this.setState({ missionInputOpen: !this.state.missionInputOpen })}>
              Add Mission
            </button>
            <button type="button" onClick={() => this.setState({ taskInputOpen: !this.state.taskInputOpen })}>
              Add Task
            </button>
          </div>
          <div>
            {this.state.taskInputOpen ? (
              <div>
                <input
                  value={this.state.nameInput}
                  onChange={e => this.handleChange(e, "name")}
                  placeholder="Task Name"
                />
                <input
                  value={this.state.timeInput}
                  onChange={e => this.handleChange(e, "time")}
                  placeholder="HH:MM:SS"
                />
                <input type="submit" value="Submit" />
              </div>
            ) : null}
          </div>
          <div>
            {this.state.missionInputOpen ? (
              <div>
                <input
                  value={this.state.nameInput}
                  onChange={e => this.handleChange(e, "name")}
                  placeholder="Mission Name"
                />
                <input type="submit" value="Submit" />
              </div>
            ) : null}
          </div>
          <div style={{ ...column, padding: "5px" }}>
            {this.state.parentTask.map(task => {
              return (
                <div key={task.id} style={{ ...row }}>
                  <input
                    type="radio"
                    value={task.id}
                    checked={this.state.selectedOption === task.id}
                    onChange={() =>
                      this.setState({
                        selectedOption: task.id,
                        currentChild: 0,
                        currentParentTime: 0,
                        currentChildTime: 0,
                      })
                    }
                  />
                  <div style={{ ...column, height: "20px", padding: "2px", width: "33%" }}>
                    <div>{task.title}</div>
                    <div>{packOutput(task.setTime)}</div>
                  </div>
                  <button
                    type="button"
                    onClick={() => this.removeInstance(task.id)}
                    style={{ height: "34px", margin: "3px" }}
                  >
                    <img src={TrashCanIcon} alt="Trash Can Icon" />
                  </button>
                  <div style={{ marginBottom: "20px", width: "55%", flexWrap: "wrap" }}>
                    {task.childTasks.map(subTask => {
                      return (
                        <div key={subTask.id} style={row}>
                          <div style={{ ...column, flexBasis: "content", padding: "3px" }}>
                            <div>{subTask.title}</div>
                            <div>
                              {this.state.chosenEdit === subTask.id ? (
                                <div>
                                  <input
                                    value={this.state.timeInput}
                                    onChange={e => this.handleChange(e, "time")}
                                    placeholder={packOutput(subTask.setTime)}
                                    style={{ width: "40%" }}
                                  />
                                  <button type="button" onClick={() => this.handleEdit(subTask.id)}>
                                    save
                                  </button>
                                </div>
                              ) : (
                                <div>
                                  {packOutput(subTask.setTime)}
                                  <button type="button" onClick={() => this.setState({ chosenEdit: subTask.id })}>
                                    Edit
                                  </button>
                                </div>
                              )}
                            </div>
                          </div>
                          <button
                            type="button"
                            onClick={() => this.removeInstance(subTask.id)}
                            style={{ padding: "auto" }}
                          >
                            <img src={TrashCanIcon} alt="Trash Can Icon" />
                          </button>
                        </div>
                      )
                    })}
                  </div>
                </div>
              )
            })}
          </div>
        </form>
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
    )
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Timer</div>
        <div style={{ ...container, ...column }}>
          <div id="TotalTimeContainer" style={{ ...column, marginTop: "-2.5%", width: "98%" }}>
            <p style={{ marginBottom: "0px", fontSize: "15px", marginTop: "10px", fontWeight: "bold" }}>
              {this.state.parentTask[this.findIndex(this.state.selectedOption)].title}
            </p>
            <ProgressBar
              current={this.state.currentParentTime}
              total={this.state.parentTask[this.findIndex(this.state.selectedOption)].setTime}
              name="total"
            />
            <div style={{ ...timeRead, marginLeft: "1%", marginRight: "1%", marginTop: "-2.8%" }}>
              <p>{packOutput(this.state.currentParentTime)}</p>
              <p>
                -
                {packOutput(
                  this.state.currentParentTime -
                    this.state.parentTask[this.findIndex(this.state.selectedOption)].setTime
                )}
              </p>
            </div>
          </div>
          <div id="CurrentTaskContainer" style={{ ...column, marginTop: "-5%", width: "100%" }}>
            <p style={{ margin: "0px", fontSize: "23px", fontWeight: "bold" }}>
              {
                this.state.parentTask[this.findIndex(this.state.selectedOption)].childTasks[this.state.currentChild]
                  .title
              }
            </p>
            {this.state.delta ? (
              <div style={{ zIndex: 5, marginBottom: "-4%", fontWeight: "bold", fontSize: "20px", color: "white" }}>
                {packOutput(this.state.delta)}
              </div>
            ) : null}
            <ProgressBar
              current={this.state.currentChildTime}
              total={
                this.state.parentTask[this.findIndex(this.state.selectedOption)].childTasks[this.state.currentChild]
                  .setTime
              }
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
                  this.state.currentChildTime -
                    this.state.parentTask[this.findIndex(this.state.selectedOption)].childTasks[this.state.currentChild]
                      .setTime
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
              <button
                type="button"
                style={{ height: "30px", fontSize: "23px" }}
                onClick={() => (this.state.currentChildTime ? this.reset() : this.resetParentTask())}
              >
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
          {this.state.advOptionsOpen ? this.advancedOptionsMenu() : null}
          {this.state.timeSplitOpen ? this.timeSplitMenu() : null}
          {this.state.rmvAddOptionOpen ? this.rmvAddMenu() : null}
        </div>
      </div>
    )
  }
}
export default Timer
