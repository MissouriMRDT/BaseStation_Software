import { Component } from "react"
import ReactDOM from "react-dom"

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
    this.reactTableLink.href = "https://unpkg.com/react-table-v6@latest/react-table.css"

    this.reactVisLink.type = "text/css"
    this.reactVisLink.rel = "stylesheet"
    this.reactVisLink.href = "https://unpkg.com/react-vis/dist/style.css"

    // Append the container div and register the event that will get fired when the
    // window is closed
    if (this.externalWindow) {
      this.externalWindow.document.body.appendChild(this.reactTableLink)
      this.externalWindow.document.body.appendChild(this.reactVisLink)
      this.externalWindow.document.body.appendChild(this.containerEl)
      this.externalWindow.onunload = () => this.props.onClose()
    }
  }

  render(): JSX.Element {
    return ReactDOM.createPortal(this.props.children, this.containerEl)
  }
}
