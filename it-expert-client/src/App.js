import './App.css';
import {ObjectsRendering} from './ObjectsRendering';
import {CreateObjects} from './CreateObjects';
import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';

function App() {
  const [isCreateMode, setMode] = useState(false);

  return (
    <div className="App">
      <Button id="mainButton" variant="primary" onClick={() => setMode(!isCreateMode)}>{isCreateMode ? "Find objects" : "Create objects"}</Button>
      {
        isCreateMode ? 
        <CreateObjects/>
          : <ObjectsRendering/>
      }
    </div>
  );
}

export default App;
