using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cfeya_webapp.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? NotificationTitle { get; set; }
        public DateTime? DateTime { get; set; }
        public bool MarkasRead { get; set; }
        public int WishID { get; set; }

        [ForeignKey("UserID")]
        public string? UserId { get; set; }
        public ApplicationUser? UserID { get; set; }

    }
}
