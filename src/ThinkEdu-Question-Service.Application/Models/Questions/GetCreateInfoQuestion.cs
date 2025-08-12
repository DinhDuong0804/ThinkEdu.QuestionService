using System.ComponentModel.DataAnnotations;
using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;

namespace ThinkEdu_Question_Service.Application.Models.Questions
{
    public class GetCreateInfoQuestion
    {
      
        public List<EnumOptionSourceDto> QuestionTypes { get; set; } = new ();

        public List<EnumOptionSourceDto> QuestionLevels { get; set; } = new ();

        public List<EnumOptionSourceDto> QuestionGroups { get; set; } = new ();

        public List<EnumOptionSourceDto> Statuses { get; set; } = new ();

        public List<FieldValidationConfigDto> FieldValidates { get; set; } = new List<FieldValidationConfigDto>();
    }

    public class GetCreateInfoQuestionDto
    {
        public ICollection<ValidationEnumOptionDto> EnumOption { get; set; } = new List<ValidationEnumOptionDto>();

        public List<FieldValidationConfigDto> FieldValidates { get; set; } = new List<FieldValidationConfigDto>();
    }
}