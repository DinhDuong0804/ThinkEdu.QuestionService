using System.Text.Json;

namespace ThinkEdu_Question_Service.Domain.Extensions
{
    public static class TryConvertExtension
    {
        public static T? TryDeserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }
    }
}