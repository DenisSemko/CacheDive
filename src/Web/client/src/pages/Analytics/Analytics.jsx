import React, { useState } from 'react';
import "./Analytics.scss";
import { Button } from "@mui/material";
import { InnerHeader } from "../../components/InnerHeader";
import { ExperimentPerDatabase } from './ExperimentPerDatabase/ExperimentPerDatabase';
import { ExecutionTime } from './ExecutionTime/ExecutionTime';
import { DriverTime } from './DriverTime/DriverTime';
import { CacheHit } from './CacheHit/CacheHit';
import { CacheMiss } from './CacheMiss/CacheMiss';

  const ChartOptions = ({ onSelect }) => (
    <div style={{ display: 'flex', justifyContent: 'space-around', padding: '0 5px' }}>
      <Button color="secondary" variant="contained" onClick={() => onSelect('experiment')}>Experiment Per Database</Button>
      <Button color="secondary" variant="contained" onClick={() => onSelect('execution')}>Execution Time</Button>
      <Button color="secondary" variant="contained" onClick={() => onSelect('driver')}>Driver Time</Button>
      <Button color="secondary" variant="contained" onClick={() => onSelect('hit')}>Cache Hit</Button>
      <Button color="secondary" variant="contained" onClick={() => onSelect('miss')}>Cache Miss</Button>
    </div>
  );

export const Analytics = () => {
    const [selectedOption, setSelectedOption] = useState(null);

    return (
        <>
        <InnerHeader title="ANALYTICS" subtitle="Find out the Experiments analytics" />
        <ChartOptions onSelect={setSelectedOption} />
            <div style={{ width: '500px', height: '500px' }}>
                {selectedOption === 'experiment' && <ExperimentPerDatabase width={1000} height={400} />}
                {selectedOption === 'execution' && <ExecutionTime width={1000} height={400} />}
                {selectedOption === 'driver' && <DriverTime width={1000} height={400} />}
                {selectedOption === 'hit' && <CacheHit width={400} height={400} />}
                {selectedOption === 'miss' && <CacheMiss width={400} height={400} />}
            </div>
        </>
    )
}