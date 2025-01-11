using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Entities
{
    public class VehicleKhatabookDbContext : DbContext
    {
        public VehicleKhatabookDbContext(DbContextOptions<VehicleKhatabookDbContext> options) : base(options) { }
        public DbSet<OtpRequest> OtpRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DeviceInfo> DeviceInfos { get; set; }
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
        public DbSet<OwnerKhataCredit> OwnerKhataCredits { get; set; }
        public DbSet<OwnerKhataDebit> OwnerKhataDebits { get; set; }
        public DbSet<DriverOwnerUser> DriverOwnerUsers { get; set; }
        public DbSet<ApplicationConfiguration> ApplicationConfigurations { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<PaymentHistory> PaymentHistory { get; set; }
        public DbSet<SubscriptionMaster> subscriptionMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleID);
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Subscription>().HasKey(s => s.SubscriptionID);
            modelBuilder.Entity<IncomeCategory>().HasKey(ic => ic.IncomeCategoryID);
            modelBuilder.Entity<ExpenseCategory>().HasKey(ec => ec.ExpenseCategoryID);
            modelBuilder.Entity<UserIncome>().HasKey(i => i.IncomeID);
            modelBuilder.Entity<UserExpense>().HasKey(e => e.ExpenseID);
            modelBuilder.Entity<FuelTracking>().HasKey(ft => ft.Id);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationID);
            modelBuilder.Entity<Backup>().HasKey(b => b.BackupID);
            modelBuilder.Entity<ScreenContent>().HasKey(sc => sc.ScreenContentID);
            modelBuilder.Entity<VechileType>().HasKey(v => v.VehicleTypeId);
            modelBuilder.Entity<OtpRequest>().HasKey(o => o.OtpRequestId);
            modelBuilder.Entity<LanguageType>().HasKey(v => v.LanguageTypeId);
            modelBuilder.Entity<AdminUser>().HasKey(v => v.AdminID);
            modelBuilder.Entity<OwnerKhataCredit>().HasKey(s => s.Id);
            modelBuilder.Entity<OwnerKhataDebit>().HasKey(s => s.Id);
            modelBuilder.Entity<DriverOwnerUser>().HasKey(s => s.DriverOwnerUserId);
            modelBuilder.Entity<ApplicationConfiguration>().HasKey(s => s.ApplicationConfigurationId);
            modelBuilder.Entity<PaymentHistory>().HasKey(s => s.PaymentId);
            modelBuilder.Entity<State>().HasKey(s => s.Id);
            modelBuilder.Entity<SubscriptionMaster>().HasKey(s => s.SubscriptionId);
            modelBuilder.Entity<AdminUser>()
                .HasOne(s => s.CreatedByAdmin)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy)
                .HasForeignKey(s => s.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasOne(s => s.LanguageType)
                .WithMany()
                .HasForeignKey(s => s.LanguageTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserID)
                .HasConstraintName("FK_User_Vehicle");

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

            modelBuilder.Entity<UserIncome>()
                .HasOne(i => i.Vehicle)
                .WithMany()
                .HasForeignKey(i => i.IncomeVehicleId)
                .HasConstraintName("FK_UserIncome_IncomeVehicle");

            modelBuilder.Entity<UserExpense>()
                .HasOne(e => e.ExpenseCategory)
                .WithMany()
                .HasForeignKey(e => e.ExpenseCategoryID);

            modelBuilder.Entity<UserExpense>()
                .HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.ExpenseVehicleId)
                .HasConstraintName("FK_UserExpense_ExpenseVehicle");

            modelBuilder.Entity<IncomeCategory>()
                .HasIndex(i => i.Name)
                .IsUnique();

            modelBuilder.Entity<ExpenseCategory>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<OwnerKhataDebit>()
                .HasOne(f => f.DriverOwnerUser)
                .WithMany()
                .HasForeignKey(f => f.DriverOwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<OwnerKhataDebit>()
            //   .HasOne(f => f.DriverOwnerUser)
            //   .WithMany()
            //   .HasForeignKey(f => f.DriverOwnerId)
            //   .OnDelete(DeleteBehavior.NoAction)
            //  .HasConstraintName("FK_OwnerKhataDebit_DriverOwner");

            modelBuilder.Entity<DeviceInfo>()
            .HasOne(d => d.User)
            .WithMany(u => u.DeviceInfos)
            .HasForeignKey(d => d.UserID);

            modelBuilder.Entity<OwnerKhataCredit>()
               .HasOne(f => f.DriverOwnerUser)
               .WithMany()
               .HasForeignKey(f => f.DriverOwnerId)
               .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<OwnerKhataCredit>()
            //  .HasOne(f => f.DriverOwnerUser)
            //  .WithMany()
            //  .HasForeignKey(f => f.DriverOwnerId)
            //  .OnDelete(DeleteBehavior.NoAction)
            //  .HasConstraintName("FK_OwnerKhataCredit_DriverOwner");

            modelBuilder.Entity<FuelTracking>()
            .Property(f => f.StartFuelLevelInLiters)
            .HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<FuelTracking>()
                .Property(f => f.EndFuelLevelInLiters)
                .HasColumnType("decimal(18, 0)")
                .IsRequired(false); // nullable

            modelBuilder.Entity<FuelTracking>()
                .Property(f => f.FuelAddedInLitersJson)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<FuelTracking>()
                .Property(f => f.UserId)
                .HasColumnType("uniqueidentifier");

            modelBuilder.Entity<FuelTracking>()
                .HasOne(f => f.User)
                .WithMany()  // Assuming the User class is set up properly
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
