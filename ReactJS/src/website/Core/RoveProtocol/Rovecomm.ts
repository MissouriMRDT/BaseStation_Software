const dgram = require('dgram')
const net = require('net')

class Rovecomm {
    
    UDPSocket: any
    TCPServer: any

    constructor() {
        //metadataManager
        //registrations
        //subscriptions
        
        //allDevices

        this.UDPSocket = dgram.createSocket('udp4')
        this.TCPServer = net.createServer((TCPSocket: any) => this.TCPListen(TCPSocket))
        
        this.UDPListen()
    }

    UDPListen() {
        this.UDPSocket.on('message', (msg: string, rinfo: {address: string, port: number}) => {
            console.log('server got: ${msg} from ${rinfo.address}:${rinfo.port}')
        })        
        this.UDPSocket.listen(8080)
    }

    TCPListen(socket: any) {
        socket.write("Hello.")
        socket.on("data", (data: any) => {
            console.log(data.toString())
        })
        socket.bind(8081)
    }

    /*
    HandleRecievedPacket(srcIP, encodedPacket) {
        packet = decodePacket(encodedPacket)

        switch (packet.Name) {
            case "Null":
                log.Log("Packet recieved with null dataId")
                break
            case null:
                log.Log("Packet recieved with null name")
                break
            case "Ping":
                SendCommand(Packet.Create("PingReply"), false, srcIP)
                break
            case "Subscribe":
                log.Log("Packet recieved requesting subscription to dataId=${packet.Name}")
                break
            case "Unsubscribe":
                log.Log("Packet recieved requesting unsubscription from dataId=${packet.Name}")
                break
            default: //Regular DataId

                if (true registrations.TryGetValue(packet.Name, out List<IRovecommReceiver> registered))
                {
                    foreach (IRovecommReceiver subscription in registered)
                    {
                        subscription.ReceivedRovecommMessageCallback(packet, false)
                        try
                        {
                            subscription.ReceivedRovecommMessageCallback(packet, false)
                        }
                        catch (System.Exception e)
                        {
                            log.Log("Error parsing packet with dataid={0}{1}{2}", packet.DataType, System.Environment.NewLine, e)
                        }
                    }
                }
                break
        }        
    }
*/
}

export var rovecomm = new Rovecomm()