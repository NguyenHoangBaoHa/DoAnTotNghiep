using Backend.Entities.Booking.Dto;

namespace Backend.Service.Booking
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetBookingsByRoleAsync(string role);
        Task UpdateBookingCheckInStatusAsync(int IdBooking, bool checkIn);
    }
}
