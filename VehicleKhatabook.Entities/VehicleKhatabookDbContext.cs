using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Entities
{
    public class VehicleKhatabookDbContext : DbContext
    {
        public VehicleKhatabookDbContext(DbContextOptions<VehicleKhatabookDbContext> options) : base(options) { }

        public DbSet<OtpRequest> OtpRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<UserIncome> UserIncomes { get; set; }
        public DbSet<UserExpense> UserExpenses { get; set; }
        public DbSet<FuelTracking> FuelTrackings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Backup> Backups { get; set; }
        public DbSet<ScreenContent> ScreenContents { get; set; }
        public DbSet<VechileType> VehicleTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LanguageType> LanguageTypes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<SMSProviderConfig> SMSProviderConfigs { get; set; }
        public DbSet<OwnerKhataCredit> OwnerKhataCredits { get; set; }
        public DbSet<OwnerKhataDebit> OwnerKhataDebits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleID);
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Subscription>().HasKey(s => s.SubscriptionID);
            modelBuilder.Entity<IncomeCategory>().HasKey(ic => ic.IncomeCategoryID);
            modelBuilder.Entity<ExpenseCategory>().HasKey(ec => ec.ExpenseCategoryID);
            modelBuilder.Entity<UserIncome>().HasKey(i => i.IncomeID);
            modelBuilder.Entity<UserExpense>().HasKey(e => e.ExpenseID);
            modelBuilder.Entity<FuelTracking>().HasKey(ft => ft.FuelTrackingID);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationID);
            modelBuilder.Entity<Backup>().HasKey(b => b.BackupID);
            modelBuilder.Entity<ScreenContent>().HasKey(sc => sc.ScreenContentID);
            modelBuilder.Entity<VechileType>().HasKey(v => v.VehicleTypeId);
            modelBuilder.Entity<OtpRequest>().HasKey(o => o.OtpRequestId);
            modelBuilder.Entity<LanguageType>().HasKey(v => v.LanguageTypeId);
            modelBuilder.Entity<AdminUser>().HasKey(v => v.AdminID);
            modelBuilder.Entity<SMSProviderConfig>().HasKey(s => s.ProviderID);
            modelBuilder.Entity<OwnerKhataCredit>().HasKey(s => s.Id);
            modelBuilder.Entity<OwnerKhataDebit>().HasKey(s => s.Id);
            //modelBuilder.Entity<SMSProviderConfig>()
            //    .HasOne(s => s.CreatedByUser)
            //    .WithMany()
            //    .HasForeignKey(s => s.CreatedBy);

            //modelBuilder.Entity<SMSProviderConfig>()
            //    .HasOne(s => s.ModifiedByUser)
            //    .WithMany()
            //    .HasForeignKey(s => s.ModifiedBy);

            modelBuilder.Entity<AdminUser>()
                .HasOne(s => s.CreatedByAdmin)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy)
                .HasForeignKey(s => s.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasOne(s => s.LanguageType)
                .WithMany()
                .HasForeignKey(s => s.LanguageTypeId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany()
                .HasForeignKey(v => v.VehicleTypeId);


            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID);

            modelBuilder.Entity<Backup>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID);

            modelBuilder.Entity<UserIncome>()
                .HasOne(i => i.IncomeCategory)
                .WithMany()
                .HasForeignKey(i => i.IncomeCategoryID);

            modelBuilder.Entity<UserExpense>()
                .HasOne(e => e.ExpenseCategory)
                .WithMany()
                .HasForeignKey(e => e.ExpenseCategoryID);

            modelBuilder.Entity<FuelTracking>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FuelTracking>()
                .HasOne(f => f.Vehicle)
                .WithMany()
                .HasForeignKey(f => f.VehicleID);


            modelBuilder.Entity<IncomeCategory>()
                .HasIndex(i => i.Name)
                .IsUnique();

            modelBuilder.Entity<ExpenseCategory>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder.Entity<OwnerKhataDebit>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);
            modelBuilder.Entity<OwnerKhataCredit>()
               .HasOne(f => f.User)
               .WithMany()
               .HasForeignKey(f => f.UserId);
            //modelBuilder.Entity<OtpRequest>()
            //    .HasOne(o => o.User)
            //    .WithMany()  
            //    .HasForeignKey(o => o.UserID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
