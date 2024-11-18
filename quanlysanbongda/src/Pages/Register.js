import React, { useEffect, useState } from "react";
import { AccountAPI } from "../Api/api"; // Import API từ file api.js
import "../Css/Register.css"; // Import file CSS

const Register = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    displayName: "",
    dateOfBirth: "",
    cccd: "",
    gender: "",
    phoneNumber: "",
    address: "",
  });

  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const username = localStorage.getItem('username');
    if (username) {
      setFormData((prevData) => ({ ...prevData, username }));
    }
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const payload = {
        customer: {
          displayName: formData.displayName,
          dateOfBirth: formData.dateOfBirth,
          cccd: formData.cccd,
          gender: formData.gender,
          phoneNumber: formData.phoneNumber,
          address: formData.address,
        },
        account: {
          email: formData.email,
          password: formData.password,
        },
      };

      await AccountAPI.registerCustomer(payload); // Gọi API từ file api.js
      alert("Registration successful!");
    } catch (err) {
      setError(err.error || "Failed to register. Please try again.");
      console.error("Registration error:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <h2>Register as Customer</h2>
      <form onSubmit={handleSubmit} className="register-form">
        {error && <p className="error-message">{error}</p>}

        <div className="form-group">
          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Display Name:</label>
          <input
            type="text"
            name="displayName"
            value={formData.displayName}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Date of Birth:</label>
          <input
            type="date"
            name="dateOfBirth"
            value={formData.dateOfBirth}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Citizen ID (CCCD):</label>
          <input
            type="text"
            name="cccd"
            value={formData.cccd}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Gender:</label>
          <select
            name="gender"
            value={formData.gender}
            onChange={handleChange}
            required
          >
            <option value="">Select</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
        </div>

        <div className="form-group">
          <label>Phone Number:</label>
          <input
            type="tel"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label>Address:</label>
          <textarea
            name="address"
            value={formData.address}
            onChange={handleChange}
            rows="3"
            required
          ></textarea>
        </div>

        <button type="submit" disabled={loading} className="submit-btn">
          {loading ? "Registering..." : "Register"}
        </button>
      </form>
    </div>
  );
};

export default Register;
