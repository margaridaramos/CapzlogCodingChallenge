using System.Reflection;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            string? fileName = "";
            
            if (args.Length > 0)
            {
                fileName = string.Join("", args[0]);
            }
            else
            {
                Console.WriteLine("Please enter the PDF file name:");
                fileName = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("Please enter a valid PDF file name.");
                return;
            }

            string? directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (directory == null)
            {
                Console.WriteLine("Unable to determine directory.");
                return;
            }

            string pathToFile = Path.Combine(directory, "../../../", fileName);

            string result = PDFParser.ProcessFile(pathToFile.Split(""));
            Console.WriteLine(result);

        }
    }
}
