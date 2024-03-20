import React, { useEffect, useState } from 'react';
import "./Login.scss";
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { Box, Button, TextField, InputAdornment, IconButton } from "@mui/material";
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { Formik } from "formik";
import * as yup from "yup";
import { InnerHeader } from "../../components/InnerHeader";
import { usePost } from "../../hooks/usePost.js";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const Login = ({ setIsSidebar }) => {
  const navigate = useNavigate();
  const { error, postData } = usePost();
  const [showPassword, setShowPassword] = useState(false);
  const handleClickShowPassword = () => setShowPassword(!showPassword);
  const handleMouseDownPassword = () => setShowPassword(!showPassword);


  useEffect(() => {
    const storedIsSidebar = localStorage.getItem('isSidebar');
    if(storedIsSidebar) {
      localStorage.setItem('isSidebar', JSON.stringify(false));
    }
  }, []);

  const handleFormSubmit = async (values) => {

    await postData({
      url: '/Auth/login', 
      data: values 
    });

    if (error) {
      toast.error(error.request.responseText);
    } else {
      toast.success('User signed in successfully!');
      setIsSidebar(true);
      localStorage.setItem('isSidebar', JSON.stringify(true));
      navigate('/dashboard');
    }
  };

  const initialValues = {
    username: "",
    password: ""
  };

  const passwordRegExp = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{8,}$/;

  const checkoutSchema = yup.object().shape({
    username: yup.string().required("required"),
    password: yup.string().matches(passwordRegExp, "Password is not valid").required("required")
  });

  return (
    <Box className="login-container">
        <InnerHeader title="LOGIN" subtitle="Sign In a User" />
        <Formik onSubmit={ async (values, { resetForm }) => { await handleFormSubmit(values); resetForm(); }} 
          initialValues={initialValues} validationSchema={checkoutSchema}>
          {({ values, errors, touched, handleBlur, handleChange, handleSubmit }) => (
            <form onSubmit={handleSubmit}>
              <Box className="form-container">
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
                  type={showPassword ? "text" : "password"}
                  label="Password"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  value={values.password}
                  name="password"
                  error={!!touched.password && !!errors.password}
                  helperText={touched.password && errors.password}
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          aria-label="toggle password visibility"
                          onClick={handleClickShowPassword}
                          onMouseDown={handleMouseDownPassword}
                        >
                          {showPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                        </IconButton>
                      </InputAdornment>
                    )
                  }}
                />
              </Box>
              <Link className='link' to="/registration">Not Registered Yet?</Link>
              <Box display="flex" justifyContent="center" mt="30px">
                <Button type="submit" color="secondary" variant="contained">
                  Sign In
                </Button>
              </Box>
            </form>
          )}
        </Formik>
    </Box>
  )
}