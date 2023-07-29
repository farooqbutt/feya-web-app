using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cfeya_webapp.Models
{
    public class Wishes
    {
        [Key]
        public int Id { get; set; }
        [StringLength(2500)]
        public string? StoryText { get; set; }
        [StringLength(500)]
        public string? WishText { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SubmitedDate { get; set; }
        public string? Status { get; set; }
        public DateTime? Picked_up_date { get; set; }   //This time stamp will be for Wish discovered status
        public DateTime? FullfilingDate { get; set; }   //This time stamp will be for Wish Fullfiling status
        public DateTime? CompletedDate { get; set; }    //This time stamp will be for Wish Completed status
        public DateTime? AcceptedDate { get; set; }     //This time stamp will be for Wish Accepted status
        public DateTime? LastModifiedDate { get; set; } //This time stamp will be for storing last modification date.
        public DateTime? CancelledDate { get; set; }    //This time stamp will be for storing wish cancellation date.
        public bool IsCancelled { get; set; }
        public string? CreatedBy { get; set; }
        public string? PickedByFairy { get; set; }

        [ForeignKey("CreatedByID")]
        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByID { get; set; }

        [ForeignKey("PickedByFairyID")]
        public string? PickedByUserId { get; set; } 
        public ApplicationUser? PickedByFairyID { get; set; }
    }
}
