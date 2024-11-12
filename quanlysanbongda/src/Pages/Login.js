import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AccountAPI } from '../Api/api'; // Đảm bảo rằng đường dẫn là chính xác
import '../Css/Login.css';

const Login = () => {
  const [email, setEmail] = useState('admin'); // Đặt mặc định cho tài khoản Admin
  const [password, setPassword] = useState('Admin@123'); // Mật khẩu của Admin
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await AccountAPI.login(email, password);
      localStorage.setItem('username', response.username);
      localStorage.setItem('token', response.token); // Lưu token vào localStorage
      navigate('/create-staff'); // Chuyển hướng đến trang chính
      // Không cần window.location.reload() ở đây, navbar sẽ tự động cập nhật
    } catch (err) {
      console.error(err);
      setError('Đăng nhập thất bại');
    }
  };

  return (
    <div>
      <h1>Đăng Nhập</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div>
          <label>Mật khẩu:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button type="submit">Đăng Nhập</button>
        {error && <p>{error}</p>}
      </form>
    </div>
  );
};

export default Login;
