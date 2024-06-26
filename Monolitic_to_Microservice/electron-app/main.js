
const { BrowserWindow, app, ipcMain, Notification, dialog } = require('electron');
const path = require('path');
const fs = require('fs');
const os = require('os');
const { exec } = require('child_process');
const execOptions = { maxBuffer: 1024 * 1024 };

const isDev = !app.isPackaged;
let globalFileName = ''
let globalOutputPath = ''

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    backgroundColor: "white",
    webPreferences: {
      nodeIntegration: false,
      worldSafeExecuteJavaScript: true,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.js')
    }
  })
  win.webContents.openDevTools();
  win.loadFile('index.html');
}

if (isDev) {
  // require('electron-reload')(__dirname, {
  //   electron: path.join(__dirname, 'node_modules', '.bin', 'electron')
  // })
}

ipcMain.on('getJson', (event, { projectPath, outputPath }) => {

  const extension = path.extname(projectPath);
  globalFileName = path.parse(projectPath).name;
  globalOutputPath = outputPath;
  console.log("fileName : "+globalFileName + "  outputPath : "+globalOutputPath);
  let arg = '';
  if (extension === '.sln') {
    arg = '-s';
  } else if (extension === '.csproj') {
    arg = '-p';
  } else {
    console.error(`Invalid project file extension: ${extension}`);
    new Notification({ title: 'Error', body: 'Invalid project file extension' }).show();
    event.reply('fetch:json-data-response', JSON.stringify({ error: 'Invalid project file extension' }));
    return;
  }
  const codelyzerPath = `"D:\\Monolitic_to_Microservice\\codelyzer\\src\\Analysis\\Codelyzer.Analysis\\bin\\Debug\\net6.0\\Codelyzer.Analysis.exe"`;

  const codelyzerCommand = `${codelyzerPath} ${arg} ${projectPath} -o ${__dirname}`

  exec(codelyzerCommand, execOptions, (error, stdout, stderr) => {
    if (error) {
      console.error(`Error executing Codelyzer command: ${error.message}`);
      new Notification({ title: 'Error', body: `Error: ${error.message}` }).show();
      return;
    }
    new Notification({ title: 'Success', body: `Successfully executed codelyzer command` }).show();


    const filePath = path.join(__dirname, `codelyzer.json`);
    console.log("filePath : ", filePath)
    fs.readFile(filePath, 'utf-8', (err, data) => {
      if (err) {
        new Notification({ title: 'Error', body: 'Failed to read JSON file' }).show();
        event.reply('fetch:json-data-response', JSON.stringify({ error: 'Failed to read JSON file' }));
        return;
      }
      new Notification({ title: 'Notification', body: 'JSON Acquired' }).show();
      event.reply('fetch:json-data-response', data);
    });
  });
});

ipcMain.on('sendController', (event, controllers) => {
  console.log("Inside service extractor exec");
  const exePath = `"D:\\Monolitic_to_Microservice\\Naveen\\poc-samples-service-extractor\\poc-samples-service-extractor\\src\\ServiceExtractor\\ServiceExtractor\\bin\\Debug\\net8.0\\Service.Extractor.Console.exe"`;
  const jsonPath = path.join(__dirname , 'codelyzer.json');
  console.log("jsonPath : ",jsonPath);
  //const outputPath = `"D:\\Monolitic_to_Microservice\\Naveen\\ServiceExtractor-Output"`;

  controllers.forEach(controller => {
    console.log(controller)
    const lastWord = controller.split('\\').pop().split('.').shift();

    //const cdCommand = `"D:\\Monolitic_to_Microservice\\Naveen\\poc-samples-service-extractor\\poc-samples-service-extractor\\src\\ServiceExtractor\\ServiceExtractor\\bin\\Debug\\net8.0\\Service.Extractor.Console.exe" -c extract -r AccountController -j "D:\\Monolitic_to_Microservice\\codeelyzer-output\\DemoWebApi.json" -n "D:\\Monolitic_to_Microservice\\Naveen\\ServiceExtractor-Output"`
    const command = `${exePath} -c extract -r AccountController -j "${jsonPath}" -n "${globalOutputPath}"`
    console.log("service command : "+command);
    exec(command, (error, stdout, stderr) => {
      if (error) {
        console.error(`Error executing Service Extractor command: ${error.message}`);
        new Notification({ title: 'Error', body: `Error: ${error.message}` }).show();
        return;
      }
      new Notification({ title: 'Success', body: `Successfully extracted Service Extractor command` }).show();
    });
  });
});

ipcMain.on('open-file-dialog', (event, message) => {
  if (os.platform() === 'linux' || os.platform() === 'win32') {
    dialog.showOpenDialog({
      properties: ['openFile']
    }).then(result => {
      if (!result.canceled && result.filePaths.length > 0) {
        event.sender.send('selected-file', result.filePaths[0]);
      }
    }).catch(err => {
      console.log(err);
    });
  } else {
    dialog.showOpenDialog({
      properties: ['openDirectory', 'openFile']
    }).then(result => {
      if (!result.canceled && result.filePaths.length > 0) {
        event.sender.send('selected-file', result.filePaths[0]);
      }
    }).catch(err => {
      console.log(err);
    });
  }
});

ipcMain.on('open-folder-dialog', (event, message) => {
  dialog.showOpenDialog({
    properties: ['openFile', 'openDirectory']
  }).then(result => {
    if (!result.canceled && result.filePaths.length > 0) {
      event.sender.send('selected-folder', result.filePaths[0]);
    }
  }).catch(err => {
    console.log(err);
  });
});

app.whenReady().then(createWindow)
