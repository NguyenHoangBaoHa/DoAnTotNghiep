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
            var bookings = await _unitOfWork.Bookings.GetAllBookingsAsync();

            var bookingDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                HasCheckedIn = b.HasCheckedIn,
                IsPaid = b.IsPaid,
                CustomerName = b.Customer.DisplayName,
                CustomerPhone = b.Customer.PhoneNumber,
                PitchDetails = $"{b.Pitch.Name} - {b.Pitch.PitchType.Name}"
            }).ToList();
            
            if(role == "Admin")
            {
                bookingDtos.ForEach(dto => dto.HasCheckedIn = null); // Admin không cần hiển thị "Nhận Sân"
            }

            return bookingDtos;
        }

        public async Task<bool> UpdateCheckedInStatusAsync(int bookingId, bool hasCheckedIn)
        {
            return await _unitOfWork.Bookings.UpdateCheckedInStatusAsync(bookingId, hasCheckedIn);
        }
    }
}
