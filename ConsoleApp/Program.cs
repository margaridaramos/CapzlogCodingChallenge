namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = PDFParser.ProcessFile(args);
            Console.WriteLine(result);
        }
    }
}
