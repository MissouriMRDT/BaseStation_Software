import React, {Component} from 'react'

class GPS extends Component {
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
                {
                    [
                        {title: "Fix Obtained", value: "False"},
                        {title: "Satellite Count", value: "255"},
                        {title: "Current Lat.", value: "0"},
                        {title: "Lidar", value: "0.00"},
                        {title: "Fix Quality", value: "255"},
                        {title: "Odometer (Miles)", value: "0"},
                        {title: "Current Lon.", value: "0"},
                    ].map((datum) => {
                        var {title, value} = datum
                        return (
                            <h1>{title}: {value}</h1>
                        )
                    })
                }
            </div>
        )
    }
}



export default GPS