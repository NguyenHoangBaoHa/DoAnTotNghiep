import './App.css';
import Navbar from './Components/Navbar';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom'; // Sửa chỗ này
import Home from "../src/Pages/Home";
import Login from "../src/Pages/Login";
import Register from "../src/Pages/Register";
import ManagerPitches from "../src/Pages/Admin/ManagePitches";
import ManagerPitchTypes from "../src/Pages/Admin/ManagePitchTypes";
import ManageBookingsAdmin from "../src/Pages/Admin/ManageBookingsAdmin";
import RevenueReport from './Pages/Admin/RevenueReport';
import CreateStaff from './Pages/Admin/CreateStaff';

const ProtectedRoute = ({ role, children }) => {
  const token = localStorage.getItem("token");
  const userRole = localStorage.getItem("role");

  if (!token) return <Navigate to="/login" />

  if (role & userRole !== role) return <Navigate to="/" />
  return children;
}

const App = () => {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        {/* Admin Routes */}
        <Route
          path="/manage-pitches"
          element={
            <ProtectedRoute role="Admin">
              <ManagerPitches />
            </ProtectedRoute>
          }
        />
        <Route
          path='/manage-pitch-types'
          element={
            <ProtectedRoute>
              <ManagerPitchTypes />
            </ProtectedRoute>
          }
        />
        <Route
          path='/manage-bookings'
          element={
            <ProtectedRoute>
              <ManageBookingsAdmin />
            </ProtectedRoute>
          }
        />
        <Route
          path='/revenue-report'
          element={
            <ProtectedRoute>
              <RevenueReport />
            </ProtectedRoute>
          }
        />
        <Route
          path='/create-staff'
          element={
            <ProtectedRoute>
              <CreateStaff />
            </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
}

export default App;
