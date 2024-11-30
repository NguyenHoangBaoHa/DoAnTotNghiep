using Backend.Entities.Customer.Model;
using Backend.Entities.PitchType.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities.Booking.Model
{
    public class BookingModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime BookingDate { get; set; }
        public bool HasCheckedIn { get; set; }
        public bool IsPaid { get; set; } = false;

        public int IdCustomer { get; set; }
        [ForeignKey("IdCustomer")]
        public CustomerModel Customer { get; set; }

        public int IdPitchType { get; set; }
        [ForeignKey("IdPitchType")]
        public PitchTypeModel PitchType { get; set; }
    }
}
