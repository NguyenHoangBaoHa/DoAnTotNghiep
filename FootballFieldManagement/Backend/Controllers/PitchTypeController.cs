using Backend.Entities.PitchType.Dto;
using Backend.Service.PitchType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class PitchTypeController : ControllerBase
    {
        private readonly IPitchTypeService _service;

        public PitchTypeController(IPitchTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pitchTypes = await _service.GetAllPitchTypesAsync();
            return Ok(pitchTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pitchType = await _service.GetPitchTypeByIdAsync(id);
                return Ok(pitchType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPitchType([FromBody] PitchTypeDto pitchTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddAsync(pitchTypeDto);
            return CreatedAtAction(nameof(GetById), new { id = pitchTypeDto.Id }, pitchTypeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePitchType(int id, [FromBody] PitchTypeDto pitchTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.UpdateAsync(id, pitchTypeDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePitchType(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
