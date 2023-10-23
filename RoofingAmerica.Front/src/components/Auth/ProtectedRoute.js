import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useAuthentication } from './AuthenticationContext';

function ProtectedRoute({ component: Component, ...rest }) {
  const { authenticated } = useAuthentication();

  return (
    <Route
      {...rest}
      render={(props) =>
        authenticated ? <Component {...props} /> : <Redirect to="/login" />
      }
    />
  );
}

export default ProtectedRoute;