import { Component } from "react"
import ReactDOM from "react-dom"

class NewWindowComponent extends Component {
  // Create a container <div> for the window
  private containerEl = document.createElement("div")

  // This will keep a reference of the window
  private externalWindow: Window | null = null

  // When the component mounts, Open a new window
  componentDidMount() {
    // The second argument in window.open is optional and can be set to whichever
    // value you want. You will notice the use of this value when we modify the main
    // electron.js file
    this.externalWindow = window.open("", "NewWindowComponent ")

    // Append the container div and register the event that will get fired when the
    // window is closed
    if (this.externalWindow) {
      this.externalWindow.document.body.appendChild(this.containerEl)
      this.externalWindow.onunload = () => this.props.onClose()
    }
  }

  render() {
    return ReactDOM.createPortal(this.props.children, this.containerEl)
  }
}
