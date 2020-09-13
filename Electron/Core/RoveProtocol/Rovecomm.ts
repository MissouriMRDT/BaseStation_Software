/* eslint-disable @typescript-eslint/no-var-requires */
const dgram = require("dgram")
const net = require("net")

function TCPListen(socket: any) {
  socket.write("Hello.")
  socket.on("data", (data: any) => {
    console.log(data, data.toString())
  })
}

class Rovecomm {
  UDPSocket: any

  TCPServer: any

  constructor() {
    // metadataManager
    // registrations
    // subscriptions

    // allDevices

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
      }
    )
    this.UDPSocket.bind(8081)
  }
}

const rovecomm = new Rovecomm()
export default rovecomm
