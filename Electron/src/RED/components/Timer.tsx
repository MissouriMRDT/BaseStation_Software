import React, { useState, useEffect } from "react"
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
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "30px",
}

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
