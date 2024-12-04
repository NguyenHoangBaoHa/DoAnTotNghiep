using Backend.Entities.Customer.Model;
using Backend.Entities.Pitch.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities.Booking.Model
{
    public class BookingModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime BookingDate { get; set; }
        public bool? HasCheckedIn { get; set; }
        public bool IsPaid { get; set; } = false;

        public int IdCustomer { get; set; }
        [ForeignKey("IdCustomer")]
        public virtual CustomerModel Customer { get; set; }

        public int IdPitch { get; set; }
        [ForeignKey("IdPitch")]
        public virtual PitchModel Pitch { get; set; }
    }
}
