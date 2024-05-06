import React from 'react';
import "./Dashboard.scss";
import { InnerHeader } from "../../components/InnerHeader";
import { Box, Typography } from '@mui/material';
import { ExperimentPerDatabase } from '../Analytics/ExperimentPerDatabase/ExperimentPerDatabase';
import { useTheme } from "@mui/material";
import { tokens } from "../../theme";

export const Dashboard = () => {
  const theme = useTheme();
  const colors = tokens(theme.palette.mode);

  return (
    <Box className="dashboard-container">
      <Box className="inner-header-container">
        <InnerHeader title="DASHBOARD" subtitle="Welcome to the dashboard" />
          <Box
            display="grid"
            gridTemplateColumns="repeat(12, 1fr)"
            gridAutoRows="140px"
            gap="20px"
          >
            <Box
              gridColumn="span 8"
              gridRow="span 2"
              backgroundColor={colors.primary[400]}
              width="900px"
              height="500px"
            >
              <Box
                mt="25px"
                p="0 30px"
                display="flex "
                justifyContent="space-between"
                alignItems="center"
              >
                <Box>
                  <Typography
                    variant="h5"
                    fontWeight="600"
                    color={colors.grey[100]}
                  >
                    The Success of the Caching Mechanism in each Experiment per Database
                  </Typography>
                  <Typography
                    variant="h3"
                    fontWeight="bold"
                    color={colors.greenAccent[500]}
                  >
                    Total Experiments: 7
                  </Typography>
                </Box>
              </Box>
              <Box height="250px" m="-20px 0 0 0">
                <ExperimentPerDatabase width={900} height={400} />
              </Box>
            </Box>
          </Box>
      </Box>
    </Box>
  )
}