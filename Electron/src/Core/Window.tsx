import path from "path"
import { Component } from "react"
import ReactDOM from "react-dom"

const reactTable = path.join(__dirname, "../assets/react-table.css")
const reactVis = path.join(__dirname, "../assets/react-vis.css")
const leafletStyle = path.join(__dirname, "../assets/leaflet.css")
const leafletJS = path.join(__dirname, "../assets/leaflet.js")
export const windows: any = { RED: window }

interface IProps {
  onClose: any
  name: string
}

interface IState {}

export default class NewWindowComponent extends Component<IProps, IState> {
  // Create a container <div> for the window
  private containerEl = document.createElement("div")

  private reactTableLink = document.createElement("link")

  private reactVisLink = document.createElement("link")

  private leafletLink = document.createElement("link")

  private leafletScript = document.createElement("script")

  // This will keep a reference of the window
  private externalWindow: Window | null = null
  // When the component mounts, Open a new window

  // When the component mounts, Open a new window
  componentDidMount(): void {
    // The second argument in window.open is optional and can be set to whichever
    // value you want. You will notice the use of this value when we modify the main
    // electron.js file
    this.externalWindow = window.open("", this.props.name)

    this.reactTableLink.type = "text/css"
    this.reactTableLink.rel = "stylesheet"
    this.reactTableLink.href = reactTable
    this.reactVisLink.type = "text/css"
    this.reactVisLink.rel = "stylesheet"
    this.reactVisLink.href = reactVis
    this.leafletLink.type = "text/css"
    this.leafletLink.rel = "stylesheet"
    this.leafletLink.href = leafletStyle
    this.leafletScript.src = leafletJS

    // Append the container div and register the event that will get fired when the
    // window is closed
    if (this.externalWindow) {
      this.externalWindow.document.body.appendChild(this.reactTableLink)
      this.externalWindow.document.body.appendChild(this.reactVisLink)
      this.externalWindow.document.body.appendChild(this.leafletLink)
      this.externalWindow.document.body.appendChild(this.leafletScript)
      this.externalWindow.document.body.appendChild(this.containerEl)
      windows[this.props.name] = this.externalWindow
      this.externalWindow.onunload = () => {
        this.props.onClose()
        delete windows[this.props.name]
      }
    }
  }

  render(): JSX.Element {
    return ReactDOM.createPortal(this.props.children, this.containerEl)
  }
}
