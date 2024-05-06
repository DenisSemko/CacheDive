import React from 'react';
import { useFetch } from "../../../hooks/useFetch.js";
import { PieChart } from '../../../components/PieChart';

export const CacheHit = ({width, height}) => {
    const { data, isLoading, error } = useFetch({
        method: 'GET',
        url: '/Analytics/cache-hit-per-database',
        headers: { "Accept": '*/*' }
    });

    return (
        <>
            <PieChart data={data} width={width} height={height} />
        </>
    );
}

