using AutoMapper;
using Backend.Entities.PitchType.Dto;
using Backend.Entities.PitchType.Model;
using Backend.UnitOfWork;

namespace Backend.Service.PitchType
{
    public class PitchTypesService : IPitchTypesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PitchTypesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PitchTypeDto>> GetAllPitchTypesAsync()
        {
            var pitchType = await _unitOfWork.PitchesType.GetAllPitchTypesAsync();
            return _mapper.Map<IEnumerable<PitchTypeDto>>(pitchType);
        }

        public async Task<PitchTypeDto> GetPitchTypeByIdAsync(int id)
        {
            var pitchType = await _unitOfWork.PitchesType.GetPitchTypeByIdAsync(id);
            if(pitchType == null)
            {
                throw new KeyNotFoundException("PitchType not found");
            }
            return _mapper.Map<PitchTypeDto>(pitchType);
        }

        public async Task<int> AddAsync(PitchTypeDto pitchTypeDto)
        {
            if (string.IsNullOrWhiteSpace(pitchTypeDto.Name))
            {
                throw new ArgumentException("PitchType name cannot be empty");
            }

            // Chuẩn hóa chuỗi trước khi so sánh (ví dụ: chuyển về chữ thường)
            var normalizedName = pitchTypeDto.Name.Trim().ToLower();

            // Kiểm tra trùng lặp
            var existingPitchType = await _unitOfWork.PitchesType.GetPitchTypeByNameAsync(normalizedName);
            if (existingPitchType != null)
            {
                throw new InvalidOperationException("PitchType already exists");
            }

            var pitchType = _mapper.Map<PitchTypeModel>(pitchTypeDto);
            var newId = await _unitOfWork.PitchesType.AddAsync(pitchType); // Chuyển đổi để nhận Id từ Repository
            return newId;  // Trả về Id của loại sân mới được thêm
        }


        public async Task UpdateAsync(int id, PitchTypeDto pitchTypeDto)
        {
            var pitchType = await _unitOfWork.PitchesType.GetPitchTypeByIdAsync(id);
            if(pitchType == null)
            {
                throw new KeyNotFoundException("PitchType not found");
            }

            _mapper.Map(pitchTypeDto, pitchType);
            await _unitOfWork.PitchesType.UpdateAsync(pitchType);
        }

        public async Task DeleteAsync(int id)
        {
            var pitchType = await _unitOfWork.PitchesType.GetPitchTypeByIdAsync(id);
            if(pitchType == null)
            {
                throw new KeyNotFoundException("PitchType not found");
            }

            await _unitOfWork.PitchesType.DeleteAsync(id);
        }
    }
}
