import { useState } from 'react';
import axios from 'axios';

export const usePost = (baseURL = "https://localhost:7053") => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    axios.defaults.baseURL = baseURL;

    const postData = async ({ url, headers, data }) => {
        setIsLoading(true);
        try {
            const response = await axios.post(url, data, { withCredentials: true, headers: { "Accept": "text/plain", "Content-Type": "application/json", ...headers} });
            return response.data;
        } catch (error) {
            setError(error);
            throw error;
        } finally {
            setIsLoading(false);
        }
    };

    return { isLoading, error, postData };
};