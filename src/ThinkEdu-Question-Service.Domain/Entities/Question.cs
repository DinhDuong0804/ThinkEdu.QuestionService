using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Domain.Entities
{
    public class Question : BaseStatusEntity<string>
    {
        public Question() => Id = Ulid.NewUlid().ToString();

        public string? LessonId { get; set; } // BaiHocId

        public string Title { get; set; } = null!; // TieuDe

        public string Type { get; set; } = null!;

        public string Level { get; set; } = null!;

        public string Group { get; set; } = null!;

        public string? FileUrl { get; set; }

        public string? Explanation { get; set; } // TraLoi

        public string? ParentId { get; set; }

        public int? STT { get; set; }

        public int? Tenant_Id { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); 
    }
}