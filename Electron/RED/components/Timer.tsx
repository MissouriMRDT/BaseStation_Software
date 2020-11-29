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
  // display: "grid",
  fontFamily: "arial",
  textAlign: "center",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "30px",
  // gridRowStart: "2 & {}",
  // grid: "repeat(2, 28px) / auto-flow dense",
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
  showTaskList: boolean
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
          allottedTime: "00:01:00",
          runningTime: this.convertStringToSeconds("00:01:00"),
        },
      ],
      newTask: { name: "", allottedTime: "", runningTime: 0 },
      showTaskList: false,
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
      if (this.state.tasks.length > 1) {
        this.setState(previousState => ({
          isRunning: false,
          tasks: previousState.tasks.slice(1),
        }))
      } else {
        this.setState({ isRunning: false })
      }
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

  addNewTask = (event): void => {
    if (this.state.newTask.allottedTime.match(/\d{2}:\d{2}:\d{2}/)) {
      this.setState(previousState => ({
        tasks: [...previousState.tasks, previousState.newTask],
        newTask: { name: "", allottedTime: "", runningTime: 0 },
      }))
    } else {
      alert("Improper format. allottedTime HH:MM:SS")
    }
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
        runningTime: this.convertStringToSeconds(event.target.value),
      },
    }))
  }

  toggleTaskList = (): void => {
    this.setState(previousState => ({
      showTaskList: !previousState.showTaskList,
    }))
  }

  render(): JSX.Element {
    const currentTask = this.state.tasks[0]
    const allTasks = this.state.tasks.map(task => (
      <li key={task.name}>
        {task.name}, {task.allottedTime}, {task.runningTime}s
      </li>
    ))
    return (
      <div>
        <div style={label}>Timer</div>
        <div style={container}>
          <h2>{currentTask.name}</h2>
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
          <div
            style={{
              display: "flex",
              justifyContent: "space-evenly",
              alignItems: "center",
            }}
          >
            <h3>Tasks</h3>
            <button onClick={this.toggleTaskList} type="button">
              {this.state.showTaskList ? "Hide" : "Show"}
            </button>
          </div>
          {this.state.showTaskList && (
            <section>
              <ul style={{ listStyle: "none", padding: 0 }}>{allTasks}</ul>
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
            </section>
          )}
        </div>
      </div>
    )
  }
}

export default Timer
