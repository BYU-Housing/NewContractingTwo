using CsvHelper;
using System.Globalization;
using System.Reflection;

namespace ReslifeFiveFrontEnd.Application.Services
{
    public class CsvService: ICsvService
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

                await foreach(var record in csv.GetRecordsAsync<T>())
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


        public async Task<(List<T> Records, Dictionary<string, bool> HasData)> ParseCsvWithNullColsAsync<T>(Stream csvStream) where T : class
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.TypeConverterOptionsCache.GetOptions<int>().NullValues.Add(""); // Treat empty strings as null
            csv.Context.TypeConverterOptionsCache.GetOptions<bool>().NullValues.Add(""); // Treat empty strings as null
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().NullValues.Add(""); // Treat empty strings as null
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "M/d/yyyy", "yyyy-MM-dd" }; // Ensure the date format matches your CSV



            // Asynchronously parse the CSV into a list of objects of type T
            var records = new List<T>();
            var hasData = typeof(T).GetProperties()
                .ToDictionary(prop => prop.Name, _ => false);//create a dictionary with all false values



            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);

                // Check for meaningful data in each property
                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value = prop.GetValue(record);
                    if (value != null && !IsDefaultValue(value))
                    {
                        hasData[prop.Name] = true; // Mark column as having meaningful data
                    }
                }
            }



            return (records, hasData);
        }
        private bool IsDefaultValue(object value)
        {
            if (value is int intValue) return intValue == 0;
            if (value is bool boolValue) return !boolValue;
            if (value is DateTime dateTimeValue) return dateTimeValue == default;
            if (value is string stringValue) return string.IsNullOrWhiteSpace(stringValue);
            return false; // Assume null or default for other types
        }


    }
}
