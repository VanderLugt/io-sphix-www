using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AWSS3.Utility;
using Data.Context;
using Microsoft.AspNetCore.Http;
using Sphix.DataModels.Communities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;
using Sphix.ViewModels.User;

namespace Sphix.Service.Communities
{
    public class CommunitiesService : ICommunitiesService
    {
        private UnitOfWork _unitOfWork ;
        private List<SelectListItems> _list;
        private readonly EFDbContext _context;
        private readonly IAWSS3Bucket _awsS3Bucket;
        public CommunitiesService(EFDbContext context, IAWSS3Bucket awsS3Bucket)
        {
            _unitOfWork = new UnitOfWork(context);
            _context = context;
            _awsS3Bucket = awsS3Bucket;
        }
        public async Task<BaseModel> SaveAsync(CommunityTypeViewModel model, IFormFile articleShareDocument)
        {
          
            try
            {
                //save with attcahed doc
                string articeDocUrl = string.Empty;
                string articleFolderPath = "CommunityTypes/" + DateTime.Now.Year.ToString();
                string articleFileName = string.Empty;
                if (articleShareDocument != null && articleShareDocument.Length > 0)
                {
                    articleFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(articleShareDocument.FileName);
                    articeDocUrl = articleFolderPath + "/" + articleFileName;
                    //model.ImageUrl = articeDocUrl;
                }
                if (model.Id == 0)
                {
                    CommunityDataModel communityData = new CommunityDataModel();
                    if (articleShareDocument != null && articleShareDocument.Length > 0)
                    {
                        await _awsS3Bucket.UploadFileAsync(articleFolderPath, articleShareDocument, articleFileName);
                        communityData.ImageUrl = articeDocUrl;
                    }
                    communityData.Name = model.Name;
                    communityData.Description = model.Description;
                    communityData.IsActive = model.IsActive;
                    communityData.Color = model.Color;
                    communityData.FooterLinkText = model.FooterLinkText;
                    communityData.DisplayIndex = model.DisplayIndex;
                    communityData.CommunityUrl = Urlhelper.GenerateSeoFriendlyURL(model.Name);
                   // communityData.CommunityUrl = model.CommunityUrl;
                    await _unitOfWork.CommunityRepository.Insert(communityData);
                }
                else
                {
                    CommunityDataModel communityData = await _unitOfWork.CommunityRepository.GetByID(Convert.ToInt32(model.Id));
                    if (articleShareDocument != null && articleShareDocument.Length > 0)
                    {
                        //first delete old file
                        await _awsS3Bucket.DeleteFileAsync(communityData.ImageUrl);
                        await _awsS3Bucket.UploadFileAsync(articleFolderPath, articleShareDocument, articleFileName);
                        communityData.ImageUrl = articeDocUrl;
                    }
                    communityData.Name = model.Name;
                    communityData.Description = model.Description;
                    communityData.IsActive = model.IsActive;
                    communityData.Color = model.Color;
                    communityData.FooterLinkText = model.FooterLinkText;
                    communityData.DisplayIndex = model.DisplayIndex;
                   // communityData.CommunityUrl = Urlhelper.GenerateSeoFriendlyURL(model.Name);
                    await _unitOfWork.CommunityRepository.Update(communityData);
                    //baseModel = await Update(model);
                }

                return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
            }
            catch (Exception)
            {

                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
        public async Task<IList<SelectListItems>> GetActiveCommunities()
        {
            var _data = await _unitOfWork.CommunityRepository.FindAllBy(c=>c.IsActive==true);
            _list = new List<SelectListItems>();
            foreach (var item in _data)
            {
                _list.Add(new SelectListItems {  Value = item.Id, Text = item.Name });
            }
            return _list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cType"></param>
        /// <returns></returns>
        public async Task<SignUpStep3ViewModel> GetActiveSubCommunities(int id)
        {
            SignUpStep3ViewModel listData = new SignUpStep3ViewModel();
            IList<CommunityCatgoryDataModel> _data = await _unitOfWork.CommunityCatgoryRepository.FindAllBy(c => c.CommunityId == id && c.IsActive==true);

            //Groups
            _list = new List<SelectListItems>();
            foreach (var item in _data.ToList().Where(c => c.Type == 1))
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            listData.GroupsList = _list;
            
            //Association
            _list = new List<SelectListItems>();
            foreach (var item in _data.ToList().Where(c => c.Type == 2))
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            listData.Association = _list;
            //Type1List
            _list = new List<SelectListItems>();
            foreach (var item in _data.ToList().Where(c => c.Type == 3))
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            listData.Type1List = _list;

            //Type2List
            _list = new List<SelectListItems>();
            foreach (var item in _data.ToList().Where(c => c.Type == 4))
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            listData.Type2List = _list;

            //Interests
            _list = new List<SelectListItems>();
            foreach (var item in _data.ToList().Where(c => c.Type == 5))
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            listData.Interests = _list;

            return listData;
        }
        public async Task<IList<SelectListItems>> GetActiveCommunityThemes(int id)
        {
            var _data = await _unitOfWork.CommunityThemesRepository.FindAllBy(c => c.IsActive == true);
            _list = new List<SelectListItems>();
            foreach (var item in _data)
            {
                _list.Add(new SelectListItems { Value = item.Id, Text = item.Name });
            }
            return _list;
        }
        public async Task<CommunityTypeViewModel> getCommunityTypeAsync(int id)
        {
            var _result =await _unitOfWork.CommunityRepository.GetByID(id);
            CommunityTypeViewModel model = new CommunityTypeViewModel();
            if (_result != null)
            {
                model.Id = _result.Id;
                model.Name = _result.Name;
                model.Color = _result.Color;
                model.Description = _result.Description;
                model.ImageUrl = _result.ImageUrl;
                model.IsActive = _result.IsActive;
                model.CommunityUrl = _result.CommunityUrl;
                model.FooterLinkText = _result.FooterLinkText;
                model.DisplayIndex = _result.DisplayIndex;
            }
           
            return model;
        }
        public async Task<IList<CommunityTypesListViewModel>> getCommunitiesGroupsTypeList(CustomeSearchFilter model)
        {
            IList<CommunityTypesListViewModel> list = new List<CommunityTypesListViewModel>();
            await _context.LoadStoredProc("GetAdminCommunitiesTypeList")
                       .WithSqlParam("IsActive", model.IsPublish)
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .WithSqlParam("SearchValue", model.SearchValue)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<CommunityTypesListViewModel>();
                           // do something with your results.
                       });
            return list;
        }
    }
}
