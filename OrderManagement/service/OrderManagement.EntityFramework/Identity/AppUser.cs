using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.EntityFramework.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
