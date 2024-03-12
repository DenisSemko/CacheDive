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
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import { useNavigate } from 'react-router-dom';


export const Topbar = ({setIsSidebar}) => {
  const theme = useTheme();
  const colors = tokens(theme.palette.mode);
  const colorMode = useContext(ColorModeContext);
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    // add logout logic
    setIsSidebar(false);
    navigate('/');
    handleClose();
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
        <IconButton className="icon">
          <SettingsOutlinedIcon />
        </IconButton>
        <IconButton className="icon" onClick={handleClick}>
          <PersonOutlinedIcon />
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleClose}
          >
            <MenuItem onClick={handleLogout}>Logout</MenuItem>
          </Menu>
        </IconButton>
      </Box>
    </Box>
  )
}