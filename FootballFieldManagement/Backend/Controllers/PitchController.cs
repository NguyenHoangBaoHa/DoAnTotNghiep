using Backend.Entities.Pitch.Dto;
using Backend.Service.Pitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PitchController : Controller
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
            try
            {
                var pitch = await _service.GetPitchById(id);
                return Ok(pitch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPitch = await _service.CreatePitch(pitchDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPitch.Id }, createdPitch);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPitch = await _service.UpdatePitch(id, pitchDto);
                return Ok(updatedPitch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeletePitch(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
