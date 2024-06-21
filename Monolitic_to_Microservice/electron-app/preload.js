
const { ipcRenderer, contextBridge } = require('electron');

contextBridge.exposeInMainWorld('electron', {
  notificationApi: {
    sendNotification(message) {
      ipcRenderer.send('notify', message);
    }
  },
  getJsonApi: {
    getJson(paths){
      ipcRenderer.send('getJson',paths);
    }
  },
  sendControllerApi: {
    sendController(message) {
      ipcRenderer.send('sendController', message);
    }
  },
  ipcRenderer: {
    on(channel, listener) {
      ipcRenderer.on(channel, listener);
    },
    send(channel, ...args) {
      ipcRenderer.send(channel, ...args);
    },
    removeAllListeners(channel) {
      ipcRenderer.removeAllListeners(channel);
    }
  }
})
