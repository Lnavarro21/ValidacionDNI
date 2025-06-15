using System;

namespace ValidacionDNI_Backend.cronjob
{
    public class CronSettings<T>
    {
        public string CronExpression { get; set; } = default!;
        public TimeZoneInfo TimeZone { get; set; } = default!;
    }
}
