using Microsoft.EntityFrameworkCore;
using Backend.Entities.Account.Model;
using Backend.Entities.Customer.Model;
using Backend.Entities.Staff.Model;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<StaffModel> Staffs { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa cấu trúc cho bảng Account
            modelBuilder.Entity<AccountModel>()
                .ToTable("Account")
                .HasKey(a => a.Id);
            modelBuilder.Entity<AccountModel>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(256);
            modelBuilder.Entity<AccountModel>()
                .Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(256);
            modelBuilder.Entity<AccountModel>()
                .Property(a => a.Role)
                .IsRequired();

            // Seed dữ liệu cho tài khoản Admin
            modelBuilder.Entity<AccountModel>().HasData(new AccountModel
            {
                Id = 1,
                Email = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Mã hóa mật khẩu
                Role = "Admin",
                IdCustomer = null,
                IdStaff = null
            });

            // Định nghĩa cấu trúc cho bảng Staff
            modelBuilder.Entity<StaffModel>()
                .ToTable("Staff")
                .HasKey(s => s.Id);

            modelBuilder.Entity<StaffModel>()
                .Property(s => s.DisplayName)
                .IsRequired();

            modelBuilder.Entity<StaffModel>()
                .Property(s => s.StartDate)
                .IsRequired();

            // Định nghĩa cấu trúc cho bảng Customer
            modelBuilder.Entity<CustomerModel>()
                .ToTable("Customer")
                .HasKey(c => c.Id);

            modelBuilder.Entity<CustomerModel>()
                .Property(c => c.DisplayName)
                .IsRequired();

            // Thiết lập mối quan hệ 1-1 giữa Staff và Account nếu cần
            modelBuilder.Entity<AccountModel>()
                .HasOne(a => a.Staff)
                .WithOne(s => s.Account)
                .HasForeignKey<AccountModel>(a => a.IdStaff);

            // Thiết lập mối quan hệ 1-1 giữa Customer và Account nếu cần
            modelBuilder.Entity<AccountModel>()
                .HasOne(a => a.Customer)
                .WithOne(c => c.Account)
                .HasForeignKey<AccountModel>(a => a.IdCustomer);
        }
    }
}
