import React from 'react';
import { useFetch } from "../../../hooks/useFetch.js";
import { PieChart } from '../../../components/PieChart';

export const CacheMiss = () => {
    const { data, isLoading, error } = useFetch({
        method: 'GET',
        url: '/Analytics/cache-miss-per-database',
        headers: { "Accept": '*/*' }
    });

    return (
        <>
            <PieChart data={data} width={500} height={500} />
        </>
    );
}

