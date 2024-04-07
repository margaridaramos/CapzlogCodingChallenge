public class FlightInfo
{
    public string Date { get; set; }
    public string FlightNumber { get; set; }
    public string ATC { get; set; }
    public string AircraftRegistration { get; set; }

    public FlightInfo(string date, string flightNumber, string atc, string aircraftRegistration )
    {
        Date = date;
        FlightNumber = flightNumber;
        ATC = atc;
        AircraftRegistration = aircraftRegistration;
    }
}
