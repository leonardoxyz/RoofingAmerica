import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthenticationProvider, useAuthentication } from './components/Auth/AuthenticationContext';
import SaleForm from './components/Sale/SaleForm';
import Login from './components/Auth/Login';
import Register from './components/Auth/Register';

function ProtectedRoute({ children }) {
  const { authenticated } = useAuthentication();

  if (!authenticated) {
    return <Navigate to="/login" />;
  }

  return children;
}

function App() {
  return (
    <Router>
      <AuthenticationProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<ProtectedRoute><SaleForm /></ProtectedRoute>} />
        </Routes>
      </AuthenticationProvider>
    </Router>
  );
}

export default App;
