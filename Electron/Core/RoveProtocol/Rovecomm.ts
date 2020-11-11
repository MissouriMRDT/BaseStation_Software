import { DATAID } from "./RovecommManifest"
/* eslint-disable @typescript-eslint/no-var-requires */
const dgram = require("dgram")
const net = require("net")
const EventEmitter = require("events")

enum DataTypes {
  INT8_T = 0,
  UINT8_T = 1,
  INT16_T = 2,
  UINT16_T = 3,
  INT32_T = 4,
  UINT32_T = 5,
  FLOAT_T = 6,
}

const dataSizes = [1, 1, 2, 2, 4, 4, 2]

function decodePacket(
  dataType: number,
  dataLength: number,
  data: Buffer
): number[] {
  let readBytes: (i: number) => number
  switch (dataType) {
    case DataTypes.INT8_T:
      readBytes = data.readInt8.bind(data)
      break
    case DataTypes.UINT8_T:
      readBytes = data.readUInt8.bind(data)
      break
    case DataTypes.INT16_T:
      readBytes = data.readInt16BE.bind(data)
      break
    case DataTypes.UINT16_T:
      readBytes = data.readUInt16BE.bind(data)
      break
    case DataTypes.INT32_T:
      readBytes = data.readInt32BE.bind(data)
      break
    case DataTypes.UINT32_T:
      readBytes = data.readUInt32BE.bind(data)
      break
    case DataTypes.FLOAT_T:
      readBytes = data.readFloatBE.bind(data)
      break
    default:
      return []
  }

  const retArray = []
  let offset: number
  for (let i = 0; i < dataLength; i += 1) {
    offset = i * dataSizes[dataType]
    retArray.push(readBytes(offset))
  }
  return retArray
}

function parse(packet: Buffer): void {
  // RoveComm Header Format:
  //
  //  0                   1                   2                   3
  //  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
  // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
  // |    Version    |            Data Id            |  Data Length  |
  // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
  // |   Data Type   |                Data (Variable)                |
  // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
  //
  // Note: the size of Data is dataSizes[DataType] * dataSizes bytes

  const VersionNumber = 2

  const version = packet.readUInt8(0)
  const dataId = packet.readUInt16BE(1)
  const dataLength = packet.readUInt8(3)
  const dataType = packet.readUInt8(4)

  const rawdata = packet.slice(5)
  let data: number[]

  if (version === VersionNumber) {
    data = decodePacket(dataType, dataLength, rawdata)

    let dataIdStr = "null"
    for (let i = 0; i < DATAID.length; i++) {
      if (dataId in Object.keys(DATAID[i].Telemetry)) {
        dataIdStr = DATAID[i].Telemetry[dataId]
      }
    }

    // eslint-disable-next-line
    rovecomm.emit(dataIdStr, data)
    // eslint-disable-next-line
    rovecomm.emit(
      "all",
      `Data Id: ${dataId} (aka ${dataIdStr}), Type: ${dataType}, Length: ${dataLength}, Data: ${data}`
    )
  }
}

function TCPListen(socket: any) {
  socket.on("message", (msg: Buffer) => {
    parse(msg)
  })
}

class Rovecomm extends EventEmitter {
  UDPSocket: any

  TCPServer: any

  constructor() {
    super()
    this.UDPSocket = dgram.createSocket("udp4")
    this.TCPServer = net.createServer((TCPSocket: any) => TCPListen(TCPSocket))

    this.UDPListen()
    this.TCPServer.listen(11111)
  }

  UDPListen() {
    this.UDPSocket.on("message", (msg: Buffer) => {
      parse(msg)
    })
    this.UDPSocket.bind(11000)
  }

  sendUDP(packet: Buffer, destinationIp: string, port = 11000): void {
    this.UDPSocket.send(packet, port, destinationIp)
  }

  sendCommand(dataIdStr: string, data: any, reliability = false): void {
    const VersionNumber = 2
    const dataLength = data.length
    let destinationIp = ""
    let port = 8081
    let dataType
    let dataId
    for (let i = 0; i < DATAID.length; i++) {
      if (dataIdStr in DATAID[i].Commands) {
        destinationIp = DATAID[i].Ip
        port = parseInt(DATAID[i].Port, 10)
        dataType = DATAID[i].Commands[dataIdStr].dataType
        dataId = DATAID[i].Commands[dataIdStr].dataId
        break
      }
    }
    const headerBuffer = Buffer.allocUnsafe(5)
    headerBuffer.writeUInt8(VersionNumber, 0)
    headerBuffer.writeUInt16BE(dataId, 1)
    headerBuffer.writeUInt8(dataLength, 3)
    headerBuffer.writeUInt8(dataType, 4)
    const dataBuffer = Buffer.allocUnsafe(dataLength * dataSizes[dataType])
    switch (dataType) {
      case DataTypes.INT8_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeInt8(data[i], i * dataSizes[DataTypes.INT8_T])
        }
        break
      case DataTypes.UINT8_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeUInt8(data[i], i * dataSizes[DataTypes.UINT8_T])
        }
        break
      case DataTypes.INT16_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeInt16BE(data[i], i * dataSizes[DataTypes.INT16_T])
        }
        break
      case DataTypes.UINT16_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeUInt16BE(data[i], i * dataSizes[DataTypes.UINT16_T])
        }
        break
      case DataTypes.INT32_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeInt32BE(data[i], i * dataSizes[DataTypes.INT32_T])
        }
        break
      case DataTypes.UINT32_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeUInt32BE(data[i], i * dataSizes[DataTypes.UINT32_T])
        }
        break
      case DataTypes.FLOAT_T:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeFloatBE(data[i], i * dataSizes[DataTypes.FLOAT_T])
        }
        break
      default:
        for (let i = 0; i < data.length; i++) {
          dataBuffer.writeUInt8(0, i * dataSizes[DataTypes.INT8_T])
        }
        break
    }
    const packet = Buffer.concat([headerBuffer, dataBuffer])
    if (reliability === false) {
      this.sendUDP(packet, destinationIp)
    } else {
      this.sendTCP(packet, destinationIp, port)
    }
  }
}
export const rovecomm = new Rovecomm()
