import { useState, useEffect } from 'react';
import { Client } from './api/ITExpertServiceClient';
import { appConfig } from './config';
import React from 'react';
import './ObjectsRendering.css';
import Button from 'react-bootstrap/Button';

export function ObjectsRendering() {
    const[currentPage, setCurrentPage] = useState(1);
    const[findobjectsResult, setfindobjectsResult] = useState(null);
    const [dataFetched, setDataFetched] = useState(false);
    const itemsOnPage = 4;

    useEffect(() => {
        const client = new Client(appConfig.serviceUrl);
        if (!dataFetched)
            client.find(null, null, null, (currentPage - 1) * itemsOnPage, itemsOnPage)
                .then(m => setfindobjectsResult(m.result))
                .catch(e => console.error(e));

    return () => {
        setDataFetched(true);
    }}, [currentPage]);

    function handleClick(number) {
        setDataFetched(false);
        setCurrentPage(number);
    }

    let getButtons = pageCount => {
        const buttons = [];
        for (let i = 1; i <= pageCount; i++) {
            buttons.push(<Button id="paginationBtn" variant="secondary" onClick={() => handleClick(i)}>{i}</Button>)
        };

        return buttons;
    };

    let objectsInfo = findobjectsResult?.objects.map(element => 
        (<div className='item'>
            <p>Serial number: {element.serialNumber}</p>
            <p>Code: {element.code}</p>
            <p>Value: {element.value}</p>
        </div>)
    );
    let pages = Math.ceil(findobjectsResult?.totalCount/itemsOnPage);
    return (
        <div>
            <p >Show all objects from datatbase!</p>
            <div className='find-items'>
                {objectsInfo}
                <div> 
                    {getButtons(pages ?? 1)}
                </div>
            </div>
        </div>
    );
}