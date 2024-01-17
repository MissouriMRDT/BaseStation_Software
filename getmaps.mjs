import fs from 'fs';
import https from 'https';

const url = 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/';
const directory = './assets/maps';
/**
 * To cache a new map location, you will need to manually discover the range of tiles you need to cache.
 * The way I did this was to open the developer console and go to the desired area which wasn't saved and
 * manually find the minimum and maximum y coordinate, than the minimum and maximum x coordinate, and use
 * those to set up your ranges. Then zoom in and repeat for the next zoom level. Then go back through the
 * values, round them out, and add a bit of a buffer to allow the user to move around. While it is a painful
 * process, it shouldn't have to be done often.
 */
// prettier-ignore
const MAPS = [
    // Common
    [[0, 0, 0], [0, 0, 0]],
    [[1, 0, 0], [1, 1, 0]],
    [[2, 0, 0], [0, 2, 3]],
    [[3, 2, 0], [3, 4, 3]],
    [[4, 5, 2], [4, 7, 5]],
    [[5, 11, 5], [5, 13, 8]],
    // SDELC/Rolla
    [[6, 20, 10], [6, 27, 20]],
    [[7, 45, 25], [7, 55, 35]],
    [[8, 95, 60], [8, 100, 65]],
    [[9, 195, 120], [9, 200, 130]],
    [[10, 390, 245], [10, 400, 255]],
    [[11, 785, 495], [11, 795, 505]],
    [[12, 1575, 1000], [12, 1585, 1010]],
    [[13, 3155, 2005], [13, 3165, 2010]],
    [[14, 6320, 4010], [14, 6325, 4020]],
    [[15, 12640, 8025], [15, 12650, 8035]],
    [[16, 25285, 16055], [16, 25295, 16065]],
    [[17, 50575, 32115], [17, 50585, 32125]],
    [[18, 101155, 64230], [18, 101165, 64245]],
    [[19, 202320, 128470], [19, 202330, 128485]],
    // MDRS/Hanksville
    [[6, 20, 8], [6, 27, 16]],
    [[7, 45, 20], [7, 52, 30]],
    [[8, 95, 45], [8, 101, 52]],
    [[9, 195, 95], [9, 200, 102]],
    [[10, 390, 190], [10, 400, 200]],
    [[11, 785, 390], [11, 790, 400]],
    [[12, 1570, 785], [12, 1580, 802]],
    [[13, 3145, 1570], [13, 3152, 1580]],
    [[14, 6290, 3145], [14, 6300, 3155]],
    [[15, 12590, 6295], [15, 12595, 6305]],
    [[16, 25180, 12595], [16, 25190, 12605]],
    [[17, 50365, 25195], [17, 50375, 25202]],
    [[18, 100735, 50390], [18, 100745, 50400]],
    [[19, 201475, 100785], [19, 201485, 100795]]
];

fs.mkdir(directory, { recursive: true }, (err) => {
  if (err) {
    return console.error(err);
  }
});
MAPS.forEach((mapSet) => {
  const lower = mapSet[0];
  const upper = mapSet[1];
  const z = lower[0];
  const lowerY = lower[1];
  const lowerX = lower[2];
  const upperY = upper[1];
  const upperX = upper[2];
  fs.mkdir([directory, '/', z.toString()].join(''), { recursive: true }, () => {});
  for (let y = lowerY; y <= upperY; y++) {
    fs.mkdir([directory, '/', z.toString(), '/', y.toString()].join(''), { recursive: true }, () => {});
    for (let x = lowerX; x <= upperX; x++) {
      const filename = [z.toString(), '/', y.toString(), '/', x.toString()].join('');
      if (!fs.existsSync([directory, '/', filename].join(''))) {
        https
          .request([url, filename].join(''), (response) => {
            const data = [];

            response.on('data', (chunk) => {
              data.push(chunk);
            });

            response.on('end', () => {
              fs.writeFile([directory, '/', filename].join(''), Buffer.concat(data), () => {});
            });
          })
          .end();
      }
    }
  }
});
