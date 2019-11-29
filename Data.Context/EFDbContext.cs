using Microsoft.EntityFrameworkCore;
using Sphix.DataModels;
using Sphix.DataModels.Authorization;
using Sphix.DataModels.Communities;
using Sphix.DataModels.EmailInvitation;
using Sphix.DataModels.Logger;
using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.DataModels.UserCommunitiesGroups.Join;
using Sphix.DataModels.UserCommunitiesGroups.PublishCommunityGroupLink;
using Sphix.DataModels.VToken;

namespace Data.Context
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options)
             : base(options)
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<LoggerDataModel> loggerManager { get; set; }
        public DbSet<RoleDataModel> rolesMaster { get; set; }
        public DbSet<CountriesDataModel> countriesMaster { get; set; }
        public DbSet<TokensDataModel> verificationTokens { get; set; }
        public DbSet<MailSentBoxDataModel> mailSentBoxes { get; set; }
        #region communities DBSet
        public DbSet<CommunityDataModel> communities { get; set; }
        public DbSet<CommunityCatgoryDataModel> communityCatgories { get; set; }
        public DbSet<CommunityThemesDataModel> communityThemes { get; set; }
        
        #endregion
        #region user DBSet
        public DbSet<UsersLoginDataModel>  usersLogin { get; set; }
        public DbSet<UsersRolesDataModel> UsersRoles { get; set; }
        public DbSet<UsersProfileDataModel> usersProfiles { get; set; }
        public DbSet<UserNotificationSettingsDataModel> userNotificationSettings { get; set; }
        public DbSet<PhoneVerificationDataModel> phoneVerification { get; set; }
        public DbSet<EmailVerificationDataModel> emailVerifications { get; set; }
        public DbSet<RestPasswordLinkDataModel> restPasswordLinks { get; set; }
        public DbSet<UserCommunitiesDataModel> userCommunities { get; set; }
        public DbSet<UserSubCommunitiesDataModel> userSubCommunities { get; set; }
        public DbSet<UserGroupsDataModel> userGroups { get; set; }
        public DbSet<UserInterestsDataModel> userInterests { get; set; }
        public DbSet<UserAssociationsDataModel> userAssociations { get; set; }
        #endregion
        #region UserCommunitiesGroups
        public DbSet<CommunityGroupsDataModel> userCommunityGroups { get; set; }
        public DbSet<CommunityOpenOfficeHours> userCommunityOpenOfficeHours { get; set; }
        public DbSet<OpenOfficeHoursMeetingsStatusDataModel> openOfficeHoursMeetingsStatus { get; set; }
        public DbSet<CommunityLiveEvents> userCommunityLiveEvents { get; set; }
        public DbSet<UserArticleCommentsDataModel> eventsComments { get; set; }
        public DbSet<CommunityArticles> userCommunityArticles { get; set; }
        public DbSet<UserEventCommentsDataModel> articleComments { get; set; }
        public DbSet<UserCommunityTargetedGroupsDataModel> userCommunityTargetedGroups { get; set; }
        public DbSet<UserCommunityTargetedType2DataModel> userCommunityTargetedType2s { get; set; }
        public DbSet<UserCommunityTargetedType1DataModel> userCommunityTargetedType1s { get; set; }
        public DbSet<UserCommunityTargetedInterestsDataModel> userCommunityTargetedInterests { get; set; }
        public DbSet<UserCommunityTargetedAssociationsDataModel> userCommunityTargetedAssociations { get; set; }

        public DbSet<UserCommunityGroupThemeDataModel> userCommunityGroupThemes { get; set; }
        public DbSet<UserCommunityGroupPublishLinksDataModel> communityGroupLinksDataModels { get; set; }
        public DbSet<JoinCommunityGroupDataModel> joinCommunityGroups { get; set; }
        public DbSet<JoinOpenHoursMeetingDataModel> joinCommunityOpenHoursMeeting { get; set; }
        public DbSet<JoinEventMeetingDataModel> joinCommunityEventMeeting { get; set; }
        public DbSet<GroupEmailInvitationDataModel> groupEmailInvitation { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleDataModel>(entity =>
            {
                entity.HasIndex(e => e.RoleName).IsUnique();
            });
            builder.Entity<CountriesDataModel>(entity =>
            {
                entity.HasIndex(e => e.CountryName).IsUnique();
            });

            builder.Entity<UsersLoginDataModel>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });
            builder.Entity<UsersProfileDataModel>(entity =>
            {
                entity.HasIndex(e => e.ProfileLink).IsUnique();
            });

            builder.Entity<PhoneVerificationDataModel>(entity =>
            {
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });
        }
        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLazyLoadingProxies();
        //}
    }
}
