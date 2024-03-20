import { useState, useEffect } from 'react';
import axios from 'axios';

export const useFetch = (requestParams, baseURL = "https://localhost:7053") => {
    
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    axios.defaults.baseURL = baseURL;

    const fetchData = async (params) => {
        setIsLoading(true);
        try {
            const response = await axios.request({...params, withCredentials: true});
            setData(response.data);
        }
        catch (error) {
            setError(error);
        }
        finally {
            setIsLoading(false);
        }

    };

    useEffect(() => {

        if (window.location.pathname !== '/') {
            fetchData(requestParams);
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [requestParams.url, window.location.pathname]);

    return { data, isLoading, error };
}