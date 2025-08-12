using FastEndpoints;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions;
using ThinkEdu_Question_Service.Application.Models.Answers;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Domain.Common;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetDetailQuestion
{
    public class GetDetailQuestionQuery : EndpointWithoutRequest<BaseResponse<QuestionModel>>
    {
        private readonly ILogger<GetDetailQuestionQuery> _logger;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;

        public GetDetailQuestionQuery(
            ILogger<GetDetailQuestionQuery> logger,
            IQuestionRepository questionRepository,
            IStringLocalizer<LocalizedMessage> localizedMessage
            )
        {
            _logger = logger;
            _questionRepository = questionRepository;
            _localizedMessage = localizedMessage;

        }
        public override void Configure()
        {
            Get("Question/{id}");
            DontThrowIfValidationFails();
            AllowAnonymous();
            Summary(summary =>
            {
                summary.Summary = "xem chi tiết câu hỏi";
            });
        }

        public override async Task HandleAsync( CancellationToken ct)
        { 
            var id = Route<string>("id");

            var response = new BaseResponse<QuestionModel>();
            response.Success = false;

            _logger.LogInformation("GetDetailQuestionQuery@Handle --Start--{0}", id);

            var (question, listChildQuestion) = await _questionRepository.GetQuestionByIdAsync(id!);

            if (question is null && listChildQuestion.Count == 0)
            {
                throw new BadRequestException(_localizedMessage["NoDataFound"]);
            }

            if (question is not null)
            {
                var questionResponse = question.Adapt<QuestionModel>();
                questionResponse.Answers = question.Answers?
                       .Select(item => item.Adapt<AnswerDto>()).ToList() ?? [];

                if (listChildQuestion is not null && listChildQuestion.Count > 0)
                {
                    questionResponse.ChildQuestions = listChildQuestion
                        .Select(item =>
                        {
                            var childQuestionDto = item.Adapt<ChildQuestionDto>();
                            childQuestionDto.Answers = item.Answers?
                                .Select(answer => answer.Adapt<AnswerDto>()).ToList() ?? [];
                            return childQuestionDto;
                        }).ToList();
                }

                response.Success = true;
                response.Data = questionResponse;
                response.Message = _localizedMessage["Success"].Value;
                
                _logger.LogInformation("GetDetailQuestionQuery@Handle --End--{0}", id);

                await SendOkAsync(response, ct);
            }
        }

    }
}