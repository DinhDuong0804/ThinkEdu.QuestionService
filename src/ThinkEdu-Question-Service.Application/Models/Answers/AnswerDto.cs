using Mapster;
using System.ComponentModel.DataAnnotations;
using ThinkEdu_Question_Service.Domain.Common;
using ThinkEdu_Question_Service.Domain.Entities;
using ThinkEdu_Question_Service.Domain.Enums;

namespace ThinkEdu_Question_Service.Application.Models.Answers
{
    public class AnswerDto : BaseDto<string>, IMapFrom<Answer>
    {
      
        [Display(Name = "content")]
        public string? Content { get; set; } // NoiDung

        [Display(Name = "answer_file_url")]
        public string? AnswerFileUrl { get; set; }

        [Display(Name = "is_correct")]
        public bool IsCorrect { get; set; } // isDapAn

        [Display(Name = "status")]
        [Required(ErrorMessage = ("Required"))]
        public string Status { get; set; } = nameof(EStatus.Pending).ToString();

        public void ConfigureMapping(TypeAdapterConfig config)
        {
            config.NewConfig<AnswerDto, Answer>()
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.IsCorrect, src => src.IsCorrect)
                .Map(dest => dest.Status, src => src.Status)

                .AfterMapping((src, dest) => AfterMapping(src, dest))
                .TwoWays();
        }
        protected virtual void AfterMapping(AnswerDto source, Answer destination)
        {
           
        }
    }
}