namespace codingChallengeCapzlog
{
    public partial class Program
    {
        public static List<Field> OFPFields = new List<Field>
        {
            new Field { FieldName = "Date", MatchingRegex = @"\b(\d{2}[A-Z]{3}\d{2})\b" },
            new Field { FieldName = "AircraftRegistration", MatchingRegex = @"Reg\.: \s*([A-Z0-9]+)" },
            new Field { FieldName = "From", MatchingRegex = @"From: \s*([A-Z]+)" },
            new Field { FieldName = "To", MatchingRegex = @"To: \s*([A-Z]+)" },
            new Field { FieldName = "AlternateAirdrome1", MatchingRegex = @"ALTN1: \s*([A-Z]{4})\b(?=.*\bCTOT:)" },
            new Field { FieldName = "AlternateAirdrome2", MatchingRegex = @"ALTN2: \s*([A-Z]{4})\b(?=.*\bDelay:)" },
            new Field { FieldName = "FlightNumber", MatchingRegex = @"FltNr: \s*([A-Z0-9]+)" },
            new Field { FieldName = "DepartureTime", MatchingRegex = @"STD:\s*(\d{2}:\d{2})\b(?=.*\bSTA:)" },
            new Field { FieldName = "ArrivalTime", MatchingRegex = @"STA:\s*(\d{2}:\d{2})\b(?=.*\bSTE:)" },
            new Field { FieldName = "ZeroFuelMass", MatchingRegex = @"ZFM: \s*(\d+)\b(?=.*\bkg)" },
            new Field { FieldName = "TimeToDestination", MatchingRegex = @"Taxi:[\s\S]*?(\d:\d{2})[\s\S]*?CF 5%" },
            new Field { FieldName = "FuelToDestination", MatchingRegex = @"Taxi:[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?CF 5%" },
            new Field { FieldName = "TimeToAlternate", MatchingRegex = @"CF 5%:[\s\S]*?\d+:\d+[\s\S]*?\d\.\d+[\s\S]*?(\d+:\d+)[\s\S]*?ADD:" },
            new Field { FieldName = "FuelToAlternate", MatchingRegex = @"CF 5%:[\s\S]*?\d:\d{2}[\s\S]*?\d\.\d{1}[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?ADD:" },
            new Field { FieldName = "MinimumFuelRequired", MatchingRegex = @"MIN:[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?DF" },
            new Field { FieldName = "RouteFirstNavigationPoint", MatchingRegex = @"To DEST:[\s]*([A-Z0-9]*)[\s\S]*?To ALTN1:" },
            new Field { FieldName = "RouteLastNavigationPoint", MatchingRegex = @"To DEST:[\s]*[A-Z0-9]*[\s\S]*?([A-Z0-9]*)[\s]*To ALTN1:" },
            new Field { FieldName = "GainLoss", MatchingRegex = @"Gain \/ Loss:\s*(\w+\s*\d+)" },
            new Field { FieldName = "ATC", MatchingRegex = @"ATC: \s*([A-Z0-9]+)" }
        };
    }
}

public class OFPFlightData: DynamicField
{
    public string? Date { get; set; }
    public string? AircraftRegistration { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string? AlternateAirdrome1 { get; set; }
    public string? AlternateAirdrome2 { get; set; }
    public string? FlightNumber { get; set; }
    public string? DepartureTime { get; set; }
    public string? ArrivalTime { get; set; }
    public string? ZeroFuelMass { get; set; }
    public string? TimeToDestination { get; set; }
    public string? FuelToDestination { get; set; }
    public string? TimeToAlternate { get; set; }
    public string? FuelToAlternate { get; set; }
    public string? MinimumFuelRequired { get; set; }
    public string? RouteFirstNavigationPoint { get; set; }
    public string? RouteLastNavigationPoint { get; set; }
    public string? GainLoss { get; set; }
    public string? ATC { get; set; }
}
