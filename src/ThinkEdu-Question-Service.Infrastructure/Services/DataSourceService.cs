using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Application.Resources;

namespace ThinkEdu_Question_Service.Infrastructure.Services
{
    public class DataSourceService : IDataSourceService
    {
        private readonly IStringLocalizer<LocalizedModel> _localizedModel;

        public DataSourceService(IStringLocalizer<LocalizedModel> localizedModel)
        {
            _localizedModel = localizedModel;
        }

        public string? GetTextNameFromPropertyInfo(PropertyInfo profile)
        {
            var displayAttr = profile.GetCustomAttribute<DisplayAttribute>(false);
            var name = displayAttr?.Name;
            return name == null ? name : _localizedModel[name];
        }
    }
}