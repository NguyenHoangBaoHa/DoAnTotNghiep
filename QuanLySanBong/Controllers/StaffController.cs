using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanBong.Entities.Staff.Dto;
using QuanLySanBong.Service.Staff;

namespace QuanLySanBong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _service;

        public StaffController(IStaffService service)
        {
            _service = service;
        }

        // Lấy thông tin cá nhân
        [Authorize(Roles = "Staff")]
        [HttpGet("me")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var accountId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var staffInfo = await _service.GetPersonalInfo(accountId);

            if (staffInfo == null)
                return NotFound("Thông tin nhân viên không tồn tại.");

            return Ok(staffInfo);
        }

        // Cập nhật thông tin cá nhân
        [Authorize(Roles = "Staff")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] StaffDto staffDto)
        {
            var accountId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var result = await _service.UpdatePersonalInfo(accountId, staffDto);

            if (!result)
                return BadRequest("Cập nhật thông tin không thành công.");

            return Ok("Cập nhật thông tin thành công.");
        }
    }
}
