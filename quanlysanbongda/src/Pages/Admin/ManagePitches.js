import React, { useEffect, useState } from 'react';
import { PitchAPI, PitchTypesAPI } from '../../Api/api';
import Modal from 'react-modal';
import '../../Css/Admin/ManagePitches.css';

const ManagePitches = () => {
  const [pitches, setPitches] = useState([]);
  const [pitchTypes, setPitchTypes] = useState([]);
  const [form, setForm] = useState({
    name: '',
    idPitchType: 1,
    status: 'Trống',
  });
  const [isEdit, setIsEdit] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Fetch pitch types and pitches when component mounts
    const fetchData = async () => {
      try {
        const pitchTypesData = await PitchTypesAPI.getAll();
        setPitchTypes(pitchTypesData);

        const pitchesData = await PitchAPI.getAll();
        setPitches(pitchesData);
      } catch (error) {
        console.error('Error fetching pitch types or pitches', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  // Find pitch type name by ID
  const getPitchTypeNameById = (id) => {
    const pitchType = pitchTypes.find((type) => type.id === id);
    return pitchType ? pitchType.name : 'Chưa xác định';
  };

  const openModal = () => {
    setForm({
      name: '',
      idPitchType: 1,
      status: 'Trống',
    });
    setIsEdit(false);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEdit) {
        await PitchAPI.updatePitch(form.id, form);
      } else {
        await PitchAPI.createPitch(form);
      }
      setIsModalOpen(false);
      setForm({ name: '', idPitchType: 1, status: 'Trống' });
      // Fetch updated data
      const updatedPitches = await PitchAPI.getAll();
      setPitches(updatedPitches);
    } catch (error) {
      console.error('Error saving pitch:', error);
    }
  };

  const handleEdit = (pitch) => {
    setForm(pitch);
    setIsEdit(true);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    try {
      await PitchAPI.deletePitch(id);
      const updatedPitches = await PitchAPI.getAll();
      setPitches(updatedPitches);
    } catch (error) {
      console.error('Error deleting pitch:', error);
    }
  };

  return (
    <div className="manage-pitches">
      <h1>Quản Lý Sân</h1>
      <button className="add-btn" onClick={openModal}>
        Thêm mới
      </button>
      {loading ? (
        <div>Đang tải...</div>
      ) : (
        <table className="pitch-table">
          <thead>
            <tr>
              <th>STT</th>
              <th>Tên sân</th>
              <th>Loại sân</th>
              <th>Trạng thái</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            {pitches.map((pitch, index) => (
              <tr key={pitch.id}>
                <td>{index + 1}</td>
                <td>{pitch.name}</td>
                <td>{getPitchTypeNameById(pitch.idPitchType) || "Không xác định"}</td>
                <td>{pitch.status}</td>
                <td>
                  <button onClick={() => handleEdit(pitch)}>Sửa</button>
                  <button onClick={() => handleDelete(pitch.id)}>Xóa</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {/* Modal */}
      <Modal isOpen={isModalOpen} onRequestClose={closeModal}>
        <h2>{isEdit ? "Chỉnh sửa sân" : "Thêm sân mới"}</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            value={form.name}
            onChange={(e) => setForm({ ...form, name: e.target.value })}
            placeholder="Tên sân"
            required
          />
          <select
            name="idPitchType"
            value={form.idPitchType}
            onChange={(e) => setForm({ ...form, idPitchType: e.target.value })}
            required
          >
            {pitchTypes.map((type) => (
              <option key={type.id} value={type.id}>
                {type.name}
              </option>
            ))}
          </select>
          <select
            name="status"
            value={form.status}
            onChange={(e) => setForm({ ...form, status: e.target.value })}
            required
          >
            <option value="Trống">Trống</option>
            <option value="Đã đặt">Đã đặt</option>
          </select>
          <button type="submit">{isEdit ? "Cập nhật" : "Thêm mới"}</button>
        </form>
        <button onClick={closeModal}>Đóng</button>
      </Modal>
    </div>
  );  
};

export default ManagePitches;
