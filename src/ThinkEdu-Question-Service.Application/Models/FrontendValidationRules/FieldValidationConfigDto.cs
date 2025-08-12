namespace ThinkEdu_Question_Service.Application.Models.FrontendValidationRules
{
    public class FieldValidationConfigDto
    {
        public string FieldName { get; set; } = null!;
        public List<FieldValidationRuleDto> Rules { get; set; } = new();
    }

    public class FieldValidationRuleDto
    {
        public string RuleName { get; set; } = null!;
        public string RuleValue { get; set; } = null!;
        public string? Message { get; set; }
    }
}