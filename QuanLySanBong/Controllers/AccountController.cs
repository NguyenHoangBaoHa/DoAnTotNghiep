using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanBong.Entities.Account.Dto;
using QuanLySanBong.Entities.Account.Model;
using QuanLySanBong.Entities.Staff.Dto;
using QuanLySanBong.Service.Account;
using System.Security.Claims;

namespace QuanLySanBong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        // Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var loginModel = await _service.Login(loginDto);

            if (string.IsNullOrEmpty(loginModel.Token))
                return Unauthorized("Sai email hoặc mật khẩu.");

            return Ok(loginModel);
        }

        // Tạo tài khoản Staff (chỉ dành cho Admin)
        [HttpPost("create-staff")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto createStaffDto)
        {
            if (createStaffDto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Lấy claims của user đang đăng nhập
            var userClaims = HttpContext.User.Claims;
            Console.WriteLine($"User Claims: {string.Join(", ", userClaims.Select(c => c.Type + ": " + c.Value))}");

            // Kiểm tra xem người dùng có quyền admin không
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim != "Admin")
            {
                return Unauthorized("You do not have permission to perform this action.");
            }

            try
            {
                // Gọi dịch vụ để tạo tài khoản nhân viên
                var account = await _service.CreateStaff(createStaffDto);

                // Trả về kết quả thành công
                return Ok(account);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có và trả về thông báo lỗi chi tiết
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
