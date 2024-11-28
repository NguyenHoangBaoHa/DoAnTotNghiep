using Backend.Entities.Account.Dto;
using Backend.Entities.Account.Model;
using Backend.Entities.Customer.Dto;
using Backend.Entities.Customer.Model;
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
        return new LoginModel() { Username = account.Email, Token = token, Role = account.Role };
    }

    // Tạo tài khoản Staff (chỉ Admin mới làm được)
    public async Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto)
    {
        // Kiểm tra tài khoản đã tồn tại
        var existingAccount = await _unitOfWork.Accounts.GetByEmail(createStaffDto.Email);
        if (existingAccount != null)
        {
            //Console.WriteLine("Account with this email already exists.");
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

        await _unitOfWork.Staffs.Add(staff);
        await _unitOfWork.CompleteAsync();

        if(staff.Id == 0)
        {
            throw new InvalidOperationException("Failed to retrieve Staff ID after saving.");
        }

        // Tạo AccountModel
        var account = new AccountModel
        {
            Email = createStaffDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createStaffDto.Password),
            Role = "Staff",
            IdStaff = staff.Id
        };

        // Liên kết Staff và Account
        staff.Account = account;

        // Lưu AccountModel
        await _unitOfWork.Accounts.Add(account);
        await _unitOfWork.CompleteAsync();

        return account;
    }

    public async Task<AccountModel> RegisterCustomer(CustomerDto customerDto, AccountDto accountDto)
    {
        var existingAccount = await _unitOfWork.Accounts.GetByEmail(accountDto.Email);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("Email đã tồn tại.");
        }

        var customer = new CustomerModel
        {
            DisplayName = customerDto.DisplayName,
            DateOfBirth = customerDto.DateOfBirth,
            CCCD = customerDto.CCCD,
            Gender = customerDto.Gender,
            PhoneNumber = customerDto.PhoneNumber,
            Address = customerDto.Address,
        };

        var account = new AccountModel
        {
            Email = accountDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password), // Hash mật khẩu
            Role = "Customer",
            IdCustomer = customer.Id,
        };

        try
        {
            // Lưu khách hàng trước để lấy ID
            await _unitOfWork.Accounts.AddCustomer(customer);
            await _unitOfWork.CompleteAsync();

            if(customer.Id == 0)
            {
                throw new InvalidOperationException("Không thể lấy ID của khách hàng sau khi lưu.");
            }

            // Liên kết ID khách hàng với tài khoản
            account.IdCustomer = customer.Id;

            // Lưu tài khoản
            await _unitOfWork.Accounts.Add(account);
            await _unitOfWork.CompleteAsync();

            return account;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Lỗi khi đăng ký tài khoản: {ex.Message}");
        }
    }
}
