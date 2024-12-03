using Backend.Entities.Booking.Model;

namespace Backend.Repository.Booking
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingModel>> GetAllBookingsAssync();
        Task<BookingModel> GetByIdAssync(int Idbooking);
        Task UpdateAssync(BookingModel booking);
    }
}
