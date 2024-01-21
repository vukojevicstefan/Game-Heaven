import React from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';
import Navbar from './Navbar';
import Games from './Games';
import MyGames from './MyGames';
import About from './About';
import LoginRegister from './LogInRegister';
import '../css/App.css';

function App() {
  const location = useLocation();
  const isLoginRegisterPage = location.pathname === '/login-register';

  return (
    <div className="app">
      {!isLoginRegisterPage && <Navbar />}
      <section className="content">
        <Routes>
          <Route path="/" element={<Games />} />
          <Route path="/my-games" element={<MyGames />} />
          <Route path="/about" element={<About />} />
          <Route path="/login-register" element={<LoginRegister />} />
        </Routes>
      </section>
    </div>
  );
}

export default App;
