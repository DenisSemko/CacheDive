import { useState } from "react";
import { ColorModeContext, useMode } from "./theme";
import { ThemeProvider } from "@mui/material";
import CssBaseline from '@mui/material/CssBaseline';
import { Topbar } from "./components/Topbar";
import { Sidebar } from "./components/Sidebar";
import { Routes, Route } from "react-router-dom";
import { Registration } from "./pages/Registration";
import { Login } from "./pages/Login";
import { Dashboard } from "./pages/Dashboard";
import { Config } from "./pages/Config";
import { Experiment } from "./pages/Experiment";
import { Analytics } from "./pages/Analytics";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const App = () => {
  const [theme, colorMode] = useMode();
  const [isSidebar, setIsSidebar] = useState(() => {
    const storedIsSidebar = localStorage.getItem('isSidebar');
    return storedIsSidebar ? JSON.parse(storedIsSidebar) : false;
  });

  return (
    <ColorModeContext.Provider value={colorMode}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <div className="app">
          <Sidebar isSidebar={isSidebar} />
          <main className="content" style={{ overflow: 'hidden'}}>
            <Topbar setIsSidebar={setIsSidebar} />
            <Routes>
              <Route path="/" element={<Login setIsSidebar={setIsSidebar} />} />
              <Route path="/registration" element={<Registration />} />
              <Route path="/dashboard" element={<Dashboard />}/>
              <Route path="/configuration" element={<Config />}/>
              <Route path="/experiment" element={<Experiment />}/>
              <Route path="/analytics" element={<Analytics />}/>
            </Routes>
            <ToastContainer position="top-right" autoClose={5000} />
          </main>
        </div>
      </ThemeProvider>
    </ColorModeContext.Provider>
  );
}