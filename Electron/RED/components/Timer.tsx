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
  allottedTime: string
  runningTime: number
}

interface IProps {}

interface IState {
  isRunning: boolean
  tasks: Task[]
  newTask: Task
  // selectedTaskIndex: number
}

class Timer extends React.Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      isRunning: false,
      tasks: [
        {
          name: "Sample Task",
          allottedTime: "00:10:00",
          runningTime: this.convertStringToSeconds("00:10:00"),
        },
      ],
      newTask: { name: "", allottedTime: "", runningTime: 0 },
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
    if (this.state.tasks[0].runningTime === 0) {
      this.setState({
        isRunning: false,
      })
    }

    if (this.state.isRunning) {
      this.setState(previousState => ({
        tasks: [
          {
            name: previousState.tasks[0].name,
            allottedTime: previousState.tasks[0].allottedTime,
            runningTime: previousState.tasks[0].runningTime - 1,
          },
        ].concat(previousState.tasks.splice(1)),
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
      tasks: [
        {
          name: previousState.tasks[0].name,
          allottedTime: previousState.tasks[0].allottedTime,
          runningTime: this.convertStringToSeconds(
            previousState.tasks[0].allottedTime
          ),
        },
      ].concat(previousState.tasks.splice(1)),
    }))
  }

  addNewTask = (event: { preventDefault: () => void }): void => {
    this.setState(previousState => ({
      tasks: [...previousState.tasks, previousState.newTask],
      newTask: { name: "", allottedTime: "", runningTime: 0 },
    }))
    event.preventDefault()
  }

  changeNewTaskName = (event: React.ChangeEvent<HTMLInputElement>): void => {
    event.persist()
    this.setState(previousState => ({
      newTask: {
        ...previousState.newTask,
        name: event.target.value,
      },
    }))
  }

  changeNewTaskTime = (event: React.ChangeEvent<HTMLInputElement>): void => {
    event.persist()
    this.setState(previousState => ({
      newTask: {
        ...previousState.newTask,
        allottedTime: event.target.value,
      },
    }))
  }

  render(): JSX.Element {
    const currentTask = this.state.tasks[0]
    const allTasks = this.state.tasks.map(task => (
      <li key={task.name}>
        {task.name}, {task.allottedTime}, {task.runningTime}
      </li>
    ))
    return (
      <div>
        <div style={label}>Timer</div>
        <div style={container}>
          <p>{currentTask.name}</p>
          <p>{this.convertSecondsToString(currentTask.runningTime)}</p>
          <ProgressBar
            current={currentTask.runningTime}
            total={this.convertStringToSeconds(currentTask.allottedTime)}
          />
          <div>
            <button onClick={this.toggle} type="button">
              {this.state.isRunning ? "Stop" : "Start"}
            </button>
            <button onClick={this.reset} type="button">
              Reset
            </button>
          </div>
          <ul>{allTasks}</ul>
          <form onSubmit={this.addNewTask}>
            <input
              type="text"
              placeholder="Name"
              value={this.state.newTask.name}
              onChange={this.changeNewTaskName}
            />
            <input
              type="text"
              placeholder="Allotted Time"
              value={this.state.newTask.allottedTime}
              onChange={this.changeNewTaskTime}
            />
            <input type="submit" value="+ Add Task" />
          </form>
        </div>
      </div>
    )
  }
}

export default Timer