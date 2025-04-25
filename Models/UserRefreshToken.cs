using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Api.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpirationDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? RevokedDate { get; set; }
        public bool IsActive => RevokedDate == null && ExpirationDate > DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; } = null!;
    }
}
