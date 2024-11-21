import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:8001/api', // Đảm bảo URL đúng
  headers: {
    'Content-Type': 'application/json',
  }
});

// Interceptor để tự động thêm token vào header nếu có
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  console.log('Interceptor Token:', token);  // Kiểm tra token trong interceptor
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// API Functions
const AccountAPI = {
  login: async (email, password) => {
    try {
      const response = await api.post('/Account/login', { email, password });
      
      // Lưu trữ token, role và username vào localStorage
      const { token, role, username } = response.data;
      localStorage.setItem('token', token);
      localStorage.setItem('role', role);
      localStorage.setItem('username', username);

      console.log('Login successful:', response.data); // Debug thông tin đăng nhập
      return response.data; // Trả về dữ liệu đăng nhập
    } catch (error) {
      console.error('Login failed:', error);
      throw error.response ? error.response.data : new Error('Login failed');
    }
  },

  createStaff: async (staffData) => {
    try {
      const token = localStorage.getItem('token');
      const response = await api.post('/Account/create-staff', staffData, {
        headers: { Authorization: `Bearer ${token}` },
        withCredentials: true
      });
      console.log('Staff created successfully:', response.data);
      return response.data;
    } catch (error) {
      if (error.response) {
        console.log('Error response:', error.response.data);
        throw error.response.data;
      } else {
        console.error('Request failed:', error.message);
        throw new Error('Cannot connect to server');
      }
    }
  },

  registerCustomer: async (customerData) => {
    try {
      const token = localStorage.getItem('token');
      const response = await api.post('/Account/register-customer', customerData, {
        headers: { Authorization: `Bearer ${token}` },
        withCredentials: true
      });
      console.log('User registered successfully:', response.data);
      return response.data;
    } catch (error) {
      if (error.response) {
        console.log('Register failed:', error.response.data);
        throw error.response.data;
      } else {
        console.error('Request failed:', error.message);
        throw new Error('Cannot connect to server');
      }
    }
  }
};

const isAdmin = () => {
  const token = localStorage.getItem('token');
  if (!token) {
    console.error('No token found');
    throw new Error('User is not logged in');
  }

  try {
    const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
    console.log('Decoded payload:', payload); // Log payload để kiểm tra
    // Sử dụng namespace để lấy role
    const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    console.log('User role:', role);
    return role === 'Admin';
  } catch (error) {
    console.error('Failed to decode token:', error);
    throw new Error('Invalid token format');
  }
};

const PitchTypesAPI = {
  getAll: async () => {
    try {
      const token = localStorage.getItem('token');
      const isAdminUser = isAdmin(); // Gọi isAdmin() để kiểm tra quyền
      console.log('Is Admin:', isAdminUser);

      const response = await api.get('/PitchTypes/GetAll', {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (!response || !response.data) {
        throw new Error('No data in response');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch pitch types:', error);
      throw error.response ? error.response.data : new Error('Failed to fetch pitch types');
    }
  },
  
  getById: async (id) => {
    try {
      if (!isAdmin()) {
        throw new Error('You do not have permission to view this data');
      }

      const token = localStorage.getItem('token');
      const response = await api.get(`/PitchTypes/GetById/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (!response || !response.data) {
        throw new Error('No data in response');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch pitch type by ID:', error);
      throw error.response ? error.response.data : new Error('Failed to fetch pitch type by ID');
    }
  },

  createPitchType: async (pitchTypeData) => {
    try {
      if (!isAdmin()) {
        throw new Error('You do not have permission to create a pitch type');
      }

      const token = localStorage.getItem('token');
      const response = await api.post('/PitchTypes/Add', pitchTypeData, {
        headers: { Authorization: `Bearer ${token}` }
      });
      return response.data;
    } catch (error) {
      console.error('Failed to create pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to create pitch type');
    }
  },

  updatePitchType: async (id, pitchTypeData) => {
    try {
      if (!isAdmin()) {
        throw new Error('You do not have permission to update this pitch type');
      }

      const token = localStorage.getItem('token');
      const response = await api.put(`/PitchTypes/Update/${id}`, pitchTypeData, {
        headers: { Authorization: `Bearer ${token}` }
      });
      return response.data;
    } catch (error) {
      console.error('Failed to update pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to update pitch type');
    }
  },

  deletePitchType: async (id) => {
    try {
      if (!isAdmin()) {
        throw new Error('You do not have permission to delete this pitch type');
      }

      const token = localStorage.getItem('token');
      const response = await api.delete(`/PitchTypes/Delete/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      return response.data;
    } catch (error) {
      console.error('Failed to delete pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to delete pitch type');
    }
  }
};



const PitchAPI = {
  getAll: async () => {
    try {
      const response = await api.get('/Pitches');
      return response.data;
    } catch (error) {
      console.error('Failed to fetch pitches:', error);
      throw error.response ? error.response.data : new Error('Failed to fetch pitches');
    }
  },

  createPitch: async (pitchData) => {
    try {
      const response = await api.post('/Pitches', pitchData);
      return response.data;
    } catch (error) {
      console.error('Failed to create pitch:', error);
      throw error.response ? error.response.data : new Error('Failed to create pitch');
    }
  },

  updatePitch: async (id, pitchData) => {
    try {
      const response = await api.put(`/Pitches/${id}`, pitchData);
      return response.data;
    } catch (error) {
      console.error('Failed to update pitch:', error);
      throw error.response ? error.response.data : new Error('Failed to update pitch');
    }
  },

  deletePitch: async (id) => {
    try {
      const response = await api.delete(`/Pitches/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to delete pitch:', error);
      throw error.response ? error.response.data : new Error('Failed to delete pitch');
    }
  },
}

export { AccountAPI, PitchTypesAPI, PitchAPI };
