import React, { useState } from 'react';
import "./Experiment.scss";
import { Box, Button, TextField, Paper, MenuItem } from "@mui/material";
import { Formik } from "formik";
import * as yup from "yup";
import { styled } from '@mui/system';
import { InnerHeader } from "../../components/InnerHeader";
import { ModalExperimentResult } from "../../components/ModalExperimentResult";
import { ExperimentOutcomesGrid } from "../../components/ExperimentOutcomesGrid";
import { usePost } from "../../hooks/usePost.js";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const ScrollableContainer = styled(Paper)({
    overflowY: 'auto',
    maxHeight: 550,
    background: 'transparent'
  });

export const Experiment = () => {
  const { error, postData } = usePost();
  const [isResultBtnShown, setResultButtonShown] = useState(false);
  const [isModalOpen, setModalOpen] = useState(false);
  const [experimentResponse, setExperimentResponse] = useState(null);
  const [selectedDatabaseType, setSelectedDatabaseType] = useState('');

  const handleFormSubmit = async (values) => {
    const experimentRequest = {
      databaseType: parseInt(values.databaseType),
      experimentType: values.experimentType,
      queryExecutionNumber: values.queryExecutionNumber,
      isCacheCleaned: JSON.parse(values.isCacheCleaned)
    };

    const response = await postData({
      url: '/ExperimentRequest',
      headers: { accept: 'text/plain', "Content-Type": "application/json" },
      data: experimentRequest 
    });

    if (error) {
      toast.error(error.request.responseText);
    } else {
      setExperimentResponse(response);
      toast.success('Experiment is created successfully! Wait till it is done!');
      setResultButtonShown(true);
    }
  };

  const initialValues = {
    databaseType: "",
    experimentType: "",
    queryExecutionNumber: 0,
    isCacheCleaned: ""
  };

  const checkoutSchema = yup.object().shape({
    databaseType: yup.string().required("required"),
    experimentType: yup.string().required("required"),
    queryExecutionNumber: yup.string().required("required")
  });

  const handleModalOpen = () => setModalOpen(true);
  const handleModalClose = () => setModalOpen(false);

  return (
    <>
      <InnerHeader title="EXPERIMENTS" subtitle="Create experiments for your database" />
      <ScrollableContainer>
          <Box className="experiment-container">
            <Formik onSubmit={ async (values, { resetForm }) => { await handleFormSubmit(values); resetForm(); }} 
              initialValues={initialValues} validationSchema={checkoutSchema}>
              {({ values, errors, touched, handleBlur, handleChange, handleSubmit }) => (
              <form onSubmit={handleSubmit}>
                <Box className="form-container">
                  <TextField
                      className="input"
                      variant="filled"
                      id="select"
                      label="DatabaseType"
                      onBlur={handleBlur}
                      onChange={(e) => {
                        setSelectedDatabaseType(e.target.value);
                        handleChange(e);
                      }}
                      value={values.databaseType}
                      name="databaseType"
                      error={!!touched.databaseType && !!errors.databaseType}
                      helperText={touched.databaseType && errors.databaseType}
                      select
                    >
                      <MenuItem value="1">MSSQL</MenuItem>
                      <MenuItem value="2">Redis</MenuItem>
                      <MenuItem value="3">Memcached</MenuItem>
                  </TextField>
                  <TextField
                      className="input"
                      variant="filled"
                      id="select"
                      label="ExperimentType"
                      onBlur={handleBlur}
                      onChange={handleChange}
                      value={values.experimentType}
                      name="experimentType"
                      error={!!touched.experimentType && !!errors.experimentType}
                      helperText={touched.experimentType && errors.experimentType}
                      select
                    >
                      <MenuItem value="1">Select Products from Basket by BasketId</MenuItem>
                      <MenuItem value="2">Select Products from Basket for each User</MenuItem>
                      <MenuItem value="3">Count Total Price and Quantity for the Basket</MenuItem>
                      <MenuItem value="4">Get Smartphones from the Catalog</MenuItem>
                      <MenuItem value="5">Get Products with Price option</MenuItem>
                      <MenuItem value="6">Get Apple Products with Filtered Price</MenuItem>
                      <MenuItem value="7">Count Total Paid Orders' Price for each User</MenuItem>
                  </TextField>
                  <TextField
                    className="input"
                    variant="filled"
                    type="number"
                    label="QueryExecutionNumber"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.queryExecutionNumber}
                    name="queryExecutionNumber"
                    error={!!touched.queryExecutionNumber && !!errors.queryExecutionNumber}
                    helperText={touched.queryExecutionNumber && errors.queryExecutionNumber}
                  />
                  { selectedDatabaseType === "1" && (
                    <TextField
                    className="input"
                    variant="filled"
                    id="select"
                    label="IsCacheCleaned"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    value={values.isCacheCleaned}
                    name="isCacheCleaned"
                    select
                    >
                      <MenuItem value="true">True</MenuItem>
                      <MenuItem value="false">False</MenuItem>
                  </TextField>
                  )}
                </Box>
                <Box display="flex" justifyContent="flex-start" mt="30px" gap="50px">
                  <Button type="submit" color="secondary" variant="contained">
                    Run Request
                  </Button>
                  { isResultBtnShown && (
                    <Button color="secondary" variant="contained" onClick={handleModalOpen}>
                      See Experiment's Result
                    </Button>
                  )}
                  { isModalOpen && (
                    <ModalExperimentResult open={isModalOpen} onClose={handleModalClose} values={experimentResponse} />
                  )}
                </Box>
              </form>
              )}
            </Formik>               
          </Box>
          <ExperimentOutcomesGrid />
        </ScrollableContainer>
    </>
  )
}