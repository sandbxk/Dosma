using System.Reflection;

namespace Backend.Helpers
{
    public static class ReflectionHelpers
    {
        public static bool FieldEquals(object t, string fieldName, object value)
        {
            PropertyInfo? propertyInfo = t.GetType().GetProperty(fieldName);

            if (propertyInfo != null)
            {
                return Object.Equals(propertyInfo.GetValue(t), value);
            }

            return false;
        }
    }
} 
  
