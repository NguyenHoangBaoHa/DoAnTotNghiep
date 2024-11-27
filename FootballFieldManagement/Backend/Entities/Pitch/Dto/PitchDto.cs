namespace Backend.Entities.Pitch.Dto
{
    public class PitchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } // Trạng thái dưới dạng chuỗi
        public int? IdPitchType { get; set; }
        //public string PitchTypeName { get; set; } // Tên loại sân
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
