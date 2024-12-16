using Microsoft.JSInterop;
using System;

namespace ReslifeFiveFrontEnd.Application.Services
{
    public class TimeZoneService: ITimeZoneService
    {
        private readonly IJSRuntime _jsRuntime;
        private string? _timeZone;
        public TimeZoneService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
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
            Console.WriteLine(timeZoneId);
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


        public DateTime ConvertLocalToUtc(DateTime LocalDateTime)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly intialized."));
            return TimeZoneInfo.ConvertTimeToUtc(LocalDateTime, timeZone);
        }
        public DateTime ConvertUtcToLocal(DateTime UtcDateTime)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZone ?? throw new NullReferenceException("Time Zone Property of TimeZoneService class was not properly intialized."));
            return TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, timeZone);
        }
        public DateTime ConvertUtcToLocal(DateTime UtcDateTime, string TimeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(UtcDateTime, timeZone);
        }
        public DateTime ConvertLocalToUtc(DateTime LocalDateTime, string TimeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(LocalDateTime, timeZone);
        }





















    }
}
