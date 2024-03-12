import React from 'react';
import "./Dashboard.scss";
import { InnerHeader } from "../../components/InnerHeader";
import { Box } from '@mui/material';

export const Dashboard = () => {
  return (
    <Box className="dashboard-container">
      <Box className="inner-header-container">
        <InnerHeader title="DASHBOARD" subtitle="Welcome to the dashboard" />
      </Box>
    </Box>
  )
}