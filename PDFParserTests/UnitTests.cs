using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    private string GetTestFilePath(string fileName)
    {
        // Get the path to the test PDF file in the project directory
        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return Path.Combine(directory, "../../../TestFiles", fileName);
    }

    [Test]
    public void Main_Method_Shows_Usage_Message_When_No_Arguments_Provided()
    {
        string expectedMessage = "Usage: dotnet run <path_to_pdf>";

        string output = PDFParser.ProcessFile(new string[] { });

        Assert.That(output, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Main_Method_Shows_Expected_Output_When_Test_File_Is_Provided()
    {
        // Path to your JSON file
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

        // Act
        string result = PDFParser.ProcessFile(new string[] { pdfFilePath });

        JToken token1 = JToken.Parse(result);
        JToken token2 = JToken.Parse(expectedMessage);

        bool areEqual = JToken.DeepEquals(token1, token2);

        Assert.IsTrue(areEqual, "The flight data doesn't match the one provided in sampleFile.json.");
    }
}
