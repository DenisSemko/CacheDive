import React from 'react';
import { Box, TextField, Modal } from "@mui/material";

export const ModalExperimentResult = ({ open, onClose, values }) => {
    const style = {
        position: 'absolute',
        display: 'flex',
        flexDirection: 'column',
        gap: '15px',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 600,
        height: 520,
        bgcolor: 'background.paper',
        border: '2px solid #000',
        boxShadow: 24,
        p: 4,
    };

    const getTypeString = (type) => {
        switch (type) {
          case "BasketBasketId":
            return "Select Products from Basket by BasketId";
          case "BasketUserId":
            return "Select Products from Basket for each User";
          case "BasketTotalPrice":
            return "Count Total Price and Quantity for the Basket";
          case "ProductSmartphones":
            return "Get Smartphones from the Catalog";
          case "ProductPrice":
            return "Get Products with Price option";
          case "ProductLike":
            return "Get Apple Products with Filtered Price";
          case "OrderGroupBy":
            return "Count Total Paid Orders' Price for each User";
          default:
            return "unknown";
        }
      }

  return (
    <Modal
        open={open}
        onClose={onClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
            <TextField
                className="input"
                variant="filled"
                label="Experiment"
                value={getTypeString(values.experimentType)}
                name="experiment"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Cache Hit Rate (%)"
                value={values.cacheHitRate}
                name="cacheHitRate"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Cache Miss Rate (%)"
                value={values.cacheMissRate}
                name="cacheMissRate"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Cache Size (Mb)"
                value={values.cacheSize}
                name="cacheSize"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Query Execution Number"
                value={values.queryExecutionNumber}
                name="queryExecutionNumber"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Experiment Execution Time"
                value={values.experimentExecutionTime}
                name="experimentExecutionTime"
                disabled
            />
            <TextField
                className="input"
                variant="filled"
                label="Resources: Size of the cache plan (kb), CPU compile time / Driver time (ms), amount of memory used for the query (kb)"
                value={values.resources}
                name="resources"
                disabled
            />
        </Box>
      </Modal>
  )
}