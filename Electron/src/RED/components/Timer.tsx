import React, { Component } from "react"
import CSS from "csstype"
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
  fontSize: "20px",
  marginTop: "-26px",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
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

interface IProps {}

interface IState {
  totalTime: any
  timerInstance: any
  currentInstance: number
}

class Timer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      totalTime: {
        title: "Total Time",
        setTime: 300,
        currentTime: 195,
      },
      timerInstance: {
        1: {
          title: "Default Instance",
          setTime: 30,
          currentTime: 10,
          difference: 0,
        },
      },
      currentInstance: 1,
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
          <div id="TotalTimeContainer" style={{ ...column, marginTop: "-15px" }}>
            <h3 style={{ marginBottom: "0px", fontSize: "25px", marginTop: "10px" }}>{this.state.totalTime.title}</h3>
            <ProgressBar current={this.state.totalTime.currentTime} total={this.state.totalTime.setTime} name="total" />
            <div style={{ ...timeRead, marginLeft: "1%", marginRight: "1%" }}>
              <h4>{packOutput(this.state.totalTime.currentTime)}</h4>
              <h4>-{packOutput(this.state.totalTime.setTime)}</h4>
            </div>
          </div>
          <div id="CurrentTaskContainer" style={{ ...column, marginTop: "-20px" }}>
            <h1 style={{ margin: "0px" }}>{this.state.timerInstance[this.state.currentInstance].title}</h1>
            <ProgressBar
              current={this.state.timerInstance[this.state.currentInstance].currentTime}
              total={this.state.timerInstance[this.state.currentInstance].setTime}
              name="other"
            />
            <div style={{ ...timeRead, fontSize: "30px", marginTop: "-38px", marginLeft: "3%", marginRight: "3%" }}>
              <h2>{packOutput(this.state.timerInstance[this.state.currentInstance].currentTime)}</h2>
              <h2>-{packOutput(this.state.timerInstance[this.state.currentInstance].setTime)}</h2>
            </div>
          </div>
          <div id="NextTaskContainer" />
          <div
            id="StartStopContainer"
            style={{
              display: "grid",
              gridTemplateColumns: "auto auto auto",
              justifyContent: "space-between",
              alignItems: "flex-end",
            }}
          >
            <button type="button" style={{ height: "100px", width: "180px", fontSize: "25px" }}>
              START/STOP
            </button>
            <button type="button" style={{ height: "20px", width: "130px" }}>
              Advanced Options
            </button>
            <button type="button" style={{ height: "100px", width: "180px", fontSize: "25px" }}>
              RESET
            </button>
          </div>
          <div id="ResetButtonContainer" />
        </div>
      </div>
    )
  }
}
export default Timer
/*
const generateId = (): string => Math.random().toString(36).substr(2, 8)

const convertStringToSeconds = (time: string): number => {
  const [hours, minutes, seconds] = time.split(":").map(n => Number(n))
  return hours * 3600 + minutes * 60 + seconds
}

const convertSecondsToString = (seconds: number): string => {
  const padNumber = (n: number): string => {
    return String(n).padStart(2, "0")
  }
  const hours = padNumber(Math.floor(seconds / 3600))
  const minutes = padNumber(Math.floor((seconds - hours * 3600) / 60))
  const remainingSeconds = padNumber(seconds % 60)
  return `${hours}:${minutes}:${remainingSeconds}`
}

const Timer = (): JSX.Element => {
  const [tasks, setTasks] = useState([
    {
      id: generateId(),
      name: "Sample Task",
      allottedTime: "00:00:30",
      runningTime: 30,
    },
  ])
  const [runningTime, setRunningTime] = useState(tasks[0].runningTime)
  const [newTaskName, setNewTaskName] = useState("")
  const [newTaskTime, setNewTaskTime] = useState("")
  const [isRunning, setIsRunning] = useState(false)

  useEffect(() => {
    const tick = setTimeout(() => {
      // If the current timer is finished, check if there is another task.
      // If so, set running time to that of the next task and remove the
      // finished one. Otherwise, stop.
      if (runningTime === 0) {
        if (tasks.length - 1 > 0) {
          setRunningTime(tasks[1].runningTime)
          setTasks(tasks.slice(1))
        } else {
          setIsRunning(false)
        }
      }

      if (isRunning && runningTime > 0) {
        setRunningTime(runningTime - 1)
      }
    }, 1000)
    return () => clearTimeout(tick)
  })

  const removeTask = (taskId: string): void => {
    setTasks(tasks.filter(task => task.id !== taskId))
  }

  const addTask = (): void => {
    setTasks(
      tasks.concat({
        id: generateId(),
        name: newTaskName,
        allottedTime: newTaskTime,
        runningTime: convertStringToSeconds(newTaskTime),
      })
    )
  }

  return (
    <div>
      <div style={label}>Timer</div>
      <div style={container}>
        <h2>{tasks[0].name}</h2>
        <p>{convertSecondsToString(runningTime)}</p>
        <ProgressBar current={runningTime} total={tasks[0].runningTime} />
        <button type="button" onClick={() => setIsRunning(!isRunning)}>
          {isRunning ? "Stop" : "Start"}
        </button>
        <button
          type="button"
          onClick={() => {
            setRunningTime(tasks[0].runningTime)
            setIsRunning(false)
          }}
        >
          Reset
        </button>

        <ul style={{ listStyle: "none" }}>
          {tasks.map(task => (
            <li key={task.id}>
              <p>
                {task.id}, {task.name}, {task.allottedTime}, {task.runningTime}s
              </p>
              <button type="button" onClick={() => removeTask(task.id)}>
                Remove Task
              </button>
            </li>
          ))}
        </ul>

        <form>
          <input
            type="text"
            value={newTaskName}
            placeholder={newTaskName}
            onChange={event => setNewTaskName(event.target.value)}
          />
          <input
            type="text"
            value={newTaskTime}
            placeholder={newTaskTime}
            onChange={event => setNewTaskTime(event.target.value)}
          />
          <button type="button" onClick={addTask}>
            + Add Task
          </button>
        </form>
      </div>
    </div>
  )
}

export default Timer
*/
