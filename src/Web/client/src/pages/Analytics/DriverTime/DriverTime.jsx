import React from 'react';
import { useFetch } from "../../../hooks/useFetch.js";
import { BarChart } from '../../../components/BarChart';

export const DriverTime = ({width, height}) => {

    const { data, isLoading, error } = useFetch({
            method: 'GET',
            url: '/Analytics/driver-time-per-experiment',
            headers: { "Accept": '*/*' }
    });

    const processedData = data.map(item => {
        const experiment = Object.keys(item)[0];
        const values = item[experiment];
        return {
            experiment,
            ...values
        };
    });

    const keys = ['MSSQL', 'Redis', 'MongoDb'];
    const indexBy = "experiment";
    const axisBottom = 'Experiment';
    const axisLeft = 'Driver Time (ms)';



    return (
        <>
            <BarChart data={processedData} keys={keys} indexBy={indexBy} axisBottomLegend={axisBottom} axisLeftLegend={axisLeft} width={width} height={height} />
        </>
    )
}