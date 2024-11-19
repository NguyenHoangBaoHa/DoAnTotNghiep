using Backend.Entities.Pitch.Model;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities.PitchType.Model
{
    public class PitchTypeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int LimitPerson { get; set; }

        public virtual ICollection<PitchModel> Pitches { get; set; } = new List<PitchModel>();
    }
}
