import React from 'react';

function LogoutButton() {
  const handleLogout = () => {
    localStorage.removeItem('token');
    window.location.href = '/login'; 
  };

  return <button className='logout-cta' onClick={handleLogout}>Logout</button>;
}

export default LogoutButton;
