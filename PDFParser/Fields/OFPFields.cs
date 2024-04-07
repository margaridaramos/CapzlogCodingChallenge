    public partial class PDFParser
    {
        public static List<Field> OFPFields = new List<Field>
        {
            new Field("Date", @"\b(\d{2}[A-Z]{3}\d{2})\b", true),
            new Field("AircraftRegistration", @"Reg\.: \s*([A-Z0-9]+)", true),
            new Field("From", @"From: \s*([A-Z]+)"),
            new Field("To", @"To: \s*([A-Z]+)"),
            new Field("AlternateAirdrome1", @"ALTN1: \s*([A-Z]{4})\b(?=.*\bCTOT:)"),
            new Field("AlternateAirdrome2", @"ALTN2: \s*([A-Z]{4})\b(?=.*\bDelay:)"),
            new Field("FlightNumber", @"FltNr: \s*([A-Z0-9]+)", true),
            new Field("DepartureTime", @"STD:\s*(\d{2}:\d{2})\b(?=.*\bSTA:)"),
            new Field("ArrivalTime", @"STA:\s*(\d{2}:\d{2})\b(?=.*\bSTE:)"),
            new Field("ZeroFuelMass", @"ZFM: \s*(\d+)\b(?=.*\bkg)"),
            new Field("TimeToDestination", @"Taxi:[\s\S]*?(\d:\d{2})[\s\S]*?CF 5%"),
            new Field("FuelToDestination", @"Taxi:[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?CF 5%"),
            new Field("TimeToAlternate", @"CF 5%:[\s\S]*?\d+:\d+[\s\S]*?\d\.\d+[\s\S]*?(\d+:\d+)[\s\S]*?ADD:"),
            new Field("FuelToAlternate", @"CF 5%:[\s\S]*?\d:\d{2}[\s\S]*?\d\.\d{1}[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?ADD:"),
            new Field("MinimumFuelRequired", @"MIN:[\s\S]*?\d:\d{2}[\s\S]*?(\d\.\d{1})[\s\S]*?DF"),
            new Field("RouteFirstNavigationPoint", @"To DEST:[\s]*([A-Z0-9]*)[\s\S]*?To ALTN1:"),
            new Field("RouteLastNavigationPoint", @"To DEST:[\s]*[A-Z0-9]*[\s\S]*?([A-Z0-9]*)[\s]*To ALTN1:"),
            new Field("GainLoss", @"Gain \/ Loss:\s*(\w+\s*\d+)"),
            new Field("ATC", @"ATC: \s*([A-Z0-9]+)", true)
        };
    }


public class OFPFlightData : DynamicField
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
