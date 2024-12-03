using Backend.Data;
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

        public async Task<IEnumerable<BookingModel>> GetAllBookingsAssync()
        {
            return await _context.Bookings
                 .Include(b => b.Customer)
                 .Include(b => b.PitchType)
                 .ToListAsync();
        }

        public async Task<BookingModel> GetByIdAssync(int Idbooking)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.PitchType)
                .FirstOrDefaultAsync(b => b.Id == Idbooking);
        }

        public async Task UpdateAssync(BookingModel booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
