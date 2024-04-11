# CapzLog coding challenge

This .net console application takes in a flight log PDF and outputs a JSON string containing all the relevant extractable fields.

## Solution structure
- PDFParser: Module to parse and extract flight data from PDFs
- ConsoleApp: Sample .net console application to demonstrate the usage of PDFParser module

## Running the project

To run this project, follow these steps:

 1. clone this repository
 2. cd into the `ConsoleApp` directory
 3. run `dotnet run sampleFile.pdf`

 If a file name is not provided, the console App will request it from the user.

## How to run the tests

To run the tests follow these steps:

 1. cd into the `PDFParser` directory
 2. run `dotnet test`
 
## How to use the PDFParser in your project

You can use PDFParser in your project by following these steps:

1. Add a reference to PDFParser module to your project by modifying the .csproj file of your project and adding the reference to PDFParser module.
Inside `<ItemGroup>` add `<ProjectReference Include="PATH_TO_PDF_PARSER/PDFParser.csproj" />`
2. Now you can use PDFParser by calling the `ProcessFile` method: `PDFParser.ProcessFile(args);`

contact: margaridaramos.92@gmail.com
