using Data.Context;
using Sphix.DataModels.Authorization;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization
{
    public class RoleService : IRoleService
    {
        private UnitOfWork _unitOfWork;
        private readonly EFDbContext _context;
        public RoleService(EFDbContext context)
        {
          _unitOfWork = new UnitOfWork(context);
          _context = context;
        }
        /// <summary>
        /// Function for Insert and Update records
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseModel> SaveAsync(RoleViewModel model)
        {
            RoleDataModel roleModel = new RoleDataModel
            {   Id=model.Id,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Description = model.Description,
                IsActive = model.IsActive,
                RoleColor = model.RoleColor,
                RoleName = model.RoleName
            };
            if (model.Id>0)
            {
             return await Update(roleModel);
            }
            else
            {
              return await Insert(roleModel);
            }
        }
        /// <summary>
        /// Function for deleting records
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public async Task<BaseModel> Delete(long Id)
        {
            if( await _unitOfWork.RoleRepository.Delete(Id)==true)
            {
                return new BaseModel { Id = Id, Status = true, Messsage = UMessagesInfo.RecordDeleted };
            }
            return new BaseModel { Id = Id, Status = false, Messsage = UMessagesInfo.Error };

        }
        public async Task<IList<RoleViewModel>> GetList(SearchFilter model)
        {
            IList<RoleViewModel> list = new List<RoleViewModel>();
            await _context.LoadStoredProc("GetRolesList")
                       .WithSqlParam("Start", model.PageNumber)
                       .WithSqlParam("PageLimit", model.PageLimit)
                       .WithSqlParam("OrderBy", model.OrderBy)
                       .ExecuteStoredProcAsync((handler) =>
                       {
                           list = handler.ReadToList<RoleViewModel>();
                         // do something with your results.
                     });
            return list;
        }
        public async Task<IList<RoleDataModel>> GetActiveRoles()
        {
            return await _unitOfWork.RoleRepository.FindAllBy(c => c.IsActive == true);
        }

        #region private functions
        private async Task<BaseModel> Insert(RoleDataModel model)
        {
            var existingData = _unitOfWork.RoleRepository.FindAllBy(c => c.RoleName == model.RoleName);
            if (existingData != null)
            {
                existingData = null;
                return new BaseModel { Id = 0, Status = false, Messsage = UMessagesInfo.RecordExist };
            }
            existingData = null;
            var _date = await _unitOfWork.RoleRepository.Insert(model);
            return new BaseModel { Id = _date.Id, Status = true, Messsage = UMessagesInfo.RecordSaved };
        }
        private async Task<BaseModel> Update(RoleDataModel model)
        {
            var existingData = _unitOfWork.RoleRepository.FindAllBy(c => c.RoleName == model.RoleName && c.Id != model.Id);
            if (existingData != null)
            {
                existingData = null;
                return new BaseModel { Id = 0, Status = false, Messsage = UMessagesInfo.RecordExist };
            }
            existingData = null;
            var _date = await _unitOfWork.RoleRepository.Update(model);
            return new BaseModel { Id = _date.Id, Status = true, Messsage = UMessagesInfo.RecordSaved };
        }
        #endregion
    }
}
