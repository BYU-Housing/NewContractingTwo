namespace ReslifeFiveFrontEnd.Application.Services
{
    public interface ICsvService
    {
        Task<List<T>> ParseCsvAsync<T>(Stream csvStream) where T : class;
    }
}
