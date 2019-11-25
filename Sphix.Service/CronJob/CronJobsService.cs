using Sphix.Service.Logger;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.CronJob
{
    public class CronJobsService : ICronJobsService
    {
        private readonly ILoggerService _loggerService;
        public CronJobsService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
        public async Task<bool> MeetingsFollowUpMailSendAsync()
        {
            await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel {
                AddedDate=DateTime.UtcNow,
                ErrorCode= "Hangfire",
                Detail="This is hangfire test job",
                Message = "Called at "+ DateTime.UtcNow.ToString(),
                Source="Hangfire",
            });
            return true;
        }
    }
}
