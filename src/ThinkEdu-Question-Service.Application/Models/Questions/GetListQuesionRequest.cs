using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Application.Models.Questions
{
    public class QuestionSearchModel : BaseFilterDto
    {
        public string? KeySearch { get; set; } = string.Empty;

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Level { get; set; }

        public string? Group { get; set; }

        public string? LessonId { get; set; }


    }

    public class GetListQuesionRequest : BaseQuestionDto<GetListQuesionRequest>
    {
    }

    public class GetListWithFilterQuestion
    {
        public ICollection<ValidationEnumOptionDto> Filters { get; set; } = new List<ValidationEnumOptionDto>();

        public ICollection<HeaderTableResponse> Headers { get; set; } = new List<HeaderTableResponse>();

        public ICollection<GetListQuesionRequest> DataTable { get; set; } = new List<GetListQuesionRequest>();

        public int Count { get; set; } = 0;
    }
}
