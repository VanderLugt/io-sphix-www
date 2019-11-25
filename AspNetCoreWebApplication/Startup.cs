using AspNetCoreWebApplication;
using AspNetCoreWebApplication.Models;
using AWSS3.Utility;
using CustomTwilioClient;
using Data.Context;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Sphix.Service.Authorization;
using Sphix.Service.Authorization.Login;
using Sphix.Service.Authorization.Login.ForgotPassword;
using Sphix.Service.Authorization.SignUp.EmailVerification;
using Sphix.Service.Communities;
using Sphix.Service.Communities.ComunitySubTypes;
using Sphix.Service.CronJob;
using Sphix.Service.EmailInvitation;
using Sphix.Service.Logger;
using Sphix.Service.SendGridManager;
//using CustomTwilioClient.
using Sphix.Service.Settings;
using Sphix.Service.User;
using Sphix.Service.User.Associations;
using Sphix.Service.User.Notification;
using Sphix.Service.User.UserCommunities;
using Sphix.Service.UserCommunities;
using Sphix.Service.UserCommunities.ArticleComments;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using Sphix.Service.UserCommunities.CommunityGroupPublishMail;
using Sphix.Service.UserCommunities.EventComments;
using Sphix.Service.UserCommunities.JoinCommunityEventMeeting;
using Sphix.Service.UserCommunities.JoinCommunityGroup;
using Sphix.Service.UserCommunities.JoinCommunityOpenHoursMeeting;
using Sphix.Service.UserCommunities.OpenOfficeHours;
using Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursThanksMail;
using Sphix.Utility.DateTimeDifference;
using System;
using System.Security.Claims;

namespace Sphix.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("websitePolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("websitePolicy"));
            });

            services.Configure<AuthorizationOptions>(options => {
                options.AddPolicy("hangfireDashboardPolicy", policy => {
                    // require the user to be authenticated
                    policy.RequireAuthenticatedUser();
                    // Maybe require a claim here, if you need that.
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;

            });
          

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Home/Login";
                options.SlidingExpiration = true;
            });

            //Twilio Rest Client ID
            //services.AddHttpClient<ITwilioRestClient, CustomTwilioClient>();
            services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("SendGrid"));
            services.Configure<AwsS3Settings>(Configuration.GetSection("AwsS3Settings"));
            services.Configure<PasswordSettings>(Configuration.GetSection("PasswordSettings"));
            var accountSid = Configuration["Twilio:AccountSID"];
            var authToken = Configuration["Twilio:AuthToken"];
            //TwilioClient.Init(accountSid, authToken);
            //Set database connection string from appsetings.json
            services.AddDbContext<EFDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Set data connection for Hangfire  
            //services.AddHangfire(
            //     x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"))
            // );
            // follow steps from this link https://docs.hangfire.io/en/latest/getting-started/aspnet-core-applications.html
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddScoped<IUDateTimeDifference, UDateTimeDifference>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddTransient<ITwilioVideoService, TwilioVideoService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICommunitiesService, CommunitiesService>();
            services.AddScoped<ISignUpService, SignUpService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            //user
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssociationsSettingService, AssociationsSettingService>();
            services.AddScoped<INotificationSettingsService, NotificationSettingsService>();
            services.AddScoped<IUserCommunitiesService, UserCommunitiesService>();
            services.AddScoped<ICommunityGroupEmailService, CommunityGroupEmailService>();
            services.AddScoped<ICommunitiesForGoodService, CommunitiesForGoodService>();

            services.AddScoped<ICommunityGroupsService, CommunityGroupsService>();
            services.AddScoped<IOpenOfficeHoursService, OpenOfficeHoursService>();
            services.AddScoped<IOpenHoursMailService, OpenHoursMailService>();
            services.AddScoped<ICommunityGroupEventService, CommunityGroupEventService>();
            services.AddScoped<IEventCommentsService, EventCommentsService>(); 
            services.AddScoped<ICommunityGroupArticleService, CommunityGroupArticleService>();
            services.AddScoped<IJoinCommunityGroupService, JoinCommunityGroupService>();
            services.AddScoped<IJoinCommunityOpenHoursMeetingService, JoinCommunityOpenHoursMeetingService>();
            services.AddScoped<IJoinCommunityEventMeetingService, JoinCommunityEventMeetingService>();
            services.AddScoped<IArticleCommentsService, ArticleCommentsService>();
            services.AddScoped<IEmailInvitationService, EmailInvitationService>();
            services.AddScoped<IComunitySubTypesService, ComunitySubTypesService>();
            services.AddScoped<ICronJobsService, CronJobsService>();
            //send grid
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddSingleton<IAWSS3Bucket, AWSS3Bucket>();
            services.AddSingleton(Configuration.GetSection("SphixEmails").Get<SphixConfiguration>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ClaimAccessor, ClaimAccessor>();

            
            // services.AddSingleton(Configuration.GetSection("TwilioAuthOptions").Get<TwilioAuthOptions>());

            //--< set uploadsize large files >----
            services.Configure<FormOptions>(options =>

            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });
            //--</ set uploadsize large files >----


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<CustomExceptionMiddleware>();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<CustomExceptionMiddleware>();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var options = new RewriteOptions()
               // .AddRedirect("redirect-rule/(.*)", "redirected/$1")
               .AddRewrite(@"CommunityGroups/academia-and-research", "/CommunityGroups/Index?id=1&title=Academia And Research", skipRemainingRules: true)
               .AddRewrite(@"CommunityGroups/businesses-and-organizations", "/CommunityGroups/Index?id=3&title=Businesses And Organizations", skipRemainingRules: true)
               .AddRewrite(@"CommunityGroups/technology-and-environment", "/CommunityGroups/Index?id=4&title=Technology & Environment", skipRemainingRules: true)
               .AddRewrite(@"CommunityGroups/philanthropy-and-relationships", "/CommunityGroups/Index?id=5&title=Philanthropy And Relationships", skipRemainingRules: true)
               .AddRewrite(@"CommunityGroups/philosophy-and-religion", "/CommunityGroups/Index?id=6&title=Philosophy And Religion", skipRemainingRules: true)
               .AddRewrite(@"CommunityGroups/politics-and-government", "/CommunityGroups/Index?id=2&title=Politics And Government", skipRemainingRules: true)
               /*
               .AddRewrite(@"MoreGroups/academia-and-research", "/MoreCommunityGroups/Index?id=1&title=Academia And Research", skipRemainingRules: true)
               .AddRewrite(@"MoreGroups/businesses-and-organizations", "/MoreCommunityGroups/Index?id=3&title=Businesses And Organizations", skipRemainingRules: true)
               .AddRewrite(@"MoreGroups/data-tech-and-info-systems", "/MoreCommunityGroups/Index?id=4&title=DataTech And InfoSystems", skipRemainingRules: true)
               .AddRewrite(@"MoreGroups/philanthropy-and-relationships", "/MoreCommunityGroups/Index?id=5&title=Philanthropy And Relationships", skipRemainingRules: true)
               .AddRewrite(@"MoreGroups/philosophy-and-religion", "/MoreCommunityGroups/Index?id=6&title=Philosophy And Religion", skipRemainingRules: true)
               .AddRewrite(@"MoreGroups/politics-and-government", "/MoreCommunityGroups/Index?id=2&title=Politics And Government", skipRemainingRules: true)
               */
               .AddRewrite(@"CommunityGroup/(.*)", "CommunityGroup/Index?Id=$1", skipRemainingRules: false)
               //.AddRewrite(@"CommunityGroups/(.*)", "/CommunityGroups/Index?id=$1", skipRemainingRules: false)
               ;
            app.UseRewriter(options);
            //app.Run(context => context.Response.WriteAsync($"Rewritten or Redirected Url: {context.Request.Path + context.Request.QueryString}"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            //app.UseHangfireDashboard();
            app.UseHangfireDashboard(
            pathMatch: "/cronjobs",
            options: new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[] {
                    new HangfireAuthorizationFilter("hangfireDashboardPolicy")
                }
            });
            app.UseHangfireServer();
            //TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")
            // cron jobs setup
            //RecurringJob.AddOrUpdate<ICronJobsService>(
            //    cronJobs => cronJobs.MeetingsFollowUpMailSendAsync(), Cron.Weekly(DayOfWeek.Thursday,20), TimeZoneInfo.Utc);
            RecurringJob.AddOrUpdate<ICronJobsService>(
              cronJobs => cronJobs.MeetingsFollowUpMailSendAsync(), Cron.Hourly(), TimeZoneInfo.Utc);


            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
           name: "SuperAdmin",
           areaName: "SuperAdmin",
           template: "SuperAdmin/{controller=Home}/{action=Index}/{id?}"
              );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        static void RedirectXMLRequests(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            if (request.Path.StartsWithSegments(new PathString("/xmlfiles")))
            {
                return;
            }
            if (request.Path.Value.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Result = RuleResult.EndResponse;
                response.Headers[HeaderNames.Location] = "/xmlfiles" + request.Path + request.QueryString;
            }
        }
    }
}
