"use client";
import { Container, Navbar } from "reactstrap";
import { useRouter } from "next/router";
  
const Admin = () => {
  const router = useRouter();

  const handleLogout = () => {
    console.log("Logout button clicked");
    router.push ('/language-management')
    // Cookies.remove("session");
    // router.push("/auth/admin/login");
  };
  return (
    <Container>
      <h1>Admin Page</h1>
      <button className="btn btn-secondary" onClick={handleLogout}>
        Logout
      </button>
    </Container>
    
  );
};

export default Admin;
