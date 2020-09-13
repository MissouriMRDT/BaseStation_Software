const dgram = require('dgram')
const net = require('net')

    
//metadataManager
//registrations
//subscriptions

//allDevices
const UDPSocket = dgram.createSocket('udp4')

const TCPServer = net.createServer(TCPSocket => {
    TCPSocket.write("Hello.")
    TCPSocket.on("data", data => {
        console.log(data.toString())
    })
})
TCPServer.listen(8080)


UDPListen()

function UDPListen() {
    UDPSocket.on('message', (msg, rinfo) => {
        console.log('server got: ${msg} from ${rinfo.address}:${rinfo.port}')
    })        
    UDPSocket.bind(8081)
}

function TCPListen(socket) {
    TCPServer.listen(8080)

    socket.write("Hello.")
    socket.on("data", data => {
        console.log(data.toString())
    })
}

function HandleRecievedPacket(srcIP, encodedPacket) {
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
            if (true /*registrations.TryGetValue(packet.Name, out List<IRovecommReceiver> registered)*/)
            {
                foreach (/*IRovecommReceiver*/ subscription in registered)
                {
                    subscription.ReceivedRovecommMessageCallback(packet, false)
                    /*
                    try
                    {
                        subscription.ReceivedRovecommMessageCallback(packet, false)
                    }
                    catch (System.Exception e)
                    {
                        log.Log("Error parsing packet with dataid={0}{1}{2}", packet.DataType, System.Environment.NewLine, e)
                    }
                    */
                }
            }
            break
    }        
}
