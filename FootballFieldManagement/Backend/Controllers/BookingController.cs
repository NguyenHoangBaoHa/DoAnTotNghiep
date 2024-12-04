using Backend.Entities.Booking.Dto;
using Backend.Service.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBookings()
        {
            var role = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role)?.Value;
            if(string.IsNullOrEmpty(role) )
            {
                return Unauthorized();
            }

            var bookings = await _service.GetBookingsByRoleAsync(role);
            return Ok(bookings);
        }

        [HttpPut("update-checkedin/{id}")]
        [Authorize(Policy ="StaffOnlly")]
        public async Task<IActionResult> UpdateCheckedInStatus(int id, [FromBody] bool hasCheckedIn)
        {
            var result = await _service.UpdateCheckedInStatusAsync(id, hasCheckedIn);
            if(!result)
            {
                return NotFound(new { message = "Booking not found" });
            }

            return Ok(new { message = "Checked-in status updated successfully" });
        }
    }
}
