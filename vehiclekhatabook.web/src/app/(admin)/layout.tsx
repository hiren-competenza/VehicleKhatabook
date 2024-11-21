"use client";
import { useState } from "react";
import Footer from "@/Components/Footer";
import SideMenu from "@/Components/Header/SideMenu";
import NavbarTop from "@/Components/Header/Navbar";
import "@/scss/Admin/index.scss";
import { ToastContainer } from 'react-toastify';

export default function AdminLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const [showMenu, setShowMenu] = useState(true);
  const ToggleMenu = () => {
    return setShowMenu(!showMenu);
  };
  return (
    <>
      {/* Header section */}
      <div id="db-wrapper" className={`${showMenu ? "" : "toggled"}`}>
        <div className="navbar-vertical navbar">
          <SideMenu />
        </div>
        <div id="page-content">
          <div className="header">
            <NavbarTop
              data={{
                showMenu: showMenu,
                SidebarToggleMenu: ToggleMenu,
              }}
            />
          </div>
          {children}
        </div>
          </div>
          <ToastContainer />
      <Footer />
    </>
  );
}

