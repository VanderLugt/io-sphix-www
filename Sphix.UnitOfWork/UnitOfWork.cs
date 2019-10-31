using Data.Context;
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
using Sphix.GenericRepository;
using System;

namespace Sphix.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly EFDbContext _context;

        public UnitOfWork(EFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
       
        private GenericRepository<RoleDataModel> roleRepository;
        public GenericRepository<RoleDataModel> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                    this.roleRepository = new GenericRepository<RoleDataModel>(_context);
                return roleRepository;
            }
        }
        #region User verification repository
        private GenericRepository<RestPasswordLinkDataModel> restPasswordLinkRepository;
        public GenericRepository<RestPasswordLinkDataModel> RestPasswordLinkRepository
        {
            get
            {
                if (this.restPasswordLinkRepository == null)
                    this.restPasswordLinkRepository = new GenericRepository<RestPasswordLinkDataModel>(_context);
                return restPasswordLinkRepository;
            }
        }
        
        private GenericRepository<EmailVerificationDataModel> emailVerificatioRepository;
        public GenericRepository<EmailVerificationDataModel> EmailVerificatioRepository
        {
            get
            {
                if (this.emailVerificatioRepository == null)
                    this.emailVerificatioRepository = new GenericRepository<EmailVerificationDataModel>(_context);
                return emailVerificatioRepository;
            }
        }
        private GenericRepository<PhoneVerificationDataModel> phoneVerificationRepository;
        public GenericRepository<PhoneVerificationDataModel> PhoneVerificationRepository
        {
            get
            {
                if (this.phoneVerificationRepository == null)
                    this.phoneVerificationRepository = new GenericRepository<PhoneVerificationDataModel>(_context);
                return phoneVerificationRepository;
            }
        }
        #endregion
        #region User respository

        private GenericRepository<UsersLoginDataModel> usersLoginRepository;
        public GenericRepository<UsersLoginDataModel> UserLoginRepository
        {
            get
            {
                if (this.usersLoginRepository == null)
                    this.usersLoginRepository = new GenericRepository<UsersLoginDataModel>(_context);
                return usersLoginRepository;
            }
        }
        private GenericRepository<UsersRolesDataModel> usersRolesRepository;
        public GenericRepository<UsersRolesDataModel> UsersRolesRepository
        {
            get
            {
                if (this.usersRolesRepository == null)
                    this.usersRolesRepository = new GenericRepository<UsersRolesDataModel>(_context);
                return usersRolesRepository;
            }
        }
        private GenericRepository<UsersProfileDataModel> usersProfileRepository;
        public GenericRepository<UsersProfileDataModel> UserProfileRepository
        {
            get
            {
                if (this.usersProfileRepository == null)
                    this.usersProfileRepository = new GenericRepository<UsersProfileDataModel>(_context);
                return usersProfileRepository;
            }
        }
        private GenericRepository<UserNotificationSettingsDataModel> userNotificationSettingsRepository;
        public GenericRepository<UserNotificationSettingsDataModel> UserNotificationSettingsRepository
        {
            get
            {
                if (this.userNotificationSettingsRepository == null)
                    this.userNotificationSettingsRepository = new GenericRepository<UserNotificationSettingsDataModel>(_context);
                return userNotificationSettingsRepository;
            }
        }
        private GenericRepository<UserCommunitiesDataModel> userCommunitiesRepository;
        public GenericRepository<UserCommunitiesDataModel> UserCommunitiesRepository
        {
            get
            {
                if (this.userCommunitiesRepository == null)
                    this.userCommunitiesRepository = new GenericRepository<UserCommunitiesDataModel>(_context);
                return userCommunitiesRepository;
            }
        }
        private GenericRepository<UserSubCommunitiesDataModel> userSubCommunitiesRepository;
        public GenericRepository<UserSubCommunitiesDataModel> UserSubCommunitiesRepository
        {
            get
            {
                if (this.userSubCommunitiesRepository == null)
                    this.userSubCommunitiesRepository = new GenericRepository<UserSubCommunitiesDataModel>(_context);
                return userSubCommunitiesRepository;
            }
        }
        private GenericRepository<UserInterestsDataModel> userInterestsRepository;
        public GenericRepository<UserInterestsDataModel> UserInterestsRepository
        {
            get
            {
                if (this.userInterestsRepository == null)
                    this.userInterestsRepository = new GenericRepository<UserInterestsDataModel>(_context);
                return userInterestsRepository;
            }
        }
        private GenericRepository<UserGroupsDataModel> userGroupsRepository;
        public GenericRepository<UserGroupsDataModel> UserGroupsRepository
        {
            get
            {
                if (this.userGroupsRepository == null)
                    this.userGroupsRepository = new GenericRepository<UserGroupsDataModel>(_context);
                return userGroupsRepository;
            }
        }
        private GenericRepository<UserAssociationsDataModel> userAssociationsRepository;
        public GenericRepository<UserAssociationsDataModel> UserAssociationsRepository
        {
            get
            {
                if (this.userAssociationsRepository == null)
                    this.userAssociationsRepository = new GenericRepository<UserAssociationsDataModel>(_context);
                return userAssociationsRepository;
            }
        }
        private GenericRepository<GroupEmailInvitationDataModel> groupEmailInvitationRepository;
        public GenericRepository<GroupEmailInvitationDataModel> GroupEmailInvitationRepository
        {
            get
            {
                if (this.groupEmailInvitationRepository == null)
                    this.groupEmailInvitationRepository = new GenericRepository<GroupEmailInvitationDataModel>(_context);
                return groupEmailInvitationRepository;
            }
        }
        
        #endregion

        #region Repository User Communities Groups
        private GenericRepository<CommunityGroupsDataModel> communityGroupsRepository;
        public GenericRepository<CommunityGroupsDataModel> UserCommunityGroupsRepository
        {
            get
            {
                if (this.communityGroupsRepository == null)
                    this.communityGroupsRepository = new GenericRepository<CommunityGroupsDataModel>(_context);
                return communityGroupsRepository;
            }
        }
        private GenericRepository<UserCommunityGroupPublishLinksDataModel> communityGroupPublishLinksRepository;
        public GenericRepository<UserCommunityGroupPublishLinksDataModel> CommunityGroupPublishLinksRepository
        {
            get
            {
                if (this.communityGroupPublishLinksRepository == null)
                    this.communityGroupPublishLinksRepository = new GenericRepository<UserCommunityGroupPublishLinksDataModel>(_context);
                return communityGroupPublishLinksRepository;
            }
        }
        private GenericRepository<CommunityOpenOfficeHours> communityOpenOfficeHoursRepository;
        public GenericRepository<CommunityOpenOfficeHours> UserCommunityOpenOfficeHoursRepository
        {
            get
            {
                if (this.communityOpenOfficeHoursRepository == null)
                    this.communityOpenOfficeHoursRepository = new GenericRepository<CommunityOpenOfficeHours>(_context);
                return communityOpenOfficeHoursRepository;
            }
        }
        private GenericRepository<CommunityLiveEvents> communityLiveEventsRepository;
        public GenericRepository<CommunityLiveEvents> UserCommunityLiveEventsRepository
        {
            get
            {
                if (this.communityLiveEventsRepository == null)
                    this.communityLiveEventsRepository = new GenericRepository<CommunityLiveEvents>(_context);
                return communityLiveEventsRepository;
            }
        }
        private GenericRepository<UserEventCommentsDataModel> eventCommentsRepository;
        public GenericRepository<UserEventCommentsDataModel> EventCommentsRepository
        {
            get
            {
                if (this.eventCommentsRepository == null)
                    this.eventCommentsRepository = new GenericRepository<UserEventCommentsDataModel>(_context);
                return eventCommentsRepository;
            }
        }
        
        private GenericRepository<CommunityArticles> communityArticlesRepository;
        public GenericRepository<CommunityArticles> UserCommunityArticlesRepository
        {
            get
            {
                if (this.communityArticlesRepository == null)
                    this.communityArticlesRepository = new GenericRepository<CommunityArticles>(_context);
                return communityArticlesRepository;
            }
        }
        private GenericRepository<UserArticleCommentsDataModel> articleCommentsRepository;
        public GenericRepository<UserArticleCommentsDataModel> ArticleCommentsRepository
        {
            get
            {
                if (this.articleCommentsRepository == null)
                    this.articleCommentsRepository = new GenericRepository<UserArticleCommentsDataModel>(_context);
                return articleCommentsRepository;
            }
        }
        
        private GenericRepository<UserCommunityTargetedGroupsDataModel> userCommunityTargetedGroupsRepository;
        public GenericRepository<UserCommunityTargetedGroupsDataModel> UserCommunityTargetedGroupsRepository
        {
            get
            {
                if (this.userCommunityTargetedGroupsRepository == null)
                    this.userCommunityTargetedGroupsRepository = new GenericRepository<UserCommunityTargetedGroupsDataModel>(_context);
                return userCommunityTargetedGroupsRepository;
            }
        }
        private GenericRepository<UserCommunityTargetedType1DataModel> userCommunityTargetedType1Repository;
        public GenericRepository<UserCommunityTargetedType1DataModel> UserCommunityTargetedType1Repository
        {
            get
            {
                if (this.userCommunityTargetedType1Repository == null)
                    this.userCommunityTargetedType1Repository = new GenericRepository<UserCommunityTargetedType1DataModel>(_context);
                return userCommunityTargetedType1Repository;
            }
        }
        private GenericRepository<UserCommunityTargetedType2DataModel> userCommunityTargetedType2Repository;
        public GenericRepository<UserCommunityTargetedType2DataModel> UserCommunityTargetedType2Repository
        {
            get
            {
                if (this.userCommunityTargetedType2Repository == null)
                    this.userCommunityTargetedType2Repository = new GenericRepository<UserCommunityTargetedType2DataModel>(_context);
                return userCommunityTargetedType2Repository;
            }
        }
        private GenericRepository<UserCommunityTargetedAssociationsDataModel> userCommunityTargetedAssociationsRepository;
        public GenericRepository<UserCommunityTargetedAssociationsDataModel> UserCommunityTargetedAssociationsRepository
        {
            get
            {
                if (this.userCommunityTargetedAssociationsRepository == null)
                    this.userCommunityTargetedAssociationsRepository = new GenericRepository<UserCommunityTargetedAssociationsDataModel>(_context);
                return userCommunityTargetedAssociationsRepository;
            }
        }
        private GenericRepository<UserCommunityTargetedInterestsDataModel> userCommunityTargetedInterestRepository;
        public GenericRepository<UserCommunityTargetedInterestsDataModel> UserCommunityTargetedInterestRepository
        {
            get
            {
                if (this.userCommunityTargetedInterestRepository == null)
                    this.userCommunityTargetedInterestRepository = new GenericRepository<UserCommunityTargetedInterestsDataModel>(_context);
                return userCommunityTargetedInterestRepository;
            }
        }
       
        private GenericRepository<UserCommunityGroupThemeDataModel> userCommunityGroupThemeRepository;
        public GenericRepository<UserCommunityGroupThemeDataModel> UserCommunityGroupThemeRepository
        {
            get
            {
                if (this.userCommunityGroupThemeRepository == null)
                    this.userCommunityGroupThemeRepository = new GenericRepository<UserCommunityGroupThemeDataModel>(_context);
                return userCommunityGroupThemeRepository;
            }
        }
        private GenericRepository<JoinCommunityGroupDataModel> joinCommunityGroupRepository;
        public GenericRepository<JoinCommunityGroupDataModel> JoinCommunityGroupRepository
        {
            get
            {
                if (this.joinCommunityGroupRepository == null)
                    this.joinCommunityGroupRepository = new GenericRepository<JoinCommunityGroupDataModel>(_context);
                return joinCommunityGroupRepository;
            }
        }
        private GenericRepository<JoinOpenHoursMeetingDataModel> joinOpenHoursMeetingRepository;
        public GenericRepository<JoinOpenHoursMeetingDataModel> JoinOpenHoursMeetingRepository
        {
            get
            {
                if (this.joinOpenHoursMeetingRepository == null)
                    this.joinOpenHoursMeetingRepository = new GenericRepository<JoinOpenHoursMeetingDataModel>(_context);
                return joinOpenHoursMeetingRepository;
            }
        }
        private GenericRepository<JoinEventMeetingDataModel> joinEventMeetingRepository;
        public GenericRepository<JoinEventMeetingDataModel> JoinEventMeetingRepository
        {
            get
            {
                if (this.joinEventMeetingRepository == null)
                    this.joinEventMeetingRepository = new GenericRepository<JoinEventMeetingDataModel>(_context);
                return joinEventMeetingRepository;
            }
        }
        #endregion
        //CommunityDataModel Rpository
        private GenericRepository<CommunityDataModel> communityRpository;
        public GenericRepository<CommunityDataModel> CommunityRepository
        {
            get
            {
                if (this.communityRpository == null)
                    this.communityRpository = new GenericRepository<CommunityDataModel>(_context);
                return communityRpository;
            }
        }
        private GenericRepository<CommunityThemesDataModel> communityThemesRepository;
        public GenericRepository<CommunityThemesDataModel> CommunityThemesRepository
        {
            get
            {
                if (this.communityThemesRepository == null)
                    this.communityThemesRepository = new GenericRepository<CommunityThemesDataModel>(_context);
                return communityThemesRepository;
            }
        }
        
        //CommunityDataModel Rpository
        private GenericRepository<CommunityCatgoryDataModel> communityCatgoryRpository;
        public GenericRepository<CommunityCatgoryDataModel> CommunityCatgoryRepository
        {
            get
            {
                if (this.communityCatgoryRpository == null)
                    this.communityCatgoryRpository = new GenericRepository<CommunityCatgoryDataModel>(_context);
                return communityCatgoryRpository;
            }
        }
        //CountriesDataModel
        private GenericRepository<CountriesDataModel> countryRepository;
        public GenericRepository<CountriesDataModel> CountryRepository
        {
            get
            {
                if (this.countryRepository == null)
                    this.countryRepository = new GenericRepository<CountriesDataModel>(_context);
                return countryRepository;
            }
        }
        //LoggerDataModel Rpository
        private GenericRepository<LoggerDataModel> loggerRepository;
        public GenericRepository<LoggerDataModel> LoggerRepository
        {
            get
            {
                if (this.loggerRepository == null)
                    this.loggerRepository = new GenericRepository<LoggerDataModel>(_context);
                return loggerRepository;
            }
        }

        public async void Save()
        {
            try
            {
              await  _context.SaveChangesAsync();
            }
            catch (Exception )
            {
                throw;
            }
        }
        /*
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
        */
    }
}
