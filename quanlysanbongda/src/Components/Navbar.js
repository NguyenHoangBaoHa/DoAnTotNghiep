import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import '../Css/Navbar.css';

const Navbar = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [role, setRole] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userRole = localStorage.getItem("role");

    if (token) {
      setIsLoggedIn(true);
      setRole(userRole);
    } else {
      setIsLoggedIn(false);
      setRole("");
    }
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    setIsLoggedIn(false);
    setRole("");
    navigate('/login')
  }

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">
          Trang Chủ
        </Link>
        <div className="navbar-menu">
          {isLoggedIn ? (
            <>
              {/* Admin Menu */}
              {role === "Admin" && (
                <>
                  <Link to="/manage-pitches">Quản lý sân</Link>
                  <Link to="/manage-pitch-types">Quản lý loại sân</Link>
                  <Link to="/manage-bookings">Quản lý đặt sân</Link>
                  <Link to="/revenue-report">Báo cáo doanh thu</Link>
                  <Link to="/create-staff">Đăng ký tài khoản nhân viên</Link>
                </>
              )}

              {/* Staff Menu */}
              {role === "Staff" && (
                <>
                  <Link to="/manage-pitches">Quản lý sân</Link>
                  <Link to="/manage-bookings">Quản lý đặt sân</Link>
                  <Link to="/manage-customers">Quản lý khách hàng</Link>
                </>
              )}

              {/* Customer Menu */}
              {role === "Customer" && (
                <>
                  <Link to="/my-bookings">Lịch sử đặt sân</Link>
                  <Link to="/profile">Hồ sơ</Link>
                </>
              )}

              <button onClick={handleLogout} className="logout-btn">
                Đăng Xuất
              </button>
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
