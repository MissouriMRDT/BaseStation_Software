import React, {Component} from 'react'
import GPS from './GPS'

class App extends Component {
    /*
    This class is meant to be called by the index page and handles the base
    logic for what is displayed
    */

    constructor(props: Readonly<{}>) {
        super(props)
        this.state = {
            
        }
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