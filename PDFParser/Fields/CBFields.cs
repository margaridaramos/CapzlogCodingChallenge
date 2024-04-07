
    public partial class PDFParser
    {
        public static List<Field> CBFields = new List<Field>
        {
            new Field("FlightInfo", @"Crew\n([\s\S]*)DEP ARR", true),
            new Field("NumberOfPassengersInBusiness", @"Fuel[\s\S]*(\d+)\/\d+[\s\S]*Scheduled"),
            new Field("NumberOfPassengersInEconomy", @"Fuel[\s\S]*\d+\/(\d+)[\s\S]*Scheduled"),
            new Field("DryOperatingWeight", @"EZFW:\n(\d+)kg"),
            new Field("DryOperatingIndex", @"EZFW:\n\d+kg\n([\d+.\d+]*0*)\n"),
            new Field("CrewAndFunctions", @"[A-Z]{4}-[A-Z]{4}\n([[A-Z]{3}[\w\W]*]*)\nFunction Name")
        };
    }


public class CBFlightData : DynamicField
{
    public FlightInfo? FlightInfo { get; set; }
    public string? NumberOfPassengersInBusiness { get; set; }
    public string? NumberOfPassengersInEconomy { get; set; }
    public string? DryOperatingWeight { get; set; }
    public string? DryOperatingIndex { get; set; }
    public IEnumerable<Crew>? CrewAndFunctions { get; set; }
}
