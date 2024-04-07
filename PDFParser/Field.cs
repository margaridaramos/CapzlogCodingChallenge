public class Field
{
    public string FieldName { get; set; }
    public string MatchingRegex { get; set; }

    public Field(string fieldName, string matchingRegex)
    {
        FieldName = fieldName;
        MatchingRegex = matchingRegex;
    }
}
