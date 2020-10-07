import { DATAID } from "./RovecommManifest"
/* eslint-disable @typescript-eslint/no-var-requires */
const dgram = require("dgram")
const net = require("net")
const EventEmitter = require("events")

function decodePacket(
  size: number,
  dataLength: number,
  data: Uint8Array
): number[] {
  const retArray = []
  let i = 0
  let j = 0
  switch (size) {
    case 1:
      while (i < dataLength) {
        retArray.push(Number(data[i]))
        i += 1
      }
      return retArray
    case 2:
      while (i < dataLength) {
        // eslint-disable-next-line no-bitwise
        const val = Number((data[j] << (1 * 8)) | data[j + 1])
        retArray.push(val)
        j += 2
        i += 1
      }
      return retArray
    case 4:
      while (i < dataLength) {
        const val = Number(
          // eslint-disable-next-line no-bitwise
          (data[j] << (3 * 8)) |
            // eslint-disable-next-line no-bitwise
            (data[j + 1] << (2 * 8)) |
            // eslint-disable-next-line no-bitwise
            (data[j + 2] << (1 * 8)) |
            data[j + 3]
        )
        retArray.push(val)
        i += 1
        j += 4
      }
      return retArray
    default:
      return []
  }
}

export function parse(packet: string): string {
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

  enum DataTypes {
    INT8_T = 0,
    UINT8_T = 1,
    INT16_T = 2,
    UINT16_T = 3,
    INT32_T = 4,
    UINT32_T = 5,
    FLOAT_T = 6,
  }

  const sizes = [1, 1, 2, 2, 4, 4, 4]

  const version = Number(packet[0])
  // eslint-disable-next-line no-bitwise
  const dataId = Number((packet[1] << 8) | packet[2])
  const dataLength = Number(packet[3])
  const dataType = packet[4]

  const rawdata = packet.slice(5)
  let data: number[]

  if (version === VersionNumber) {
    console.log(dataId)
    data = decodePacket(sizes[dataType], dataLength, new Uint8Array(rawdata))
    // eslint-disable-next-line
    rovecomm.emit(dataID, data)
  } else {
    return "null"
  }
  return DATAID[dataId]
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
    this.TCPServer.listen(8080)
  }

  UDPListen() {
    this.UDPSocket.on(
      "message",
      (msg: string, rinfo: { address: string; port: number }) => {
        console.log(`server got: ${msg} from ${rinfo.address}:${rinfo.port}`)
        parse(msg)
      }
    )
    this.UDPSocket.bind(8081)
  }
}
export const rovecomm = new Rovecomm()

export function sendCommand(packet: Packet, reliability = false) {
  console.log(`Not yet implemented. Recieved ${packet}, ${reliability}`)
}
