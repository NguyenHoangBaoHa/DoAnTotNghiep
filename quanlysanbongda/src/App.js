import './App.css';
import Navbar from './Components/Navbar';
import Login from './Pages/Login';
import CreateStaff from './Pages/Admin/CreateStaff';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; // Sửa chỗ này

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/dashboard" element={<h1>Trang Chủ</h1>} />
        <Route path="/" element={<Login />} />
        <Route path="/register" element={<h1>Đăng Ký</h1>} />
        <Route path="/manage-pitches" element={<h1>Quản lý sân</h1>} />
        <Route path="/manage-pitch-types" element={<h1>Quản lý loại sân</h1>} />
        <Route path="/manage-bookings" element={<h1>Quản lý đặt sân</h1>} />
        <Route path="/create-staff" element={<CreateStaff />} />
      </Routes>
    </Router>
  );
}

export default App;
