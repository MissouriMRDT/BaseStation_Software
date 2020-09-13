import React, {Component} from 'react'
import GPS from './GPS'
import { IpcRenderer } from 'electron'

declare global {
    interface Window {
      require: (module: 'electron') => {
        ipcRenderer: IpcRenderer
    };
  }
}
  
const { ipcRenderer } = window.require('electron');

class App extends Component {
    /*
    This class is meant to be called by the index page and handles the base
    logic for what is displayed
    */

    constructor(props: Readonly<{}>) {
        super(props)

        ipcRenderer.on('targetPriceVal', (event: any) => console.log("ahh?"))
    }

    render() {
        return (
            <div>
                <h1>Control Center      Settings</h1>
                <GPS/>
            </div>
        )
    }
}

export default App