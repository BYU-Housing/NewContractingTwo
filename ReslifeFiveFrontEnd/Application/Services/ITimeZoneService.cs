﻿namespace ReslifeFiveFrontEnd.Application.Services
{
    public interface ITimeZoneService
    {
        Task<string> GetUserTimeZoneNameAsync();

        Task<TimeZoneInfo> GetUserTimeZoneAsync();
        Task<DateTime> ConvertUtcToLocalAsync(DateTime UtcDateTime);

        Task<DateTime> ConvertLocalToUtcAsync(DateTime LocalDateTime);
        DateTime ConvertLocalToUtc(DateTime LocalDateTime);
        DateTime ConvertUtcToLocal(DateTime UtcDateTime);
        DateTime ConvertUtcToLocal(DateTime UtcDateTime, string TimeZoneId);
        DateTime ConvertLocalToUtc(DateTime LocalDateTime, string TimeZoneId);

    }
}