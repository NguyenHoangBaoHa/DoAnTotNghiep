﻿using Backend.Entities.PitchType.Dto;
using Backend.Service.PitchType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")] // Sử dụng Policy để rõ ràng
    public class PitchTypesController : ControllerBase
    {
        private readonly IPitchTypesService _service;

        public PitchTypesController(IPitchTypesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lấy danh sách tất cả các loại sân
        /// </summary>
        /// <returns>Danh sách loại sân</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pitchTypes = await _service.GetAllPitchTypesAsync();
                if (pitchTypes == null || !pitchTypes.Any())
                {
                    return NotFound(new { message = "No pitch types found." });
                }
                return Ok(pitchTypes);
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần thiết
                return StatusCode(500, new { message = "An error occurred while fetching pitch types.", details = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin loại sân theo ID
        /// </summary>
        /// <param name="id">ID của loại sân</param>
        /// <returns>Chi tiết loại sân</returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pitchType = await _service.GetPitchTypeByIdAsync(id);
                if (pitchType == null)
                {
                    return NotFound(new { message = $"Pitch type with ID {id} not found." });
                }
                return Ok(pitchType);
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần thiết
                return StatusCode(500, new { message = "An error occurred while fetching pitch type.", details = ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới một loại sân
        /// </summary>
        /// <param name="pitchTypeDto">Dữ liệu loại sân</param>
        /// <returns>Trạng thái thêm thành công</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddPitchType([FromBody] PitchTypeDto pitchTypeDto)
        {
            // Kiểm tra xem pitchTypeDto có null không
            if (pitchTypeDto == null)
            {
                return BadRequest(new { message = "PitchType data is null" });
            }

            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                // Gọi service để thêm loại sân
                await _service.AddAsync(pitchTypeDto);

                // Trả về 201 Created khi thêm thành công
                return CreatedAtAction(nameof(GetById), new { id = pitchTypeDto.Id }, pitchTypeDto);
            }
            catch (InvalidOperationException ex)
            {
                // Trường hợp loại sân đã tồn tại
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi thêm không thành công
                return StatusCode(500, new { message = "An error occurred while adding pitch type.", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin loại sân
        /// </summary>
        /// <param name="id">ID loại sân</param>
        /// <param name="pitchTypeDto">Dữ liệu mới</param>
        /// <returns>Trạng thái cập nhật</returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePitchType(int id, [FromBody] PitchTypeDto pitchTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state.", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                await _service.UpdateAsync(id, pitchTypeDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating pitch type.", details = ex.Message });
            }
        }

        /// <summary>
        /// Xóa loại sân theo ID
        /// </summary>
        /// <param name="id">ID loại sân</param>
        /// <returns>Trạng thái xóa</returns>
        [HttpDelete("Delete/{id}")]
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting pitch type.", details = ex.Message });
            }
        }
    }
}
