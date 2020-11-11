import { Socket } from "dgram"
import { Server } from "http"
import { DATAID, dataSizes, DataTypes } from "./RovecommManifest"

// There is a fundamental implementation difference between these required imports
// and the traditional typescript imports.
/* eslint-disable @typescript-eslint/no-var-requires */
const dgram = require("dgram")
const net = require("net")
const EventEmitter = require("events")

function decodePacket(
  dataType: number,
  dataLength: number,
  data: Buffer
): number[] {
  /*
   * Takes in a dataType, dateLength, and data from an incoming rovecomm packet,
   * and uses the dataType to return an array of the properly typed data.
   * Note: even if dataLength is only 1, this returns an array of one item.
   */

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
  /*
   * Parse takes in a packet buffer and will call decodePacket and emit
   *  the rovecomm event with the proper dataId and that typed data
   *
   *  RoveComm Header Format:
   *
   *   0                   1                   2                   3
   *   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   *  |    Version    |            Data Id            |  Data Length  |
   *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   *  |   Data Type   |                Data (Variable)                |
   *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   *
   *  Note: the size of Data is dataSizes[DataType] * dataSizes bytes
   */

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
    // Here we loop through all of the Boards in the manifest,
    // looking specifically if this dataId is a known Telemetry of the board
    for (let i = 0; i < DATAID.length; i++) {
      if (dataId in DATAID[i].Telemetry) {
        dataIdStr = DATAID[i].Telemetry[dataId]
      }
    }

    // rovecomm depends on parse, and parse depends on rovecomm. Since parse is defined with the
    // function keyword, this isn't a problem, but we don't want to disable this use before definition
    // error for the whole file or project since it can be risky

    // First emit is for the dataIdString (like DriveSpeeds)
    // eslint-disable-next-line @typescript-eslint/no-use-before-define
    rovecomm.emit(dataIdStr, data)

    // Second emit is for "all" which is used for logging purposes
    // eslint-disable-next-line @typescript-eslint/no-use-before-define
    rovecomm.emit(
      "all",
      `Data Id: ${dataId} (aka ${dataIdStr}), Type: ${dataType}, Length: ${dataLength}, Data: ${data}`
    )

    // More emits will potentially follow to allow RON to listen to only a certain board,
    // Telemetry vs Commands vs Errors, etc.
  }
}

function TCPListen(socket: Socket) {
  /*
   * Listens on the passed in TCP socket, always calling parse if it recieves anything
   */
  socket.on("message", (msg: Buffer) => {
    parse(msg)
  })
}

class Rovecomm extends EventEmitter {
  UDPSocket: Socket

  TCPServer: Server

  constructor() {
    super()

    // Initialization of UDP socket and server
    this.UDPSocket = dgram.createSocket("udp4")
    this.TCPServer = net.createServer((TCPSocket: Socket) =>
      TCPListen(TCPSocket)
    )

    this.UDPListen()
    this.TCPServer.listen(11111)

    this.resubscribe = this.resubscribe.bind(this)
  }

  UDPListen() {
    /*
     * Listens on the class UDP socket, always calling parse if it recieves anything,
     * and properly binding the socket to a port
     */
    this.UDPSocket.on("message", (msg: Buffer) => {
      parse(msg)
    })
    this.UDPSocket.bind(11000)
  }

  sendUDP(packet: Buffer, destinationIp: string, port = 11000): void {
    /*
     * Takes a packet (Buffer) and sends it out over the existing UDP socket
     * to the correct destination IP
     */
    this.UDPSocket.send(packet, port, destinationIp)
  }

  // While most "any" variable types have been removed, data really can be almost any type
  sendCommand(dataIdStr: string, data: any, reliability = false): void {
    /*
     * Takes a dataIdString, data, and optional reliability (to determine)
     * UDP or TCP, properly types the data according to the type in the manifest
     * creates a Buffer, and calls the proper send function
     */
    const VersionNumber = 2

    const dataLength = data.length
    let destinationIp = ""
    let port = 11000
    let dataType
    let dataId

    // Boolean to keep track of if the dataId was found
    let found = false
    // Here we loop through all of the Boards in the manifest,
    // looking specifically if this dataId is a known Command of the board
    for (let i = 0; i < DATAID.length; i++) {
      if (dataIdStr in DATAID[i].Commands) {
        destinationIp = DATAID[i].Ip
        port = DATAID[i].Port
        dataType = DATAID[i].Commands[dataIdStr].dataType
        dataId = DATAID[i].Commands[dataIdStr].dataId
        found = true
        break
      }
    }
    if (found === false) {
      this.emit(
        "all",
        `Attempting to send packet with DataId: ${dataIdStr} and data ${data} but dataId was not found. Packet not sent.`
      )
      return
    }

    /* Create the header buffer. Packet is formatted as below:
     *  RoveComm Header Format:
     *   0                   1                   2                   3
     *   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
     *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     *  |    Version    |            Data Id            |  Data Length  |
     *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     *  |   Data Type   |                Data (Variable)                |
     *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     *
     *  Note: the size of Data is dataLength * dataSizes[DataType] bytes
     */
    const headerBuffer = Buffer.allocUnsafe(5)
    headerBuffer.writeUInt8(VersionNumber, 0)
    headerBuffer.writeUInt16BE(dataId, 1)
    headerBuffer.writeUInt8(dataLength, 3)
    headerBuffer.writeUInt8(dataType, 4)

    // Create the data buffer
    const dataBuffer = Buffer.allocUnsafe(dataLength * dataSizes[dataType])

    // Switch on the data type, and properly encode each number in the data
    // array depending on the enumerated type, computing the offset and pushing
    // to the dataBuffer
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

    // Concatenate together the header and data before sending
    const packet = Buffer.concat([headerBuffer, dataBuffer])

    // And send over the proper reliability connection
    if (reliability === false) {
      this.sendUDP(packet, destinationIp)
    } else {
      this.sendTCP(packet, destinationIp, port)
    }
  }

  resubscribe() {
    const VersionNumber = 2
    const dataId = 3
    const dataLength = 0
    const dataType = DataTypes.UINT8_T
    const data = 0

    const subscribe = Buffer.allocUnsafe(6)
    subscribe.writeUInt8(VersionNumber, 0)
    subscribe.writeUInt16BE(dataId, 1)
    subscribe.writeUInt8(dataLength, 3)
    subscribe.writeUInt8(dataType, 4)
    subscribe.writeUInt8(data, 5)

    for (let i = 0; i < DATAID.length; i++) {
      this.sendUDP(subscribe, DATAID[i].Ip)
    }
  }
}

// Export a master rovecomm to be used by each component
export const rovecomm = new Rovecomm()
