import React from 'react';
import "./Topbar.scss";
import { Box, IconButton, useTheme } from "@mui/material";
import { useContext, useState } from "react";
import { ColorModeContext, tokens } from "../../theme";
import InputBase from "@mui/material/InputBase";
import LightModeOutlinedIcon from "@mui/icons-material/LightModeOutlined";
import DarkModeOutlinedIcon from "@mui/icons-material/DarkModeOutlined";
import NotificationsOutlinedIcon from "@mui/icons-material/NotificationsOutlined";
import SettingsOutlinedIcon from "@mui/icons-material/SettingsOutlined";
import PersonOutlinedIcon from "@mui/icons-material/PersonOutlined";
import SearchIcon from "@mui/icons-material/Search";
import LoginIcon from '@mui/icons-material/Login';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import { useNavigate, useLocation } from 'react-router-dom';
import { usePost } from "../../hooks/usePost.js";


export const Topbar = ({setIsSidebar}) => {
  const theme = useTheme();
  const colors = tokens(theme.palette.mode);
  const colorMode = useContext(ColorModeContext);
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();
  const isLoginPage = location.pathname === '/';
  const isRegistrationPage = location.pathname === '/registration';
  const { error, postData } = usePost();
  const logoutData = {
    username: "test"
  };

  const handlePersonClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handlePersonClose = () => {
    setAnchorEl(null);
  };

  const handleLoginClick = () => {
    window.location.href = "/";
  };

  const handleSettingsClick = () => {
    window.location.href = "/configuration";
  };

  const handleLogout = async () => {
    const response = await postData({
      url: '/Auth/logout',
      headers: { "Accept": '*/*' }, 
      data: logoutData 
    });

    if (!error) {
      setIsSidebar(false);
      navigate('/');
      handlePersonClose();
    }
   
  };

  return (
    <Box className="box-container">
      <Box className="search-bar-container" backgroundColor={colors.primary[400]}>
        <InputBase className="search-input" placeholder="Search" />
        <IconButton className="search-btn">
          <SearchIcon />
        </IconButton>
      </Box>
      <Box className="icons-container">
        <IconButton onClick={colorMode.toggleColorMode} className="icon">
          {theme.palette.mode === "dark" ? (
            <DarkModeOutlinedIcon />
          ) : (
            <LightModeOutlinedIcon />
          )}
        </IconButton>
        <IconButton className="icon">
          <NotificationsOutlinedIcon />
        </IconButton>
        { !isLoginPage && !isRegistrationPage ? 
        ( 
          <IconButton className="icon" onClick={handleSettingsClick}>
            <SettingsOutlinedIcon />
          </IconButton>
        ) : (
          <></>
        )}
        { !isLoginPage && !isRegistrationPage ? 
        (
          <IconButton className="icon" onClick={handlePersonClick}>
          <PersonOutlinedIcon />
            <Menu
              anchorEl={anchorEl}
              open={Boolean(anchorEl)}
              onClose={handlePersonClose}
            >
              <MenuItem onClick={handleLogout}>Logout</MenuItem>
            </Menu>
          </IconButton>
        ) : 
          <IconButton className="icon" onClick={handleLoginClick}>
            <LoginIcon />
          </IconButton>
        }
      </Box>
    </Box>
  )
}