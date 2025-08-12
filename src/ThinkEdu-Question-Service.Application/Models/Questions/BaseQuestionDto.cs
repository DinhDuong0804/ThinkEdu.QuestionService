using Mapster;
using System.ComponentModel.DataAnnotations;
using ThinkEdu_Question_Service.Application.Models.Answers;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Models.Questions
{
    public abstract class BaseQuestionDto<T> : BaseDto<string>, IMapFrom<Question> where T : BaseQuestionDto<T>
    {
        [Display(Name = "lesson_id")]
        public string? LessonId { get; set; }

        [Display(Name = "title")]
        [Required(ErrorMessage = ("Required"))]
        public string Title { get; set; } = null!; // TieuDe

        [Display(Name = "type")]
        [Required(ErrorMessage = ("Required"))]
        public string Type { get; set; } = null!;

        [Display(Name = "level")]
        [Required(ErrorMessage = ("Required"))]
        public string Level { get; set; } = null!;

        [Display(Name = "group")]
        [Required(ErrorMessage = ("Required"))]
        public string Group { get; set; } = null!;

        [Display(Name = "file_url")]
        public string? FileUrl { get; set; }

        [Display(Name = "explanation")]
        public string? Explanation { get; set; } // TraLoi

        [Display(Name = "status")]
        [Required(ErrorMessage = ("Required"))]
        public string Status { get; set; } = nameof(EStatus.Pending).ToString();

        public List<AnswerDto>? Answers { get; set; } = [];

        public void ConfigureMapping(TypeAdapterConfig config)
        {
            config.NewConfig<T, Question>()
                .Map(dest => dest.LessonId, src => src.LessonId)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.Level, src => src.Level)
                .Map(dest => dest.Group, src => src.Group)
                .Map(dest => dest.FileUrl, src => src.FileUrl)
                .Map(dest => dest.Explanation, src => src.Explanation)
                .AfterMapping(AfterMapping);
        }
        protected virtual void AfterMapping(T src, Question dest)
        {
        }
    }
}
