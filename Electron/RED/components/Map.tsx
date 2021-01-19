import React, { Component } from "react"
import CSS from "csstype"
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet"
import localforage from "localforage"
import L, { LatLngTuple } from "leaflet"
import "leaflet-offline"

import icon from "./Icon"
import compassNeedle from "./CompassNeedle"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const mapStyle: CSS.Properties = {
  height: "400px",
  width: "100%",
}

interface IProps {
  storedWaypoints: any
  currentCoords: { lat: number; lon: number }
  store: (name: string, coords: any) => void
}

interface IState {
  centerLat: number
  centerLon: number
  zoom: number
  maxZoom: number
  heading: number
}

class Map extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      // Default map to near the SDELC
      centerLat: 37.951631,
      centerLon: -91.770001,
      zoom: 15,
      maxZoom: 19,
      heading: 0,
    }

    rovecomm.on("PitchHeadingRoll", (data: any) => this.IMUData(data))
  }

  componentDidMount(): void {
    const map = L.map("map-id")
    const offlineLayer = L.tileLayer.offline(
      "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
      localforage,
      {
        minZoom: 13,
        maxZoom: 19,
        crossOrigin: true,
      }
    )
    offlineLayer.addTo(map)
  }

  IMUData(data: any): void {
    this.setState({
      heading: data[1],
    })
  }

  render(): JSX.Element {
    const position: LatLngTuple = [this.state.centerLat, this.state.centerLon]
    return (
      <div>
        <div style={label}>Map</div>
        <div style={container}>
          <div id="map-id" style={mapStyle}>
            <MapContainer
              style={mapStyle}
              center={position}
              zoom={this.state.zoom}
              maxZoom={this.state.maxZoom}
              id="map"
              whenReady={(map: any): void =>
                map.target.on("click", (e: { latlng: { lat: number; lng: number } }) => {
                  this.props.store(new Date().toLocaleTimeString(), { lat: e.latlng.lat, lon: e.latlng.lng })
                })
              }
            >
              <TileLayer
                maxZoom={this.state.maxZoom}
                url="https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}"
              />
              {this.props.currentCoords.lat && this.props.currentCoords.lon && (
                <Marker
                  position={[this.props.currentCoords.lat, this.props.currentCoords.lon]}
                  icon={compassNeedle(this.state.heading)}
                />
              )}
              {Object.keys(this.props.storedWaypoints).map((waypointName: string) => {
                const waypoint = this.props.storedWaypoints[waypointName]
                const post: LatLngTuple = [waypoint.latitude, waypoint.longitude]
                return (
                  <Marker key={waypoint.name} position={post} icon={icon(waypoint.color)}>
                    <Popup>{waypoint.name}</Popup>
                  </Marker>
                )
              })}
            </MapContainer>
          </div>
        </div>
      </div>
    )
  }
}

export default Map
