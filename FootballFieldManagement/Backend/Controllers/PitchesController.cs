using Backend.Entities.Pitch.Dto;
using Backend.Service.Pitch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOrStaffPolicy")] // Sử dụng Policy để rõ ràng
    public class PitchesController : ControllerBase
    {
        private readonly IPitchService _service;

        public PitchesController(IPitchService service)
        {
            _service = service;
        }

        // Lấy danh sách tất cả sân
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pitches = await _service.GetAllPitches();
                if(pitches == null || !pitches.Any())
                {
                    return NotFound(new { message = "No pitches found." });
                }
                return Ok(pitches);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return StatusCode(500, new { message = "An error occurred while fetching pitches", error = ex.Message });
            }
        }

        // Lấy thông tin sân theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pitch = await _service.GetPitchById(id);
                if (pitch == null)
                {
                    return NotFound(new { message = "Pitch not found" });
                }
                return Ok(pitch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the pitch", error = ex.Message });
            }
        }

        // Thêm mới sân
        [HttpPost]
        public async Task<IActionResult> AddPitch([FromBody] PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var isPitchTypeValid = await _service.CheckPitchTypeExists(pitchDto.IdPitchType);
                if (!isPitchTypeValid)
                {
                    return BadRequest(new { message = "Invalid PitchType ID" });
                }

                var createdPitch = await _service.CreatePitch(pitchDto);
                return CreatedAtAction(nameof(GetById), new { id = createdPitch.Id }, createdPitch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the pitch", error = ex.Message });
            }
        }

        // Cập nhật thông tin sân
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePitch(int id, [FromBody] PitchDto pitchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var isPitchTypeValid = await _service.CheckPitchTypeExists(pitchDto.IdPitchType);
                if (!isPitchTypeValid)
                {
                    return BadRequest(new { message = "Invalid PitchType ID" });
                }

                var updatedPitch = await _service.UpdatePitch(id, pitchDto);
                return Ok(updatedPitch);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pitch not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the pitch", error = ex.Message });
            }
        }

        // Xóa sân
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePitch(int id)
        {
            try
            {
                await _service.DeletePitch(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pitch not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the pitch", error = ex.Message });
            }
        }
    }

}
