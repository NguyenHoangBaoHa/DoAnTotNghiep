using AutoMapper;
using Backend.Entities.Pitch.Dto;
using Backend.Entities.Pitch.Model;
using Backend.UnitOfWork;

namespace Backend.Service.Pitch
{
    public class PitchService : IPitchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PitchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PitchDto>> GetAllPitches()
        {
            var pitches = await _unitOfWork.Pitches.GetAllAsync();
            return _mapper.Map<IEnumerable<PitchDto>>(pitches);
        }

        public async Task<PitchDto> GetPitchById(int id)
        {
            var pitch = await _unitOfWork.Pitches.GetByIdAsync(id);
            if (pitch == null)
                throw new KeyNotFoundException("Pitch not found");

            return _mapper.Map<PitchDto>(pitch);
        }

        public async Task<PitchDto> CreatePitch(PitchDto pitchDto)
        {
            if(!await CheckPitchTypeExists(pitchDto.IdPitchType))
            {
                throw new ArgumentException("Invalid PitchType ID");
            }

            var pitchModel = _mapper.Map<PitchModel>(pitchDto);
            await _unitOfWork.Pitches.AddAsync(pitchModel);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PitchDto>(pitchModel);
        }

        public async Task<PitchDto> UpdatePitch(int id, PitchDto pitchDto)
        {
            var existingPitch = await _unitOfWork.Pitches.GetByIdAsync(id);
            if (existingPitch == null)
            {
                throw new KeyNotFoundException("Pitch not found");
            }

            if (!await CheckPitchTypeExists(pitchDto.IdPitchType))
            {
                throw new ArgumentException("Invalid PitchType ID");
            }

            _mapper.Map(pitchDto, existingPitch);
            await _unitOfWork.Pitches.UpdateAsync(existingPitch);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PitchDto>(existingPitch);
        }

        public async Task DeletePitch(int id)
        {
            var pitch = await _unitOfWork.Pitches.GetByIdAsync(id);
            if (pitch == null)
                throw new KeyNotFoundException("Pitch not found");

            await _unitOfWork.Pitches.DeleteAsync(pitch);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> CheckPitchTypeExists(int? IdPitchType)
        {
            if(!IdPitchType.HasValue) return false; // Trả về false nếu IdPitchType là null

            return await _unitOfWork.PitchesType.GetPitchTypeByIdAsync(IdPitchType.Value) != null;
        }

        public async Task<IEnumerable<PitchDto>> GetPagedPitchesAsync(int pageNumber, int pageSize)
        {
            var pagedPitches = await _unitOfWork.Pitches.GetPagedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<PitchDto>>(pagedPitches);
        }
    }
}
