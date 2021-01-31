import CSS from "csstype"

export function ColorConverter(
  value: number,
  min: number,
  cutoff: number,
  max: number,
  lowHue: number,
  highHue: number
): string {
  let color = ""
  if (value <= min) {
    color = `hsl(${lowHue}, 100%, ${50}%)`
  } else if (value > min && value <= cutoff) {
    color = `hsl(${lowHue}, 100%, ${((value - min) / (cutoff - min) / 2) * 100 + 50}%)`
  } else if (value > cutoff && value < max) {
    color = `hsl(${highHue}, 100%, ${((max - value) / (max - cutoff) / 2) * 100 + 50}%)`
  } else if (value >= max) {
    color = `hsl(${highHue}, 100%, ${50}%)`
  }
  return color
}

export function ColorStyleConverter(
  value: number,
  min: number,
  cutoff: number,
  max: number,
  lowHue: number,
  highHue: number,
  otherProperties: CSS.Properties
): CSS.Properties {
  return {
    ...otherProperties,
    backgroundColor: ColorConverter(value, min, cutoff, max, lowHue, highHue),
  }
}
