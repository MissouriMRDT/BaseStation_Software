/* eslint-disable max-classes-per-file */
/* eslint-disable @typescript-eslint/no-var-requires */
const db = require("electron-db")
const path = require("path")

// Documentation here: https://www.npmjs.com/package/electron-db

class Database {
  tableNames = ["spectrometers", "specdata", "testinfo"]

  location = path.join(__dirname, "../Core/Spectrometer")

  createAllTables() {
    this.tableNames.forEach(element => {
      db.createTable(element, this.location, (succ: boolean, msg: string) => {
        // succ - boolean, tells if the call is successful
        console.log(`Success: ${succ}`)
        console.log(`Location: ${this.location}`)
        console.log(`Message: ${msg}`)
      })
    })
  }

  insertData(data: SpecDataEntry) {
    db.insertTableContent(this.tableNames[1], this.location, data, (succ: boolean, msg: string) => {
      console.log(`Success: ${succ}`)
      console.log(`Message: ${msg}`)
    })
  }

  insertTestInfo(info: TestInfoEntry) {
    if (info.testID !== -1) {
      return
    }

    db.insertTableContent(this.tableNames[2], this.location, info, (succ: boolean, msg: string) => {
      console.log(`Success: ${succ}`)
      console.log(`Message: ${msg}`)
    })
  }

  retrieveAllTestInfo(callback: CallableFunction) {
    db.getAll(this.tableNames[2], this.location, (succ: boolean, data: any) => {
      callback(succ, data)
    })
  }

  retrieveAllTests(callback: CallableFunction) {
    db.getAll(this.tableNames[1], this.location, (succ: boolean, data: any) => {
      callback(succ, data)
    })
  }

  retrieveSpectrometers(callback: CallableFunction) {
    db.getAll(this.tableNames[0], this.location, (succ: boolean, data: any) => {
      callback(succ, data)
    })
  }

  retrieveTestByTimestamp(timestamp: string, callback: CallableFunction) {
    db.search(this.tableNames[1], this.location, "datetime", timestamp, (succ: boolean, data: any) => {
      callback(succ, data)
    })
  }
}

class SpectrometerEntry {
  specID = -1

  model: string

  description: string

  constructor(model: string, description: string) {
    this.model = model
    this.description = description
  }
}

class SpecDataEntry {
  specID: number

  testID = -1

  datetime: string

  life: boolean

  integral: number

  data: { x: number; y: number }[]

  constructor(specID: number, datetime: string, life: boolean, integral: number, data: { x: number; y: number }[]) {
    this.specID = specID
    this.datetime = datetime
    this.life = life
    this.integral = integral
    this.data = data
  }
}

class TestInfoEntry {
  testID = -1

  composition: string

  constructor(composition: string) {
    this.composition = composition
  }
}

export const database = new Database()
export { SpectrometerEntry, SpecDataEntry, TestInfoEntry }
