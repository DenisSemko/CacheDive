import { useState } from "react";
import { ColorModeContext, useMode } from "./theme";
import { ThemeProvider } from "@mui/material";
import CssBaseline from '@mui/material/CssBaseline';
import { Topbar } from "./components/Topbar";
import { Sidebar } from "./components/Sidebar";
import { Routes, Route } from "react-router-dom";
import { Registration } from "./pages/Registration";
import { Dashboard } from "./pages/Dashboard";

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
          <main className="content">
            <Topbar setIsSidebar={setIsSidebar} />
            <Routes>
              <Route path="/" />
              <Route path="/registration" element={<Registration setIsSidebar={setIsSidebar} />} />
              <Route path="/dashboard" element={<Dashboard />}/>
            </Routes>
          </main>
        </div>
      </ThemeProvider>
    </ColorModeContext.Provider>
  );
}