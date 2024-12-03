using Backend.Entities.Booking.Dto;
using Backend.UnitOfWork;

namespace Backend.Service.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByRoleAsync(string role)
        {
            var bookings = await _unitOfWork.Bookings.GetAllBookingsAssync();
            var bookingDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                HasCheckedIn = b.HasCheckedIn,
                IsPaid = b.IsPaid,
                CustomerName = b.Customer.DisplayName,
                CustomerPhone = b.Customer.PhoneNumber,
                PitchTypeName = b.PitchType.Name
            }).ToList();

            if(role == "Admin")
            {
                bookingDtos.ForEach(dto => dto.HasCheckedIn = null);
            }

            return bookingDtos;
        }

        public async Task UpdateBookingCheckInStatusAsync(int IdBooking, bool checkIn)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAssync(IdBooking);
            if(booking != null)
            {
                booking.HasCheckedIn = checkIn;
                await _unitOfWork.Bookings.UpdateAssync(booking);
            }
        }
    }
}
