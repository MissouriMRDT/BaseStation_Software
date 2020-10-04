import React from "react"
import CSS from "csstype"

import ProgressBar from "./ProgressBar"

const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
}

interface Task {
  name: string
  time: string
}

interface IProps {}

interface IState {
  isRunning: boolean
  tasks: Task[]
  currentTask: Task
  time: number
}

class Timer extends React.Component<IProps, IState> {
  constructor(props: any) {
    super(props)

    const tasks = [{ name: "Sample Task", time: "00:10:00" }]
    const currentTask = tasks[0]
    this.state = {
      isRunning: false,
      tasks,
      currentTask,
      time: this.convertStringToSeconds(currentTask.time),
    }
  }

  componentDidMount(): void {
    this.timerID = setInterval(() => this.tick(), 1000)
  }

  componentWillUnmount(): void {
    clearInterval(this.timerID)
  }

  convertStringToSeconds = (time: string): number => {
    const hours = Number(time.split(":")[0])
    const minutes = Number(time.split(":")[1])
    const seconds = Number(time.split(":")[2])

    return hours * 3600 + minutes * 60 + seconds
  }

  convertSecondsToString = (seconds: number): string => {
    const hours = Math.floor(seconds / 3600)
    const minutes = Math.floor((seconds - hours * 3600) / 60)
    const remainingSeconds = seconds % 60

    const padWithZeroes = (num: number, desiredLength: number): string => {
      return String(num).padStart(desiredLength, "0")
    }

    let timeString = ""
    timeString += `${padWithZeroes(hours, 2)}:`
    timeString += `${padWithZeroes(minutes, 2)}:`
    timeString += padWithZeroes(remainingSeconds, 2)

    return timeString
  }

  tick = (): void => {
    if (this.state.time === 0) {
      this.setState({
        isRunning: false,
      })
    }

    if (this.state.isRunning) {
      this.setState(previousState => ({
        time: previousState.time - 1,
      }))
    }
  }

  toggle = (): void => {
    this.setState(previousState => ({
      isRunning: !previousState.isRunning,
    }))
  }

  reset = (): void => {
    this.setState(previousState => ({
      isRunning: false,
      time: this.convertStringToSeconds(previousState.currentTask.time),
    }))
  }

  render(): JSX.Element {
    const allTasks = this.state.tasks.map(task => (
      <li key={task.name}>
        {task.name}, {task.time}
      </li>
    ))
    return (
      <div>
        <div style={label}>Timer</div>
        <div style={container}>
          <p>{this.state.currentTask.name}</p>
          <p>{this.convertSecondsToString(this.state.time)}</p>
          <ProgressBar
            current={this.state.time}
            total={this.convertStringToSeconds(this.state.currentTask.time)}
          />
          <div>
            <button onClick={this.toggle} type="button">
              {this.state.isRunning ? "Stop" : "Start"}
            </button>
            <button onClick={this.reset} type="button">
              Reset
            </button>
          </div>
          {/* Convert List to Form. Tasks can be added, removed, and modified. */}
          <ul>{allTasks}</ul>
        </div>
      </div>
    )
  }
}

export default Timer
