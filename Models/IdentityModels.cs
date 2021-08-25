using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolManagement.Model.Entity;
using SchoolManagement.Models;
using SchoolManagement.Model;
using SchoolManagement.Models.Entity;
using SchoolManagement.Models.CartModels;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<GuardianType> GuardianTypes { get; set; }
        public DbSet<ApplicationDocuments> ApplicationDocuments { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ClassFee> ClassFees { get; set; }
        public DbSet<AssignTeacheToClass> AssignTeacheToClasses { get; set; }
        public DbSet<ClassName> ClassNames { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<StudentApplication> Studentapplications { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<WorkSchedule> WorkSchedule { get; set; }
        public DbSet<ThemeColor> colors { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Term> terms { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Stationary> stationaries { get; set; }
        public DbSet<Meeting> meetings { get; set; }
        public DbSet<Residence> Residences { get; set; }
        public DbSet<MeetingTeacher> meetingTeachers { get; set; }
        public DbSet<ResidenceStudent> residenceStudents { get; set; }

        public System.Data.Entity.DbSet<SchoolManagement.Models.ResidenceType> ResidenceTypes { get; set; }

        public System.Data.Entity.DbSet<SchoolManagement.Models.ResidenceApplication> ResidenceApplications { get; set; }

        // public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Order_Tracking> Order_Trackings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shipping_Address> Shipping_Addresses { get; set; }

        public System.Data.Entity.DbSet<SchoolManagement.Models.Room_Type> Room_Type { get; set; }

        public System.Data.Entity.DbSet<SchoolManagement.Models.Room> Rooms { get; set; }
    }
}