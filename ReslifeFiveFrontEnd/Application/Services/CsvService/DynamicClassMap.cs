using CsvHelper.Configuration;
using System.Reflection;
namespace ReslifeFiveFrontEnd.Application.Services.CsvService
{
    public sealed class DynamicClassMap<T> : ClassMap<T>
    {

        public DynamicClassMap(IEnumerable<string> headers)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var property in properties)
            {
                if(headers.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    Map(typeof(T), property);
                    Console.WriteLine($"found property {property.Name}, and decided to map it");
                }
                else
                {
                    Map(typeof(T), property).Ignore();
                    Console.WriteLine($"found property {property.Name}, and decided to ignore it");
                }
            }
        }
    }
}
