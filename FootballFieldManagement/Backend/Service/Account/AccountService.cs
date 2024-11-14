using Backend.Entities.Account.Dto;
using Backend.Entities.Account.Model;
using Backend.Entities.Staff.Model;
using Backend.Helpers;
using Backend.Service.Account;
using Backend.UnitOfWork;


public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private IConfiguration _config;
    private ITokenService _tokenService;

    public AccountService(IUnitOfWork unitOfWork, IConfiguration config, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _tokenService = tokenService;
    }

    // Đăng nhập: Trả về chuỗi token nếu thành công
    public async Task<LoginModel> Login(LoginDto loginDto)
    {
        var account = await _unitOfWork.Accounts.GetByEmail(loginDto.Email);
        if (account == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
        {
            return null;
        }

        var token = _tokenService.CreateToken(account, _config);

        // Tạo token JWT sau khi xác thực thành công
        return new LoginModel() { Username = account.Email, Token = token };
    }

    // Tạo tài khoản Staff (chỉ Admin mới làm được)
    public async Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto)
    {
        // Kiểm tra tài khoản đã tồn tại
        var existingAccount = await _unitOfWork.Accounts.GetByEmail(createStaffDto.Email);
        if (existingAccount != null)
        {
            Console.WriteLine("Account with this email already exists.");
            throw new InvalidOperationException("Email đã tồn tại.");
        }

        // Xử lý tạo Staff
        var staff = new StaffModel
        {
            DisplayName = createStaffDto.DisplayName,
            DateOfBirth = createStaffDto.DateOfBirth,
            CCCD = createStaffDto.CCCD,
            Gender = createStaffDto.Gender,
            PhoneNumber = createStaffDto.PhoneNumber,
            Address = createStaffDto.Address,
            StartDate = createStaffDto.StartDate,
        };

        try
        {
            Console.WriteLine("Saving staff to the database...");
            await _unitOfWork.Staffs.Add(staff);
            await _unitOfWork.CompleteAsync();

            if (staff.Id == 0)
            {
                throw new InvalidOperationException("Failed to retrieve Staff ID after saving.");
            }

            var account = new AccountModel
            {
                Email = createStaffDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(createStaffDto.Password),
                Role = "Staff",
                IdStaff = staff.Id
            };

            // Lưu tài khoản
            await _unitOfWork.Accounts.Add(account);
            await _unitOfWork.CompleteAsync();
            Console.WriteLine($"Account for {account.Email} saved successfully.");

            return account;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateStaff: {ex.Message}");
            throw new InvalidOperationException($"Error saving staff or account: {ex.Message}");
        }
    }
}
