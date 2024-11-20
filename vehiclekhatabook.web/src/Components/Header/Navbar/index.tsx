"use client";
import React, { useState } from "react";
import { Nav, Navbar, NavbarBrand } from "reactstrap";
import MenuIcon from "@mui/icons-material/Menu";
import Link from "next/link";
import Cookies from "js-cookie";
import { useRouter } from "next/navigation";
import {
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
} from "reactstrap";
import { Avatar, Box, Divider, Typography } from "@mui/material";
import {
  Person,
  LocalActivity,
  Star,
  Settings,
  PowerSettingsNew,
} from "@mui/icons-material";

const NavbarTop = (props: any) => {
  const router = useRouter();

  const [dropdownOpen, setDropdownOpen] = useState(false);

  const handleMouseEnter = () => setDropdownOpen(true);
  const handleMouseLeave = () => setDropdownOpen(false);

  const handleLogout = () => {
    Cookies.remove("session");
    router.push("/login");
  };

  return (
    <header>
      <Navbar
        expanded="lg"
        className="navbar-classic navbar navbar-expand-lg"
        style={{
          width: "100%",
          backgroundColor: "#C1212B", // Red background color
          color: "white", // Optional: white text for better contrast
        }}
      >
        <div className="d-flex justify-content-between w-100">
          <div className="d-flex align-items-center">
            <Link
              href="#"
              id="nav-toggle"
              className="nav-icon me-2 icon-xs"
              onClick={() => props.data.SidebarToggleMenu(!props.data.showMenu)}
            >
              <MenuIcon sx={{ color: "white" }} /> {/* White menu icon */}
              </Link>
          </div>

          <Nav className="ms-auto d-flex">
            <Box
              className="border-0 ms-2 p-0"
              onMouseEnter={handleMouseEnter}
              onMouseLeave={handleMouseLeave}
            >
              <Dropdown isOpen={dropdownOpen} toggle={() => {}}>
                <DropdownToggle tag="span" style={{ cursor: "pointer" }}>
                  <Avatar sx={{ bgcolor: "#e72900" }} className="mx-2 pb-0">
                    A
                  </Avatar>
                </DropdownToggle>
                <DropdownMenu>
                  <DropdownItem onClick={handleLogout} className="text-primary">
                    <PowerSettingsNew className="me-2" fontSize="small" />{" "}
                    Logout
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>
            </Box>
          </Nav>
        </div>
      </Navbar>
    </header>
  );
};

export default NavbarTop;
