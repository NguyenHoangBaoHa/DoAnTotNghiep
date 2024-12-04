import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import { useNavigate } from 'react-router-dom';
import '../../Css/Admin/ManagePitchType.css';
import { PitchTypesAPI } from '../../Api/api';

Modal.setAppElement('#root');

const ManagerPitchTypes = () => {
  const [pitchTypes, setPitchTypes] = useState([]);
  const [form, setForm] = useState({ id: null, name: '', price: '', limitPerson: '' });
  const [isEdit, setIsEdit] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false); // Trạng thái khi đang xử lý
  const navigate = useNavigate();
  const [role, setRole] = useState(null);

  // Kiểm tra quyền truy cập
  useEffect(() => {
    const token = localStorage.getItem('token');
    const storedRole = localStorage.getItem('role');
    setRole(storedRole);

    if (!token || storedRole !== 'Admin') {
      alert('Bạn không có quyền truy cập trang này.');
      navigate('/unauthorized'); // Điều hướng đến trang lỗi hoặc đăng nhập
    }
  }, [navigate]);

  // Fetch tất cả các loại sân
  useEffect(() => {
    if (role === 'Admin') {
      fetchPitchTypes();
    }
  }, [role]);

  const fetchPitchTypes = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await PitchTypesAPI.getAll();
      setPitchTypes(data);
    } catch (error) {
      setError('Không thể tải danh sách loại sân.');
    } finally {
      setLoading(false);
    }
  };

  const openModal = () => {
    setForm({ id: null, name: '', price: '', limitPerson: '' });
    setIsEdit(false);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Kiểm tra các trường hợp không hợp lệ
    if (!form.name || !form.price || !form.limitPerson) {
      alert("Tất cả các trường đều phải được điền đầy đủ!");
      return;
    }

    // Chuyển đổi price và limitPerson sang số
    const price = parseFloat(form.price);
    const limitPerson = parseInt(form.limitPerson, 10);

    // Kiểm tra xem giá trị có hợp lệ không
    if (isNaN(price) || isNaN(limitPerson)) {
      alert("Giá hoặc số người tối đa không hợp lệ.");
      return;
    }

    const formData = {
      ...form,
      price,  // Chuyển price thành số (decimal)
      limitPerson,  // Chuyển limitPerson thành số nguyên (int)
    };

    console.log("Form data:", formData); // Kiểm tra lại dữ liệu sau khi chuyển đổi

    try {
      if (isEdit) {
        await PitchTypesAPI.updatePitchType(formData.id, formData);
        alert("Cập nhật loại sân thành công!");
      } else {
        await PitchTypesAPI.createPitchType(formData);
        alert("Thêm loại sân mới thành công!");
      }
      fetchPitchTypes();
      closeModal();
    } catch (error) {
      console.error("Lỗi khi lưu loại sân:", error);
      if (error.response && error.response.data) {
        alert(error.response.data.message || "Đã xảy ra lỗi.");
      } else {
        alert("Không thể lưu loại sân. Vui lòng thử lại sau.");
      }
    }
    finally {
      setIsSubmitting(false);
    }
  };

  const handleEdit = (pitchType) => {
    setForm(pitchType);
    setIsEdit(true);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa loại sân này không?')) {
      try {
        await PitchTypesAPI.deletePitchType(id);
        fetchPitchTypes();
      } catch (error) {
        console.error('Failed to delete pitch type:', error);
        alert('Không thể xóa loại sân.');
      }
    }
  };

  if (role !== 'Admin') {
    return null; // Không render gì nếu không phải Admin
  }

  return (
    <div className="pitch-type-management">
      <h1>Quản Lý Loại Sân</h1>
      <button className="add-btn" onClick={openModal} disabled={isSubmitting}>
        {isSubmitting ? 'Đang xử lý...' : 'Thêm mới'}
      </button>

      {loading && <div>Đang tải...</div>}
      {error && <div className="error">{error}</div>}
      {pitchTypes.length === 0 && !loading && !error && (
        <div>Không có loại sân nào.</div>
      )}

      <table className="pitch-type-table">
        <thead>
          <tr>
            <th>STT</th>
            <th>Loại sân</th>
            <th>Giá (đồng)</th>
            <th>Số lượng người chơi</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {pitchTypes.map((pitchType, index) => (
            <tr key={pitchType.id}>
              <td>{index + 1}</td>
              <td>{pitchType.name}</td>
              <td>{pitchType.price}</td>
              <td>{pitchType.limitPerson}</td>
              <td>
                <button onClick={() => handleEdit(pitchType)}>Sửa</button>
                <button onClick={() => handleDelete(pitchType.id)}>Xóa</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal */}
      <Modal isOpen={isModalOpen} onRequestClose={closeModal}>
        <h2>{isEdit ? 'Chỉnh sửa loại sân' : 'Thêm loại sân mới'}</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            value={form.name}
            onChange={handleChange}
            placeholder="Tên loại sân"
            required
          />
          <input
            type="number"
            name="price"
            value={form.price}
            onChange={handleChange}
            placeholder="Giá sân"
            required
          />
          <input
            type="number"
            name="limitPerson"
            value={form.limitPerson}
            onChange={handleChange}
            placeholder="Số người tối đa"
            required
          />
          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Đang xử lý...' : isEdit ? 'Cập nhật' : 'Thêm mới'}
          </button>
        </form>
        <button onClick={closeModal}>Đóng</button>
      </Modal>
    </div>
  );
};

export default ManagerPitchTypes;
