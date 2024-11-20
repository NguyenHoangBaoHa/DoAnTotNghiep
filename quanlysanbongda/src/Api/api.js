import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:8001/api', // Ensure the correct backend URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor to automatically add the token to headers if available
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// API Functions
const AccountAPI = {
  // Login function
  login: async (email, password) => {
    try {
      const response = await api.post('/Account/login', { email, password });
      console.log(response.data);
      return response.data;
    } catch (error) {
      console.error('Login failed:', error);
      throw error.response ? error.response.data : new Error('Login failed');
    }
  },

  // API function to create a staff account
  createStaff: async (staffData) => {
    try {
      console.log("Sending staff data:", staffData); // Log staff data before sending
      const token = localStorage.getItem('token');
      const response = await api.post('/Account/create-staff', staffData, {
        headers: {
          'Authorization': `Bearer ${token}`
        },
        withCredentials: true
      });
      console.log("Staff created successfully:", response.data); // Log success response
      return response.data; // Return success data
    } catch (error) {
      if (error.response) {
        console.log('Error response:', error.response.data); // Log chi tiết lỗi từ backend
        throw error.response.data; // Throw error data từ backend
      } else {
        console.error('Request failed:', error.message);
        throw new Error('Cannot connect to server');
      }
    }
  },

  registerCustomer: async (customerData) => {
    try {
      console.log("Sending customer data: ", customerData);

      const token = localStorage.getItem('token');
      const response = await api.post('/Account/register-customer', customerData, {
        headers: {
          'Authorization': `Bearer ${token}`
        },
        withCredentials: true
      });
      console.log('User registered successfully: ', response.data);
      return response.data;
    } catch (error) {
      if (error.response) {
        console.log('Register failed: ', error.response.data); // Log chi tiết lỗi từ backend
        throw error.response.data; // Throw error data từ backend
      } else {
        console.error('Request failed:', error.message);
        throw new Error('Cannot connect to server');
      }
    }
  },
};

const PitchTypeAPI = {
  getAll: async () => {
    try {
      const response = await api.get('/PitchTypes');
      return response.data;
    }
    catch (error) {
      console.error('Failed to fetch pitch types:', error);
      throw error.response ? error.response.data : new Error('Failed to fetch pitch types');
    }
  },

  createPitchType: async (pitchTypeData) => {
    try {
      const response = await api.post('/PitchTypes', pitchTypeData);
      return response.data;
    }
    catch (error) {
      console.error('Failed to create pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to create pitch type');
    }
  },

  updatePitchType: async (id, pitchTypeData) => {
    try {
      const response = await api.put(`/PitchTypes/${id}`, pitchTypeData);
      return response.data;
    }
    catch (error) {
      console.error('Failed to update pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to update pitch type');
    }
  },

  deletePitchType: async (id) => {
    try {
      const response = await api.delete(`/PitchTypes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to delete pitch type:', error);
      throw error.response ? error.response.data : new Error('Failed to delete pitch type');
    }
  }
}

export { AccountAPI, PitchTypeAPI };
