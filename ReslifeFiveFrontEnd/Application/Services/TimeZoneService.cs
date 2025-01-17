using Microsoft.JSInterop;
using System;

namespace ReslifeFiveFrontEnd.Application.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly IJSRuntime _jsRuntime;
        private string? _timeZone;
        private readonly ILogger<TimeZoneService> _logger;
        public TimeZoneService(IJSRuntime jsRuntime, ILogger<TimeZoneService> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public async Task<string> GetUserTimeZoneNameAsync()
        {
            if (_timeZone == null)
            {
                _timeZone = await _jsRuntime.InvokeAsync<string>("getUserTimeZone");
            }
            return _timeZone;
        }
        public async Task<TimeZoneInfo> GetUserTimeZoneAsync()
        {
            var timeZoneId = await GetUserTimeZoneNameAsync();
            try
            {
                // Find the time zone using the identifier
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                // Convert the UTC time to the local time zone
                return timeZone;
            }
            catch (TimeZoneNotFoundException)
            {
                throw new Exception("The registry does not define " + timeZoneId + " time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                throw new Exception("Registry data on the " + timeZoneId + " time zone has been corrupted.");
            }
        }

        public async Task<DateTime> ConvertUtcToLocalAsync(DateTime UtcDateTime)
        {
            var timeZoneId = await GetUserTimeZoneNameAsync();
            Console.WriteLine(timeZoneId);
            try
            {
                // Find the time zone using the identifier
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                // Convert the UTC time to the local time zone
                return TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Handle invalid time zone (fallback to UTC or handle as needed)
                return UtcDateTime;
            }
            catch (InvalidTimeZoneException)
            {
                // Handle invalid time zone (fallback to UTC or handle as needed)
                return UtcDateTime;
            }
        }

        public async Task<DateTime> ConvertLocalToUtcAsync(DateTime LocalDateTime)
        {
            var timeZoneId = await GetUserTimeZoneNameAsync();
            Console.WriteLine(timeZoneId);
            try
            {
                // Find the time zone using the identifier
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                // Convert the local time to UTC
                return TimeZoneInfo.ConvertTimeToUtc(LocalDateTime, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                return LocalDateTime; // Handle invalid time zone
            }
            catch (InvalidTimeZoneException)
            {
                return LocalDateTime; // Handle invalid time zone
            }
        }

        //methods that receive a datetime parameter only
        public DateTime ConvertLocalToUtc(DateTime LocalDateTime)
        {
            if (LocalDateTime.Kind != DateTimeKind.Utc)
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly intialized."));
                var Time = TimeZoneInfo.ConvertTimeToUtc(LocalDateTime, timeZone);
                DateTime.SpecifyKind(Time, DateTimeKind.Utc);
                return Time;
            }
            else
            {
                return LocalDateTime;
            }

        }
        public DateTime ConvertUtcToLocal(DateTime UtcDateTime)
        {
            if (UtcDateTime.Kind == DateTimeKind.Utc)
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly intialized."));
                var Time = TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, timeZone);
                DateTime.SpecifyKind(Time, DateTimeKind.Unspecified);
                return Time;
            }
            else
            {
                return UtcDateTime;
            }

        }





        //overload methods that receive a nullable datetime parameter
        public DateTime? ConvertLocalToUtc(DateTime? LocalDateTime)
        {
            if (LocalDateTime == null)
            {
                _logger.LogWarning($"ConvertLocalToUtc(DateTime?) received a null value ({LocalDateTime})");
                return null; // Return null as is if input is null
            }
         
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(
                _timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly initialized."));

                // Specify DateTimeKind to avoid ambiguity
                var Time = TimeZoneInfo.ConvertTimeToUtc(LocalDateTime.Value, timeZone);
                Time = DateTime.SpecifyKind(Time, DateTimeKind.Utc);
                return Time;
        }

        public DateTime? ConvertUtcToLocal(DateTime? UtcDateTime)
        {
            if (UtcDateTime == null)
            {
                _logger.LogWarning($"ConvertUtcToLocal(DateTime?) received a null value ({UtcDateTime})");
                return null; // Return null as is if input is null
            }

          
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(
                _timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly initialized."));


                // Specify DateTimeKind to ensure conversion works correctly
                var Time = TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime.Value, timeZone);
                DateTime.SpecifyKind(Time, DateTimeKind.Utc);
                return Time;
            
        }



        public DateTime? DisplayLocalTimeFromUtc(DateTime? UtcDateTime)
        {
            if (UtcDateTime == null)
            {
                return null;
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(
                _timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly initialized."));

            DateTime dateTime = new DateTime();
            dateTime = TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime.Value, timeZone);
            return dateTime;
        }
    }
}
