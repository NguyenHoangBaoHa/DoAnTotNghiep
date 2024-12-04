import axios from 'axios';

const api = axios.create({
  baseURL: 'https://192.168.0.2:8001/api', // Đảm bảo URL đúng
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

// Hàm kiểm tra quyền người dùng trong API
const checkAdminPermission = () => {
  if (!isAdmin()) {
    throw new Error('Bạn không có quyền thực hiện thao tác này.');
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

      console.log("Username: ", username + "\nToken: ", token + "\nRole: ", role);

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
      checkAdminPermission(); // Kiểm tra quyền Admin

      localStorage.getItem('token');
      localStorage.getItem('role');

      const response = await api.get('/PitchTypes/GetAll');
      if (!response || !response.data) {
        throw new Error("Không có dữ liệu trong phản hồi.");
      }
      return response.data;
    } catch (error) {
      console.error("Lỗi khi tải danh sách loại sân:", error);
      throw new Error(error.response?.data || error.message || "Lỗi khi tải danh sách loại sân.");
    }
  },

  createPitchType: async (pitchTypeData) => {
    try {
      checkAdminPermission(); // Kiểm tra quyền Admin

      const response = await api.post('/PitchTypes/Add', pitchTypeData);
      console.log("Response: ", response);
      if (response.status !== 200 && response.status !== 201) {
        throw new Error('Không thể tạo loại sân.');
      }
      return response.data; // Đảm bảo trả về dữ liệu từ API
    } catch (error) {
      console.error('Lỗi khi tạo loại sân:', error);
      throw new Error(error.response?.data || error.message || 'Không thể tạo loại sân');
    }
  },

  updatePitchType: async (id, pitchTypeData) => {
    try {
      checkAdminPermission(); // Kiểm tra quyền Admin

      const response = await api.put(`/PitchTypes/Update/${id}`, pitchTypeData);
      if (response.status !== 200) {
        throw new Error('Không thể cập nhật loại sân.');
      }
      return response.data; // Đảm bảo trả về dữ liệu từ API
    } catch (error) {
      console.error('Lỗi khi cập nhật loại sân:', error);
      throw new Error(error.response?.data || error.message || 'Không thể cập nhật loại sân');
    }
  },

  deletePitchType: async (id) => {
    try {
      checkAdminPermission(); // Kiểm tra quyền Admin

      const response = await api.delete(`/PitchTypes/Delete/${id}`);
      return response.data;
    } catch (error) {
      console.error('Lỗi khi xóa loại sân:', error);
      throw new Error(error.response?.data || error.message || 'Không thể xóa loại sân');
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
