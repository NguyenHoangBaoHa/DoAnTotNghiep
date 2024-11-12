using Microsoft.IdentityModel.Tokens;
using QuanLySanBong.Entities.Account.Dto;
using QuanLySanBong.Entities.Account.Model;
using QuanLySanBong.Entities.Staff.Model;
using QuanLySanBong.Helpers;
using QuanLySanBong.Service.Account;
using QuanLySanBong.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;

    public AccountService(IUnitOfWork unitOfWork, JwtSettings jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings;
    }

    // Đăng nhập: Trả về chuỗi token nếu thành công
    public async Task<LoginModel> Login(LoginDto loginDto)
    {
        var account = await _unitOfWork.Accounts.GetByEmail(loginDto.Email);
        if (account == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
        {
            return null;
        }

        var token = GenerateJwtToken(account);

        // Tạo token JWT sau khi xác thực thành công
        return new LoginModel() { Username = account.Email, Token = token };
    }

    // Tạo tài khoản Staff (chỉ Admin mới làm được)
    public async Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto)
    {
        Console.WriteLine("Entered CreateStaff method");

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


    // Hàm tạo JWT
    private string GenerateJwtToken(AccountModel account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var claims = new[]
        {
            new Claim("Id", account.Id.ToString()),
            new Claim(ClaimTypes.Role, account.Role),
            new Claim(ClaimTypes.Email, account.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDay),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
