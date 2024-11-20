"use client";
import { Container } from "reactstrap";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";

const Admin = () => {
  const router = useRouter();

  const handleLogout = () => {
    Cookies.remove("session");
    router.push("/login");
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
