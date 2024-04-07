using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PDFParserTests
{
    [SetUp]
    public void Setup()
    {
    }

    private string GetTestFilePath(string fileName)
    {
        // Get the path to the test PDF file in the project directory
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return Path.Combine(directory, "../../../Tests/TestFiles", fileName);
    }

    [Test]
    public void Main_Method_Shows_Usage_Message_When_No_Arguments_Provided()
    {
        string expectedMessage = "Usage: dotnet run <path_to_pdf>";

        string output = PDFParser.ProcessFile(new string[] { });

        Assert.That(output, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void FormaGainLossData_CorrectlyFormatsPositiveGainLossValues()
    {
        string expectedMessage = "+50";

        string output = PDFParser.FormaGainLossData("GAIN 50");

        Assert.That(output, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void FormaGainLossData_CorrectlyFormatsNegativeGainLossValues()
    {
        string expectedMessage = "-50";

        string output = PDFParser.FormaGainLossData("LOSS 50");

        Assert.That(output, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void FormaCrewData_CorrectlyParsesExtractedCrewDataStrings()
    {
        string rawCrewData = "CMD TRW Werner Trütsch Commander\nCOP MRL Luca Andrea Marchetti Copilot\nCAB MHE Helen Meier Cabin\nAttendant\nCAB RCA Ena Ramic Cabin\nAttendant\nSEN VEN Nico Verhelst Senior Cabin\nAttendant";
        
        var expectedCrewArray = new[]
        {
            new Crew("Werner Trütsch","CMD"),
            new Crew("Luca Andrea Marchetti","COP"),
            new Crew("Helen Meier","CAB"),
            new Crew("Ena Ramic","CAB"),
            new Crew("Nico Verhelst","SEN")
        };

        string expectedMessage = JsonConvert.SerializeObject(expectedCrewArray);
        string output = JsonConvert.SerializeObject(PDFParser.FormatCrewData(rawCrewData));

        JToken token1 = JToken.Parse(output);
        JToken token2 = JToken.Parse(expectedMessage);

        bool areEqual = JToken.DeepEquals(token1, token2);

        Assert.IsTrue(areEqual, "There Crew data doesn't match");
    }

    [Test]
    public void FormaFlightInfoData_CorrectlyParsesExtractedFlightInfoDataStrings()
    {
        string rawFlightInfoData = "19.Mar.2024\nLX1073\nSWR890M\nHBJVN\n";
        
        string expectedMessage = JsonConvert.SerializeObject(new FlightInfo("19MAR2024", "LX1073", "SWR890M", "HBJVN"));
        string output = JsonConvert.SerializeObject(PDFParser.FormatFlightInfoData(rawFlightInfoData));

        JToken token1 = JToken.Parse(output);
        JToken token2 = JToken.Parse(expectedMessage);

        bool areEqual = JToken.DeepEquals(token1, token2);

        Assert.IsTrue(areEqual, "There Flight Info data doesn't match");
    }


    [Test]
    public void Main_Method_Shows_Expected_Output_When_Test_File_Is_Provided()
    {
        string expectedMessage = "";

        try
        {
            expectedMessage = File.ReadAllText(GetTestFilePath("sampleFile.json"));
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while reading the file: {e.Message}");
        }

        string pdfFilePath = GetTestFilePath("sampleFile.pdf");

        string result = PDFParser.ProcessFile(new string[] { pdfFilePath });

        JToken token1 = JToken.Parse(result);
        JToken token2 = JToken.Parse(expectedMessage);

        bool areEqual = JToken.DeepEquals(token1, token2);

        Assert.IsTrue(areEqual, "The flight data doesn't match the one provided in sampleFile.json.");
    }
}
