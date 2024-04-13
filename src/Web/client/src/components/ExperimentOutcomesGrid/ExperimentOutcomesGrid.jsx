import React from 'react';
import "./ExperimentOutcomesGrid.scss";
import { Box, useTheme } from "@mui/material";
import { DataGrid } from '@mui/x-data-grid';
import { InnerHeader } from "../../components/InnerHeader";
import { OutcomesGridCustomButton } from "../../components/OutcomesGridCustomButton";
import { useFetch } from "../../hooks/useFetch.js";
import { tokens } from "../../theme";

export const ExperimentOutcomesGrid = () => {
    const theme = useTheme();
    const colors = tokens(theme.palette.mode);
    const columns = [
        { field: "id", headerName: "ID" },
        {
          field: "query",
          headerName: "Query",
          flex: 1,
          cellClassName: "name-column--cell",
        },
        {
          field: "databaseType",
          headerName: "Database",
          headerAlign: "left",
          align: "left",
        },
        {
            field: "isExecutedFromCache",
            headerName: "Executed From Cache",
            headerAlign: "left",
            align: "left",
          },
        {
            field: 'button',
            headerName: 'Action',
            width: 150,
            renderCell: (params) => <OutcomesGridCustomButton row={params.row} />,
        },
    ];

    const { data, isLoading, error } = useFetch({
        method: 'GET',
        url: '/ExperimentOutcome',
        headers: { "Accept": '*/*' }
    });

  return (
    <Box className="outcomes-container">
        <InnerHeader title="Experiment Outcomes" />
        <Box
            m="40px 0 0 0"
            height="65vh"
            sx={{
            "& .MuiDataGrid-root": {
                border: "none",
            },
            "& .MuiDataGrid-cell": {
                borderBottom: "none",
            },
            "& .name-column--cell": {
                color: colors.greenAccent[300],
            },
            "& .MuiDataGrid-columnHeaders": {
                backgroundColor: colors.blueAccent[700],
                borderBottom: "none",
            },
            "& .MuiDataGrid-virtualScroller": {
                backgroundColor: colors.primary[400],
            },
            "& .MuiDataGrid-footerContainer": {
                borderTop: "none",
                backgroundColor: colors.blueAccent[700],
            },
            "& .MuiCheckbox-root": {
                color: `${colors.greenAccent[200]} !important`,
            },
            }}
        >
            <DataGrid checkboxSelection rows={data} columns={columns} />
        </Box>
    </Box>
  )
}