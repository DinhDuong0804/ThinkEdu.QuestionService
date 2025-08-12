using FastEndpoints;
using FluentValidation;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Commands.CreateQuestionCommands
{
    public class CreateQuestionCommandValidator : Validator<QuestionModel>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .WithMessage("title@{PropertyName} is required.");

            RuleFor(t => t.Type)
                .NotEmpty()
                .WithMessage("type@{PropertyName} is required.")
                .Must(value => Enum.IsDefined(typeof(EQuestionType), value!))
                .WithMessage("type@{PropertyName} is not invalid");

            RuleFor(t => t.Group)
                .NotEmpty()
                .WithMessage("group@{PropertyName} is required.")
                .Must(value => Enum.IsDefined(typeof(EQuestionGroup), value!))
                .WithMessage("group@{PropertyName} is not invalid");

            RuleFor(t => t.Level)
                .NotEmpty()
                .WithMessage("level@{PropertyName} is required.")
                .Must(value => Enum.IsDefined(typeof(EQuestionLevel), value!))
                .WithMessage("level@{PropertyName} is not invalid");

            RuleFor(t => t.Status)
                .NotEmpty()
                .WithMessage("status@{PropertyName} is required.")
                .Must(value => Enum.IsDefined(typeof(EStatus), value!))
                .WithMessage("status@{PropertyName} is not invalid");

            RuleFor(t => t)
                .Custom((model, context) =>
                {
                    if (model.Type == nameof(EQuestionType.SingleChoice))
                    {
                        if (model.Answers == null || model.Answers.Count < 2)
                        {
                            context.AddFailure("answers", "answers@Answers must have at least 2 items for SingleMutliple type.");
                        }
                        else
                        {
                            var correctCount = model.Answers.Count(a => a.IsCorrect);
                            if (correctCount != 1)
                            {
                                context.AddFailure("answers", "answers@Answers must have exactly 1 correct answer for SingleMutliple type.");
                            }
                        }
                    }
                });
        }
    }
}