import React, { useState } from 'react';
import "./Config.scss";
import { Box, Button, TextField } from "@mui/material";
import { InnerHeader } from "../../components/InnerHeader";
import { usePost } from "../../hooks/usePost.js";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { JsonViewForm } from "../../components/JsonViewForm";

export const Config = () => {
  const [file, setFile] = useState(null);
  const { error, postData } = usePost("https://localhost:7095/api");

  const handleChange = (event) => {
    setFile(event.target.files[0]);
  }

  //temp approach to use axios directly to the agent, gateway returns 500 because of the request body
  const handleSubmit = async (event) => {
    event.preventDefault();
    const formData = new FormData();
    formData.append('jsonFile', file);

    const response = await postData({
      url: '/ConfigAgent', 
      headers: { "Accept": '*/*', "Content-Type": "multipart/form-data" },
      data: formData 
    });

    // await executeRequest({
    //     method: "POST", 
    //     url: '/ConfigAgent', 
    //     headers: { "Accept": '*/*', "Content-Type": "multipart/form-data" }, 
    //     data: formData 
    // });

    if (error) {
      toast.error(error.request.responseText);
    } else {
      toast.success('Data is configured successfully!');
    }
  };

  return (
    <Box className="config-container">
        <InnerHeader title="CONFIGURATION" subtitle="Configure data for the experiments" />
        <Box className="row-container">
          <Box className="form-container">
            <form onSubmit={handleSubmit} className='upload-form'>
                <TextField type="file" onChange={handleChange} />
                <Button type="submit" color="secondary" variant="contained">Upload</Button>
            </form>
          </Box>
          <Box className="json-view-container">
            <JsonViewForm />
          </Box>
        </Box>
    </Box>
  )
}