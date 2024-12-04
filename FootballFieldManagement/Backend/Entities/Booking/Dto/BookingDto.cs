namespace Backend.Entities.Booking.Dto
{
    public class BookingDto
    {
        public int Id { get; set; } // ID của đặt sân
        public DateTime BookingDate { get; set; } // Ngày đặt sân
        public bool? HasCheckedIn { get; set; } // Trạng thái nhận sân
        public bool IsPaid { get; set; } // Trạng thái thanh toán
        public string CustomerName { get; set; } // Họ tên của khách hàng (DisplayName từ CustomerModel)
        public string CustomerPhone { get; set; } // Số điện thoại của khách hàng (PhoneNumber từ CustomerModel)
        public string PitchName { get; set; } // Loại sân (tên sân)
        public string PitchTypeName { get; set; } // Tên loại sân
        public string PitchDetails { get; set; } // Tên Sân - Tên Loại Sân
    }
}
