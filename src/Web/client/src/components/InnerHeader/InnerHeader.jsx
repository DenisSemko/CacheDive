import React from 'react';
import "./InnerHeader.scss";
import { Typography, Box, useTheme } from "@mui/material";
import { tokens } from "../../theme";

export const InnerHeader = ({ title, subtitle }) => {
    const theme = useTheme();
    const colors = tokens(theme.palette.mode);
    return (
      <Box className="header-container">
        <Typography className='header-title'
          variant="h2"
          color={colors.grey[100]}>
          {title}
        </Typography>
        <Typography variant="h5" color={colors.greenAccent[400]}>
          {subtitle}
        </Typography>
      </Box>
    );
  };