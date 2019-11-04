
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.Communities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;

namespace Sphix.Service.Communities.ComunitySubTypes
{
   public class ComunitySubTypesService:IComunitySubTypesService
    {
        private UnitOfWork _unitOfWork;
        public ComunitySubTypesService(EFDbContext context)
            {
                _unitOfWork = new UnitOfWork(context);
            }
        public async Task<CommunitySubTypesViewModel> getSubCategorybyIdAsync(int Id)
        {
            CommunitySubTypesViewModel model =new CommunitySubTypesViewModel();
            var _result = await _unitOfWork.CommunityCatgoryRepository.GetByID(Id);
            if (_result != null)
            {
                model.Id = _result.Id;
                model.Name = _result.Name;
                model.IsActive = _result.IsActive;
                model.CommunityId = _result.CommunityId;
                model.Type = _result.Type;
            }
            return model;
        }
        public async Task<IEnumerable<CommunityCatgoryDataModel>> getSubCategoreisbyTypeAsync(int communityId)
        {
            return await _unitOfWork.CommunityCatgoryRepository.FindAllBy(c => c.CommunityId == communityId );
        }
        public async Task<BaseModel> SaveAsync(CommunitySubTypesViewModel model)
        {
            try
            {
                CommunityCatgoryDataModel _model = null;
                if (model.Id == 0)
                {
                    if (await NameIsExistOrNot(model.Name, model.Type))
                    {
                        return new BaseModel { Status = false, Messsage = UMessagesInfo.RecordExist };
                    }
                    _model = new CommunityCatgoryDataModel();
                    _model.Name = model.Name;
                    _model.CommunityId = model.CommunityId;
                    _model.Type = model.Type;
                    _model.IsActive = model.IsActive;
                    await _unitOfWork.CommunityCatgoryRepository.Insert(_model);
                }
                else
                {
                    _model = await _unitOfWork.CommunityCatgoryRepository.GetByID(model.Id);
                    if(_model.Name!=model.Name)
                    {
                        if (await NameIsExistOrNot(model.Name,model.Type))
                        {
                            return new BaseModel { Status = false, Messsage = UMessagesInfo.RecordExist };
                        }
                    }
                   
                    _model.Name = model.Name;
                    _model.CommunityId = model.CommunityId;
                    _model.Type = model.Type;
                    _model.IsActive = model.IsActive;
                    await _unitOfWork.CommunityCatgoryRepository.Update(_model);
                }
                return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };
            }
            catch (System.Exception ex)
            {

                return new BaseModel { Status = false, Messsage = ex.Message };
            }
        }
        private async Task<bool> NameIsExistOrNot(string name,int type)
        {
            var _data = await _unitOfWork.CommunityCatgoryRepository.FindAllBy(c => c.Name.ToLower() == name.Trim().ToLower() && c.Type == type);
            if (_data.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
