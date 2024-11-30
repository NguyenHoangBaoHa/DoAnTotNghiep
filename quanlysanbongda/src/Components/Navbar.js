import React, { useContext, useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import {AuthContext} from '../Context/AuthContext';
import "../Css/Navbar.css";

const Navbar = () => {
  const {auth, logout} = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  }

  return (
    <nav className="navbar">
      <div className="navbar-container">
        {/* Mục chung cho tất cả người dùng */}
        <div className="common-menu">
          <Link to="/">Trang Chủ</Link>
        </div>

        {/* Hiển thị mục theo từng role */}
        <div className="role-menu">
          {auth.isLoggedIn && auth.role === "Admin" && (
            <div className="admin-menu">
              <Link to="/manage-pitches-admin">Quản lý sân</Link>
              <Link to="/manage-pitch-types-admin">Quản lý loại sân</Link>
              <Link to="/manage-bookings-admin">Quản lý đặt sân</Link>
              <Link to="/revenue-report-admin">Báo cáo doanh thu</Link>
              <Link to="/create-staff">Đăng ký tài khoản nhân viên</Link>
            </div>
          )}

          {auth.isLoggedIn && auth.role === "Staff" && (
            <div className="staff-menu">
              <Link to="/manage-pitches-staff">Quản lý sân</Link>
              <Link to="/manage-bookings-staff">Quản lý đặt sân</Link>
            </div>
          )}

          {auth.isLoggedIn && auth.role === "Customer" && (
            <div className="customer-menu">
              <Link to="/my-bookings">Lịch sử đặt sân</Link>
              <Link to="/profile">Hồ sơ</Link>
            </div>
          )}
        </div>

        {/* Hiển thị nút Đăng Nhập/Đăng Xuất */}
        <div className="auth-menu">
          {auth.isLoggedIn ? (
            <button onClick={handleLogout} className="logout-btn">
              Đăng Xuất
            </button>
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
