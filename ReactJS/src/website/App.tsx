import React, {Component} from 'react'
import GPS from './GPS'
import { IpcRenderer } from 'electron'
import { css, jsx } from '@emotion/core'
import CSS from 'csstype'


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
                <h1 style={h1Style}>Control Center      Settings</h1>
                <GPS/>
            </div>
        )
    }
}


const h1Style: CSS.Properties = {
    backgroundColor: 'gray',
    right: '200rem',
    bottom: '2rem',
    padding: '0.5rem',
    fontFamily: 'arial',
    fontSize: '1rem',
    boxShadow: '0 0 20px rgba(0, 0, 0, 0.3)'
  }
  


export default App