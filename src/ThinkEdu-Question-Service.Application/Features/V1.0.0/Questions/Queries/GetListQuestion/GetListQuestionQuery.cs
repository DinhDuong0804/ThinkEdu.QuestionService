using FastEndpoints;
using Mapster;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Application.Contracts.IRepository.Questions;
using ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetDetailQuestion;
using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;
using ThinkEdu_Question_Service.Application.Models.Questions;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Features.V1._0._0.Questions.Queries.GetListQuestion
{
    public class GetListQuestionQuery : Endpoint<QuestionSearchModel, BaseResponse<GetListWithFilterQuestion>>
    {
        private readonly ILogger<GetDetailQuestionQuery> _logger;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;
        private readonly IStringLocalizer<LocalizedModel> _localizedModel;
        private readonly IDataSourceService _dataSourceService;
        public GetListQuestionQuery(
           ILogger<GetDetailQuestionQuery> logger,
           IQuestionRepository questionRepository,
           IStringLocalizer<LocalizedModel> localizedModel,
           IStringLocalizer<LocalizedMessage> localizedMessage,
           IDataSourceService dataSourceService
           )
        {
            _logger = logger;
            _questionRepository = questionRepository;
            _localizedMessage = localizedMessage;
            _localizedModel = localizedModel;
            _dataSourceService = dataSourceService;
        }
        public override void Configure()
        {
            Get("Question");
            AllowAnonymous();
            DontThrowIfValidationFails();
            Summary(summary =>
            {
                summary.Summary = "lấy danh sách câu hỏi theo lọc hoặc tìm kiếm";
            });
        }

        public override async Task HandleAsync(QuestionSearchModel req, CancellationToken ct)
        {
            var response = new BaseResponse<GetListWithFilterQuestion>();
            response.Success = false;

            _logger.LogInformation("GetListQuestionQuery@Handle --Start : {0}", JsonSerializer.Serialize(req));

            var (count, listQuestion) = await _questionRepository.GetListQuestionAsync(req);


            if (listQuestion is null || listQuestion.Count == 0)
            {
                response.Success = true;
                response.Data = new GetListWithFilterQuestion
                {
                    Filters = GetPropertiesFilter(),
                    DataTable = [],
                    Headers = GetPropertiesHeaderTable(),
                    Count = 0

                };
                _logger.LogInformation("GetListQuestionQuery@Handle --End-- No data found");
                response.Message = _localizedMessage["NoDataFound"].Value;
                await SendAsync(response, cancellation: ct);
                return;
            }

            var listQuestionDto = listQuestion
              .Select(item => item.Adapt<GetListQuesionRequest>())
              .ToList();

            response.Success = true;
            response.Data = new GetListWithFilterQuestion
            {
                Count = count,
                Headers = GetPropertiesHeaderTable(),
                DataTable = listQuestionDto,
                Filters = GetPropertiesFilter()
            };
            response.Message = _localizedMessage["DataRetrievedSuccessfully"].Value;
            _logger.LogInformation("GetListQuestionQuery@Handle --End--");
            await SendAsync(response, cancellation: ct);


        }
        private List<HeaderTableResponse> GetPropertiesHeaderTable()
        {
            var properties = typeof(GetListQuesionRequest).GetProperties()
             .Where(p => p.IsDefined(typeof(DisplayAttribute), false) &&
                               !p.IsDefined(typeof(JsonIgnoreAttribute), false))
                   .Select(p => new HeaderTableResponse
                   {
                       Text = _dataSourceService.GetTextNameFromPropertyInfo(p),
                       Value = string.Concat(p.Name[..1].ToLower(), p.Name.AsSpan(1)),
                   }).ToList();
            return properties;
        }

        private List<ValidationEnumOptionDto> GetPropertiesFilter()
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