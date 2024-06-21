import React, { useState, useEffect } from 'react';
import './components/ControllerExtractor.css';

export default function App() {
  const [jsonData, setJsonData] = useState(null);
  const [selectedControllers, setSelectedControllers] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [projectPath, setProjectPath] = useState('');
  const [outputPath, setOutputPath] = useState('');
  const electron = window.electron;

  const getData = () => {
    setIsLoading(true);
    console.log("Calling getJson");
    electron.getJsonApi.getJson({ projectPath, outputPath });
  };

  useEffect(() => {
    // Listen for the event
    electron.ipcRenderer.on('fetch:json-data-response', (event, arg) => {
      console.log("Received fetch:json-data-response");
      try {
        const data = JSON.parse(arg);
        setJsonData(data);
      } catch (error) {
        console.log("Failed to parse JSON:", error);
      }
      setIsLoading(false);
    });
    // Clean the listener after the component is dismounted
    return () => {
      electron.ipcRenderer.removeAllListeners('fetch:json-data-response');
    };
  }, []);



  const getFilePath = () => {
    electron.ipcRenderer.send('open-file-dialog');
  }

  useEffect(() => {
    electron.ipcRenderer.on('selected-file', (event, path) => {
      console.log("Selected file path:", path);
      setProjectPath(path); // Update state with selected file path
    });
    // Clean up the listener when component unmounts
    return () => {
      electron.ipcRenderer.removeAllListeners('selected-file');
    };
  }, []);



  const getFolderPath = () => {
    electron.ipcRenderer.send('open-folder-dialog');
  }
  
  useEffect(() => {
    electron.ipcRenderer.on('selected-folder', (event, path) => {
      console.log("Selected folder path:", path);
      setOutputPath(path); // Update state with selected file path
    });
    // Clean up the listener when component unmounts
    return () => {
      electron.ipcRenderer.removeAllListeners('selected-folder');
    };
  }, []);
  


  const handleCheckboxChange = (event) => {
    const { value, checked } = event.target;
    if (checked) {
      setSelectedControllers([...selectedControllers, value]);
    } else {
      setSelectedControllers(selectedControllers.filter(controller => controller !== value));
    }
  };



  const handleExecuteCommand = () => {
    // Add logic to execute command based on selected controllers
    electron.sendControllerApi.sendController(selectedControllers);
  };



  return (
    <div className="container">
      <div>
        <label htmlFor="projectPath">Project Path:</label>
        <input
          type="text"
          id="projectPath"
          value={projectPath}
          onChange={(e) => setProjectPath(e.target.value)}
        />
        <button onClick={getFilePath}>Choose File</button>
      </div>
      <div>
        <label htmlFor="outputPath">Output Path:</label>
        <input
          type="text"
          id="outputPath"
          value={outputPath}
          onChange={(e) => setOutputPath(e.target.value)}
        />
        <button onClick={getFolderPath}>Choose File</button>
      </div>
      <h2 className="header">Select Controllers to Extract</h2>
      {!jsonData && !isLoading && <button onClick={getData}>Get Controllers</button>}
      {isLoading && <div>Loading...</div>}
      {jsonData && (
        <>
          <div className="checkbox-container">
            {jsonData['source-files']
              .filter(file => file.toLowerCase().includes('controller'))
              .map((file, index) => (
                <label key={index}>
                  <input
                    type="checkbox"
                    value={file}
                    checked={selectedControllers.includes(file)}
                    onChange={handleCheckboxChange}
                  />
                  {file}
                </label>
              ))}
          </div>
          <button className="button" onClick={handleExecuteCommand}>Extract Selected Controllers</button>
        </>
      )}
    </div>
  );
}
