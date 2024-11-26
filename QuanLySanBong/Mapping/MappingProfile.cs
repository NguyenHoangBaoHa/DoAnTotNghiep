using AutoMapper;
using QuanLySanBong.Entities.Account.Dto;
using QuanLySanBong.Entities.Account.Model;
using QuanLySanBong.Entities.Customer.Dto;
using QuanLySanBong.Entities.Customer.Model;
using QuanLySanBong.Entities.Staff.Dto;
using QuanLySanBong.Entities.Staff.Model;

namespace QuanLySanBong.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountModel, AccountDto>().ReverseMap();
            CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            CreateMap<StaffModel, StaffDto>().ReverseMap();

            // Map từ CreateStaffDto sang AccountModel
            CreateMap<CreateStaffDto, AccountModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Staff")) // Gán role là "Staff" cho Account
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password))) // Mã hóa mật khẩu
                .ForMember(dest => dest.IdStaff, opt => opt.Ignore()); // Ignore IdStaff vì sẽ thiết lập sau khi tạo StaffModel

            // Map từ CreateStaffDto sang StaffModel
            CreateMap<CreateStaffDto, StaffModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.CCCD, opt => opt.MapFrom(src => src.CCCD))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate));
        }
    }
}
