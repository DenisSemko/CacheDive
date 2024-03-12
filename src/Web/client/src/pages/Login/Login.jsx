import React, { useEffect } from 'react';
import "./Login.scss";
import { useNavigate } from 'react-router-dom';
import Button from '@mui/material/Button';

export const Login = ({ setIsSidebar }) => {

  const navigate = useNavigate();
  
  function handleSidebar () {
    setIsSidebar(true);
    localStorage.setItem('isSidebar', JSON.stringify(true));
    navigate('/dashboard');
  }

  useEffect(() => {
    const storedIsSidebar = localStorage.getItem('isSidebar');
    if(storedIsSidebar) {
      localStorage.setItem('isSidebar', JSON.stringify(false));
    }
  }, []);



  return (
    <div>
        <Button onClick={handleSidebar} color="success">Click me</Button>
    </div>
  )
}