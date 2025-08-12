using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;

namespace ThinkEdu_Question_Service.Application.Models.Questions
{
    public class GetUpdateInfoQuestion  
    {
        public List<EnumOptionSourceDto> QuestionTypes { get; set; } = new();

        public List<EnumOptionSourceDto> QuestionLevels { get; set; } = new();

        public List<EnumOptionSourceDto> QuestionGroups { get; set; } = new();

        public List<EnumOptionSourceDto> Statuses { get; set; } = new();

        public QuestionModel Detail { get; set; } = null!;

        public List<FieldValidationConfigDto> FieldValidates { get; set; } = new List<FieldValidationConfigDto>();
    }

    public class GetUpdateInfoQuestionDto
    {
        public ICollection<ValidationEnumOptionDto> EnumOption { get; set; } = new List<ValidationEnumOptionDto>();

        public QuestionModel Detail { get; set; } = null!;

        public List<FieldValidationConfigDto> FieldValidates { get; set; } = new List<FieldValidationConfigDto>();

    }
}

