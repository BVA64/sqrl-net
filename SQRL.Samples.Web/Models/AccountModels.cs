using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Devtalk.EF.CodeFirst;

namespace SQRL.Samples.Web.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#if DEBUG
            Database.SetInitializer(new DropCreateDatabaseAlways<UsersContext>());
#else
            Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<UsersContext>());
#endif
            base.OnModelCreating(modelBuilder);
        }

        public class AutomaticMigrationConfiguration : DbMigrationsConfiguration<UsersContext>
        {
            public AutomaticMigrationConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }
        }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class UserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public string SqrlId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? AuthenticatedDatetime { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        public string SqrlUrl { get; private set; }
    }
}
