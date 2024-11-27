using AutoMapper;
using Backend.Entities.Pitch.Dto;
using Backend.Entities.Pitch.Model;
using Backend.Entities.PitchType.Dto;
using Backend.Entities.PitchType.Model;

namespace Backend.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping cho PitchModel và PitchDto
            CreateMap<PitchModel, PitchDto>()
                .ForMember(dest => dest.IdPitchType, opt => opt.MapFrom(src => src.IdPitchType)) // Giữ nguyên ánh xạ IdPitchType
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) // Ánh xạ trạng thái từ Enum sang chuỗi
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt)) // Ánh xạ thời gian tạo
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => src.UpdateAt)) // Ánh xạ thời gian cập nhật
                .ReverseMap(); // Ánh xạ ngược lại

            // Mapping cho PitchTypeModel và PitchTypeDto (nếu cần)
            CreateMap<PitchTypeModel, PitchTypeDto>().ReverseMap();
        }
    }
}
