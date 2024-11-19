using Backend.Entities.Enums;
using Backend.Entities.PitchType.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities.Pitch.Model
{
    public class PitchModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public PitchStatus Status { get; set; } = PitchStatus.Trống;

        public int? IdPitchType { get; set; }
        [ForeignKey("IdPitchType")]
        public virtual PitchTypeModel PitchType { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
