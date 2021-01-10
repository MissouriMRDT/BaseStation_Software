import CSS from "csstype"

export default function ColorConverter(
  value: number,
  min: number,
  cutoff: number,
  max: number,
  lowHue: number,
  highHue: number,
  otherProperties: CSS.Properties
): CSS.Properties {
  let newStyle: CSS.Properties = {}
  if (value <= min) {
    newStyle = { backgroundColor: `hsl(${lowHue}, 100%, ${50}%)` }
  } else if (value > min && value <= cutoff) {
    newStyle = {
      backgroundColor: `hsl(${lowHue}, 100%, ${((value - min) / (cutoff - min) / 2) * 100 + 50}%)`,
    }
  } else if (value > cutoff && value < max) {
    newStyle = {
      backgroundColor: `hsl(${highHue}, 100%, ${((max - value) / (max - cutoff) / 2) * 100 + 50}%)`,
    }
  } else if (value >= max) {
    newStyle = { backgroundColor: `hsl(${highHue}, 100%, ${50}%)` }
  }
  console.log(otherProperties, newStyle)
  return { ...otherProperties, ...newStyle }
}
