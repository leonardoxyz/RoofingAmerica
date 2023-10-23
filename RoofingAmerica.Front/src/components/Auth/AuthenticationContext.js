
import { createContext, useContext, useState } from 'react';

const AuthenticationContext = createContext();

export function useAuthentication() {
  return useContext(AuthenticationContext);
}

export function AuthenticationProvider({ children }) {
  const [authenticated, setAuthenticated] = useState(false);

  const login = () => {
    // Implement your login logic here
    setAuthenticated(true);
  };

  const logout = () => {
    // Implement your logout logic here
    setAuthenticated(false);
  };

  return (
    <AuthenticationContext.Provider value={{ authenticated, login, logout }}>
      {children}
    </AuthenticationContext.Provider>
  );
}
