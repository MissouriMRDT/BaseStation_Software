/* eslint-disable import/no-mutable-exports */
import { Socket as netSocket } from 'net';
import Deque from 'double-ended-queue';
import fs from 'fs';
import path from 'path';
/* eslint-disable import/no-cycle */
import { parse } from './Rovecomm';

export let dataSizes: any = [];
export let DataTypes: any = {};
const filepath = path.join(__dirname, '../assets/manifest/manifest.json');
export const VersionNumber = 2;
const headerLength = 5;

if (fs.existsSync(filepath)) {
  const manifest = JSON.parse(fs.readFileSync(filepath).toString());
  dataSizes = manifest.dataSizes;
  DataTypes = manifest.DataTypes;
}

interface TCPSocket {
  RCSocket: netSocket;
  RCDeque: Deque<any>;
}

export function createHeader(dataId: number, dataCount: number, dataType: string) {
  const headerBuffer = Buffer.allocUnsafe(headerLength);
  headerBuffer.writeUInt8(VersionNumber, 0);
  headerBuffer.writeUInt16BE(dataId, 1);
  headerBuffer.writeUInt8(dataCount, 3);
  headerBuffer.writeUInt8(DataTypes[dataType], 4);
  return headerBuffer;
}

export function parseHeader(packet: any) {
  const header = {
    version: packet.readUInt8(0),
    dataId: packet.readUInt16BE(1),
    dataCount: packet.readUInt8(3),
    dataType: packet.readUInt8(4),
  };
  return header;
}

export function TCPParseWrapper(socket: TCPSocket) {
  /*
   * Takes in an object of the TCPSocket type defined in the interface TCPSocket at the top of this file
   * Iterates while there is still at least five bytes in the Deque in the TCPSocket, parsing one RC Packet at a time
   * Returns when there is either less than 5 bytes in the Deque or not a complete packet in the Deque
   */
  // While the Deque contains at least a header to allow parsing control packets
  while (socket.RCDeque.length >= headerLength) {
    const dataCount = socket.RCDeque.get(3);
    const dataSize = dataSizes[socket.RCDeque.get(4)];
    // If the length of the Deque is more than the header size and the size of the packet, make a buffer and then parse that buffer
    if (socket.RCDeque.length >= headerLength + dataCount * dataSize) {
      // create another list to put the entire packet into
      const packet = [];
      for (let i = 0; i < headerLength + dataCount * dataSize; i++) {
        // Here we use Shift to get and remove the elements that make up the packet to prevent parsing the same packet multiple times
        packet.push(socket.RCDeque.shift());
      }
      // Make a buffer from that list, needed to provide the correct input for the parse function
      const packetBuffer = Buffer.from(packet);
      parse(packetBuffer);
      // If the Deque doesn't contain a full packet, return
    } else return;
  }
}
