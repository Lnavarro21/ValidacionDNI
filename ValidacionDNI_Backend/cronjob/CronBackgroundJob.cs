﻿using Cronos;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace ValidacionDNI_Backend.cronjob
{
    public abstract class CronBackgroundJob : BackgroundService
    {

        private PeriodicTimer? _timer;

        private readonly CronExpression _cronExpression;

        private readonly TimeZoneInfo _timeZone;

        public CronBackgroundJob(string rawCronExpression, TimeZoneInfo timeZone)
        {
            _cronExpression = CronExpression.Parse(rawCronExpression);
            _timeZone = timeZone;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DateTimeOffset? nextOcurrence = _cronExpression.GetNextOccurrence(DateTimeOffset.UtcNow, _timeZone);

            if (nextOcurrence.HasValue)
            {

                var delay = nextOcurrence.Value - DateTimeOffset.UtcNow;
                _timer = new PeriodicTimer(delay);

                if (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    _timer.Dispose();
                    _timer = null;

                    await DoWork(stoppingToken);

                    // Reagendamos
                    await ExecuteAsync(stoppingToken);
                }
            }
        }

        protected abstract Task DoWork(CancellationToken stoppingToken);
    }
}
