namespace ReslifeFiveFrontEnd.Application.Services
{
    public interface ICsvService
    {
        Task<List<T>> ParseCsvAsync<T>(Stream csvStream) where T : class;

        Task<(List<T> Records, Dictionary<string, bool> HasData)> ParseCsvWithNullColsAsync<T>(Stream csvStream) where T : class;

    }
}
