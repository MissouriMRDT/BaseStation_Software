import L from 'leaflet';
import React from 'react';
import ReactDOMServer from 'react-dom/server';

export default function icon(color: string): L.DivIcon {
  return L.divIcon({
    className: color,
    html: ReactDOMServer.renderToString(
      <svg width="28" height="41" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink">
        <g>
          <title>Layer 1</title>
          <rect id="svg_1" fill="#fff" width="12.625" height="14.5" x="411.279" y="508.575" />
          <path
            stroke="#FFFFFF"
            id="svg_2"
            strokeLinecap="round"
            strokeWidth="1.1"
            fill={color}
            d="m14.095833,1.55c-6.846875,0 -12.545833,5.691 -12.545833,11.866c0,2.778 1.629167,6.308 2.80625,8.746l9.69375,17.872l9.647916,-17.872c1.177083,-2.438 2.852083,-5.791 2.852083,-8.746c0,-6.175 -5.607291,-11.866 -12.454166,-11.866zm0,7.155c2.691667,0.017 4.873958,2.122 4.873958,4.71s-2.182292,4.663 -4.873958,4.679c-2.691667,-0.017 -4.873958,-2.09 -4.873958,-4.679c0,-2.588 2.182292,-4.693 4.873958,-4.71z"
          />
          <path
            id="svg_3"
            fill="none"
            strokeOpacity="0.122"
            strokeLinecap="round"
            strokeWidth="1.1"
            stroke="#fff"
            d="m347.488007,453.719c-5.944,0 -10.938,5.219 -10.938,10.75c0,2.359 1.443,5.832 2.563,8.25l0.031,0.031l8.313,15.969l8.25,-15.969l0.031,-0.031c1.135,-2.448 2.625,-5.706 2.625,-8.25c0,-5.538 -4.931,-10.75 -10.875,-10.75zm0,4.969c3.168,0.021 5.781,2.601 5.781,5.781c0,3.18 -2.613,5.761 -5.781,5.781c-3.168,-0.02 -5.75,-2.61 -5.75,-5.781c0,-3.172 2.582,-5.761 5.75,-5.781z"
          />
        </g>
      </svg>
    ),
  });
}
