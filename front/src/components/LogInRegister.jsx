import React, { useState } from 'react';
import '../css/LoginRegister.css';

const LoginRegister = () => {
  const [loginData, setLoginData] = useState({
    loginEmail: '',
    loginPassword: '',
  });

  const [registerData, setRegisterData] = useState({
    registerUsername: '',
    registerEmail: '',
    registerPassword: '',
  });

  const handleLoginChange = (e) => {
    setLoginData({
      ...loginData,
      [e.target.name]: e.target.value,
    });
  };

  const handleRegisterChange = (e) => {
    setRegisterData({
      ...registerData,
      [e.target.name]: e.target.value,
    });
  };

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
    console.log('Login data:', loginData);
  
    try {
      // Step 1: Perform the login
      const response = await fetch(`http://localhost:5163/Login/LoginPlayer/${loginData.loginEmail}/${loginData.loginPassword}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });
  
      if (response.ok) {
        console.log('Login successful!');
        
        // Step 2: Call the /Login/GetToken endpoint to get the token
        const getTokenResponse = await fetch('http://localhost:5163/Login/GetToken', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            email: loginData.loginEmail,
            password: loginData.loginPassword,
          }),
        });
  
        if (getTokenResponse.ok) {
          const tokenData = await getTokenResponse.json();
          const token = tokenData.token;
  
          // Step 3: Store the token in local storage
          localStorage.setItem('token', token);
  
          // Step 4: Reload the page or redirect to another page if needed
          window.location.reload();
        } else {
          console.error('Failed to get token.');
          // Handle the error response for getting the token
        }
      } else {
        console.error('Login failed.');
        // You can handle the error response here
      }
    } catch (error) {
      console.error('An error occurred during login:', error);
    }
  };
  

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    console.log('Register data:', registerData);

    try {
      const response = await fetch(`http://localhost:5163/Login/SignUp/${registerData.registerUsername}/${registerData.registerEmail}/${registerData.registerPassword}/${registerData.registerPassword}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.ok) {
        console.log('Registration successful!');
        window.location.reload();
      } else {
        console.error('Registration failed.');
        // You can handle the error response here
      }
    } catch (error) {
      console.error('An error occurred during registration:', error);
    }
  };

  const existingToken = localStorage.getItem('token');
  if (existingToken) {
    // If a token is present, redirect the user (adjust the redirect path as needed)
    window.location.href = '/';
    return null; // Optionally, you can return null to prevent rendering the login/register form
  }

  return (
    <div className="login-register-container">
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleLoginSubmit}>
        <label>
          Email:
          <input
            type="text"
            name="loginEmail"
            value={loginData.loginEmail}
            onChange={handleLoginChange}
          />
        </label>
        <label>
          Password:
          <input
            type="password"
            name="loginPassword"
            value={loginData.loginPassword}
            onChange={handleLoginChange}
          />
        </label>
        <button type="submit">Login</button>
      </form>
    </div>

    <div className="register-container">
      <h2>Register</h2>
      <form onSubmit={handleRegisterSubmit}>
        <label>
          Username:
          <input
            type="text"
            name="registerUsername"
            value={registerData.registerUsername}
            onChange={handleRegisterChange}
          />
        </label>
        <label>
          Email:
          <input
            type="email"
            name="registerEmail"
            value={registerData.registerEmail}
            onChange={handleRegisterChange}
          />
        </label>
        <label>
          Password:
          <input
            type="password"
            name="registerPassword"
            value={registerData.registerPassword}
            onChange={handleRegisterChange}
          />
        </label>
        <button name='button-register' type="submit">Register</button>
      </form>
    </div>
  </div>
  );
};

export default LoginRegister;
