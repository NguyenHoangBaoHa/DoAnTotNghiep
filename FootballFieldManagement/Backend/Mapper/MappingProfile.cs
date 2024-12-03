using AutoMapper;
using Backend.Entities.Booking.Dto;
using Backend.Entities.Booking.Model;
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

            CreateMap<BookingModel, BookingDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.DisplayName)) // Ánh xạ tên khách hàng
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber)) // Ánh xạ số điện thoại khách hàng
            .ForMember(dest => dest.PitchTypeName, opt => opt.MapFrom(src => src.PitchType.Name)) // Ánh xạ tên loại sân
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.IsPaid)) // Trạng thái thanh toán
            .ForMember(dest => dest.HasCheckedIn, opt => opt.MapFrom(src => src.HasCheckedIn)) // Trạng thái nhận sân
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate)) // Ngày đặt sân
            .ReverseMap() // Hỗ trợ ánh xạ ngược từ DTO sang Model
            .ForPath(src => src.Customer.DisplayName, opt => opt.Ignore()) // Bỏ qua khi ánh xạ ngược
            .ForPath(src => src.Customer.PhoneNumber, opt => opt.Ignore()) // Bỏ qua khi ánh xạ ngược
            .ForPath(src => src.PitchType.Name, opt => opt.Ignore()); // Bỏ qua khi ánh xạ ngược
        }
    }
}
