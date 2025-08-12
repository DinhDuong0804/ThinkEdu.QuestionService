using Mapster;
using System.Reflection;

namespace ThinkEdu_Question_Service.Application.Mappings
{
    public class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            var mapFromType = typeof(IMapFrom<>);

            var types = Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == mapFromType))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod(nameof(IMapFrom<object>.ConfigureMapping));

                if (methodInfo != null && instance != null)
                {
                    methodInfo.Invoke(instance, [config]);
                }
            }
        }
    }
}