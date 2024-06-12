import { Component } from 'react';
import * as THREE from 'three';
import { GLTF, GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';
import React from 'react';
import path from 'path';
import { clearInterval } from 'timers';

const MODELS_PATH = path.join(__dirname, '../assets/models');
const STL_ROVER_FILE = path.join(MODELS_PATH, 'rover.stl');
const GLTF_ROVER_FILE = path.join(MODELS_PATH, 'rover_preview.glb');
const TEXTURE_FILE = path.join(MODELS_PATH, 'tex01.png');

interface IState {
  showGltf: boolean;
  loaded: boolean;
}

type IProps = JSX.IntrinsicElements['group'] & { color?: THREE.ColorRepresentation; showGltf?: boolean };

class RoverScene extends Component<IProps, IState> {
  private gltfModel?: THREE.Object3D;

  private stlGeometry?: THREE.BufferGeometry;

  private texture: THREE.Texture;

  // The materials Blender exports cause crashes when you open 3D Rover in RID, because Three JS tries to optimize things and share information
  // between rendering contexts, which is bad because RED and RID are in different windows.
  private materials: {
    face: THREE.MeshLambertMaterial;
    skin: THREE.MeshPhongMaterial;
    body: THREE.MeshLambertMaterial;
  };

  private animation?: THREE.AnimationAction;

  private mixer?: THREE.AnimationMixer;

  private updateInterval?: NodeJS.Timeout;

  // React Three Fiber provides a bunch of really nice hooks like useGLTF, useAnimations, useTexture, useFrame etc, but they break with our codebase.
  constructor(props: IProps) {
    super(props);
    this.state = { loaded: false, showGltf: false };
    this.texture = new THREE.TextureLoader().load(TEXTURE_FILE, (texture) => {
      texture.flipY = false;
      texture.encoding = THREE.sRGBEncoding;
    });
    this.materials = {
      face: new THREE.MeshLambertMaterial({ map: this.texture, name: 'face' }),
      skin: new THREE.MeshPhongMaterial({
        color: new THREE.Color('#AB8E6B').convertSRGBToLinear(),
        shininess: 100,
        specular: 'white',
        name: 'skin',
      }),
      body: new THREE.MeshLambertMaterial({
        color: new THREE.Color(this.props.color ?? '#363636').convertSRGBToLinear(), // ThreeJS has a bug
        name: 'body',
      }),
    };
  }

  componentDidMount(): void {
    this.loadStl();
  }

  loadStl() {
    if (!this.stlGeometry) {
      this.setState({ loaded: false });
      const loader = new STLLoader();
      loader.load(STL_ROVER_FILE, (geo) => {
        geo.center();
        geo.computeVertexNormals();
        this.stlGeometry = geo;
        this.setState({ showGltf: false, loaded: true });
      });
    } else this.setState({ showGltf: false, loaded: true });
  }

  loadGltf() {
    if (!this.gltfModel) {
      this.setState({ loaded: false });
      new GLTFLoader().load(GLTF_ROVER_FILE, (result: GLTF) => {
        this.gltfModel = result.scene.children[0];
        this.mixer = new THREE.AnimationMixer(result.scene);
        this.animation = this.mixer.clipAction(result.animations[0]);
        // getObjectsByProperty() is not available until ThreeJS 0.148.0
        const recursiveMaterialAssigner = (node: THREE.Object3D) => {
          if (node.type === 'Mesh' || node.type === 'SkinnedMesh') node.castShadow = true;
          if (node.name === 'head') (node as THREE.Mesh).material = this.materials?.face;
          else if (node.type === 'SkinnedMesh') (node as THREE.Mesh).material = this.materials?.skin;
          else if (node.type === 'Mesh') (node as THREE.Mesh).material = this.materials?.body;
          node.children.forEach((child) => recursiveMaterialAssigner(child));
        };
        recursiveMaterialAssigner(this.gltfModel);
        this.animation.play();
        this.updateInterval = setInterval(() => this.mixer?.update(0.01), 10);
        this.setState({ showGltf: true, loaded: true });
      });
    } else this.setState({ showGltf: true, loaded: true });
  }

  componentDidUpdate(prevProps: Readonly<IProps>): void {
    if (prevProps.color !== this.props.color) {
      this.materials.body.setValues({ color: new THREE.Color(this.props.color).convertSRGBToLinear() });
    }
    if (prevProps.showGltf !== this.props.showGltf) {
      if (this.props.showGltf) this.loadGltf();
      else this.loadStl();
    }
  }

  componentWillUnmount(): void {
    this.mixer?.stopAllAction();
    if (this.updateInterval) clearInterval(this.updateInterval);
    this.texture.dispose();
    Object.values(this.materials).forEach((material) => material.dispose());
    this.stlGeometry?.dispose();
  }

  render() {
    return (
      <>
        {this.state.loaded && (
          <group {...this.props}>
            {this.state.showGltf ? (
              <primitive object={this.gltfModel} position-y={-1.2} rotation-y={Math.PI} scale={1.7} />
            ) : (
              <mesh
                geometry={this.stlGeometry}
                material={this.materials.body}
                position-y={0.6}
                scale={1.1}
                dispose={null}
                castShadow
              />
            )}
          </group>
        )}
      </>
    );
  }
}

export default RoverScene;
