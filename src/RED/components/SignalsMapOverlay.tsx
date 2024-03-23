import L from 'leaflet';
import React from 'react';
import ReactDOMServer from 'react-dom/server';

export default function signalsMapOverlay(yaw: number): L.DivIcon {
  return L.divIcon({
    iconAnchor: [150, 150],
    className: yaw.toString(),
    html: ReactDOMServer.renderToString(
      <svg
        version="1.1"
        id="Layer_1"
        xmlns="http://www.w3.org/2000/svg"
        xmlnsXlink="http://www.w3.org/1999/xlink"
        x="0px"
        y="0px"
        width="300"
        height="300"
        viewBox="-250 -250 500 500"
        style={{ transform: `rotate(${yaw}deg)` }}
        xmlSpace="preserve"
      >
        <g>
          <polygon points="0,0 30,0 150,-150 -150,-150 -30,0" fill="#990000" opacity="0.8" />
        </g>
      </svg>
    ),
  });
}
