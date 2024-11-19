using Microsoft.EntityFrameworkCore;
using Backend.Entities.Account.Model;
using Backend.Entities.Customer.Model;
using Backend.Entities.Staff.Model;
using Backend.Entities.Pitch.Model;
using Backend.Entities.PitchType.Model;
using Backend.Entities.Enums;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<StaffModel> Staffs { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<PitchModel> Pitches { get; set; }
        public DbSet<PitchTypeModel> PitchesType { get; set; }

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

            // Cấu hình bảng Pitch (Sân)
            modelBuilder.Entity<PitchModel>()
                .ToTable("Pitch")
                .HasKey(p => p.Id);

            modelBuilder.Entity<PitchModel>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<PitchModel>()
                .Property(p => p.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(), // Convert Enum to string when saving to DB
                    v => (PitchStatus)Enum.Parse(typeof(PitchStatus), v) // Convert string back to Enum when reading from DB
                );

            modelBuilder.Entity<PitchModel>()
                .Property(p => p.CreateAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<PitchModel>()
                .Property(p => p.UpdateAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Cấu hình quan hệ giữa Pitch và PitchType
            modelBuilder.Entity<PitchModel>()
                .HasOne(p => p.PitchType)
                .WithMany(pt => pt.Pitches)
                .HasForeignKey(p => p.IdPitchType)
                .OnDelete(DeleteBehavior.SetNull); // Xóa sân sẽ không xóa loại sân
        }
    }
}
