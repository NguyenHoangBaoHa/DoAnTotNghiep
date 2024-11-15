import React, { useState, useEffect } from 'react';
import { AccountAPI } from '../../Api/api';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import '../../Css/CreateStaff.css';

const CreateStaff = () => {
  const [staffData, setStaffData] = useState({
    displayName: '',
    dateOfBirth: null,
    cccd: '',
    gender: '',
    phoneNumber: '',
    address: '',
    startDate: null,
    email: '',
    password: '',
  });
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const username = localStorage.getItem('username');
    if (username) {
      setStaffData((prevData) => ({ ...prevData, username }));
    }
  }, []);

  const handleChange = (e) => {
    setStaffData({ ...staffData, [e.target.name]: e.target.value });
  };

  const handleDateChange = (date, fieldName) => {
    setStaffData((prevData) => ({ ...prevData, [fieldName]: date }));
  };

  const formatDate = (date) => {
    if (!date) return null;
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${year}-${month}-${day}`; // Chuyển đổi thành "yyyy-MM-dd"
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Định dạng lại ngày tháng trước khi gửi
    const formattedStaffData = {
      ...staffData,
      dateOfBirth: formatDate(staffData.dateOfBirth),
      startDate: formatDate(staffData.startDate),
    };

    console.log('Submitting staff data:', formattedStaffData);

    try {
      const response = await AccountAPI.createStaff(formattedStaffData);
      setSuccess(true);
      setError(null);
      console.log('Staff created successfully:', response);
    } catch (err) {
      const errorMessage = err.response?.data?.message || 'An unexpected error occurred';
      setError(errorMessage);
      console.error('Error:', errorMessage);
      setSuccess(false);
    }
  };

  return (
    <div className="create-staff-container">
      <h2>Create Staff Account</h2>
      {success && <p className="success-msg">Staff account created successfully!</p>}
      {error && <p className="error-msg">{error}</p>}
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="displayName"
          placeholder="Full Name"
          value={staffData.displayName}
          onChange={handleChange}
          required
        />
        <DatePicker
          selected={staffData.dateOfBirth}
          onChange={(date) => handleDateChange(date, 'dateOfBirth')}
          placeholderText="Date of Birth"
          dateFormat="dd/MM/yyyy"
          showYearDropdown
          yearDropdownItemNumber={100} // Hiển thị 100 năm trước để chọn
          scrollableYearDropdown
        />
        <input
          type="text"
          name="cccd"
          placeholder="CCCD"
          value={staffData.cccd}
          onChange={handleChange}
          required
        />
        <select name="gender" value={staffData.gender} onChange={handleChange} required>
          <option value="">Chọn Giới Tính</option>
          <option value="Nam">Nam</option>
          <option value="Nữ">Nữ</option>
        </select>
        <input
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          value={staffData.phoneNumber}
          onChange={handleChange}
          required
        />
        <input
          type="text"
          name="address"
          placeholder="Address"
          value={staffData.address}
          onChange={handleChange}
          required
        />
        <DatePicker
          selected={staffData.startDate}
          onChange={(date) => handleDateChange(date, 'startDate')}
          placeholderText="Start Date"
          dateFormat="dd/MM/yyyy"
          // showYearDropdown
          // scrollableYearDropdown
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={staffData.email}
          onChange={handleChange}
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={staffData.password}
          onChange={handleChange}
          required
        />
        <button type="submit">Create Staff Account</button>
      </form>
    </div>
  );
};

export default CreateStaff;
