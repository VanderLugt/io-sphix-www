using Data.Context;
using Sphix.DataModels.Communities;
using Sphix.DataModels.User;
using Sphix.Service.User.UserCommunities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.User.Associations
{
public class AssociationsSettingService:IAssociationsSettingService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        private List<SelectListItems> _list;
        private readonly IUserCommunitiesService _userCommunitie;
        public AssociationsSettingService(EFDbContext context, IUserCommunitiesService userCommunitie)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
            _userCommunitie = userCommunitie;
        }
        public async Task<BaseModel> SaveAssociationsAsync(AssociationsModel model)
        {
            //save communities data
            await _userCommunitie.JoinCommunitiesAsync(model.CommunityId, model.UserId);
            var _user = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
            var _community = await _unitOfWork.CommunityRepository.GetByID(model.CommunityId);
            //Save data into the community table like group,association 
            //UserSubCommunitiesDataModel userCommunities = new UserSubCommunitiesDataModel
            //{
            //    Id = model.EditId,
            //    User = _user,
            //    Association = model.AssociationId,
            //    GroupId = model.GroupId,
            //    Type1Id = model.Type1Id,
            //    Type2ListId = model.Type2Id,
            //    Community = _community
            //};

            //await _userCommunitie.SaveSubCommunitiesAsync(userCommunities);

            await _userCommunitie.SaveGroupsAsync(model.GroupId, _user, _community);
            await _userCommunitie.SaveAssociationsAsync(model.AssociationId, _user, _community);
            await _userCommunitie.SaveInterestsAsync(model.InterestIds, _user, _community);
            return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
        }
        public async Task<IList<AssociationsList>> getUserAssociationsAsync(long userId)
        {
            IList<AssociationsList> list = new List<AssociationsList>();
            await this._context.LoadStoredProc("GetUserAssociations")
                       .WithSqlParam("UserId", userId)
                       // .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<AssociationsList>();
                           // do something with your results.
                       });
            return list;
        }
        public async Task<SignUpStep3ViewModel> getEditUserAssociationsAsync(int id, long userId)
        {
            SignUpStep3ViewModel listData = new SignUpStep3ViewModel();
            IList<CommunityCatgoryDataModel> _data = await _unitOfWork.CommunityCatgoryRepository.FindAllBy(c => c.CommunityId == id && c.IsActive == true);
           
            
            //Groups
           
            _list = new List<SelectListItems>();
            var _selectedGroups = await _unitOfWork.UserGroupsRepository.FindAllBy(c => c.User.Id == userId && c.Community.Id == id && c.IsActive == true);
            int _checked = 0;
            foreach (var item in _data.ToList().Where(c => c.Type == 1))
            {
                _checked = 0;
                if (_selectedGroups.Count != 0)
                {
                    _checked = _selectedGroups.Where(c => c.GroupId == item.Id).Count();
                }
                if (_checked == 1)
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name, Selected = "checked" });
                }
                else
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
                }
            }
            listData.GroupsList = _list;
            //Assocations
            _list = new List<SelectListItems>();
            var _selectedAsscocations = await _unitOfWork.UserAssociationsRepository.FindAllBy(c => c.User.Id == userId && c.Community.Id == id && c.IsActive == true);
             _checked = 0;
            foreach (var item in _data.ToList().Where(c => c.Type == 2))
            {
                _checked = 0;
                if (_selectedAsscocations.Count != 0)
                {
                    _checked = _selectedAsscocations.Where(c => c.AssociationId == item.Id).Count();
                }
                if (_checked == 1)
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name, Selected = "checked" });
                }
                else
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
                }
            }
            listData.Association = _list;
            //Interests
            _list = new List<SelectListItems>();
            var _selectedInterests = await _unitOfWork.UserInterestsRepository.FindAllBy(c => c.User.Id == userId && c.Community.Id==id && c.IsActive==true );
             _checked = 0;
            foreach (var item in _data.ToList().Where(c => c.Type == 5))
            {
                _checked = 0;
                if (_selectedInterests.Count != 0)
                {
                    _checked = _selectedInterests.Where(c => c.InterestId == item.Id).Count();
                }
                if (_checked == 1)
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name, Selected = "checked" });
                }
                else
                {
                    _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
                }
            }
            listData.Interests = _list;

            return listData;
        }
        
    }
}
