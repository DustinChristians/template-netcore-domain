using System.Linq;

namespace CompanyName.ProjectName.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool AllPropertiesAreNull<T>(this T obj)
        {
            return obj == null || typeof(T).GetProperties().All(propertyInfo => propertyInfo.GetValue(obj) == null);
        }
    }
}
