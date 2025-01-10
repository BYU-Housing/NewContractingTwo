namespace ReslifeFiveFrontEnd.Application.Services.CsvService
{
    public interface ICsvService
    {
        Task<List<T>> ParseCsvAsync<T>(Stream csvStream) where T : class;

        Task<List<T>> ParseCsvWithNullColsAsync<T>(Stream fileStream) where T : class;

    }
}
