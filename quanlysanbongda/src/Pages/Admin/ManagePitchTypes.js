import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import '../../Css/Admin/ManagePitchType.css';
import { PitchTypesAPI } from '../../Api/api';

Modal.setAppElement('#root');

const ManagerPitchTypes = () => {
  const [pitchTypes, setPitchTypes] = useState([]);
  const [form, setForm] = useState({ id: null, name: '', price: '', limitPerson: '' });
  const [isEdit, setIsEdit] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Fetch all pitch types on component mount
  useEffect(() => {
    fetchPitchTypes();
  }, []);

  const fetchPitchTypes = async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token is missing');
        return;
      }

      const data = await PitchTypesAPI.getAll();
      setPitchTypes(data);
    } catch (error) {
      console.error('Failed to fetch pitch types:', error);
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
    try {
      if (isEdit) {
        await PitchTypesAPI.updatePitchType(form.id, form);
      } else {
        await PitchTypesAPI.createPitchType(form);
      }
      fetchPitchTypes();
      closeModal(); // Close the modal after saving
    } catch (error) {
      console.error('Failed to save pitch type:', error);
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
      }
    }
  };

  return (
    <div className="pitch-type-management">
      <h1>Quản Lý Loại Sân</h1>
      <button className="add-btn" onClick={openModal}>
        Thêm mới
      </button>
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
          />
          <input
            type="number"
            name="price"
            value={form.price}
            onChange={handleChange}
            placeholder="Giá sân"
          />
          <input
            type="number"
            name="limitPerson"
            value={form.limitPerson}
            onChange={handleChange}
            placeholder="Số người tối đa"
          />
          <button type="submit">{isEdit ? 'Cập nhật' : 'Thêm mới'}</button>
        </form>
        <button onClick={closeModal}>Đóng</button>
      </Modal>
    </div>
  );
};

export default ManagerPitchTypes;
