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
    
    public void SetProperty(string propertyName, IEnumerable<Crew> value)
    {
        PropertyInfo property = GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite && property.PropertyType == typeof(IEnumerable<Crew>))
        {
            property.SetValue(this, value);
        }
        else
        {
            throw new ArgumentException($"Property '{propertyName}' not found or not writable");
        }
    }
}
