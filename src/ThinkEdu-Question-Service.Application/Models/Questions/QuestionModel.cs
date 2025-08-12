namespace ThinkEdu_Question_Service.Application.Models.Questions
{
    public class QuestionModel : BaseQuestionDto<QuestionModel>
    {
        public ICollection<ChildQuestionDto>? ChildQuestions { get; set; } = [];
    }
    public class ChildQuestionDto : BaseQuestionDto<ChildQuestionDto>
    {
      
    }
}
