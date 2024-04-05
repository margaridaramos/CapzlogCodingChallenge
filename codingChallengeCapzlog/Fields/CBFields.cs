namespace codingChallengeCapzlog
{
    public partial class Program
    {
        public static List<Field> CBFields = new List<Field>
        {
            new Field { FieldName = "FlightInfo", MatchingRegex = @"Crew\n([\s\S]*)DEP ARR" },
            new Field { FieldName = "NumberOfPassengersInBusiness", MatchingRegex = @"Fuel[\s\S]*(\d+)\/\d+[\s\S]*Scheduled" },
            new Field { FieldName = "NumberOfPassengersInEconomy", MatchingRegex = @"Fuel[\s\S]*\d+\/(\d+)[\s\S]*Scheduled" },
            new Field { FieldName = "DryOperatingWeight", MatchingRegex = @"EZFW:\n(\d+)kg" },
            new Field { FieldName = "DryOperatingIndex", MatchingRegex = @"EZFW:\n\d+kg\n(\d+.\d+)\n" },
            new Field { FieldName = "CrewAndFunctions", MatchingRegex = @"[A-Z]{4}-[A-Z]{4}\n([[A-Z]{3}[\w\W]*]*)\nFunction Name" }
        };
    }
}

public class CBFlightData: DynamicField
{
    public string? FlightInfo { get; set; }
    public string? NumberOfPassengersInBusiness { get; set; }
    public string? NumberOfPassengersInEconomy { get; set; }
    public string? DryOperatingWeight { get; set; }
    public string? DryOperatingIndex { get; set; }
    public string? CrewAndFunctions { get; set; }
}
