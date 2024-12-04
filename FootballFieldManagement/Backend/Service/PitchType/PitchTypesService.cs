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

        public async Task AddAsync(PitchTypeDto pitchTypeDto)
        {
            // Kiểm tra loại sân có trùng tên hay không
            var existingPitchType = await _unitOfWork.PitchesType.GetPitchTypeByNameAsync(pitchTypeDto.Name);
            if (existingPitchType != null)
            {
                throw new InvalidOperationException("PitchType already exists");
            }

            // Thêm loại sân mới
            var pitchType = _mapper.Map<PitchTypeModel>(pitchTypeDto);
            await _unitOfWork.PitchesType.AddAsync(pitchType);
            await _unitOfWork.CompleteAsync(); // Lưu thay đổi vào database
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
