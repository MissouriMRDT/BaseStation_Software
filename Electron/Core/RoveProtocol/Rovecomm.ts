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

const DataLength = [1, 1, 2, 2, 4, 4, 2]

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
    offset = i * DataLength[dataType]
    retArray.push(readBytes(offset))
  }
  return retArray
}

export function parse(packet: Buffer): string {
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
  // Note: the size of Data is sizes[DataType] * DataLength bytes

  const VersionNumber = 2

  const version = packet.readUInt8(0)
  const dataId = packet.readUInt16BE(1)
  const dataLength = packet.readUInt8(3)
  const dataType = packet.readUInt8(4)

  const rawdata = packet.slice(5)
  let data: number[]

  if (version === VersionNumber) {
    console.log(dataId)
    data = decodePacket(dataType, dataLength, rawdata)
    // eslint-disable-next-line
    rovecomm.emit(DATAID[dataId], data)
    // eslint-disable-next-line
    rovecomm.emit(
      "all",
      `Data Id: ${dataId} (aka ${DATAID[dataId]}), Type: ${dataType}, Length: ${dataLength}, Data: ${data}`
    )
  } else {
    return "null"
  }

  for (let i = 0; i < DATAID.length; i++) {
    if (dataId in Object.keys(DATAID[i].Telemetry)) {
      return DATAID[i].Telemetry[dataId]
    }
  }

  return "null"
}

function TCPListen(socket: any) {
  socket.on("data", (data: any) => {
    console.log(data, data.toString())
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
    this.UDPSocket.on(
      "message",
      (msg: Buffer, rinfo: { address: string; port: number }) => {
        console.log(`server got: ${msg} from ${rinfo.address}:${rinfo.port}`)
        parse(msg)
      }
    )
    this.UDPSocket.bind(11000)
  }
}
export const rovecomm = new Rovecomm()

export function sendCommand(dataId: number, data: any, reliability = false) {
  const VersionNumber = 2
  const dataLength = data.length
  let destinationIp
  let port
  let dataType
  for (let i = 0; i < DATAID.length; i++) {
    if (dataId in Object.keys(DATAID[i].Commands)) {
      destinationIp = DATAID[i].Ip
      port = DATAID[i].Port
      dataType = DATAID[i].Commands[dataId]
    }
  }

  if (reliability === false) {
    rovecomm.sendUDP(
      [VersionNumber, dataId, dataLength, dataType, data],
      destinationIp,
      port
    )
  } else {
    rovecomm.sendTCP(
      [VersionNumber, dataId, dataLength, dataType, data],
      destinationIp,
      port
    )
  }
  console.log(
    `Not yet implemented. Recieved ${dataId}: ${data}, ${reliability}`
  )
}
