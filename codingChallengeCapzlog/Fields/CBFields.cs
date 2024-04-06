namespace codingChallengeCapzlog
{
    public partial class Program
    {
        public static List<Field> CBFields = new List<Field>
        {
            new Field("FlightInfo", @"Crew\n([\s\S]*)DEP ARR"),
            new Field("NumberOfPassengersInBusiness", @"Fuel[\s\S]*(\d+)\/\d+[\s\S]*Scheduled"),
            new Field("NumberOfPassengersInEconomy", @"Fuel[\s\S]*\d+\/(\d+)[\s\S]*Scheduled"),
            new Field("DryOperatingWeight", @"EZFW:\n(\d+)kg"),
            new Field("DryOperatingIndex", @"EZFW:\n\d+kg\n(\d+.\d+)\n"),
            new Field("CrewAndFunctions", @"[A-Z]{4}-[A-Z]{4}\n([[A-Z]{3}[\w\W]*]*)\nFunction Name")
        };
    }
}

public class CBFlightData : DynamicField
{
    public string? FlightInfo { get; set; }
    public string? NumberOfPassengersInBusiness { get; set; }
    public string? NumberOfPassengersInEconomy { get; set; }
    public string? DryOperatingWeight { get; set; }
    public string? DryOperatingIndex { get; set; }
    public IEnumerable<Crew>? CrewAndFunctions { get; set; }
}
