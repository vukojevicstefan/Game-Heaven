import React from 'react';
import '../css/Navbar.css';
import { Link, useMatch, useResolvedPath } from "react-router-dom";

export default function Navbar() {
  const handleLogOut = () => {
    // Remove token from local storage
    localStorage.removeItem('token');
    // Refresh the window
    window.location.reload();
  };

  return (
    <nav className="nav">
      <ul>
        <CustomLink to="/" className="site-title">
          Game Heaven
        </CustomLink>
        <div className='other-links'>
          <CustomLink className="games-link" to="/my-games">My Games</CustomLink>
          <CustomLink to="/about">About</CustomLink>  
          <p onClick={handleLogOut}>Log out</p>
        </div>
      </ul>
    </nav>
  );
}

function CustomLink({ to, children, ...props }) {
  const resolvedPath = useResolvedPath(to);
  const isActive = useMatch({ path: resolvedPath.pathname, end: true });

  return (
    <li className={isActive ? "active" : ""}>
      <Link to={to} {...props}>
        {children}
      </Link>
    </li>
  );
}
