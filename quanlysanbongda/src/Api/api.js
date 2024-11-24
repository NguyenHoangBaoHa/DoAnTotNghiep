import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:8001/api', // Đảm bảo URL đúng
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor để tự động thêm token vào header nếu có
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => Promise.reject(error));

// Kiểm tra role người dùng (Admin)
const isAdmin = () => {
  try {
    const token = localStorage.getItem('token');
    if (!token) return false;

    const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
    const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return role === 'Admin';
  } catch (error) {
    console.error('Error decoding token:', error);
    return false;
  }
};

// API Functions
const AccountAPI = {
  login: async (email, password) => {
    try {
      const response = await api.post('/Account/login', { email, password });
      const { token, role, username } = response.data;

      // Lưu thông tin vào localStorage
      localStorage.setItem('token', token);
      localStorage.setItem('role', role);
      localStorage.setItem('username', username);

      return response.data;
    } catch (error) {
      console.error('Login failed:', error);
      throw error.response?.data || new Error('Login failed');
    }
  },

  createStaff: async (staffData) => {
    try {
      const response = await api.post('/Account/create-staff', staffData);
      return response.data;
    } catch (error) {
      console.error('Failed to create staff:', error);
      throw error.response?.data || new Error('Failed to create staff');
    }
  },

  registerCustomer: async (customerData) => {
    try {
      const response = await api.post('/Account/register-customer', customerData);
      return response.data;
    } catch (error) {
      console.error('Failed to register customer:', error);
      throw error.response?.data || new Error('Failed to register customer');
    }
  },
};

const PitchTypesAPI = {
  getAll: async () => {
    try {
      const token = localStorage.getItem('token');
      const role = localStorage.getItem('role');

      if (!token) {
        throw new Error("Token is missing. Please log in again.");
      }

      if (role !== "Admin") {
        throw new Error("You do not have permission to view this data.");
      }

      const response = await api.get('/PitchTypes/GetAll', {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (!response || !response.data) {
        throw new Error("No data in response");
      }

      return response.data;
    } catch (error) {
      console.error("Failed to fetch pitch types:", error);
      throw new Error(error.response?.data || error.message || "Failed to fetch pitch types");
    }
  },

  createPitchType: async (pitchTypeData) => {
    try {
      if (!isAdmin()) throw new Error('You do not have permission to create a pitch type.');

      const response = await api.post('/PitchTypes/Add', pitchTypeData);
      return response.data;
    } catch (error) {
      console.error('Failed to create pitch type:', error);
      throw new Error(error.response?.data || error.message || 'Failed to create pitch type');
    }
  },

  updatePitchType: async (id, pitchTypeData) => {
    try {
      if (!isAdmin()) throw new Error('You do not have permission to update this pitch type.');

      const response = await api.put(`/PitchTypes/Update/${id}`, pitchTypeData);
      return response.data;
    } catch (error) {
      console.error('Failed to update pitch type:', error);
      throw new Error(error.response?.data || error.message || 'Failed to update pitch type');
    }
  },

  deletePitchType: async (id) => {
    try {
      if (!isAdmin()) throw new Error('You do not have permission to delete this pitch type.');

      const response = await api.delete(`/PitchTypes/Delete/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to delete pitch type:', error);
      throw new Error(error.response?.data || error.message || 'Failed to delete pitch type');
    }
  },
};

const PitchAPI = {
  getAll: async () => {
    try {
      const response = await api.get('/Pitches');
      return response.data;
    } catch (error) {
      console.error('Failed to fetch pitches:', error);
      throw error.response?.data || new Error('Failed to fetch pitches');
    }
  },

  createPitch: async (pitchData) => {
    try {
      const response = await api.post('/Pitches', pitchData);
      return response.data;
    } catch (error) {
      console.error('Failed to create pitch:', error);
      throw error.response?.data || new Error('Failed to create pitch');
    }
  },

  updatePitch: async (id, pitchData) => {
    try {
      const response = await api.put(`/Pitches/${id}`, pitchData);
      return response.data;
    } catch (error) {
      console.error('Failed to update pitch:', error);
      throw error.response?.data || new Error('Failed to update pitch');
    }
  },

  deletePitch: async (id) => {
    try {
      const response = await api.delete(`/Pitches/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to delete pitch:', error);
      throw error.response?.data || new Error('Failed to delete pitch');
    }
  },
};

export { AccountAPI, PitchTypesAPI, PitchAPI };
