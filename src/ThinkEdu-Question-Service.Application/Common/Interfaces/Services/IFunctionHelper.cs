using System.Reflection;
using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;

namespace ThinkEdu_Question_Service.Application.Common.Interfaces.Services
{
    public interface IFunctionHelper
    {
        IEnumerable<PropertyInfo> GetFields<T>();

        void ValidateField(PropertyInfo propertyInfo, List<FieldValidationRuleDto> fieldValidateRules);
    }
}