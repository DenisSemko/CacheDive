import React from 'react';
import "./Registration.scss";
import { Link } from 'react-router-dom';
import { Box, Button, TextField } from "@mui/material";
import { Formik } from "formik";
import * as yup from "yup";
import { InnerHeader } from "../../components/InnerHeader";
import { usePost } from "../../hooks/usePost.js";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export const Registration = () => {
  const { error, postData } = usePost();
  
  const handleFormSubmit = async (values) => {
    await postData({
      url: '/Auth/registration',
      data: values 
    });

    if (error) {
      toast.error(error.request.responseText);
    } else {
      toast.success('User created successfully!');
    }
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
        <Formik onSubmit={ async (values, { resetForm }) => { await handleFormSubmit(values); resetForm(); }} 
          initialValues={initialValues} validationSchema={checkoutSchema}>
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
              <Link className='link' to="/">Already Registered? Sign In!</Link>
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