import React from "react"
import CSS from "csstype"

const totalBar: CSS.Properties = {
  background: "gray",
  height: "20px",
  width: "90%",
  marginLeft: "auto",
  marginRight: "auto",
}
const otherBar: CSS.Properties = {
  background: "gray",
  height: "35px",
  width: "70%",
  marginLeft: "auto",
  marginRight: "auto",
}

interface IProps {
  current: number
  total: number
  name: string
}

class ProgressBar extends React.Component<IProps> {
  calculatePercent = (current: number, total: number): number => {
    return (current / total) * 100
  }

  render(): JSX.Element {
    return (
      <div style={this.props.name === "total" ? totalBar : otherBar}>
        <div
          style={{
            background: "#990000",
            height: `${this.props.name === "total" ? "20px" : "35px"}`,
            width: `${
              this.props.current >= this.props.total ? 100 : this.calculatePercent(this.props.current, this.props.total)
            }%`,
          }}
        >
          <div
            style={{
              background: "#ca27d9",
              height: `${this.props.name === "total" ? "20px" : "35px"}`,
              /* eslint-disable */
              width: `${
                this.props.current > this.props.total
                  ? this.props.current > 2 * this.props.total
                    ? 100
                    : this.calculatePercent(this.props.current - this.props.total, this.props.total)
                  : 0
              }%`,
              /* eslint-enable */
            }}
          />
        </div>
      </div>
    )
  }
}

export default ProgressBar
