using System.Reflection;

namespace ThinkEdu_Question_Service.Application.Common.Interfaces.Services
{
    public interface IDataSourceService
    {
        string? GetTextNameFromPropertyInfo(PropertyInfo profile);
    }
}