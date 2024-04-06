using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Globalization;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace codingChallengeCapzlog
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            string result = ProcessFile(args);
            Console.WriteLine(result);
        }

        public static string ProcessFile(string[] args)
        {
            if (args.Length != 1)
            {
                return "Usage: codingChallengeCapzlog <path_to_pdf>";
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

                    foreach (var page in pdfDocument.GetPages())
                    {
                        var pageContent = ContentOrderTextExtractor.GetText(page);

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
                                    else
                                    {
                                        flightData.SetProperty(field.FieldName, match.Groups[1].Value);
                                    }
                                }
                                else
                                {
                                    flightData.SetProperty(field.FieldName, "None");
                                }
                            }

                            if (flightData.Date == null)
                            {
                                break;
                            }

                            string standardizedDate =
                                DateTime.ParseExact(flightData.Date, "ddMMMyy", CultureInfo.InvariantCulture)
                                .ToString("ddMMMyyyy").ToUpper();

                            string uniqueIdentifier = (
                                standardizedDate
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
                                    else
                                    {
                                        flightData.SetProperty(field.FieldName, match.Groups[1].Value);
                                    }
                                }
                                else
                                {
                                    flightData.SetProperty(field.FieldName, "None");
                                }
                            }

                            if (flightData.FlightInfo != null)
                            {
                                string uniqueIdentifier = flightData.FlightInfo
                                .Replace(" ", "")
                                .Replace("\n", "")
                                .Replace(".", "")
                                .ToUpper();

                                allFlightsData[uniqueIdentifier]["CB"] = flightData;
                            }
                        }
                    }

                    return JsonConvert.SerializeObject(allFlightsData, Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        static IEnumerable<Crew> FormatCrewData(string input)
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

        static string FormaGainLossData(string input)
        {
            string[] parts = input.Split(' ');
            string rawValue = parts[^1]; // Accessing last element of array using index
            return input.StartsWith("GAIN") ? "+" + rawValue : "-" + rawValue;
        }
    }
}
