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
            try
            {
                // Log kiểm tra xem phương thức có được gọi không
                Console.WriteLine("AddAsync method is called.");

                // Kiểm tra loại sân có trùng tên hay không
                var existingPitchType = await _unitOfWork.PitchesType.GetPitchTypeByNameAsync(pitchTypeDto.Name);
                if (existingPitchType != null)
                {
                    throw new InvalidOperationException("PitchType already exists");
                }

                // Chuyển đổi DTO thành model để lưu vào cơ sở dữ liệu
                var pitchType = _mapper.Map<PitchTypeModel>(pitchTypeDto);

                // Thêm loại sân mới vào cơ sở dữ liệu
                Console.WriteLine("Attempting to add pitch type.");
                await _unitOfWork.PitchesType.AddAsync(pitchType);
                await _unitOfWork.CompleteAsync(); // Lưu thay đổi vào database

                Console.WriteLine("Pitch type added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                // Xử lý lỗi khi thêm loại sân
                throw new Exception("An error occurred while adding the pitch type.", ex);
            }
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
