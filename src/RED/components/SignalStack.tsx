import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { controllerInputs } from '../../Core/components/ControlScheme';

const container: CSS.Properties = {
  // display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  flexDirection: 'column',
};
const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};

interface IProps {
  style?: CSS.Properties;
  roverLat: number;
  roverLon: number;
}
interface IState {
  controlling: string;
  image: string;
  interval: NodeJS.Timeout;
  basestationLat: number;
  basestationLon: number;
  toggle: boolean;
}

// Dynamic paths to import images used to indicate which gimbal is being controlled
// const NotConnected = path.join(__dirname, '../assets/NotConnected.png');
const UpArrow = path.join(__dirname, '../assets/UpArrow.png');
// const DownArrow = path.join(__dirname, '../assets/DownArrow.png');

const signalMotorMultiplier = 1000;

class SignalStack extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      // Controlling will be "none" by default, then set to "main" or "drive", and image will update to match
      controlling: 'Main',
      image: UpArrow,
      interval: setInterval(() => this.signalstack(), 1000),
      basestationLat: 1,
      basestationLon: 1,
      toggle: false,
    };
    this.updateCoords = this.updateCoords.bind(this);
  }

  componentWillUnmount() {
    clearInterval(this.state.interval);
  }

  updateCoords() {
    #!/bin/bash
    
    # File to store port and device information
    log_file="stream_log.txt"
    
    # Function to check if ffmpeg is running
    check_ffmpeg() {
        local port="$1"
        local device="$2"
        # pgrep returns 0 if process is running, non-zero otherwise
        pgrep -f "ffmpeg.*-loglevel warning.*-video_size 320x240.*-i $device.*-f mpegts.*udp://192.168.100.10:$port"
        return $?
    }
    
    # Main function to restart ffmpeg streams
    restart_streams() {
        local ip="192.168.100.10"
        local video_res="-video_size 320x240"
        local extra_flags="-loglevel warning"
        local input_flags="-vf eq=brightness=-0.2:contrast=0.6"
        local output_flags="-b:v 128k -maxrate 128k -v 0"
    
        # Read ports and devices from log file
        while IFS= read -r line; do
            local port=$(echo "$line" | cut -d" " -f1)
            local device=$(echo "$line" | cut -d" " -f2)
            if ! check_ffmpeg "$port" "$device"; then
                echo "FFmpeg stream on port $port stopped. Restarting..."
                # Restart ffmpeg stream
                ffmpeg $extra_flags $video_res -i "$device" $input_flags -f mpegts $output_flags "udp://$ip:$port" &
            fi
        done < "$log_file"  # Ensure to read from the log file
    }
    
    # Main function
    main() {
        while true; do
            # Ensure log file exists
            touch "$log_file"
            # Restart ffmpeg streams
            restart_streams
            sleep 1
        done
    }
    
    # Run the main function
    main
    
    const basestationLatHTML = document.getElementById('baselat') as HTMLTextAreaElement;
    const basestationLat = parseFloat(basestationLatHTML.value);
    const basestationLonHTML = document.getElementById('baselon') as HTMLTextAreaElement;
    const basestationLon = parseFloat(basestationLonHTML.value);

    if (!Number.isNaN(basestationLat) && !Number.isNaN(basestationLon)) {
      this.setState({ basestationLat });
      this.setState({ basestationLon });
    }
  }

  calculateAngle() {
    const targetAngle: number =
      Math.atan2(this.props.roverLon - this.state.basestationLon, this.props.roverLat - this.state.basestationLat) *
      (180 / Math.PI);
    // if (targetAngle === 0) {
    //   targetAngle = 90 - targetAngle;
    // } else if (targetAngle === 180) {
    //   targetAngle = 270 - targetAngle;
    // }
    return targetAngle;
  }

  rotateArrow(angle: number) {
    const arrow = document.getElementById('needle');
    if (arrow) {
      arrow.style.transform = 'rotate(' + angle + 'deg)';
    }
  }

  signalstack(): void {
    // const angle: number = this.calculateAngle();
    // if (!Number.isNaN(angle) && this.state.toggle) {
    //   this.rotateArrow(angle);
    //   console.log(angle, this.props.roverLat, this.props.roverLon);
    //   rovecomm.sendCommand('SetAngleTarget', 'SignalStack', angle);
    // }

    if ('Pan' in controllerInputs) {
      rovecomm.sendCommand('OpenLoop', 'SignalStack', controllerInputs.Pan * signalMotorMultiplier);
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Signal Stack</div>
        <div style={container}>
          <img id="needle" src={this.state.image} alt={this.state.controlling} />
          <span>
            <h6>{this.state.basestationLat}</h6>
            <h6>{this.state.basestationLon}</h6>
          </span>
          <div>
            <input type="text" id="baselat" />
            <input type="text" id="baselon" />
            <button type="button" onClick={this.updateCoords}>
              submit
            </button>
            <button type="button" onClick={() => this.setState({ toggle: !this.state.toggle })}>
              {this.state.toggle ? 'On' : 'Off'}
            </button>
          </div>
        </div>
      </div>
    );
  }
}
export default SignalStack;
