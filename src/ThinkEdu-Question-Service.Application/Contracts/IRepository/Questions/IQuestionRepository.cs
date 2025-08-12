using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Application.Common;

namespace ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
        Task<(Question, List<Question>)> GetQuestionByIdAsync(string questionId);

        Task<(int count, List<Question>)> GetListQuestionAsync(QuestionSearchModel model);

    }
}