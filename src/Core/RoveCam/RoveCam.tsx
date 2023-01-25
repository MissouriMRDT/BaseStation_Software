import { EventEmitter } from 'events';
import { Peer, MediaConnection } from 'peerjs';

class RoveCam extends EventEmitter {
  peer: Peer;

  conn: MediaConnection | null;

  remote: MediaStream | null;

  local: MediaStream | null;

  constructor() {
    super();
    this.peer = new Peer('basestation', { host: 'localhost', port: 9001 });

    this.peer.on('open', (id) => {
      console.log(`my id is ${id}`);
      this.emit('test');
      console.log(this.peer.listAllPeers());
    });

    this.peer.on('call', () => {
      console.log('called');
    });

    this.conn = null;
    this.remote = null;
    this.local = null;
  }

  async call(id: string): Promise<boolean> {
    this.local = await navigator.mediaDevices.getUserMedia({ audio: true });
    this.peer.listAllPeers((peers: any[]) => {
      console.log(peers);
    });
    this.conn = this.peer.call(id, this.local);
    this.conn.on('stream', (stream) => {
      this.remote = stream;
      console.log('got stream');
      console.log(stream);
    });
    this.conn.on('error', (e) => console.log(e));
    return true;
  }
}

export const rovecam = new RoveCam();
