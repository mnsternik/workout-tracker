namespace WorkoutTracker.Api.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? RevokedDate { get; set; }
        public bool IsActive => RevokedDate == null && ExpirationDate > DateTime.UtcNow;
        public ApplicationUser? User { get; set; }
    }
}
