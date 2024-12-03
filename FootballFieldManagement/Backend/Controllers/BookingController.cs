using Backend.Entities.Booking.Dto;
using Backend.Service.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            var role = User.FindFirst("role")?.Value;

            if(string.IsNullOrWhiteSpace(role))
            {
                return Unauthorized();
            }

            var bookings = await _service.GetBookingsByRoleAsync(role);
            return Ok(bookings);
        }

        [HttpPut("{id}/Check-in")]
        [Authorize(Policy ="StaffOnly")]
        public async Task<IActionResult> UpdateBookingCheckInStatus(int id, [FromBody] bool checkIn)
        {
            try
            {
                await _service.UpdateBookingCheckInStatusAsync(id, checkIn);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
