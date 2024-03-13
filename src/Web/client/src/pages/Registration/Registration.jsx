import React, { useEffect } from 'react';
import "./Registration.scss";
import { useNavigate } from 'react-router-dom';
import { Box, Button, TextField } from "@mui/material";
import { Formik } from "formik";
import * as yup from "yup";
import { InnerHeader } from "../../components/InnerHeader";

export const Registration = ({ setIsSidebar }) => {

  const navigate = useNavigate();
  
  useEffect(() => {
    const storedIsSidebar = localStorage.getItem('isSidebar');
    if(storedIsSidebar) {
      localStorage.setItem('isSidebar', JSON.stringify(false));
    }
  }, []);

  const handleFormSubmit = (values) => {
    console.log(values);
    setIsSidebar(true);
    localStorage.setItem('isSidebar', JSON.stringify(true));
    navigate('/dashboard');
  };

  const initialValues = {
    name: "",
    username: "",
    email: "",
    passwordHash: ""
  };

  const passwordRegExp = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{8,}$/;

  const checkoutSchema = yup.object().shape({
    name: yup.string().required("required"),
    username: yup.string().required("required"),
    email: yup.string().email("invalid email").required("required"),
    passwordHash: yup.string().matches(passwordRegExp, "Password is not valid").required("required")
  });




  return (
    <Box className="registration-container">
        <InnerHeader title="REGISTRATION" subtitle="Create a New User Profile" />
        <Formik onSubmit={handleFormSubmit} initialValues={initialValues} validationSchema={checkoutSchema}>
          {({ values, errors, touched, handleBlur, handleChange, handleSubmit }) => (
            <form onSubmit={handleSubmit}>
              <Box className="form-container">
                <TextField
                  className="input"
                  variant="filled"
                  type="text"
                  label="Name"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.name}
                  name="name"
                  error={!!touched.name && !!errors.name}
                  helperText={touched.name && errors.name}
                />
                <TextField
                  className="input"
                  variant="filled"
                  type="text"
                  label="Username"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.username}
                  name="username"
                  error={!!touched.username && !!errors.username}
                  helperText={touched.username && errors.username}
                />
                <TextField
                  className="input"
                  variant="filled"
                  type="text"
                  label="Email"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.email}
                  name="email"
                  error={!!touched.email && !!errors.email}
                  helperText={touched.email && errors.email}
                />
                <TextField
                  className="input"
                  variant="filled"
                  type="text"
                  label="Password"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.passwordHash}
                  name="passwordHash"
                  error={!!touched.passwordHash && !!errors.passwordHash}
                  helperText={touched.passwordHash && errors.passwordHash}
                />
              </Box>
              <Box display="flex" justifyContent="center" mt="30px">
                <Button type="submit" color="secondary" variant="contained">
                  Create New User
                </Button>
              </Box>
            </form>
          )}
        </Formik>
    </Box>
  )
}