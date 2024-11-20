import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AccountAPI } from '../Api/api'; // Đảm bảo rằng đường dẫn là chính xác
import '../Css/Login.css';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // Gọi API để kiểm tra tài khoản và mật khẩu
      const response = await AccountAPI.login(email, password);

      // Lưu thông tin vào localStorage
      localStorage.setItem('username', response.username);
      localStorage.setItem('token', response.token);
      localStorage.setItem('role', response.role);

      // Điều hướng theo role
      switch (response.role) {
        case 'Admin':
          navigate('/manage-pitches-admin');
          break;
        case 'Staff':
          navigate('/manage-pitches-staff');
          break;
        case 'Customer':
          navigate('/my-bookings');
          break;
        default:
          navigate('/');
      }
    } catch (err) {
      console.error(err);
      setError('Đăng nhập thất bại: Vui lòng kiểm tra email hoặc mật khẩu.');
    }
  };

  return (
    <div className="login-container">
      <h1>Đăng Nhập</h1>
      <form onSubmit={handleSubmit} className="login-form">
        <div className="form-group">
          <label>Email:</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Nhập email của bạn"
          />
        </div>
        <div className="form-group">
          <label>Mật khẩu:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Nhập mật khẩu của bạn"
          />
        </div>
        <button type="submit" className="login-button">
          Đăng Nhập
        </button>
        {error && <p className="error-message">{error}</p>}
      </form>
    </div>
  );
};

export default Login;
