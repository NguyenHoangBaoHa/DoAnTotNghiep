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
            var pitchModel = _mapper.Map<PitchModel>(pitchDto);
            await _unitOfWork.Pitches.AddAsync(pitchModel);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PitchDto>(pitchModel);
        }

        public async Task<PitchDto> UpdatePitch(int id, PitchDto pitchDto)
        {
            var existingPitch = await _unitOfWork.Pitches.GetByIdAsync(id);
            if (existingPitch == null)
                throw new KeyNotFoundException("Pitch not found");

            _mapper.Map(pitchDto, existingPitch);
            _unitOfWork.Pitches.Update(existingPitch);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PitchDto>(existingPitch);
        }

        public async Task DeletePitch(int id)
        {
            var pitch = await _unitOfWork.Pitches.GetByIdAsync(id);
            if (pitch == null)
                throw new KeyNotFoundException("Pitch not found");

            _unitOfWork.Pitches.Delete(pitch);
            await _unitOfWork.CompleteAsync();
        }
    }
}
