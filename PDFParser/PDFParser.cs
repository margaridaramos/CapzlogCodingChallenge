using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Globalization;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;


public partial class PDFParser
{
    public static string ProcessFile(string[] args)
    {
        if (args.Length != 1)
        {
            return "Usage: dotnet run <path_to_pdf>";
        }

        string pdfPath = args[0];
        string OFP_IDENTIFIER = "E-Jet Operational Flight Plan";
        string CB_IDENTIFIER = "Observer and Jumpseat Assignments";

        try
        {
            Dictionary<string, Dictionary<string, object>> allFlightsData =
                new Dictionary<string, Dictionary<string, object>>();

            using (var pdfDocument = PdfDocument.Open(pdfPath))
            {
                int index = 1;
                foreach (var page in pdfDocument.GetPages())
                {
                    var pageContent = ContentOrderTextExtractor.GetText(page);
                    pageContent = pageContent.Replace("\r", ""); // On windows it adds a /r (carriage return) before any line break

                    if (pageContent.Contains(OFP_IDENTIFIER))
                    {
                        OFPFlightData flightData = new OFPFlightData();
                        foreach (Field field in OFPFields)
                        {
                            Match match = Regex.Match(pageContent, field.MatchingRegex);
                            if (match.Success)
                            {
                                if (field.FieldName == "GainLoss")
                                {
                                    flightData.SetProperty(field.FieldName, FormaGainLossData(match.Groups[1].Value));
                                }
                                else if (field.FieldName == "Date")
                                {
                                    string standardizedDate =
                                        DateTime.ParseExact(match.Groups[1].Value, "ddMMMyy", CultureInfo.InvariantCulture)
                                        .ToString("ddMMMyyyy")
                                        .ToUpper()
                                        .Replace(".",""); // On windows, it adds a . before the year
                                    flightData.SetProperty(field.FieldName, standardizedDate);
                                }
                                else
                                {
                                    flightData.SetProperty(field.FieldName, match.Groups[1].Value);
                                }
                            }
                            else
                            {
                                if (field.Required) {
                                    Console.WriteLine($"ERROR: Required {field.FieldName} was empty or missing in the OFP document (page {index} of the pdf).");
                                    return "";
                                }

                                Console.WriteLine($"Warning: {field.FieldName} was empty or missing in the OFP document (page {index} of the pdf).");
                                flightData.SetProperty(field.FieldName, "None");
                            }
                        }

                        if (flightData.Date == null)
                        {
                            break;
                        }

                        string uniqueIdentifier = (
                            flightData.Date
                            + flightData.FlightNumber
                            + flightData.ATC
                            + flightData.AircraftRegistration
                        ).ToUpper();

                        if (!allFlightsData.ContainsKey(uniqueIdentifier))
                        {
                            // If it doesn't exist, initialize it with an empty dictionary
                            allFlightsData[uniqueIdentifier] = new Dictionary<string, object>();
                        }

                        allFlightsData[uniqueIdentifier]["OFP"] = flightData;
                    }
                    if (pageContent.Contains(CB_IDENTIFIER))
                    {
                        CBFlightData flightData = new CBFlightData();
                        foreach (Field field in CBFields)
                        {
                            Match match = Regex.Match(pageContent, field.MatchingRegex);
                            if (match.Success)
                            {
                                if (field.FieldName == "CrewAndFunctions")
                                {
                                    flightData.SetProperty(field.FieldName, FormatCrewData(match.Groups[1].Value));
                                }
                                else if (field.FieldName == "FlightInfo")
                                {
                                    flightData.SetProperty(field.FieldName, FormatFlightInfoData(match.Groups[1].Value));
                                }
                                else
                                {
                                    flightData.SetProperty(field.FieldName, match.Groups[1].Value);
                                }
                            }
                            else
                            {
                                if (field.Required) {
                                    Console.WriteLine($"ERROR: Required {field.FieldName} was empty or missing in the CB document (page {index} of the pdf).");
                                    return "";
                                }

                                Console.WriteLine($"Warning: {field.FieldName} was empty or missing in the CB document (page {index} of the pdf).");
                                flightData.SetProperty(field.FieldName, "None");
                            }
                        }

                        if (flightData.FlightInfo != null)
                        {
                            string uniqueIdentifier =
                            (flightData.FlightInfo.Date
                            + flightData.FlightInfo.FlightNumber
                            + flightData.FlightInfo.ATC
                            + flightData.FlightInfo.AircraftRegistration)
                            .Replace(" ", "")
                            .ToUpper();

                            allFlightsData[uniqueIdentifier]["CB"] = flightData;
                        }
                    }

                    index++;
                }

                string extractedData = JsonConvert.SerializeObject(allFlightsData, Formatting.Indented);

                if (extractedData == "{}")
                {
                    Console.WriteLine("Error: No flight data was found in the file provided.");
                    return "";
                }

                return extractedData;
            }
        }
        catch (Exception ex)
        {
            return $"There was an error parsing your file: {ex.Message}";
        }
    }

    public static IEnumerable<Crew> FormatCrewData(string input)
    {
        string[] ROLES = {
                "Senior Cabin Attendant",
                "Cabin Attendant",
                "Commander",
                "Copilot"
            };

        string parsedInput = input
            .Replace("\n", " ")
            .Trim()
            .Split(' ')
            .Aggregate("", (acc, curr) =>
            {
                string lastWord = acc.Trim().Split(' ').LastOrDefault() ?? "";
                bool isCurrentACode = curr.Length == 3 && curr.ToUpper() == curr;
                bool isLastWordACode = lastWord.Length == 3 && lastWord.ToUpper() == lastWord;
                if (isLastWordACode && isCurrentACode)
                {
                    return acc;
                }
                return acc.Trim() + " " + curr.Trim();
            });

        string parsedInputWithoutRoles = parsedInput;
        foreach (string role in ROLES)
        {
            parsedInputWithoutRoles = parsedInputWithoutRoles.Replace(" " + role, "\n");
        }

        // transform the extracted crew string into a list of 'Crew' members
        IEnumerable<Crew> result = parsedInputWithoutRoles
            .Trim()
            .Split('\n')
            .Select(curr =>
            {
                string[] explodedStr = curr.Trim().Split(' ');
                string role = explodedStr[0];
                string name = string.Join(' ', explodedStr.Skip(1)).Trim();

                return new Crew(name, role);
            });

        return result;
    }

    public static string FormaGainLossData(string input)
    {
        string[] parts = input.Split(' ');
        string rawValue = parts[^1];
        return input.StartsWith("GAIN") ? "+" + rawValue : "-" + rawValue;
    }

    public static FlightInfo FormatFlightInfoData(string input)
    {
        string[] parts = input.Split('\n');
        string date = parts[0].Replace(".", "").ToUpper();
        string flightNumber = parts[1];
        string atc = parts[2];
        string aircraftRegistration = parts[3];

        return new FlightInfo(date, flightNumber, atc, aircraftRegistration);
    }
}