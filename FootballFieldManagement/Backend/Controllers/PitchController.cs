using Backend.Entities.Pitch.Dto;
using Backend.Service.Pitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        private readonly IPitchService _service;

        public PitchController(IPitchService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var pitches = await _service.GetAllPitches();
            return Ok(pitches);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            var pitch = await _service.GetPitchById(id);
            if (pitch == null)
            {
                return NotFound(new { message = "Pitch not found" });
            }
            return Ok(pitch);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddPitch(PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the associated PitchType exists
            var isPitchTypeValid = await _service.CheckPitchTypeExists(pitchDto.IdPitchType);
            if (!isPitchTypeValid)
            {
                return BadRequest(new { message = "Invalid IdPitchType" });
            }

            var createdPitch = await _service.CreatePitch(pitchDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPitch.Id }, createdPitch);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdatePitch(int id, PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the associated PitchType exists
            var isPitchTypeValid = await _service.CheckPitchTypeExists(pitchDto.IdPitchType);
            if (!isPitchTypeValid)
            {
                return BadRequest(new { message = "Invalid IdPitchType" });
            }

            var updatedPitch = await _service.UpdatePitch(id, pitchDto);
            return Ok(updatedPitch);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePitch(int id)
        {
            await _service.DeletePitch(id);
            return NoContent();
        }
    }
}
