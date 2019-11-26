using System.Threading.Tasks;

namespace Sphix.Service.CronJob
{
  public interface ICronJobsService
    {
        Task<bool> ThursdayMeetingFollowUpMails();
    }
}
