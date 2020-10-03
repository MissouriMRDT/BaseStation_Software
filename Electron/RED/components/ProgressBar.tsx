import React from "react"

interface IProps {
  current: number
  total: number
}

class ProgressBar extends React.Component<IProps> {
  calculatePercent = (current: number, total: number): number => {
    return (current / total) * 100
  }

  render(): JSX.Element {
    return (
      <div style={{ background: "gray" }}>
        <div
          style={{
            background: "#990000",
            height: 28,
            width: `${
              100 - this.calculatePercent(this.props.current, this.props.total)
            }%`,
          }}
        />
      </div>
    )
  }
}

export default ProgressBar
