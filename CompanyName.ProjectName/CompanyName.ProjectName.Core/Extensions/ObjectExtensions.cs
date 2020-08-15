using System.Linq;

namespace CompanyName.ProjectName.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool AllPropertiesAreNull<T>(this T obj)
        {
            return obj == null || typeof(T).GetProperties().All(propertyInfo => propertyInfo.GetValue(obj) == null);
        }

        public static T AllStringsToLower<T>(this T obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(obj, null)?
                        .ToString()
                        .ToLower();

                    property.SetValue(obj, value);
                }
            }

            return obj;
        }
    }
}
