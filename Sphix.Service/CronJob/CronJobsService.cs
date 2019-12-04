using AWSS3.Utility;
using Data.Context;
using Microsoft.Extensions.Hosting;
using Sphix.DataModels.Logger;
using Sphix.DataModels.VToken;
using Sphix.Service.Logger;
using Sphix.Service.MailBox;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels.Communities;
using Sphix.ViewModels.CronJobMails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sphix.Service.CronJob
{
    public class CronJobsService : ICronJobsService
    {
        private readonly ILoggerService _loggerService;
        private IHostingEnvironment _env;
        private SphixConfiguration _sphixConfiguration { get; }
        private readonly IEmailSenderService _emailSender;
        private readonly EFDbContext _context;
        private UnitOfWork _unitOfWork;
        private readonly IMailBoxService _mailBoxService;
        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        static SemaphoreSlim semaphoreSlimThursday = new SemaphoreSlim(1, 1);
        static SemaphoreSlim semaphoreSlimForWednesday = new SemaphoreSlim(1, 1);
        public CronJobsService(ILoggerService loggerService
            , IHostingEnvironment env
            , IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration
            , EFDbContext context
            , IMailBoxService mailBoxService
            )
        {
            _loggerService = loggerService;
            _env = env;
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
            _context = context;
            _unitOfWork = new UnitOfWork(context);
            _mailBoxService= mailBoxService;

        }
        /// <summary>
        /// SPHIXBUILD-234 automate follow-up email with sendgrid
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ThursdayMeetingFollowUpMails()
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await semaphoreSlimThursday.WaitAsync();
            try
            {
                DateTime now = DateTime.UtcNow;
                if ((now.Hour >= 20 && now.Hour <= 21) && now.DayOfWeek == DayOfWeek.Thursday)
                {
                    //it is between 8 and 9pm on Thursday
                    IList<FollowUpMeetingMailsDetail> list = new List<FollowUpMeetingMailsDetail>();
                    await _context.LoadStoredProc("GetWeeklyFollowUpMeetingMails")
                                .ExecuteStoredProcAsync((handler) =>
                                {
                                    list = handler.ReadToList<FollowUpMeetingMailsDetail>();
                                });
                    if (list != null)
                    {
                        if (list.Count != 0)
                        {
                            var pathToFile = _env.ContentRootPath
                                      + "\\wwwroot"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "Templates"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "EmailTemplates"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "Thursday_Follow_Up_Mail.html";
                            string HtmlBody = string.Empty;
                            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                            {
                                HtmlBody = SourceReader.ReadToEnd();
                            }
                            TokensDataModel tokenModel;
                            foreach (var item in list)
                            {
                                tokenModel = new TokensDataModel();
                                string token = Guid.NewGuid().ToString().Replace("-", "");
                                tokenModel.Token = token;
                                tokenModel.TokenForId = item.Id;
                                tokenModel.TokenForTableName = TableEnum.UserCommunityOpenOfficeHours.ToString();
                                tokenModel.CreatedOn = DateTime.UtcNow;
                                await _unitOfWork.VerificationTokensRepository.Insert(tokenModel);

                                if (tokenModel.Id != 0)
                                {
                                    string meedagebody = string.Empty;
                                    meedagebody = HtmlBody;
                                    meedagebody = meedagebody.Replace("#Name", item.Name);
                                    meedagebody = meedagebody.Replace("#Link", _sphixConfiguration.SiteUrl + "Shx/CancelMeeting/" + token);
                                    meedagebody = meedagebody.Replace("#Footer", UMessagesInfo.MailFooter);

                                     var _result=await _emailSender.SendEmailAsync(
                                        "Follow Up for next meeting",
                                        meedagebody,
                                        item.Email,
                                        _sphixConfiguration.SupportEmail,
                                        UMessagesInfo.SphixSupport
                                        );

                                    await _loggerService.AddAsync(new LoggerDataModel
                                    {
                                        AddedDate = DateTime.UtcNow,
                                        ErrorCode = "Hangfire",
                                        Detail = "ThursdayMeetingFollowUpMails",
                                        Message = "Called at " + DateTime.UtcNow.ToString()+" and "+ _result.StatusCode + " email sent to " + item.Email,
                                        Source = "Hangfire",
                                    });
                                }
                                

                            }
                            
                        }
                    }
                }
                    semaphoreSlimThursday.Release();
                return true;
               
            }
            catch (Exception ex) 
            {

                await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                {
                    AddedDate = DateTime.UtcNow,
                    ErrorCode = "Hangfire",
                    Detail = "ThursdayMeetingFollowUpMails",
                    Message = ex.Message,
                    Source = "Hangfire",
                });
                semaphoreSlimThursday.Release();
                return false;
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                
               
            }
        }
        /// <summary>
        /// SPHIXBUILD-235 automate email with sendgrid to group members
        /// </summary>
        /// <returns></returns>
        public async Task<bool> WednesdayRemindersMails()
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await semaphoreSlimForWednesday.WaitAsync();
            try
            {
                DateTime now = DateTime.UtcNow;
                if ((now.Hour >= 15 && now.Hour <= 16) && now.DayOfWeek == DayOfWeek.Tuesday)
                {
                    //it is between 8 and 9pm on Thursday
                    IList<WednesdayReminderViewModel> list = new List<WednesdayReminderViewModel>();
                    await _context.LoadStoredProc("GetWednesday3PMReminders")
                                .ExecuteStoredProcAsync((handler) =>
                                {
                                    list = handler.ReadToList<WednesdayReminderViewModel>();
                                });
                    if (list != null)
                    {
                        if (list.Count != 0)
                        {
                            var pathToFile = _env.ContentRootPath
                                      + "\\wwwroot"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "Templates"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "EmailTemplates"
                                     + Path.DirectorySeparatorChar.ToString()
                                     + "Wednesday_Reminder_Mail.html";
                            string HtmlBody = string.Empty;
                            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                            {
                                HtmlBody = SourceReader.ReadToEnd();
                            }
                            StringBuilder meedagebody;
                            MailSentBoxDataModel mailSentBox;
                            foreach (var item in list)
                            {
                                meedagebody =new StringBuilder(HtmlBody);
                                meedagebody = meedagebody.Replace("#Name", item.Name);
                                meedagebody = meedagebody.Replace("#Link", UMessagesInfo.WebSiteUrl + item.CommunityGroupURL+"-"+item.Id.ToString());
                                meedagebody = meedagebody.Replace("#Title", item.Title);
                                meedagebody = meedagebody.Replace("#Footer", UMessagesInfo.MailFooter);
                                mailSentBox = new MailSentBoxDataModel
                                {
                                    SentForId = item.Id,
                                    SentForTableName = TableEnum.UserCommunitiesGroups.ToString(),
                                    ToEMailId = item.Email,
                                    SentToUserId = item.UserId,
                                    FromEMailId = _sphixConfiguration.SupportEmail,
                                    MessageType = EmailMessageTypes.Wednesday3PM.ToString(),
                                    Subject = "This is a friendly reminder",
                                    Message = meedagebody.ToString(),
                                    SentDateTime = DateTime.UtcNow,
                                    IsRead = false
                                };

                            var _status =await _mailBoxService.SaveAsync(mailSentBox);
                                if (_status.Status)
                                {
                                    var _result = await _emailSender.SendEmailAsync(
                                   "This is a friendly reminder",
                                    meedagebody.ToString(),
                                   item.Email,
                                   _sphixConfiguration.SupportEmail,
                                   UMessagesInfo.SphixSupport
                                   );
                                }
                                //await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                                //{
                                //    AddedDate = DateTime.UtcNow,
                                //    ErrorCode = "Hangfire",
                                //    Detail = "WednesdayRemindersMails",
                                //    Message = "Called at " + DateTime.UtcNow.ToString() + " and " + _result.StatusCode + " email sent to " + item.Email,
                                //    Source = "Hangfire",
                                //});
                            }

                        }
                    }
                }
                semaphoreSlimForWednesday.Release();
                return true;

            }
            catch (Exception ex)
            {

                await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                {
                    AddedDate = DateTime.UtcNow,
                    ErrorCode = "Hangfire",
                    Detail = "WednesdayRemindersMails",
                    Message = ex.Message,
                    Source = "Hangfire",
                });

                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlimForWednesday.Release();
                return false;
            }
            finally
            {
                
            }
        }
    }
}
