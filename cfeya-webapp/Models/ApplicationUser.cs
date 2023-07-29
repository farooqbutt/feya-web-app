using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace cfeya_webapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty ;
        public string? Middlename { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; } = string.Empty; 
        public string CellPhone { get; set; } = string.Empty; 
        public string? HomePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string Street { get; set; } = string.Empty; 
        public string City { get; set; } = string.Empty; 
        public string State { get; set; } = string.Empty; 
        public string Zip { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }

        //For keeping track record of User Current Role in User Table.
        public string? Role { get; set; }
        public int How_Many_Times_Fairy { get; set; }
        public int How_Many_Times_Cinderella { get; set; }
        public bool Is18 { get; set; }

    }
}
