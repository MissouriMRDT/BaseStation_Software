import L from "leaflet"
import React from "react"
import ReactDOMServer from "react-dom/server"

export default function compassNeedle(yaw: number): L.DivIcon {
  return L.divIcon({
    iconAnchor: [30, 5],
    className: yaw.toString(),
    html: ReactDOMServer.renderToString(
      <svg
        version="1.1"
        id="Layer_1"
        xmlns="http://www.w3.org/2000/svg"
        xmlnsXlink="http://www.w3.org/1999/xlink"
        x="0px"
        y="0px"
        width="75"
        height="75"
        viewBox="0 0 500 500"
        style={{ transform: `rotate(${yaw}deg)` }}
        xmlSpace="preserve"
      >
        <line fill="none" x1="160" y1="401" x2="250" y2="100" />
        <line fill="none" x1="340" y1="401" x2="250" y2="100" />
        <line fill="none" x1="160" y1="401" x2="340" y2="401" />

        <g>
          <polygon fill="#990000" points="340,389.9 250.8,338.1 160,389.9 250.5,105.9 	" />
          <polygon
            fill="none"
            stroke="#000000"
            strokeWidth="3"
            strokeMiterlimit="10"
            points="340,389.9 250.8,338.1 160,389.9 250.5,105.9 	"
          />
        </g>
      </svg>
    ),
  })
}
