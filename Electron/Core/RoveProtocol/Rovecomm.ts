import { Socket } from "dgram"
import { Server } from "http"
import Deque from "double-ended-queue"
import { DATAID, dataSizes, DataTypes } from "./RovecommManifest"

// There is a fundamental implementation difference between these required imports
// and the traditional typescript imports.
/* eslint-disable @typescript-eslint/no-var-requires */
const dgram = require("dgram")
const net = require("net")
const EventEmitter = require("events")

interface TCPSocket {
  RCSocket: Socket
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  RCDeque: Deque<any>
  newData: boolean
}

function decodePacket(
  dataType: number,
  dataCount: number,
  data: Buffer
): number[] {
  /*
   * Takes in a dataType, dateLength, and data from an incoming rovecomm packet,
   * and uses the dataType to return an array of the properly typed data.
   * Note: even if dataCount is only 1, this returns an array of one item.
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
  for (let i = 0; i < dataCount; i += 1) {
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
   *  |    Version    |            Data Id            |  Data  Count  |
   *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   *  |   Data Type   |                Data (Variable)                |
   *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   *
   *  Note: the size of Data is dataSizes[DataType] * dataSizes bytes
   */

  const VersionNumber = 2

  const version = packet.readUInt8(0)
  const dataId = packet.readUInt16BE(1)
  const dataCount = packet.readUInt8(3)
  const dataType = packet.readUInt8(4)

  const rawdata = packet.slice(5)
  let data: number[]

  if (version === VersionNumber) {
    data = decodePacket(dataType, dataCount, rawdata)

    let dataIdStr = "null"
    let endLoop = false
    // Here we loop through all of the Boards in the manifest,
    // looking specifically if this dataId is a known Telemetry of the board
    for (let i = 0; i < DATAID.length; i++) {
      // eslint-disable-next-line no-restricted-syntax
      for (const comm in DATAID[i].Telemetry) {
        if (dataId === DATAID[i].Telemetry[comm].dataId) {
          dataIdStr = comm
          endLoop = true
          break
        }
      }
      if (endLoop) {
        break
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
      `Data Id: ${dataId} (aka ${dataIdStr}), Type: ${dataType}, Count: ${dataCount}, Data: ${data}`
    )

    // More emits will potentially follow to allow RON to listen to only a certain board,
    // Telemetry vs Commands vs Errors, etc.
  }
}

function TCPParseWrapper(socket: TCPSocket) {
  // While the Deque contains at least a header to allow parsing control packets
  while (socket.RCDeque.length >= 5) {
    // Create an empty list to fill with the header contents
    const header = []
    for (let i = 0; i < 5; i++) {
      // Fill the list with the header contents, the first 5 bytes will only ever be the header of a packet if we read correctly
      header.push(socket.RCDeque.get(i))
    }
    // If the length of the Deque is more than the header size and the size of the packet, make a buffer and then parse that buffer
    if (socket.RCDeque.length > header[3] * header[4] + 5) {
      // create another list to put the entire packet into
      const packet = []
      for (let i = 0; i < 5 + header[3] * header[4]; i++) {
        packet.push(socket.RCDeque.shift())
      }
      // Make a buffer from that list
      const packetBuffer = Buffer.from(packet)
      parse(packetBuffer)
      // If the Deque doesn't contain a full packet, return
    } else return
  }
}

function TCPListen(socket: TCPSocket) {
  /*
   * Listens on the passed in TCP socket, always calling the TCP Parse Wrapper if data is received
   */
  const selfComp = 1
  socket.RCSocket.on("data", (msg: Buffer) => {
    // eslint-disable-next-line no-restricted-syntax
    for (let i = 0; i < msg.length; i++) {
      // use this if compared to self to get rid of the linter no self compare
      // eslint-disable-next-line no-self-compare
      if (selfComp === selfComp) {
        socket.RCDeque.push(msg[i])
      }
    }
    TCPParseWrapper(socket)
  })
}

class Rovecomm extends EventEmitter {
  UDPSocket: Socket

  TCPServer: Server

  TCPConnections: TCPSocket[]

  constructor() {
    super()
    this.TCPConnections = []
    // Initialization of UDP socket and server
    this.UDPSocket = dgram.createSocket("udp4")
    // TCPServer is purely for testing purposes and should not be relied on for use on Rover
    this.TCPServer = net.createServer((TCPSocket: Socket) =>
      TCPSocket.on("data", (msg: Buffer) => {
        this.parse(msg)
      })
    )

    this.UDPListen()
    this.TCPServer.listen(11111)
    this.createTCPConnection(11001, "192.168.1.131")
    this.resubscribe = this.resubscribe.bind(this)
  }

  createTCPConnection(port: number, host = "localhost") {
    console.log(host, port)
    // eslint-disable-next-line no-restricted-syntax
    for (const socket in this.TCPConnections) {
      if (
        this.TCPConnections[socket].RCSocket.remoteAddress === host &&
        this.TCPConnections[socket].RCSocket.remotePort === port
      ) {
        console.log(
          `Attempted to add a second connection to ${host}:${port}, didn't add`
        )
        return
      }
    }

    const newSocket: TCPSocket = {
      // New socket to connect with
      RCSocket: new net.Socket(),
      // Instantiate a new Deque of size 30KB, avoid runtime resizing
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      RCDeque: new Deque<any>(30 * 1024 * 1024),
      // New Data flag, to determine if we need to parse or not (async read)
      newData: false,
    }
    newSocket.RCSocket = new net.Socket()

    // Connect to the board we're intending to communicate with
    newSocket.RCSocket.connect(port, host, function handler() {
      console.log(`Connected to ${host} on Port: ${port}`)
    })

    // Listen for any data coming on this connection and parse it as it arrives
    TCPListen(newSocket)

    // Add the connection to the TCPConnections list to prevent garbage collection from deleting it
    this.TCPConnections.push(newSocket)

    // The board implementation currently doesn't require a subscribe for TCP, since its a connection-based protocol
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

  sendTCP(packet: Buffer, destinationIp: string, port: number) {
    // eslint-disable-next-line no-restricted-syntax
    for (const socket in this.TCPConnections) {
      if (
        this.TCPConnections[socket].RCSocket.remoteAddress === destinationIp &&
        this.TCPConnections[socket].RCSocket.remotePort === port
      ) {
        const temp = this.TCPConnections[socket]
        this.TCPConnections[socket].RCSocket.write(
          packet,
          "utf8",
          function handler() {
            console.log(
              `wrote ${packet} to ${temp.RCSocket.remoteAddress} on ${temp.RCSocket.remotePort}`
            )
          }
        )
        break
      }
    }
  }

  // While most "any" variable types have been removed, data really can be almost any type
  sendCommand(dataIdStr: string, data: any, reliability = false): void {
    /*
     * Takes a dataIdString, data, and optional reliability (to determine)
     * UDP or TCP, properly types the data according to the type in the manifest
     * creates a Buffer, and calls the proper send function
     */
    const VersionNumber = 2

    const dataCount = data.length
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
     *  |    Version    |            Data Id            |  Data  Count  |
     *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     *  |   Data Type   |                Data (Variable)                |
     *  +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     *
     *  Note: the size of Data is dataCount * dataSizes[DataType] bytes
     */
    const headerBuffer = Buffer.allocUnsafe(5)
    headerBuffer.writeUInt8(VersionNumber, 0)
    headerBuffer.writeUInt16BE(dataId, 1)
    headerBuffer.writeUInt8(dataCount, 3)
    headerBuffer.writeUInt8(dataType, 4)

    // Create the data buffer
    const dataBuffer = Buffer.allocUnsafe(dataCount * dataSizes[dataType])

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
