using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Reflection;

namespace ReslifeFiveFrontEnd.Application.Services.CsvService
{
    public class CsvService : ICsvService
    {
        private readonly ILogger<CsvService> _logger;
        public CsvService(ILogger<CsvService> logger)
        {
            _logger = logger;
        }


        public async Task<List<T>> ParseCsvAsync<T>(Stream csvStream) where T : class
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);


            try
            {
                var records = new List<T>();

                await foreach (var record in csv.GetRecordsAsync<T>())
                {
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error parsing csv {ex.Message}");
                throw;
            }


        }

        public async Task<List<T>> ParseCsvWithNullColsAsync<T>(Stream fileStream) where T : class
        {
            var headerStream = new MemoryStream();
            var csvStream = new MemoryStream();
            await fileStream.CopyToAsync(csvStream);
            csvStream.Position = 0;
            await csvStream.CopyToAsync(headerStream);
            csvStream.Position = 0;
            headerStream.Position = 0;    //creates two duplicate memorystreams: one for retrieving existing headers, one for reading the csv


            // Step 2: Use the copy for reading headers
            using var reader = new StreamReader(headerStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var headersLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(headersLine))
            {
                throw new NullReferenceException($"Error parsing a CSV of type {typeof(T)}: no headers found.");
            }
            //reads the csv
            var headers = headersLine
                .Split(',', StringSplitOptions.TrimEntries) // Split and trim whitespace
                .Where(header => !string.IsNullOrWhiteSpace(header)) // Filter out empty or whitespace headers
                .ToArray(); //finds the headers
            var dynamicMap = new DynamicClassMap<T>(headers); //creates a custom class map that ignores missing headers
            
          

            var records = new List<T>();
            using var recordReader = new StreamReader(csvStream);
            using var recordCsv = new CsvReader(recordReader, CultureInfo.InvariantCulture);
            recordCsv.Context.RegisterClassMap(dynamicMap);
            await foreach (var record in recordCsv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }
            //read the csv and put the items into a list to return
            return records;
        }









    }
}
