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
import ManagerPitchesStaff from '../src/Pages/Staff/ManagerPitchesStaff';
import ManageBookingsStaff from '../src/Pages/Staff/ManageBookingsStaff';
import BookingPitches from './Pages/Customer/BookingPitches';
import Profile from './Pages/Customer/Profile';

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
          path="/manage-pitches-admin"
          element={
            <ProtectedRoute role="Admin">
              <ManagerPitches />
            </ProtectedRoute>
          }
        />
        <Route
          path='/manage-pitch-types-admin'
          element={
            <ProtectedRoute>
              <ManagerPitchTypes />
            </ProtectedRoute>
          }
        />
        <Route
          path='/manage-bookings-admin'
          element={
            <ProtectedRoute>
              <ManageBookingsAdmin />
            </ProtectedRoute>
          }
        />
        <Route
          path='/revenue-report-admin'
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

        {/* Staff Routes */}
        <Route
          path="/manage-pitches-staff"
          element={
            <ProtectedRoute role="Staff">
              <ManagerPitchesStaff />
            </ProtectedRoute>
          }
        />
        <Route
          path='/manage-bookings-staff'
          element={
            <ProtectedRoute>
              <ManageBookingsStaff />
            </ProtectedRoute>
          }
        />

        {/* Customer Routes */}
        <Route
          path="/my-bookings"
          element={
            <ProtectedRoute role="Customer">
              <BookingPitches />
            </ProtectedRoute>
          }
        />
        <Route
          path='/profile'
          element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
}

export default App;
