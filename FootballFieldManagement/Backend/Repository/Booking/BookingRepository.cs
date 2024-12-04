using Backend.Data;
using Backend.Entities.Booking.Dto;
using Backend.Entities.Booking.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingModel>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Pitch)
                .ThenInclude(p => p.PitchType)
                .ToListAsync();
        }

        public async Task<bool> UpdateCheckedInStatusAsync(int bookingId, bool hasCheckedIn)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            booking.HasCheckedIn = hasCheckedIn;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
