import React, { useEffect, useState } from 'react';
import "./JsonViewForm.scss";
import { Button, Box, useTheme } from "@mui/material";
import JsonView from '@uiw/react-json-view';
import { darkTheme } from '@uiw/react-json-view/dark';
import { lightTheme } from '@uiw/react-json-view/light';
import { useFetch } from "../../hooks/useFetch.js";
import 'react-toastify/dist/ReactToastify.css';
import { tokens } from "../../theme";

export const JsonViewForm = () => {
  const [jsonTheme, setJsonTheme] = useState(darkTheme);
  const theme = useTheme();
  const colors = tokens(theme.palette.mode);
  const { data, isLoading, error } = useFetch({
    method: 'GET',
    url: '/General',
    headers: { "Accept": '*/*' }
  });
   

  useEffect(() => {
      if (theme.palette.mode === 'dark') {
          setJsonTheme(darkTheme);
      } else {
          setJsonTheme(lightTheme);
      }
  }, [theme.palette.mode])

  const refreshData = () => {
      window.location.reload();
  }


  return (
    <Box className="json-view-form-container">
        <Button type="submit" color="secondary" variant="contained" onClick={refreshData}>Refresh Data</Button>
        <JsonView value={data} style={jsonTheme} collapsed={true} />
    </Box>
  )
}