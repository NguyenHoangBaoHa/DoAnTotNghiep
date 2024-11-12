import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import '../Css/Navbar.css';

const Navbar = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    setIsLoggedIn(!!token);
  }, []);

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">Trang Chủ</Link>
        <div className="navbar-menu">
          {isLoggedIn ? (
            <>
              <Link to="/manage-pitches">Quản lý sân</Link>
              <Link to="/manage-pitch-types">Quản lý loại sân</Link>
              <Link to="/manage-bookings">Quản lý đặt sân</Link>
              <Link to="/create-staff">Đăng ký tài khoản nhân viên</Link>
            </>
          ) : (
            <>
              <Link to="/login">Đăng Nhập</Link>
              <Link to="/register">Đăng Ký</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
