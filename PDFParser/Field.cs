public class Field
{
    public string FieldName { get; set; }
    public string MatchingRegex { get; set; }
    public bool Required { get; set; }

    public Field(string fieldName, string matchingRegex, bool required = false)
    {
        FieldName = fieldName;
        MatchingRegex = matchingRegex;
        Required = required;
    }
}
