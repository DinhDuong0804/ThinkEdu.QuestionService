namespace ThinkEdu_Question_Service.Domain.Common
{
    public class BaseAuditableTimeEntity<T>  : BaseTimeEntity<T>
    {
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
       // public string? DeletedBy { get; set; }
    }

    public abstract class BaseTimeEntity<T> : BaseStatusEntity<T>
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
      //  public DateTimeOffset? DeletedAt { get; set; }
    }
}