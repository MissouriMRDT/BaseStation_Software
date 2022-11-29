import { EventEmitter } from 'events';
import { Peer, MediaConnection } from 'peerjs';

class RoveCam extends EventEmitter {
  peer: Peer;

  conn: MediaConnection | null;

  remote: MediaStream | null;

  constructor() {
    super();
    this.peer = new Peer('basestation', { host: 'localhost', port: 9001 });
    this.peer.on('open', (id) => {
      console.log(`my id is ${id}`);
      this.emit('test');
    });
    this.conn = null;
    this.remote = null;
  }

  call(id: string): boolean {
    this.conn = this.peer.call(id, new MediaStream());
    this.conn.on('stream', (stream) => {
      this.remote = stream;
      console.log('got stream');
    });
    return true;
  }
}

export const rovecam = new RoveCam();
