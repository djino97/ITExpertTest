import './CreateObjects.css';
import { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import { Client, CreateObjectsRequest, ObjectRequest } from './api/ITExpertServiceClient';
import { appConfig } from './config';
import React from 'react';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';

export function CreateObjects(){
    const[objectRequest, setObject] = useState(new ObjectRequest());
    const[objects, setObjects] = useState([]);
    const[error, setError] = useState(null)
    const[isCreateObjects, setCreateObjectFlag] = useState(false);
    const[serialNumbers, setSerialNumbers] = useState(null);

    useEffect(() => {
        let client = new Client(appConfig.serviceUrl);
        if (isCreateObjects) {
            let request = new CreateObjectsRequest();
            request.objects = objects.map(element => 
                new ObjectRequest(
                    {
                        code: element.code,
                        value: element.value
                    }));
            client.create(request)
            .then(m => {
                setSerialNumbers(m.result); 
                setError(m.error);
            })
            .catch(e => console.error(e));
        };

    return () => {
        setCreateObjectFlag(false);
    }}, [isCreateObjects]);

    function modelFilling(value, property) {
        if (property === "code") {
            if (value.length === 0 || (!value && isNaN(value))) {
                setObject({code: value, value: objectRequest.value});
            };

            value = parseInt(value);
            if (isNaN(value)) {
                setError("Code must be a number.")
                return; 
            };
            
            setError(null);
            setObject({code: value, value: objectRequest.value});
        }

        if (property === "value") {
            setObject({code: objectRequest.code, value: value});
        }
    };

    function addObject(objectRequest) {
        setObjects([
            ...objects,
            objectRequest
        ]);
    };

    let addedObjects = objects?.map((element, index) => 
        <div className='item' key={index}>
            <p>Code: {element.code}</p>
            <p>Value: {element.value}</p>
        </div>);

    return(
        <div className='create-objects'>
            <div className='input-form'>
                <p>Form for input object values</p>
                <InputGroup size="sm" className="mb-3">
                <InputGroup.Text id="inputGroup-sizing-sm">Code</InputGroup.Text>
                    <Form.Control
                        onChange={e => modelFilling(e.target.value, "code")}
                        aria-label="Small"
                        aria-describedby="inputGroup-sizing-sm"
                    />
                </InputGroup>
                <InputGroup size="sm" className="mb-3">
                <InputGroup.Text id="inputGroup-sizing-sm" >Value</InputGroup.Text>
                    <Form.Control
                        onChange={e => modelFilling(e.target.value, "value")}
                        aria-label="Small"
                        aria-describedby="inputGroup-sizing-sm"
                    />
                </InputGroup>
            </div>
            
            <div className='button-bar'>
                <Button variant="primary" onClick={() => addObject(objectRequest)}>Add objects in request</Button>
                <Button variant="success" onClick={() => setCreateObjectFlag(true)}>Send objects</Button>
            </div>
            {error ? <div className='error'>{error}</div> : null}
            {serialNumbers?.length > 0 ? <div className='success'>Successful send to server.</div> : null}
            <div>
                <p>Added models</p>
                {addedObjects}
            </div>
        </div>
    )
}
