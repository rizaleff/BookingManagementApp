using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BookingManagementDbContext : DbContext
    {
        public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options) { }

        //Menambahkan semua entitas ke DbSet untuk migrasi
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<University> Universities { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*Uniqe Tidak bisa ditambahkan scr langsung pada annotation
             *Harus ditambahkan pada override method OnModelCreating
             */
    /*        modelBuilder.Entity<Employee>().HasIndex(e => new
            {
                e.Nik,
                e.Email,
                e.PhoneNumber
            }).IsUnique();

*/
            modelBuilder.Entity<Employee>().HasIndex(e => e.Nik).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber).IsUnique();

            // One University has many educations
            modelBuilder.Entity<University>()
                .HasMany(e => e.Educations)
                .WithOne(u => u.University)
                .HasForeignKey(e => e.UniversityGuid);

            //One education has one employee
            modelBuilder.Entity<Education>()
                .HasOne(em => em.Employee)
                .WithOne(ed => ed.Education)
                .HasForeignKey<Education>(em => em.Guid);

            //One Account has one Employee
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(a => a.Guid);

            //One Account Roles has many roles
            modelBuilder.Entity<AccountRole>()
                .HasOne(a => a.Account)
                .WithMany(a => a.AccountRoles)
                .HasForeignKey(a => a.AccountGuid);

            //One Role Has Many 
            modelBuilder.Entity<Role>()
                .HasMany(r => r.AccountRoles)
                .WithOne(a => a.Role)
                .HasForeignKey(r => r.RoleGuid);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Employee)
                .HasForeignKey(b => b.EmployeeGuid);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomGuid);
        }

    }
}
