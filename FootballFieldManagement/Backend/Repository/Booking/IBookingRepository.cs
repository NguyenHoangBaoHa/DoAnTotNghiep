using Backend.Entities.Booking.Model;

namespace Backend.Repository.Booking
{
    public interface IBookingRepository
    {
        Task<List<BookingModel>> GetAllBookingsAsync();
        Task<bool> UpdateCheckedInStatusAsync(int bookingId, bool hasCheckedIn);
    }
}
