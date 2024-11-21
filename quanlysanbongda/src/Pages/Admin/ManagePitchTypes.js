import React, { useEffect, useState } from 'react';
import Modal from 'react-modal';
import '../../Css/Admin/ManagePitchType.css';
import { PitchTypeAPI } from '../../Api/api';

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
      const data = await PitchTypeAPI.getAll();
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
        await PitchTypeAPI.updatePitchType(form.id, form);
      } else {
        await PitchTypeAPI.createPitchType(form);
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
        await PitchTypeAPI.deletePitchType(id);
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
              <td>{Number(pitchType.price).toLocaleString('vi-VN')}đ</td>
              <td>{pitchType.limitPerson}</td>
              <td>
                <button className="edit-btn" onClick={() => handleEdit(pitchType)}>
                  Sửa
                </button>
                <button className="delete-btn" onClick={() => handleDelete(pitchType.id)}>
                  Xóa
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal for Adding/Editing Pitch Type */}
      <Modal
        isOpen={isModalOpen}
        onRequestClose={closeModal}
        contentLabel="Thêm/Sửa Loại Sân"
        className="modal"
        overlayClassName="modal-overlay"
      >
        <h2>{isEdit ? 'Cập nhật loại sân' : 'Thêm loại sân mới'}</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            placeholder="Tên loại sân"
            value={form.name}
            onChange={handleChange}
            required
          />
          <input
            type="number"
            name="price"
            placeholder="Giá (đồng)"
            value={form.price}
            onChange={handleChange}
            required
          />
          <input
            type="number"
            name="limitPerson"
            placeholder="Số lượng người chơi"
            value={form.limitPerson}
            onChange={handleChange}
            required
          />
          <div className="modal-actions">
            <button type="submit">{isEdit ? 'Cập nhật' : 'Lưu'}</button>
            <button type="button" onClick={closeModal}>
              Hủy
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}

export default ManagerPitchTypes;