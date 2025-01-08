using CsvHelper;
using System.Globalization;

namespace ReslifeFiveFrontEnd.Application.Services
{
    public class CsvService: ICsvService
    {
        public async Task<List<T>> ParseCsvAsync<T>(Stream csvStream) where T : class
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // Asynchronously parse the CSV into a list of objects of type T
            var records = new List<T>();
            await foreach(var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }


            return records;
        }
    }
}
