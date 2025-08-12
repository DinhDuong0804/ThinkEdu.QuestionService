namespace ThinkEdu_Question_Service.Application.Models.FrontendValidationRules
{
    public class ValidationEnumOptionDto
    {
        public string ValueFieldName { get; set; } = null!;
        public string LabelFieldName { get; set; } = null!;
        public List<EnumOptionSourceDto> Options { get; set; } = new List<EnumOptionSourceDto>();
    }

    public class EnumOptionSourceDto
    {
        public string Value { get; set; } = null!;
        public string Label { get; set; } = null!;
    }
}