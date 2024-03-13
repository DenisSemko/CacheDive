import { useState, useCallback } from 'react';
import axios from 'axios';

export const useFetch = (url) => {
    
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    axios.defaults.baseURL = "https://localhost:7053";

    const executeRequest = useCallback(async (params) => {
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

    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [url]);

    return { executeRequest, data, isLoading, error };
}