namespace Core.Entities
{
    public class Notification: BaseEntity
    {
        public int UserId { get; set; }             // quien recibe la notificación
        public User User { get; set; } = null!;
        public int? ActorId { get; set; }           // quien la generó
        public User? Actor { get; set; }
        public string Type { get; set; } = string.Empty;   // "like" | "comment"
        public string Message { get; set; } = string.Empty;
        public int? PostId { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
