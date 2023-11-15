/* eslint-disable react/jsx-props-no-spreading */
import React, { ChangeEvent, Component } from 'react';
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import CSS from 'csstype';
import path from 'path';
import fs from 'fs';
import ProgressBar from '../../Core/ProgressBar';
import TrashCanIcon from '../../../assets/icons/TrashCanIcon.png';
import ListReorderIcon from '../../../assets/icons/ListReorderIcon.png';

const label: CSS.Properties = {
  color: 'white',
  fontFamily: 'arial',
  fontSize: '16px',
  position: 'relative',
  marginTop: '-10px',
  top: '24px',
  left: '3px',
  zIndex: 1,
};
const mainButtons: CSS.Properties = {
  height: '50px',
  width: '100%',
  fontSize: '30px',
};
const trashButtons: CSS.Properties = {
  padding: 'auto',
  width: '40px',
  height: '36px',
};
const ulStyle: CSS.Properties = {
  listStyleType: 'none',
  paddingLeft: '0px',
};
const container: CSS.Properties = {
  fontFamily: 'arial',
  textAlign: 'center',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
};
const timeRead: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
  marginTop: '0px',
};
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  marginLeft: 'auto',
  marginRight: 'auto',
};
const Modal: CSS.Properties = {
  position: 'absolute',
  zIndex: 1,
  border: '2px solid #990000',
  backgroundColor: 'white',
  width: '56%',
};
const splitMainMenu: CSS.Properties = {
  position: 'absolute',
  zIndex: 2,
  width: '46%',
  backgroundColor: 'white',
  border: '2px solid #990000',
  boxSizing: 'border-box',
  boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
  textAlign: 'center',
};
const differenceMenu: CSS.Properties = {
  position: 'relative',
  border: '2px solid #990000',
  boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
  width: '95%',
  marginLeft: 'auto',
  marginRight: 'auto',
};
const timeSplitMissionTitle: CSS.Properties = {
  position: 'relative',
  width: '95%',
  fontFamily: 'Arial',
  fontSize: '25px',
  marginLeft: 'auto',
  marginRight: 'auto',
};
const timeSplitTitle: CSS.Properties = {
  position: 'relative',
  paddingTop: '25px',
  color: 'black',
  fontFamily: 'Arial',
  fontWeight: 'bold',
  fontSize: '30px',
  marginLeft: 'auto',
  marginRight: 'auto',
};

const FILEPATH = path.join(__dirname, '../assets/TaskList.json');
const DIFFPATH = path.join(__dirname, '../assets/Differences.txt');
let taskList: any;

let timer = 0;

if (fs.existsSync(FILEPATH)) {
  taskList = JSON.parse(fs.readFileSync(FILEPATH).toString());
}
// at some point work on being able to load a different file at runtime

function stopTimer(): void {
  clearInterval(timer);
  timer = 0;
}

function unpackInput(time: string): number {
  const formattedTime = time.split(':');
  const hourToSeconds = 3600 * Number(formattedTime[0]);
  const minToSeconds = 60 * Number(formattedTime[1]);
  return Number(formattedTime[2]) + minToSeconds + hourToSeconds;
}

function packOutput(time: number): string {
  const absTime = Math.abs(time);
  const secondsToHours = Math.floor(absTime / 3600);
  const secondsToMinutes = Math.floor((absTime - secondsToHours * 3600) / 60);
  const remainSeconds = absTime - (secondsToHours * 3600 + secondsToMinutes * 60);
  return `${secondsToHours}:${secondsToMinutes.toLocaleString(undefined, {
    minimumIntegerDigits: 2,
  })}:${remainSeconds.toLocaleString(undefined, { minimumIntegerDigits: 2 })}`;
}

interface Task {
  title: string;
  id: number;
  setTime: number;
  difference: number;
}

interface Mission extends Task {
  childTasks: Task[];
}

interface IProps {
  timer: number;
}

interface IState {
  startMenuOpen: boolean;
  chosenEdit: number;
  selectedMission: number;
  parentMission: Mission[];
  timeInput: string;
  nameInput: string;
  missionInputOpen: boolean;
  taskInputOpen: boolean;
  currentTask: number;
  advOptionsOpen: boolean;
  rmvAddOptionOpen: boolean;
  timeSplitOpen: boolean;
  currentMissionTime: number;
  currentTaskTime: number;
  isCounting: boolean;
  delta: number;
}

class Timer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      startMenuOpen: true,
      chosenEdit: 0,
      selectedMission: 100,
      nameInput: '',
      timeInput: '',
      missionInputOpen: false,
      taskInputOpen: false,
      advOptionsOpen: false,
      rmvAddOptionOpen: false,
      timeSplitOpen: false,
      parentMission: taskList.ParentTasks,
      currentTask: 0,
      currentMissionTime: 0,
      currentTaskTime: 0,
      isCounting: false,
      delta: 0,
    };
    this.startTimer = this.startTimer.bind(this);
    this.countDown = this.countDown.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleOnDragEnd = this.handleOnDragEnd.bind(this);
  }

  handleChange(event: ChangeEvent<HTMLInputElement>, type: string): void {
    switch (type) {
      case 'name': {
        this.setState({ nameInput: event.target.value });
        break;
      }
      case 'time': {
        this.setState({ timeInput: event.target.value });
        break;
      }
      default: {
        console.log("HandleChange didn't get passed the right argument for <type: string>");
        break;
      }
    }
  }

  handleEdit(ID: number): void {
    const { parentMission } = this.state;
    let { timeInput, chosenEdit } = this.state;
    if (this.state.timeInput) {
      let taskIndex = 0;
      const newTime = unpackInput(timeInput);
      const missionIndex = this.findIndex(ID);
      const oldTask = parentMission[missionIndex].childTasks.filter((i) => i.id === ID);
      parentMission[missionIndex].setTime -= oldTask[0].setTime;
      parentMission[missionIndex].setTime += newTime;
      for (let i = 0; i < parentMission.length; i++) {
        if (parentMission[missionIndex].childTasks[i].id === ID) {
          taskIndex = i;
          break;
        }
      }
      parentMission[missionIndex].childTasks[taskIndex].setTime = newTime;
      timeInput = '';
    }
    chosenEdit = 0;
    this.setState({ parentMission, timeInput, chosenEdit });
    this.saveJSON();
  }

  handleSubmit(event: React.FormEvent<HTMLFormElement>): void {
    event.preventDefault();
    this.addListItem(this.state.timeInput, this.state.selectedMission, this.state.nameInput);
    this.setState({ timeInput: '', nameInput: '' });
  }

  handleStartStop(): void {
    // Prevents timer from starting if current mission has no tasks of its own.
    if (!this.isTaskListEmpty(this.state.selectedMission)) {
      let { isCounting } = this.state;
      if (!isCounting) {
        this.startTimer();
        isCounting = true;
      } else {
        stopTimer();
        isCounting = false;
      }
      this.setState({ isCounting });
    }
  }

  handleOnDragEnd(result: any): void {
    const relevantMission = this.findIndex(parseInt(result.draggableId, 10));
    if (result.destination) {
      // another instance of something deeper than surface level being edited but eslint wanting const
      const { parentMission } = this.state;
      const [reorderedItem] = parentMission[relevantMission].childTasks.splice(result.source.index, 1);
      parentMission[relevantMission].childTasks.splice(result.destination.index, 0, reorderedItem);
      this.setState({ parentMission });
      this.saveJSON();
    }
  }

  removeListItem(ID: number): void {
    this.reset();
    let { parentMission, selectedMission } = this.state;
    // console.log(ID)
    if (ID % 100) {
      const index = this.findIndex(ID);
      // console.log(index)
      const oldTask = parentMission[index].childTasks.filter((i) => i.id === ID);
      parentMission[index].setTime -= oldTask[0].setTime;
      const newChildList = parentMission[index].childTasks.filter((i) => i.id !== ID);
      parentMission[index].childTasks = newChildList;
    } else {
      const newParentList = parentMission.filter((i) => i.id !== ID);
      parentMission = newParentList;
      selectedMission = parentMission[0].id;
    }
    this.setState({ parentMission, currentTask: 0, selectedMission });
    this.saveJSON();
  }

  addListItem(time: string, inputID: number, nameIn: string): void {
    const index = this.findIndex(inputID);
    const timeInSeconds = unpackInput(time);
    // eslint also wants this one made a const... not sure why as it does get changed but yeah
    const { parentMission } = this.state;
    if (time) {
      const newTask: Task = {
        title: nameIn,
        id: inputID + parentMission[index].childTasks.length + 1,
        setTime: timeInSeconds,
        difference: 0,
      };
      parentMission[index].childTasks.push(newTask);
      parentMission[index].setTime += timeInSeconds;
    } else {
      const newMission: Mission = {
        title: nameIn,
        id: this.findNewID(),
        setTime: 0,
        difference: 0,
        childTasks: [],
      };
      parentMission.push(newMission);
    }
    this.setState({ parentMission });
    this.saveJSON();
  }

  previousTask(): void {
    const { parentMission } = this.state;
    let { currentTask } = this.state;
    if (currentTask) {
      currentTask -= 1;
      this.reset();
    }

    // This clears the now current task's time difference.
    parentMission[this.findIndex(this.state.selectedMission)].childTasks[currentTask].difference = 0;
    this.setState({ currentTask, parentMission });
  }

  findNewID(): number {
    let ID = 100;
    let doesExist = true;
    while (doesExist) {
      doesExist = false;
      for (let i = 0; i < this.state.parentMission.length; i++) {
        if (ID === this.state.parentMission[i].id) {
          doesExist = true;
        }
      }
      ID += doesExist ? 100 : 0;
    }
    return ID;
  }

  reset(): void {
    const { parentMission } = this.state;
    const currentMission = this.findIndex(this.state.selectedMission);

    // If the timer is running whenever reset() is called, then only reset the current task
    if (this.state.currentTaskTime) {
      let { currentMissionTime } = this.state;
      currentMissionTime -= this.state.currentTaskTime;
      parentMission[currentMission].childTasks[this.state.currentTask].difference = 0;
      this.setState({ currentMissionTime, currentTaskTime: 0, isCounting: false });
      stopTimer();

      // If the timer is stopped and reset() is called, then reset the entire mission
    } else {
      // Resets all time differences in parentMission[], from the index in parentMission[] that reset() was called.
      // So long as the mission's tasklist is NOT empty.
      try {
        for (let i = this.state.currentTask; i >= 0; i--) parentMission[currentMission].childTasks[i].difference = 0;
        // eslint-disable-next-line no-empty
      } catch (error) {}

      this.setState({
        currentMissionTime: 0,
        currentTask: 0,
        currentTaskTime: 0,
        isCounting: false,
        delta: 0,
        parentMission,
      });
      stopTimer();
    }
  }

  saveDifferences(): void {
    // Prevents saveDifferences() from being executed if the selected mission has no childtasks.
    if (!this.isTaskListEmpty(this.state.selectedMission)) {
      const currentMission = this.state.parentMission[this.findIndex(this.state.selectedMission)];

      // If "Differences.txt" does not exist in it's directory, then make the file "Differences.txt".
      if (!fs.existsSync(DIFFPATH)) {
        fs.openSync(DIFFPATH, 'w');
      }

      // This gets the current date on the user's computer.
      const currentDate = new Date();

      // This string contains the difference data for the current mission.
      let differenceList = '';

      // A timestamp is used to mark when a difference was saved, it follows this format:
      // "Differences saved on: MM/DD/YYYY @ HH:MM:SS"
      const timeStamp = `Differences saved on: ${
        currentDate.getMonth() + 1
      }/${currentDate.getDate()}/${currentDate.getFullYear()} @ ${currentDate.getHours()}:${currentDate.getMinutes()}:${currentDate.getSeconds()}`;

      // Prints the mission name and task label on separate lines.
      differenceList += `${timeStamp}\nMission: ${currentMission.title}\nTasks:\n`;

      // Each task name is printed out, along with its difference.
      for (let i = 0; i < currentMission.childTasks.length; i++) {
        differenceList += `  ${currentMission.childTasks[i].title}: ${
          currentMission.childTasks[i].difference <= 0 ? '-' : '+'
        }${packOutput(currentMission.childTasks[i].difference)}\n`;
      }

      // Actual mission completion time.
      const completedMissionTime = currentMission.setTime - this.state.delta;

      // Various data are printed out about the time differences, including the time saved/lost between the expected
      // mission time and the actual mission time.
      differenceList += `\nExpected Mission Completion Time: ${packOutput(currentMission.setTime)}\n`;
      differenceList += `Actual Mission Time Achieved: ${packOutput(completedMissionTime)}\n`;
      differenceList += `Total Time ${
        completedMissionTime - currentMission.setTime <= 0 ? 'Saved: ' : 'Lost: '
      }${packOutput(completedMissionTime - currentMission.setTime)}\n\n`;

      // A small border is added to separate different difference data.
      differenceList += '-------------------------------------------------------------\n\n';

      // Previous difference data in Differences.txt is appended at the end of the most recent
      // data to keep an archive of dirfferences for future references.
      differenceList += fs.readFileSync(DIFFPATH, { encoding: 'utf-8', flag: 'r' });

      // The added time differences are saved onto "Differences.txt"
      fs.writeFileSync(DIFFPATH, differenceList);
    }
  }

  // currently there's no ability to save the differences for analysis after the app is closed
  // but there's the option to make that a saveable thing to the JSON
  saveJSON(): void {
    fs.writeFileSync(FILEPATH, JSON.stringify({ ParentTasks: this.state.parentMission }, null, 2));
  }

  loadNextTask(): void {
    const { parentMission } = this.state;
    let { currentTask, currentTaskTime, delta } = this.state;

    // Prevents the next task from being loaded if the current mission has no tasks.
    if (!this.isTaskListEmpty(this.state.selectedMission)) {
      // Delta will only increment if we are not on the last task in the mission OR if the timer is counting
      // (the timer is not stopped).
      if (
        currentTask !== parentMission[this.findIndex(this.state.selectedMission)].childTasks.length - 1 ||
        this.state.isCounting
      ) {
        delta +=
          parentMission[this.findIndex(this.state.selectedMission)].childTasks[currentTask].setTime -
          this.state.currentTaskTime;
      }

      // If the current task is the last task of the mission, then record the time difference of the current task and stop the timer.
      if (currentTask === parentMission[this.findIndex(this.state.selectedMission)].childTasks.length - 1) {
        console.log('End of task list');
        // Change to current time - the set time for the task.

        this.setState({ isCounting: false });
        parentMission[this.findIndex(this.state.selectedMission)].childTasks[currentTask].difference = -delta;
        stopTimer();
      } else {
        currentTask += 1;
        currentTaskTime = 0;
        parentMission[this.findIndex(this.state.selectedMission)].childTasks[currentTask - 1].difference = -delta;
      }
      this.setState({ currentTaskTime, currentTask, delta, parentMission });
    }
  }

  startTimer(): void {
    if (timer === 0) {
      timer = setInterval(this.countDown, 1000);
    }
  }

  countDown(): void {
    let { currentMissionTime, currentTaskTime } = this.state;
    currentTaskTime += 1;
    currentMissionTime += 1;
    this.setState({ currentTaskTime, currentMissionTime });
  }

  findIndex(ID: number): number {
    for (let i = 0; i < this.state.parentMission.length; i++) {
      if (Math.floor(this.state.parentMission[i].id / 100) === Math.floor(ID / 100)) {
        return i;
      }
    }
    return -1;
  }

  isTaskListEmpty(ID: number): boolean {
    // Checks to see if the selected mission has no tasks.
    // Return 'true' if it is empty, return 'false' otherwise.
    return this.state.parentMission[this.findIndex(ID)].childTasks.length === 0;
  }

  advancedOptionsMenu(): JSX.Element {
    return (
      <div style={Modal}>
        <p>Advanced Options</p>
        <div>
          <button type="button" onClick={() => this.previousTask()}>
            Prev Task
          </button>
          <button type="button" onClick={() => this.setState({ timeSplitOpen: true, advOptionsOpen: false })}>
            Open Split
          </button>
          <button type="button" onClick={() => this.setState({ rmvAddOptionOpen: true, advOptionsOpen: false })}>
            Show/Edit List
          </button>
        </div>
        <div>
          <button type="button" onClick={() => this.setState({ advOptionsOpen: false })}>
            back
          </button>
        </div>
      </div>
    );
  }

  taskDifferenceList(): JSX.Element {
    return (
      <div
        id="Time Differences"
        style={{
          ...column,
          padding: '5px',
          width: '93%',
          fontFamily: 'Arial',
          maxHeight: '150px',
          overflowY: 'scroll',
          overflow: 'auto',
        }}
      >
        {this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks.map((task) => {
          return (
            <div
              key={task.id}
              style={{
                display: 'flex',
                // Depending on if the printed out task is the current task, then that task will be highlighted.
                backgroundColor:
                  task.id ===
                  this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks[
                    this.state.currentTask
                  ].id
                    ? '#FFFF00'
                    : 'white',
              }}
            >
              <div style={{ width: '50%', textAlign: 'left' }}>{task.title}</div>
              <div style={{ width: '25%', textAlign: 'right' }}>
                {task.difference === 0 ? null : (
                  <div style={{ color: task.difference < 0 ? '#00AA00' : '#AA0000' }}>
                    {task.difference > 0 ? '+' : '-'}
                    {packOutput(task.difference)}
                  </div>
                )}
              </div>
              <div style={{ width: '25%', textAlign: 'right' }}>{packOutput(task.setTime)}</div>
            </div>
          );
        })}
      </div>
    );
  }

  timeSplitMenu(): JSX.Element {
    return (
      <div id="Time Split Menu" style={splitMainMenu}>
        {/* Large title at the top of the time split menu. */}
        <div style={timeSplitTitle}>Time Splitter</div>

        {/* This contains the actual time split table to view current time differences. */}
        <div style={differenceMenu}>
          {/* This displays the current mission title as well as the total mission time. */}
          <div
            style={{
              ...timeSplitMissionTitle,
              borderBottom: '2px solid #990000',
              justifyContent: 'space-between',
              display: 'flex',
            }}
          >
            <div>{this.state.parentMission[this.findIndex(this.state.selectedMission)].title}</div>
            <div>{packOutput(this.state.parentMission[this.findIndex(this.state.selectedMission)].setTime)}</div>
          </div>

          {/* This block will print out the task list, along with its respected target times. If the
             currently selected mission has no tasks, then do not attempt to render any task data. */}
          <div>{this.isTaskListEmpty(this.state.selectedMission) ? 'No Tasks Listed' : this.taskDifferenceList()}</div>
        </div>

        <div style={{ ...timeSplitMissionTitle, paddingTop: '5px', justifyContent: 'space-between', display: 'flex' }}>
          <div>Current Mission Time:</div>
          <div>{packOutput(this.state.currentMissionTime)}</div>
        </div>

        <div
          style={{ ...timeSplitMissionTitle, paddingBottom: '5px', justifyContent: 'space-between', display: 'flex' }}
        >
          <div>Current Task Time:</div>
          <div>{packOutput(this.state.currentTaskTime)}</div>
        </div>

        <div>
          <button
            type="button"
            style={{ height: '50px', width: 'auto', fontSize: '30px' }}
            onClick={() => this.saveDifferences()}
          >
            Save Differences
          </button>
        </div>

        <div>
          <button type="button" onClick={() => this.setState({ timeSplitOpen: false, advOptionsOpen: true })}>
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
    );
  }

  editMenu(): JSX.Element {
    return (
      <div style={{ ...Modal, zIndex: 7 }}>
        <p>Edit Task List</p>
        <form onSubmit={this.handleSubmit}>
          <div>
            <button
              type="button"
              onClick={() => this.setState((prevState) => ({ missionInputOpen: !prevState.missionInputOpen }))}
            >
              Add Mission
            </button>
            <button
              type="button"
              onClick={() => this.setState((prevState) => ({ taskInputOpen: !prevState.taskInputOpen }))}
            >
              Add Task
            </button>
          </div>
          {this.state.taskInputOpen ? (
            <div>
              <input
                value={this.state.nameInput}
                id="TaskNameInput"
                onChange={(e) => this.handleChange(e, 'name')}
                placeholder="Task Name"
              />
              <input
                value={this.state.timeInput}
                id="TaskTimeInput"
                onChange={(e) => this.handleChange(e, 'time')}
                placeholder="HH:MM:SS"
              />
              <input type="submit" id="NewTaskSubmit" value="Submit" />
            </div>
          ) : null}
          {this.state.missionInputOpen ? (
            <div>
              <input
                value={this.state.nameInput}
                id="MissionNameInput"
                onChange={(e) => this.handleChange(e, 'name')}
                placeholder="Mission Name"
              />
              <input type="submit" id="NewMissionSubmit" value="Submit" />
            </div>
          ) : null}
          <div
            style={{
              ...column,
              padding: '5px',
              justifyContent: 'space-between',
              alignItems: 'center',
              flexBasis: 'content',
            }}
          >
            {this.state.parentMission.map((mission) => {
              return (
                <div key={mission.id} style={{ ...row, width: '100%' }}>
                  <input
                    type="radio"
                    value={mission.id}
                    id={mission.id.toString()}
                    checked={this.state.selectedMission === mission.id}
                    onChange={() => {
                      // Reset is called to reset the time differences in the time split menu.
                      this.reset();
                      this.setState({
                        selectedMission: mission.id,
                        currentTask: 0,
                        currentMissionTime: 0,
                        currentTaskTime: 0,
                        delta: 0,
                      });
                    }}
                  />
                  <div style={{ ...column, height: '20px', padding: '2px', width: '23%' }}>
                    <div style={{ wordWrap: 'break-word' }}>{mission.title}</div>
                    {packOutput(mission.setTime)}
                  </div>
                  <button
                    type="button"
                    id={(mission.id * 5).toString()}
                    onClick={() => this.removeListItem(mission.id)}
                    style={trashButtons}
                  >
                    <img src={TrashCanIcon} alt="Trash Can Icon" />
                  </button>
                  <div style={{ width: '63%', maxWidth: '63%' }}>
                    <DragDropContext onDragEnd={this.handleOnDragEnd}>
                      <Droppable droppableId={mission.id.toString()}>
                        {(providedDrop: any) => (
                          <ul
                            style={ulStyle}
                            className="tasks"
                            {...providedDrop.droppableProps}
                            ref={providedDrop.innerRef}
                          >
                            <div style={{ ...column }}>
                              {mission.childTasks.map((subTask, index) => {
                                return (
                                  <Draggable key={subTask.id} draggableId={subTask.id.toString()} index={index}>
                                    {(provided: any) => (
                                      <li
                                        ref={provided.innerRef}
                                        {...provided.draggableProps}
                                        {...provided.dragHandleProps}
                                      >
                                        <div style={{ ...row, padding: '3px' }}>
                                          <img
                                            style={{ height: '36px' }}
                                            src={ListReorderIcon}
                                            alt="Drag_n_Drop Icon"
                                          />
                                          <div style={{ width: '48%' }}>{subTask.title}</div>
                                          <div style={{ width: '28%' }}>
                                            {this.state.chosenEdit === subTask.id ? (
                                              <div>
                                                <input
                                                  value={this.state.timeInput}
                                                  onChange={(e) => this.handleChange(e, 'time')}
                                                  placeholder={packOutput(subTask.setTime)}
                                                  style={{ width: '60%' }}
                                                />
                                                <button type="button" onClick={() => this.handleEdit(subTask.id)}>
                                                  save
                                                </button>
                                              </div>
                                            ) : (
                                              <div>
                                                {packOutput(subTask.setTime)}
                                                <button
                                                  style={{ margin: '2px' }}
                                                  type="button"
                                                  onClick={() => this.setState({ chosenEdit: subTask.id })}
                                                >
                                                  Edit
                                                </button>
                                              </div>
                                            )}
                                          </div>
                                          <button
                                            type="button"
                                            onClick={() => this.removeListItem(subTask.id)}
                                            style={trashButtons}
                                          >
                                            <img src={TrashCanIcon} alt="Trash Can Icon" />
                                          </button>
                                        </div>
                                      </li>
                                    )}
                                  </Draggable>
                                );
                              })}
                              {providedDrop.placeholder}
                            </div>
                          </ul>
                        )}
                      </Droppable>
                    </DragDropContext>
                  </div>
                </div>
              );
            })}
          </div>
        </form>
        <button type="button" onClick={() => this.setState({ rmvAddOptionOpen: false, advOptionsOpen: true })}>
          back
        </button>
        <button
          type="button"
          onClick={() => this.setState({ rmvAddOptionOpen: false, advOptionsOpen: false, timeSplitOpen: false })}
        >
          close all
        </button>
      </div>
    );
  }

  startMenu(): JSX.Element {
    return (
      <div style={{ ...Modal, width: '50%' }}>
        <div style={{ paddingTop: '5px' }}>Select Mission:</div>
        <div style={{ ...column, padding: '5px' }}>
          {this.state.parentMission.map((mission) => {
            return (
              <div key={mission.id} style={row}>
                <button
                  type="button"
                  style={{ width: '100%' }}
                  onClick={() => this.setState({ selectedMission: mission.id, startMenuOpen: false })}
                >
                  {mission.title}
                </button>
              </div>
            );
          })}
        </div>
        <div style={{ paddingBottom: '2px' }}>
          <button type="button" onClick={() => this.setState({ startMenuOpen: false })}>
            close
          </button>
        </div>
      </div>
    );
  }

  render(): JSX.Element {
    return (
      <div id="Primary Container">
        <div style={label}>Timer</div>
        <div style={{ ...container, ...column }}>
          <div id="TotalTimeContainer" style={{ ...column, marginTop: '-2.5%', width: '98%' }}>
            <p style={{ marginBottom: '0px', fontSize: '15px', marginTop: '10px', fontWeight: 'bold' }}>
              {this.state.parentMission[this.findIndex(this.state.selectedMission)].title}
            </p>

            <ProgressBar
              current={this.state.currentMissionTime}
              // Prevents a newly instantiated mission from having it's non-existent time being accessed.
              total={
                // Makes progress bar green when a mission has no child tasks.
                this.state.parentMission.length
                  ? this.state.parentMission[this.findIndex(this.state.selectedMission)].setTime
                  : NaN
              }
              name="total"
            />

            <div style={{ ...timeRead, marginLeft: '1%', marginRight: '1%', marginTop: '-2.8%' }}>
              <p>{packOutput(this.state.currentMissionTime)}</p>
              <p>
                -
                {packOutput(
                  this.state.currentMissionTime -
                    this.state.parentMission[this.findIndex(this.state.selectedMission)].setTime
                )}
              </p>
            </div>
          </div>

          <div id="CurrentTaskContainer" style={{ ...column, marginTop: '-5%', width: '100%' }}>
            <p style={{ margin: '0px', fontSize: '23px', fontWeight: 'bold' }}>
              {
                // Renders "No Tasks" if the current mission has an empty array of tasks.
                this.isTaskListEmpty(this.state.selectedMission)
                  ? 'No Tasks Listed'
                  : this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks[
                      this.state.currentTask
                    ].title
              }
            </p>

            <ProgressBar
              current={this.state.currentTaskTime}
              // Prevents non-existent task time data from being accessed
              total={
                this.isTaskListEmpty(this.state.selectedMission)
                  ? NaN
                  : this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks[
                      this.state.currentTask
                    ].setTime
              }
              name="other"
            />
            <div
              style={{
                ...timeRead,
                fontSize: '20px',
                marginTop: '-3.5%',
                marginLeft: '9%',
                marginRight: '9%',
                fontWeight: 'bold',
              }}
            >
              <p>
                {this.isTaskListEmpty(this.state.selectedMission) ? '0:00:00' : packOutput(this.state.currentTaskTime)}
              </p>
              <p>
                -
                {this.isTaskListEmpty(this.state.selectedMission)
                  ? '0:00:00'
                  : packOutput(
                      this.state.currentTaskTime -
                        this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks[
                          this.state.currentTask
                        ].setTime
                    )}
              </p>
            </div>
          </div>

          <div id="NextTaskContainer" />
          <div
            id="StartStopContainer"
            style={{
              display: 'flex',
              marginTop: '-3%',
              flexBasis: 'auto',
            }}
          >
            <div style={{ flexGrow: 3, width: '33%' }}>
              <button type="button" style={mainButtons} onClick={() => this.handleStartStop()}>
                {this.state.isCounting ? <div>STOP</div> : <div>START</div>}
              </button>
            </div>
            <div style={{ ...column, flexGrow: 1 }}>
              <button type="button" style={{ height: '20px' }} onClick={() => this.setState({ advOptionsOpen: true })}>
                Advanced Options
              </button>
              <button type="button" style={{ height: '30px', fontSize: '23px' }} onClick={() => this.reset()}>
                RESET
              </button>
            </div>
            <div style={{ flexGrow: 3, width: '33%' }}>
              <button type="button" style={mainButtons} onClick={() => this.loadNextTask()}>
                {this.state.currentTask !==
                this.state.parentMission[this.findIndex(this.state.selectedMission)].childTasks.length - 1 ? (
                  <div>NEXT TASK</div>
                ) : (
                  <div>END MISSION</div>
                )}
              </button>
            </div>
          </div>
          {this.state.startMenuOpen ? this.startMenu() : null}
          {this.state.advOptionsOpen ? this.advancedOptionsMenu() : null}
          {this.state.timeSplitOpen ? this.timeSplitMenu() : null}
          {this.state.rmvAddOptionOpen ? this.editMenu() : null}
        </div>
      </div>
    );
  }
}
export default Timer;
