// ExcelJS.stream.xlsx.WorkbookWriter is only available as a node server process
// to prevent the memory leak that comes from saving every single packet in the PacketLogger,
// we instead create an xlsx file stream in a background process and send packets to that process
// over websocket. browser -> ExcelJS -> file-system

const WEBSOCKET_PORT = 6969;

const fs = require('fs'),
  //http = require('http'),
  WebSocket = require('ws'),
  ExcelJS = require('exceljs');

let writer = undefined;
let rowCount = 0;

function beginExcelStream() {
  if (typeof writer !== 'undefined') return;
  if (!fs.existsSync('./PacketLogger')) {
    fs.mkdirSync('./PacketLogger');
  }
  // ISO string will be formatted YYYY-MM-DDTHH:MM:SS:sssZ
  // this regex will convert all -,T:,Z to . (which covers to . for .csv)
  // Date format is consistent with the SensorData csv
  const timestamp = new Date().toISOString().replaceAll(/[:\-TZ]/g, '.');
  const EXPORT_FILE = `./PacketLogger/log-${timestamp}.xlsx`;
  const workbookOptions = {
    filename: EXPORT_FILE,
    useStyles: true,
    useSharedStrings: true,
  };
    writer = new ExcelJS.stream.xlsx.WorkbookWriter(workbookOptions);
}
function addExcelRow(row) {
  if (typeof writer === 'undefined') return;
  let page = writer.getWorksheet(row.name);
  if (!page) {
    const worksheetOptions = { headerFooter: { firstHeader: `Name: ${row.name}, DataID: ${row.dataId}` } };
    page = writer.addWorksheet(row.name, worksheetOptions);
  }
  //console.log('adding data: ', row);
  if (typeof row.data === 'string') page.addRow([row.time, row.data]).commit();
  else page.addRow([row.time, ...row.data]).commit();
}

function closeAndSave() {
  if (typeof writer !== 'undefined') writer.commit().then(() => process.exit(0));
}

console.log('creating websocket server for excel sheet on port ' + WEBSOCKET_PORT);
const socketServer = new WebSocket.Server({ port: WEBSOCKET_PORT, perMessageDeflate: false });
socketServer.on('connection', (socket, upgradeReq) => {
  console.log(
    'New Excel WebSocket Connection: ',
    (upgradeReq || socket.upgradeReq).socket.remoteAddress,
    (upgradeReq || socket.upgradeReq).headers['user-agent']
  );
  beginExcelStream();
  socket.on('message', (data) => {
    addExcelRow(JSON.parse(data));
  });
  socket.on('close', closeAndSave);
});
