import React, { useState, useEffect } from 'react';
import './ControllerExtractor.css';
import { ipcRenderer } from 'electron';
const electron = window.electron;

const ControllerExtractor = () => { 
    const [jsonData, setJsonData] = useState(null);
    const [selectedControllers, setSelectedControllers] = useState([]);

    const getData = () => {
        // Send the event to get the data
        electron.getJsonApi.getJson();
    };

    useEffect(() => {
        // Listen for the event
        ipcRenderer.on('fetch:json-data-response', (event, arg) => {
            setJsonData(JSON.parse(arg));
        });
        // Clean the listener after the component is dismounted
        return () => {
            ipcRenderer.removeAllListeners('fetch:json-data-response');
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
    };

    if (!jsonData) {
        return <div>Loading...</div>;
    }

    const controllerFiles = (jsonData['source-files'] || []).filter(file => file.toLowerCase().includes('controller'));

    return (
        <div className="container">
            <h2 className="header">Select Controllers to Extract</h2>
            <button onClick={getData}>Get Controllers</button>
            <div className="checkbox-container">
                {controllerFiles.map((file, index) => (
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
        </div>
    );
};

export default ControllerExtractor;
