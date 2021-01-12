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

  private link = document.createElement("link")

  // This will keep a reference of the window
  private externalWindow: Window | null = null
  // When the component mounts, Open a new window

  // When the component mounts, Open a new window
  componentDidMount(): void {
    // The second argument in window.open is optional and can be set to whichever
    // value you want. You will notice the use of this value when we modify the main
    // electron.js file
    this.externalWindow = window.open("", this.props.name)

    this.link.type = "text/css"
    this.link.rel = "stylesheet"
    this.link.href = "https://unpkg.com/react-table-v6@latest/react-table.css"

    // Append the container div and register the event that will get fired when the
    // window is closed
    if (this.externalWindow) {
      this.externalWindow.document.body.appendChild(this.link)
      this.externalWindow.document.body.appendChild(this.containerEl)
      this.externalWindow.onunload = () => this.props.onClose()
    }
  }

  render(): JSX.Element {
    return ReactDOM.createPortal(this.props.children, this.containerEl)
  }
}
