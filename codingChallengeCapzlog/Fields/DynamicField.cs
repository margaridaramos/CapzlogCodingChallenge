using System.Reflection;

public class DynamicField
{
    public void SetProperty(string propertyName, string value)
    {
        PropertyInfo property = GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite && property.PropertyType == typeof(string))
        {
            property.SetValue(this, value);
        }
        else
        {
            throw new ArgumentException($"Property '{propertyName}' not found or not writable");
        }
    }
}
