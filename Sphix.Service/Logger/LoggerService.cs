using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.Logger;
using Sphix.UnitOfWorks;

namespace Sphix.Service.Logger
{
    public class LoggerService : ILoggerService
    {
        private UnitOfWork _unitOfWork;
        public LoggerService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<bool> AddAsync(LoggerDataModel model)
        {
            await _unitOfWork.LoggerRepository.Insert(model);
            return true;
        }
    }
}
