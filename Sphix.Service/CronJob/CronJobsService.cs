using Data.Context;
using Microsoft.Extensions.Hosting;
using Sphix.DataModels.VToken;
using Sphix.Service.Logger;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels.Communities;
using System;
using System.Collections.Generic;
using System.IO;
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
        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public CronJobsService(ILoggerService loggerService
            , IHostingEnvironment env
            , IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration
            , EFDbContext context
            )
        {
            _loggerService = loggerService;
            _env = env;
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
            _context = context;
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<bool> ThursdayMeetingFollowUpMails()
        {
            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await semaphoreSlim.WaitAsync();
            try
            {
                DateTime now = DateTime.UtcNow;
                if ((now.Hour >= 8 && now.Hour <= 9) && now.DayOfWeek == DayOfWeek.Monday)
                {
                    //it is between 8 and 9pm on Thursday
                    await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                    {
                        AddedDate = DateTime.UtcNow,
                        ErrorCode = "Hangfire",
                        Detail = "Start",
                        Message = "Called at " + DateTime.UtcNow.ToString(),
                        Source = "Hangfire",
                    });
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
                                tokenModel.TokenForTableName = "UserCommunityOpenOfficeHours";
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
                                    await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                                    {
                                        AddedDate = DateTime.UtcNow,
                                        ErrorCode = "Hangfire",
                                        Detail = _result.StatusCode +" email sent to "+ item.Email,
                                        Message = "Called at " + DateTime.UtcNow.ToString(),
                                        Source = "Hangfire",
                                    });
                                }
                                

                            }
                            
                        }
                    }
                  
                   
                }

              return true;

               
            }
            catch (Exception ex) 
            {

                await _loggerService.AddAsync(new DataModels.Logger.LoggerDataModel
                {
                    AddedDate = DateTime.UtcNow,
                    ErrorCode = "Hangfire",
                    Detail = ex.Message,
                    Message = ex.Message,
                    Source = "Hangfire",
                });
                return false;
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
               
            }
        }
    }
}
