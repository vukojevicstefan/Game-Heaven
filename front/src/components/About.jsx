export default function About() {
  
  const existingToken = localStorage.getItem('token');
  if (!existingToken) {
    // If a token is present, redirect the user (adjust the redirect path as needed)
    window.location.href = '/login-register';
    return null; // Optionally, you can return null to prevent rendering the login/register form
  }
  
  return <h1>
    Game Haven is a project made for course named Testing and Quality of Software. 
    University of Nis, Faculty of electrical engineering, Computer Science, Jan 2024.
    Developers: Stefan Vukojevic and Milos Stanojevic.
    Thanks for reading :)

  </h1>
}
