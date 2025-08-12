using System.ComponentModel.DataAnnotations;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Domain.Common
{
    public abstract class BaseStatusEntity<T> : BaseEntity<T>
    {
        [Required]
        public string Status { get; set; } = nameof(EStatus.Active);
    }
}