using FastEndpoints;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Application.Common;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Enums;


namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Commands.CreateQuestionCommands
{
    public class CreateQuestionCommand : Endpoint<QuestionModel, BaseResponse<Question>>
    {
        private readonly ILogger<CreateQuestionCommand> _logger;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;
        private readonly IBaseRepository<Question> _questionRepository;
        private readonly IBaseRepository<Answer> _answerRepository;

        public CreateQuestionCommand(
            ILogger<CreateQuestionCommand> logger,
            IStringLocalizer<LocalizedMessage> localizedMessage,
            IBaseRepository<Question> questionRepository,
            IBaseRepository<Answer> answerRepository
            )
        {
            _logger = logger;
            _localizedMessage = localizedMessage;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;

        }

        public override void Configure()
        {
            Post("/Question");
            AllowAnonymous();
            Summary(summary =>
            {
                summary.Summary = "tạo câu hỏi mới ";
            });
        }

        public override async Task HandleAsync(QuestionModel req, CancellationToken ct)
        {
            var response = new BaseResponse<Question>();
            response.Success = false;
            _logger.LogInformation("CreateQuestionCommandHandle@Handle --Start");

            if (ValidationFailed)
            {
                _logger.LogError("CreateQuestionCommandHandle@Handle --Validation Failed");
                throw new ValidationException(ValidationFailures);
            }

            try
            {
                var question = req.Adapt<Question>();
                question.Id = Ulid.NewUlid().ToString();
                question.Status = EStatus.Pending.ToString();

                _questionRepository.Add(question);

                if (req.Answers is not null && req.Answers.Any())
                {
                    var answers = req.Answers.Adapt<List<Answer>>();
                    for (int i = 0; i < answers.Count; i++)
                    {
                        var answer = answers[i];
                      
                        answer.Id = Ulid.NewUlid().ToString();
                        answer.QuestionId = question.Id;
                        answer.Status = EStatus.Pending.ToString();

                    }
                    await _answerRepository.AddRangeAsync(answers, ct);
                }

                // 3. Xử lý child questions
                var listChildQuestion = req.ChildQuestions;
                if (listChildQuestion is not null && listChildQuestion.Any())
                {
                    int sttCounter = 0;
                    question.STT = sttCounter;

                    foreach (var childQuestionReq in listChildQuestion)
                    {
                        var childQuestion = childQuestionReq.Adapt<Question>();

                        childQuestion.Id = Ulid.NewUlid().ToString();
                        childQuestion.ParentId = question.Id;
                        childQuestion.STT = ++sttCounter;
                        childQuestion.Status = EStatus.Pending.ToString();

                        _questionRepository.Add(childQuestion);

                        // Xử lý child answers
                        if (childQuestionReq.Answers is not null && childQuestionReq.Answers.Any())
                        {
                            var childAnswers = childQuestionReq.Answers.Adapt<List<Answer>>();
                            for (int i = 0; i < childAnswers.Count; i++)
                            {
                                var childAnswer = childAnswers[i];

                                childAnswer.Id = Ulid.NewUlid().ToString();
                                childAnswer.QuestionId = childQuestion.Id;
                                childAnswer.Status = EStatus.Pending.ToString();

                            }
                            await _answerRepository.AddRangeAsync(childAnswers, ct);
                        }
                    }
                }

                await _questionRepository.SaveDbSetAsync(ct);  
                await _answerRepository.SaveDbSetAsync(ct);

                _logger.LogInformation("CreateQuestionCommandHandle@Handle --End");

                response.Success = true;
                response.Message = _localizedMessage["CreateQuestionSuccess"].Value;
                response.ResultCode = HttpStatusCode.Created;

                question.Answers = null!;
                response.Data = question;

                await SendCreatedAtAsync<CreateQuestionCommand>(responseBody: response, cancellation: ct);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateQuestionCommandHandle@Handle --Error: {ex.Message}");
                response.Success = false;
                response.Message = _localizedMessage["CreateQuestionFailed"].Value;
                response.ResultCode = HttpStatusCode.InternalServerError;
                if (!HttpContext.Response.HasStarted)
                {
                    await SendAsync(response);
                }
            }
        }
    }
}