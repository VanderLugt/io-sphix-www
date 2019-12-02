using System.Threading.Tasks;

namespace Sphix.Service.CronJob
{
  public interface ICronJobsService
    { 
      /// <summary>
      /// SPHIXBUILD-234 automate follow-up email with sendgrid
      /// </summary>
      /// <returns></returns>
        Task<bool> ThursdayMeetingFollowUpMails();
        /// <summary>
        /// SPHIXBUILD-235 automate email with sendgrid to group members
        /// </summary>
        /// <returns></returns>
        Task<bool> WednesdayRemindersMails();
    }
}
