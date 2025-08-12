using FastEndpoints;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions;
using ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetDetailQuestion;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Application.Common;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Commands.DeleteQuestionCommands
{                                          
    public class DeleteQuestionCommand : EndpointWithoutRequest<BaseResponse<Question>>
    {
        private readonly IBaseRepository<Answer> _answerRepository;
        private readonly ILogger<GetDetailQuestionQuery> _logger;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;

        public DeleteQuestionCommand(
            ILogger<GetDetailQuestionQuery> logger,
            IQuestionRepository questionRepository,
            IBaseRepository<Answer> answerRepository,
            IStringLocalizer<LocalizedMessage> localizedMessage
            )
        {
            _answerRepository = answerRepository;
            _logger = logger;
            _questionRepository = questionRepository;
            _localizedMessage = localizedMessage;
        }

        public override void Configure()
        {
            Delete("/Question/{id}");
            AllowAnonymous();
            DontThrowIfValidationFails();
            Summary(summary =>
            {
                summary.Summary = "xóa câu hỏi";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<string>("id");
            _logger.LogInformation("DeleteQuestionCommand -- Start : {0}", id);

            var (question, listChildQuestion) = await _questionRepository.GetQuestionByIdAsync(id!);

            if (question is null)
            {
                throw new BadRequestException(_localizedMessage["NoDataFound"] + id);
            }

            question.Status = nameof(EStatus.Delete).ToString();

            if (question.Answers != null)
            {
                foreach (var answer in question.Answers)
                {
                    answer.Status = nameof(EStatus.Delete).ToString();
                }
                _answerRepository.UpdateRange(question.Answers);
            }

            foreach (var childQuestion in listChildQuestion)
            {
                childQuestion.Status = nameof(EStatus.Delete).ToString();
                if (childQuestion.Answers != null)
                {
                    foreach (var answer in childQuestion.Answers)
                    {
                        answer.Status = nameof(EStatus.Delete).ToString();
                    }
                    _answerRepository.UpdateRange(childQuestion.Answers);
                }
            }
            _questionRepository.Update(question);
            _questionRepository.UpdateRange(listChildQuestion);

            await _questionRepository.SaveDbSetAsync();
            await _answerRepository.SaveDbSetAsync();
            question.Answers = null!;

            await SendAsync(new BaseResponse<Question>
            {
                Success = true,
                Message = _localizedMessage["Delete"].Value,
                Data = question
            }, cancellation: ct);
        }

    }
}