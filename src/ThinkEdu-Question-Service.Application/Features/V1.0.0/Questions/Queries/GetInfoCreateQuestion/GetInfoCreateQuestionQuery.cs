

using FastEndpoints;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetInfoCreateQuestion
{
    public class GetInfoCreateQuestionQuery : EndpointWithoutRequest<BaseResponse<GetCreateInfoQuestionDto>>
    {
        private readonly ILogger<GetInfoCreateQuestionQuery> _logger;
        private readonly IStringLocalizer<LocalizedModel> _localizedModel;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;
        private readonly IFunctionHelper _functionHelper;
        private readonly IDataSourceService _dataSourceService;

        public GetInfoCreateQuestionQuery(
            ILogger<GetInfoCreateQuestionQuery> logger,
            IStringLocalizer<LocalizedModel> localizedModel,
            IStringLocalizer<LocalizedMessage> localizedMessage,
            IFunctionHelper functionHelper,
            IDataSourceService dataSourceService)
        {
            _logger = logger;
            _localizedModel = localizedModel;
            _localizedMessage = localizedMessage;
            _functionHelper = functionHelper;
            _dataSourceService = dataSourceService;
        }

        public override void Configure()
        {
            Get("Question/Create");
            AllowAnonymous();
            DontThrowIfValidationFails();
            Summary(summary =>
            {
                summary.Summary = "thông tin cần thiết để tạo câu hỏi ";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new BaseResponse<GetCreateInfoQuestionDto>();
         
            _logger.LogInformation("GetInfoCreateQuestionQuery@Handle --Start");
            response.Data = new GetCreateInfoQuestionDto
            {
                EnumOption = GetPropertiesEnumOption(),
                FieldValidates = GetValidateFields()
       
            };
            response.Success = true;
            await SendAsync(response, cancellation: ct);

            _logger.LogInformation("GetInfoCreateQuestionQuery@Handle --End");
        }

        private List<FieldValidationConfigDto> GetValidateFields()
        {
            var properties = typeof(QuestionModel).GetProperties();
            var fieldValidateConfigurations = new List<FieldValidationConfigDto>();
            foreach (var item in properties)
            {
                var fieldName = string.Concat(item.Name[0].ToString().ToUpper(), item.Name.Substring(1));
                var fieldValidateRules = new List<FieldValidationRuleDto>();
                var disPlayName = item.GetCustomAttributes<DisplayAttribute>().LastOrDefault()?.Name;

                switch (item.Name)
                {
                    case nameof(QuestionModel.Title):
                    case nameof(QuestionModel.Type):
                    case nameof(QuestionModel.Level):
                    case nameof(QuestionModel.Group):
                    case nameof(QuestionModel.Status):
                    case nameof(QuestionModel.LessonId):
                        fieldValidateRules.Add(new FieldValidationRuleDto
                        {
                            RuleName = nameof(ETypeRule.Type),
                            RuleValue = nameof(ETypeProperty.String),
                            Message = string.Format(
                                _localizedMessage["InvalidType"],
                                _localizedModel[$"{disPlayName}"],
                                nameof(ETypeProperty.String))
                        });
                        _functionHelper.ValidateField(item, fieldValidateRules);
                        fieldValidateConfigurations.Add(new FieldValidationConfigDto()
                        {
                            FieldName = fieldName,
                            Rules = fieldValidateRules
                        });
                        break;

                    case nameof(QuestionModel.Answers):
                    //foreach () 
                    case nameof(QuestionModel.ChildQuestions):
                        fieldValidateRules.Add(new FieldValidationRuleDto
                        {
                            RuleName = nameof(ETypeRule.Type),
                            RuleValue = nameof(ETypeProperty.Collection),
                            Message = string.Format(
                                _localizedMessage["InvalidType"],
                                _localizedModel[$"{disPlayName}"],
                                nameof(ETypeProperty.Collection))
                        });
                        _functionHelper.ValidateField(item, fieldValidateRules);
                        fieldValidateConfigurations.Add(new FieldValidationConfigDto()
                        {
                            FieldName = fieldName,
                            Rules = fieldValidateRules
                        });
                        break;

                    default:
                        break;
                }
            }
            return fieldValidateConfigurations;
        }

        private List<ValidationEnumOptionDto> GetPropertiesEnumOption()
        {
            var filter = new List<ValidationEnumOptionDto>
            {
                new ValidationEnumOptionDto
                {
                    ValueFieldName = "Type",
                    LabelFieldName = _localizedModel["type"].Value,
                    Options = Enum.GetValues<EQuestionType>()
                    .Select(x => new EnumOptionSourceDto
                    {
                        Value = x.ToString(),
                        Label = _localizedModel[x.ToString()].Value
                    }).ToList()
                },
                new ValidationEnumOptionDto
                {
                    ValueFieldName = "Level",
                    LabelFieldName = _localizedModel["level"].Value,
                    Options = Enum.GetValues<EQuestionLevel>()
                    .Select(x => new EnumOptionSourceDto
                    {
                        Value = x.ToString(),
                        Label = _localizedModel[x.ToString()].Value
                    }).ToList()
                },
                new ValidationEnumOptionDto
                {
                    ValueFieldName = "Group",
                    LabelFieldName = _localizedModel["group"].Value,
                    Options = Enum.GetValues<EQuestionGroup>()
                    .Select(x => new EnumOptionSourceDto
                    {
                        Value = x.ToString(),
                        Label = _localizedModel[x.ToString()].Value
                    }).ToList()
                },
                new ValidationEnumOptionDto
                {
                    ValueFieldName = "Status",
                    LabelFieldName = _localizedModel["status"].Value,
                    Options = Enum.GetValues<EStatus>()
                    .Where(x => x != EStatus.Delete)
                    .Select(x => new EnumOptionSourceDto
                    {
                        Value = x.ToString(),
                        Label = _localizedModel[x.ToString()].Value
                    }).ToList()
                }
            };

            return filter;
        }
    }
}
