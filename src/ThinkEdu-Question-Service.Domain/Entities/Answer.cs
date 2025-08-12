using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Domain.Entities
{
    public class Answer : BaseStatusEntity<string>
    {
        public Answer() => Id = Ulid.NewUlid().ToString();

        public string? Content { get; set; } // NoiDung

        public string? FileUrl { get; set; }

        public bool IsCorrect { get; set; } // isDapAn

        public int? Tenant_Id { get; set; }

        public string QuestionId { get; set; } = null!; // CauHoiId

        public Question Question { get; set; } = null!;
    }
}