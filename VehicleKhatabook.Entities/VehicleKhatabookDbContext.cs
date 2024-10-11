using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Entities
{
    public class VehicleKhatabookDbContext : DbContext
    {
        public VehicleKhatabookDbContext(DbContextOptions<VehicleKhatabookDbContext> options) : base(options) { }

        public DbSet<LanguagePreference> LanguagePreferences { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }   
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FuelTracking> FuelTrackings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Backup> Backups { get; set; }
        public DbSet<ScreenContent> ScreenContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleID);
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<Subscription>().HasKey(s => s.SubscriptionID);
            modelBuilder.Entity<IncomeCategory>().HasKey(ic => ic.IncomeCategoryID);
            modelBuilder.Entity<ExpenseCategory>().HasKey(ec => ec.ExpenseCategoryID);
            modelBuilder.Entity<Income>().HasKey(i => i.IncomeID);
            modelBuilder.Entity<Expense>().HasKey(e => e.ExpenseID);
            modelBuilder.Entity<FuelTracking>().HasKey(ft => ft.FuelTrackingID);
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationID);
            modelBuilder.Entity<Backup>().HasKey(b => b.BackupID);
            modelBuilder.Entity<ScreenContent>().HasKey(sc => sc.ScreenContentID);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserID);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID);

            modelBuilder.Entity<Backup>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID);

            modelBuilder.Entity<Income>()
                .HasOne(i => i.Vehicle)
                .WithMany()
                .HasForeignKey(i => i.VehicleID);

            modelBuilder.Entity<Income>()
                .HasOne(i => i.IncomeCategory)
                .WithMany()
                .HasForeignKey(i => i.IncomeCategoryID);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleID);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ExpenseCategory)
                .WithMany()
                .HasForeignKey(e => e.ExpenseCategoryID);

            modelBuilder.Entity<FuelTracking>()
                .HasOne(f => f.Driver)
                .WithMany()
                .HasForeignKey(f => f.DriverID);

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
