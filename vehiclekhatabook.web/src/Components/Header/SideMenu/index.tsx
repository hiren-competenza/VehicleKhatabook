import Image from "next/image";
import Link from "next/link";
import { usePathname } from "next/navigation";
import React from "react";

const menuData = [
  {
    title: "Dashboard",
    href: "/",
    submenu: [],
  },
  {
    title: "Language Type Management",
    href: "/language-management",
    submenu: [],
  },
  {
    title: "Vehicle Type Management",
    href: "/vehicle-type-management",
    submenu: [],
  },
  {
    title: "Income Category Management",
    href: "/income-category-management",
    submenu: [],
  },
  {
    title: "Expense Category Management",
    href: "/expense-category",
    submenu: [],
  },
  {
    title: "Application Configuration",
    href: "/application-configuration",
    submenu: [],
  },
];

const SideMenu = () => {
  const activePath = usePathname();

  return (
    <div className="nav-scroller">
      <ul className="navbar-nav flex-column">
        {menuData.map((item) => (
          <div
            key={item.title}
            className="nav-item"
            style={{ borderBottom: "1px solid #a02308" }} // Dark red separating line
          >
            <Link
              href={item.href}
              className={`nav-link justify-content-between ${
                activePath === item.href ? "active" : ""
              }`}
              style={
                item.title === "Dashboard"
                  ? { color: "gold", fontSize: "1.5rem" } // Bigger font for Dashboard
                  : {}
              }
            >
              {item.title}
            </Link>
          </div>
        ))}
      </ul>
    </div>
  );
};

export default SideMenu;
