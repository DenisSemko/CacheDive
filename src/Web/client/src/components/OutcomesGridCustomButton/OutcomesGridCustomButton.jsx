import React, { useState } from 'react';
import { ModalExperimentResult } from "../../components/ModalExperimentResult";
import { Button } from "@mui/material";

export const OutcomesGridCustomButton = ({ row }) => {
  const [isModalOpen, setModalOpen] = useState(false);
  const handleModalOpen = () => setModalOpen(true);
  const handleModalClose = () => setModalOpen(false);


  return (
    <>
        <Button color="secondary" variant="contained" onClick={handleModalOpen}>
            Details
        </Button>
        { isModalOpen && (
            <ModalExperimentResult open={isModalOpen} onClose={handleModalClose} values={row} />
        )}
    </>
  )
}