/*
Meant to be passed a number to be bitmasked and the list that the bitmask will be indexed against.
It takes an empty string and adds preceding zeros that would have otherwise gotten destoryed if the bitmask remained a number-type,
so that index positions remain consistent between the rover and basestation.
*/
export function BitmaskUnpack(data: number, referenceList: string[]): string {
  // .toString(2) takes the number and converts it to a binary string
  const startBit = data.toString(2)
  // "difference" is important because BitmaskUnpack only adds enough zeros to
  // make the lengths of the referenceList and bitmask equal
  const difference = referenceList.length - startBit.length
  let returnBitString = ""
  for (let i = 0; i < difference; i++) {
    returnBitString += "0"
  }
  for (let i = 0; i < startBit.length; i++) {
    returnBitString += startBit[i]
  }
  return returnBitString
}
