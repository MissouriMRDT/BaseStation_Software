import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import fs from 'fs';

const label: CSS.Properties = {
  color: 'white',
  fontFamily: 'arial',
  fontSize: '16px',
  position: 'relative',
  marginTop: '-10px',
  top: '24px',
  left: '3px',
  zIndex: 1,
};

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  position: 'absolute',
  width: 'calc(50% - 19px)',
  minHeight: '400px',
};

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};

const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};

const featureColumn: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'center',
  margin: '2.5px',
  width: '33%',
};

const tagList: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexWrap: 'wrap',
};

const tagStyle: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  border: '1px solid black',
  margin: '5px',
  lineHeight: '.1',
  padding: '0 5px 0 5px',
};

enum FeatureType {
  COLOR,
  CLEAVE,
  FORM,
  TEXTURE,
}

const ROCKPATH = path.join(__dirname, '../assets/rockLookupAssets/RockDatabase.tsv');

/**
 * Read the rock definitions from the tsv file
 */
function fillFromRDatabase(): Rock[] {
  if (fs.existsSync(ROCKPATH)) {
    const rockTable: Rock[] = [];
    let filePull = fs
      .readFileSync(ROCKPATH)
      .toString()
      .split(/\r\n|\n\r|\n|\r/);
    filePull = filePull.slice(1, filePull.length); // gets rid of the sheet labels
    filePull.forEach((textLine) => {
      const lineArr = textLine.split(/\t/);
      rockTable.push({
        name: lineArr[0],
        minerals: lineArr[1].split('; '),
        description: lineArr[2],
        textures: lineArr[3].split('; '),
      });
    });
    return rockTable;
  }
  console.error(`Failed loading Rock Table. File might not exist. Look in ${ROCKPATH}`);
  return [];
}

const MINPATH = path.join(__dirname, '../assets/rockLookupAssets/MineralDatabase.tsv');

/**
 * Loads the mineral data from the tsv defined by MINPATH
 * @returns Map of minerals, empty map if database was not found
 */
function fillFromMDatabase(): Map<string, Mineral> {
  if (fs.existsSync(MINPATH)) {
    const mineralTable: Map<string, Mineral> = new Map();
    let filePull = fs
      .readFileSync(MINPATH)
      .toString()
      .split(/\r\n|\n\r|\n|\r/);
    filePull = filePull.slice(1, filePull.length); // gets rid of the sheet labels
    filePull.forEach((textLine) => {
      const lineArr = textLine.split(/\t/);
      mineralTable.set(lineArr[0], {
        name: lineArr[0],
        forms: lineArr[1].split('; '),
        cleaveAndLuster: lineArr[2].split('; '),
        colors: lineArr[3].split('; '),
      });
    });
    return mineralTable;
  }
  console.error(`Failed loading Mineral Table. File might not exist. Look in ${MINPATH}`);
  return new Map();
}

const MINARR: Map<string, Mineral> = fillFromMDatabase();
const ROCKARR: Rock[] = fillFromRDatabase();

function populateColors(): string[] {
  const colorList: string[] = [];
  MINARR.forEach((mineral: Mineral) => {
    mineral.colors.forEach((color) => {
      colorList.push(color);
    });
  });
  colorList.sort();
  return [...new Set(colorList)];
}

const COLORMASTER: string[] = populateColors();

function populateForms(): string[] {
  const formList: string[] = [];
  MINARR.forEach((mineral: Mineral) => {
    mineral.forms.forEach((form) => {
      formList.push(form);
    });
  });
  formList.sort();
  return [...new Set(formList)];
}

const FORMMASTER: string[] = populateForms();

function populateCleaves(): string[] {
  const cleaveList: string[] = [];
  MINARR.forEach((mineral: Mineral) => {
    mineral.cleaveAndLuster.forEach((cleave) => {
      cleaveList.push(cleave);
    });
  });
  cleaveList.sort();
  return [...new Set(cleaveList)];
}

const CLEAVEMASTER: string[] = populateCleaves();

function populateTextures(): string[] {
  const TextureList: string[] = [];
  ROCKARR.forEach((rock: Rock) => {
    rock.textures.forEach((texture) => {
      if (texture !== '') {
        TextureList.push(texture);
      }
    });
  });
  TextureList.sort();
  return [...new Set(TextureList)];
}

const TEXTUREMASTER: string[] = populateTextures();

interface Mineral {
  name: string;
  forms: string[];
  cleaveAndLuster: string[];
  colors: string[];
}

interface Rock {
  name: string;
  minerals: string[];
  description: string;
  textures: string[];
}

interface Output {
  Rock: Rock;
  ConfidenceScore: number;
}

const outputCompare = (out1: Output, out2: Output) => {
  if (out1.ConfidenceScore > out2.ConfidenceScore) {
    return -1;
  }
  if (out1.ConfidenceScore < out2.ConfidenceScore) {
    return 1;
  }
  return 0;
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  availColors: string[];
  /** Selected Colors */
  sColors: string[];
  availForms: string[];
  /** Selected Forms */
  sForms: string[];
  availCleave: string[];
  /** Selected Cleave */
  sCleave: string[];
  availTextures: string[];
  /** Selected Texture */
  sTexture: string[];
  outputArr: Output[];
  selectedOutput: number;
  /** Whether or not we have access to a microscope. If not,
   * we show the textures column instead of the forms and habitats column. */
  ifMicroscope: boolean;
}

class RockLookUp extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: Readonly<IProps>) {
    super(props);
    this.state = {
      availColors: JSON.parse(JSON.stringify(COLORMASTER)),
      sColors: [],
      availForms: JSON.parse(JSON.stringify(FORMMASTER)),
      sForms: [],
      availCleave: JSON.parse(JSON.stringify(CLEAVEMASTER)),
      sCleave: [],
      availTextures: JSON.parse(JSON.stringify(TEXTUREMASTER)),
      sTexture: [],
      outputArr: [],
      selectedOutput: 0,
      ifMicroscope: true,
    };
    this.handleFeatureSelect = this.handleFeatureSelect.bind(this);
    this.compareSelections = this.compareSelections.bind(this);
    this.reloadOptions = this.reloadOptions.bind(this);
    this.getWeightedScore = this.getWeightedScore.bind(this);
    this.cullImpossibles = this.cullImpossibles.bind(this);
    this.removeSelectedFeature = this.removeSelectedFeature.bind(this);
    this.clearSelectedFeatures = this.clearSelectedFeatures.bind(this);
    this.possibleRocks = this.possibleRocks.bind(this);
  }

  handleFeatureSelect(event: any): void {
    if (event.target.id === 'ColorList') {
      const { sColors, availColors } = this.state;
      availColors.splice(availColors.indexOf(event.target.value), 1);
      sColors.push(event.target.value);

      const elements = event.target.options;

      for (let i = 0; i < elements.length; i++) {
        elements[i].selected = false;
      }

      this.setState({ sColors, availColors }, () => this.compareSelections());
    }
    if (event.target.id === 'CleaveList') {
      const { sCleave, availCleave } = this.state;
      availCleave.splice(availCleave.indexOf(event.target.value), 1);
      sCleave.push(event.target.value);

      const elements = event.target.options;

      for (let i = 0; i < elements.length; i++) {
        elements[i].selected = false;
      }

      this.setState({ sCleave, availCleave }, () => this.compareSelections());
    }
    if (event.target.id === 'FormList') {
      const { sForms, availForms } = this.state;
      availForms.splice(availForms.indexOf(event.target.value), 1);
      sForms.push(event.target.value);

      const elements = event.target.options;

      for (let i = 0; i < elements.length; i++) {
        elements[i].selected = false;
      }

      this.setState({ sForms, availForms }, () => this.compareSelections());
    }
    if (event.target.id === 'TextureList') {
      const { sTexture, availTextures } = this.state;
      availTextures.splice(availTextures.indexOf(event.target.value), 1);
      sTexture.push(event.target.value);

      const elements = event.target.options;

      for (let i = 0; i < elements.length; i++) {
        elements[i].selected = false;
      }

      this.setState({ sTexture, availTextures }, () => this.compareSelections());
    }
    this.setState({ selectedOutput: 0 });
  }

  getWeightedScore(rockObj: Rock): number {
    const { sColors, sForms, sCleave, sTexture } = this.state;

    const numSelectedProps = sColors.length + sForms.length + sCleave.length + sTexture.length;
    let rockObjTotalProps = 0;

    rockObj.minerals.forEach((mineral) => {
      const currMineral = MINARR.get(mineral);
      if (currMineral) {
        rockObjTotalProps += currMineral.forms.length + currMineral.colors.length + currMineral.cleaveAndLuster.length;
      }
    });
    rockObjTotalProps += rockObj.textures.length;

    return numSelectedProps / rockObjTotalProps;
  }

  reloadOptions(): void {
    this.setState(
      {
        availCleave: JSON.parse(JSON.stringify(CLEAVEMASTER)),
        availForms: JSON.parse(JSON.stringify(FORMMASTER)),
        availColors: JSON.parse(JSON.stringify(COLORMASTER)),
        availTextures: JSON.parse(JSON.stringify(TEXTUREMASTER)),
      },
      () => this.compareSelections(true)
    );
  }

  compareSelections(remove = true) {
    const { sColors, sForms, sCleave, sTexture } = this.state;
    let possRock: Output[] = [];
    let selectedMins: Mineral[] = [];

    ROCKARR.forEach((rock) => {
      let hit = true;
      sCleave.forEach((cleave) => {
        let cleaveHit = false;
        rock.minerals.forEach((mineral) => {
          const minObj = MINARR.get(mineral);
          if (minObj) {
            if (minObj.cleaveAndLuster.indexOf(cleave) >= 0) {
              cleaveHit = true;
            }
          }
        });
        if (!cleaveHit) {
          hit = false;
        }
      });
      sForms.forEach((form) => {
        let formHit = false;
        rock.minerals.forEach((mineral) => {
          const minObj = MINARR.get(mineral);
          if (minObj) {
            if (minObj.forms.indexOf(form) >= 0) {
              formHit = true;
            }
          }
        });
        if (!formHit) {
          hit = false;
        }
      });
      sColors.forEach((color) => {
        let colorHit = false;
        rock.minerals.forEach((mineral) => {
          const minObj = MINARR.get(mineral);
          if (minObj) {
            if (minObj.colors.indexOf(color) >= 0) {
              colorHit = true;
            }
          }
        });
        if (!colorHit) {
          hit = false;
        }
      });
      sTexture.forEach((texture) => {
        if (rock.textures.indexOf(texture) === -1) {
          hit = false;
        }
      });
      if (hit) {
        possRock.push({ Rock: rock, ConfidenceScore: this.getWeightedScore(rock) });
        rock.minerals.forEach((mineral) => {
          const minObj = MINARR.get(mineral);
          if (minObj) {
            selectedMins.push(minObj);
          }
        });
      }
    });
    possRock = [...new Set(possRock)];
    possRock.sort(outputCompare);
    this.setState({ outputArr: possRock });
    selectedMins = [...new Set(selectedMins)];
    if (remove) this.cullImpossibles(selectedMins, possRock);
  }

  cullImpossibles(possibleMins: Mineral[], possibleRocks: Output[]): void {
    const { availCleave, availColors, availForms, availTextures, sCleave, sForms, sColors, sTexture } = this.state;

    availCleave.forEach((cleave) => {
      let isPoss = false;
      if (sCleave.indexOf(cleave) === -1) {
        possibleMins.forEach((mineral) => {
          if (mineral.cleaveAndLuster.indexOf(cleave) >= 0) {
            isPoss = true;
          }
        });
      }
      if (!isPoss) {
        availCleave.splice(availCleave.indexOf(cleave), 1);
      }
    });
    availColors.forEach((color) => {
      let isPoss = false;
      if (sColors.indexOf(color) === -1) {
        possibleMins.forEach((mineral) => {
          if (mineral.colors.indexOf(color) >= 0) {
            isPoss = true;
          }
        });
      }
      if (!isPoss) {
        availColors.splice(availColors.indexOf(color), 1);
      }
    });
    availForms.forEach((form) => {
      let isPoss = false;
      if (sForms.indexOf(form) === -1) {
        possibleMins.forEach((mineral) => {
          if (mineral.forms.indexOf(form) >= 0) {
            isPoss = true;
          }
        });
      }
      if (!isPoss) {
        availForms.splice(availForms.indexOf(form), 1);
      }
    });
    availTextures.forEach((texture) => {
      let isPoss = false;
      if (sTexture.indexOf(texture) === -1) {
        possibleRocks.forEach((out) => {
          if (out.Rock.textures.indexOf(texture) >= 0) {
            isPoss = true;
          }
        });
      }
      if (!isPoss) {
        availTextures.splice(availTextures.indexOf(texture));
      }
    });
    this.setState({ availCleave, availColors, availForms, availTextures });
  }

  removeSelectedFeature(type: FeatureType, index: number): void {
    switch (type) {
      case FeatureType.COLOR: {
        const { availColors, sColors } = this.state;
        availColors.push(sColors[index]);
        availColors.sort();
        sColors.splice(index, 1);
        this.setState({ sColors, availColors }, () => this.reloadOptions());
        break;
      }
      case FeatureType.CLEAVE: {
        const { availCleave, sCleave } = this.state;
        availCleave.push(sCleave[index]);
        availCleave.sort();
        sCleave.splice(index, 1);
        this.setState({ sCleave, availCleave }, () => this.reloadOptions());
        break;
      }
      case FeatureType.FORM: {
        const { availForms, sForms } = this.state;
        availForms.push(sForms[index]);
        availForms.sort();
        sForms.splice(index, 1);
        this.setState({ sForms, availForms }, () => this.reloadOptions());
        break;
      }
      case FeatureType.TEXTURE: {
        const { availTextures, sTexture } = this.state;
        availTextures.push(sTexture[index]);
        availTextures.sort();
        sTexture.splice(index, 1);
        this.setState({ sTexture, availTextures }, () => this.reloadOptions());
        break;
      }
      default:
        break;
    }
  }

  clearSelectedFeatures(): void {
    this.setState(
      {
        sColors: [],
        sCleave: [],
        sForms: [],
        sTexture: [],
      },
      () => this.reloadOptions()
    );
  }

  possibleRocks(): JSX.Element | void {
    if (this.state.outputArr.length > 0) {
      return (
        <div style={{ ...row }}>
          <div style={{ ...row, flexGrow: 1 }}>
            <div style={{ ...column, flexGrow: 1, backgroundColor: '#ddd', width: '40%' }}>
              <p style={{ fontWeight: 'bold', textAlign: 'center' }}>
                {this.state.outputArr[this.state.selectedOutput].Rock.name}
              </p>
              <p style={{ textAlign: 'center', justifyContent: 'center' }}>
                {this.state.outputArr[this.state.selectedOutput].Rock.description}
              </p>
            </div>
            <div style={{ width: 'calc(60% - 62px)', alignSelf: 'center' }}>
              <img
                style={{ width: '100%' }}
                src={path.join(
                  __dirname,
                  `../assets/rockLookupAssets/images/${this.state.outputArr[this.state.selectedOutput].Rock.name}.png`
                )}
                alt={this.state.outputArr[this.state.selectedOutput].Rock.name}
              />
            </div>
          </div>
          <div style={{ ...column, alignSelf: 'right' }}>
            <button
              style={{ width: '62px', flexGrow: 1 }}
              type="button"
              onClick={() =>
                this.setState((prevState) => ({
                  selectedOutput: Math.max(0, prevState.selectedOutput - 1),
                }))
              }
            >
              ^^^
            </button>
            <p style={{ margin: '0 5 0 5', textAlign: 'center' }}>
              {this.state.selectedOutput + 1} / {this.state.outputArr.length}
            </p>
            <button
              style={{ width: '62px', flexGrow: 1 }}
              type="button"
              onClick={() =>
                this.setState((prevState) => ({
                  selectedOutput: Math.min(prevState.selectedOutput + 1, prevState.outputArr.length - 1),
                }))
              }
            >
              vvv
            </button>
          </div>
        </div>
      );
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Rock Lookup</div>
        <div style={container}>
          <div style={row}>
            <div style={featureColumn}>
              <p style={{ textAlign: 'center' }}>Colors</p>
              <select
                size={10}
                id="ColorList"
                onChange={(event) => {
                  this.handleFeatureSelect(event);
                }}
              >
                {this.state.availColors.map((colorName: string) => {
                  return (
                    <option key={colorName} value={colorName}>
                      {colorName}
                    </option>
                  );
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <p style={{ textAlign: 'center' }}>Mineral Cleave/Luster</p>
              <select
                size={10}
                id="CleaveList"
                onChange={(event) => {
                  this.handleFeatureSelect(event);
                }}
              >
                {this.state.availCleave.map((cleaveName: string) => {
                  return (
                    <option key={cleaveName} value={cleaveName}>
                      {cleaveName}
                    </option>
                  );
                })}
              </select>
            </div>
            <div style={featureColumn}>
              <button
                style={{
                  width: 'auto',
                  textAlign: 'center',
                  background: 'white',
                  borderWidth: '1px',
                  maxHeight: '48px',
                  marginBottom: '2px',
                }}
                onClick={() => this.setState((prevState) => ({ ifMicroscope: !prevState.ifMicroscope }))}
              >
                <p style={{ display: 'flex', flexDirection: 'column', fontSize: '16px' }}>
                  {this.state.ifMicroscope ? 'Forms/Habits' : 'Rock Textures'}
                </p>
              </button>
              <select
                size={10}
                id={this.state.ifMicroscope ? 'FormList' : 'TextureList'}
                onChange={(event) => {
                  this.handleFeatureSelect(event);
                }}
              >
                {this.state.ifMicroscope
                  ? this.state.availForms.map((formName: string) => {
                      return (
                        <option key={formName} value={formName}>
                          {formName}
                        </option>
                      );
                    })
                  : this.state.availTextures.map((textureName: string) => {
                      return (
                        <option key={textureName} value={textureName}>
                          {textureName}
                        </option>
                      );
                    })}
              </select>
            </div>
          </div>
          <div style={column}>
            <div style={tagList}>
              {this.state.sColors.map((tag: string, index: number) => {
                return (
                  <div key={tag} style={tagStyle} onClick={() => this.removeSelectedFeature(FeatureType.COLOR, index)}>
                    <p>{tag}</p>
                    <p style={{ marginLeft: '10px', color: 'red' }}>x</p>
                  </div>
                );
              })}
              {this.state.sCleave.map((tag: string, index: number) => {
                return (
                  <div key={tag} style={tagStyle} onClick={() => this.removeSelectedFeature(FeatureType.CLEAVE, index)}>
                    <p>{tag}</p>
                    <p style={{ marginLeft: '10px', color: 'red' }}>x</p>
                  </div>
                );
              })}
              {this.state.sForms.map((tag: string, index: number) => {
                return (
                  <div key={tag} style={tagStyle} onClick={() => this.removeSelectedFeature(FeatureType.FORM, index)}>
                    <p>{tag}</p>
                    <p style={{ marginLeft: '10px', color: 'red' }}>x</p>
                  </div>
                );
              })}
              {this.state.sTexture.map((tag: string, index: number) => {
                return (
                  <div
                    key={tag}
                    style={tagStyle}
                    onClick={() => this.removeSelectedFeature(FeatureType.TEXTURE, index)}
                  >
                    <p>{tag}</p>
                    <p style={{ marginLeft: '10px', color: 'red' }}>x</p>
                  </div>
                );
              })}
            </div>
            <div style={column}>
              <button onClick={() => this.clearSelectedFeatures()}>Clear</button>
            </div>
          </div>
          {this.possibleRocks()}
        </div>
      </div>
    );
  }
}

export default RockLookUp;
