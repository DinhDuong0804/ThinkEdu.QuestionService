using FastEndpoints;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ThinkEdu_Question_Service.Application.Common;
using ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions;
using ThinkEdu_Question_Service.Application.Exceptions;
using ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetInfoCreateQuestion;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Commands.UpdateQuestionCommands
{
    public class UpdateQuestionCommand : Endpoint<QuestionModel, BaseResponse<QuestionModel>>
    {
        private readonly ILogger<GetInfoCreateQuestionQuery> _logger;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;
        private readonly IQuestionRepository _questionRepository;
        private readonly IBaseRepository<Answer> _answerRepository;

        public UpdateQuestionCommand(
            ILogger<GetInfoCreateQuestionQuery> logger,
            IStringLocalizer<LocalizedMessage> localizedMessage,
            IQuestionRepository questionRepository,
            IBaseRepository<Answer> answerRepository)
        {
            _logger = logger;
            _localizedMessage = localizedMessage;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public override void Configure()
        {
            Put("Question/{id}");
            AllowAnonymous();
            DontThrowIfValidationFails();
            Summary(summary =>
            {
                summary.Summary = "cập nhật câu hỏi ";
            });
        }

        public override async Task HandleAsync(QuestionModel req, CancellationToken ct)
        {
            var id = Route<string>("id");

            var response = new BaseResponse<QuestionModel>();
            response.Success = false;

            _logger.LogInformation("UpdateQuestionCommandHandler@Handle --- Start : {0}", id);

            if (req.Id != id)
            {
                _logger.LogInformation("UpdateQuestionCommandHandler@Handle --- End : {0}", id);

                throw new BadRequestException(string.Format(_localizedMessage["NotFound"].Value, id));
            }

            var (question, listChildQuestion) = await _questionRepository.GetQuestionByIdAsync(id);

            if (question is null)
            {
                _logger.LogInformation("UpdateQuestionCommandHandler@Handle --- End : {0}", id);

                throw new BadRequestException(string.Format(_localizedMessage["NotFound"].Value, id));
            }

            if (ValidationFailed)
            {
                _logger.LogError("UpdateQuestionCommandHandler@Handle --- Validation Failed");
                throw new ValidationException(ValidationFailures);
            }

            // update Question status delete
            question.Status = nameof(EStatus.Delete).ToString();
            _questionRepository.Update(question);

            if (question.Answers is not null)
            {
                foreach (var answer in question.Answers)
                {
                    answer.Status = nameof(EStatus.Delete).ToString();
                }
                _answerRepository.UpdateRange(question.Answers);
            }

            if (listChildQuestion is not null)
            {
                foreach (var childQuestion in listChildQuestion)
                {
                    childQuestion.Status = nameof(EStatus.Delete).ToString();
                    if (childQuestion.Answers is not null)
                    {
                        foreach (var answer in childQuestion.Answers)
                        {
                            answer.Status = nameof(EStatus.Delete).ToString();
                        }
                        _answerRepository.UpdateRange(childQuestion.Answers);
                    }
                    _questionRepository.Update(childQuestion);

                }
            }

            await _answerRepository.SaveDbSetAsync(ct);
            await _questionRepository.SaveDbSetAsync(ct);

            // create question
            req.Id = "";
            req.Answers?.Select(a => a.Id = "").ToList();
            foreach (var child in req.ChildQuestions!)
            {
                child.Id = "";
                child.Answers?.Select(answer => answer.Id = "").ToList();
            }
         

            var newQuestion = req.Adapt<Question>();
            newQuestion.Id = Ulid.NewUlid().ToString();

            _questionRepository.Add(newQuestion);

            if (req.Answers is not null)
            {
                var answers = req.Answers.Adapt<List<Answer>>();
                answers.ForEach(x =>
                {
                    x.Id = Ulid.NewUlid().ToString();
                    x.QuestionId = newQuestion.Id;
                });
                await _answerRepository.AddRangeAsync(answers, ct);
            }
            var listChildQuestions = req.ChildQuestions;

            int sttCounter = 0;
            newQuestion.STT = sttCounter;

            foreach (var child in listChildQuestions)
            {
                child.Id = Ulid.NewUlid().ToString();
                var childQuestion = child.Adapt<Question>();
                childQuestion.ParentId = newQuestion.Id;
                childQuestion.STT = ++sttCounter;
                childQuestion.Status = nameof(EStatus.Active).ToString();

                if (child.Answers is not null)
                {
                    var childAnswers = child.Answers.Adapt<List<Answer>>();
                    childAnswers.ForEach(x =>
                    {
                        x.Id = Ulid.NewUlid().ToString();
                        x.QuestionId = childQuestion.Id;
                    });
                    await _answerRepository.AddRangeAsync(childAnswers, ct);
                }
                _questionRepository.Add(childQuestion);
            }

            await _questionRepository.SaveDbSetAsync(ct);
            await _answerRepository.SaveDbSetAsync(ct);

            response.Data = req;
            response.Success = true;
            response.Message = _localizedMessage["Update"].Value;
            _logger.LogInformation("UpdateQuestionCommandHandler@Handle --- End : {0}", id);

            await SendAsync(response, cancellation: ct);
        }
    }
}