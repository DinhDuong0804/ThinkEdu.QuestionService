using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ThinkEdu_Question_Service.Application.Models.FrontendValidationRules;
using ThinkEdu_Question_Service.Application.Resources;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;

namespace ThinkEdu_Question_Service.Infrastructure.Services
{
    public class FunctionHelper : IFunctionHelper
    {
        private readonly IStringLocalizer<LocalizedModel> _localizedModel;
        private readonly IStringLocalizer<LocalizedMessage> _localizedMessage;

        public FunctionHelper(
            IStringLocalizer<LocalizedModel> localizedModel,
            IStringLocalizer<LocalizedMessage> localizedMessage)
        {
            _localizedModel = localizedModel;
            _localizedMessage = localizedMessage;
        }

        public IEnumerable<PropertyInfo> GetFields<T>()
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var props =
                from p in propertyInfos
                group p by p.Name into g
                select g.OrderByDescending(t => t.DeclaringType == typeof(T)).First();
            return props.ToList();
        }

        public void ValidateField(PropertyInfo propertyInfo, List<FieldValidationRuleDto> fieldValidateRules)
        {
            var displayName = propertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name;
            // Bỏ DisplayAttribute
            var attributes = propertyInfo
                .GetCustomAttributes()
                .Where(x => !(x is DisplayAttribute)
                            && !(x.GetType().Name == "NullableAttribute"))
                .ToList();
            foreach (var attribute in attributes)
            {
                // Lấy tên Attribute, bỏ chữ "Attribute"
                var ruleName = attribute.GetType().Name.Replace("Attribute", "");
                string? errorMessage = (string?)attribute.GetType().GetProperty("ErrorMessage")?.GetValue(attribute, null);
                // Lấy giá trị các thuộc tính của Attribute khi khớp với ruleName 
                var value = GetAttributeValue(attribute, ruleName);
                // Thêm vào danh sách validate
                AddValidateRule(fieldValidateRules, ruleName, value, errorMessage, displayName, value);
            }
        }

        private string GetAttributeValue(object attribute, string ruleName)
        {
            var propertys = attribute.GetType().GetProperties();
            var property = attribute.GetType().GetProperties()
                                    .FirstOrDefault(p => p.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase));
            return property?.GetValue(attribute)?.ToString() ?? "true";
        }

        private void AddValidateRule(
            List<FieldValidationRuleDto> fieldValidateRules,
            string ruleName,
            string ruleValue,
            string? message,
            string? displayName,
            string? value = null)
        {
            fieldValidateRules.Add(new FieldValidationRuleDto()
            {
                RuleName = ruleName,
                RuleValue = ruleValue,
                Message = string.Format(_localizedMessage[$"{message}"],
                _localizedModel[$"{displayName}"], value)
            });
        }
    }
}