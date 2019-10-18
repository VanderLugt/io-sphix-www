using Sphix.DataModels.Logger;
using System.Threading.Tasks;

namespace Sphix.Service.Logger
{
   public interface ILoggerService
    {
        Task<bool> AddAsync(LoggerDataModel model);
    }
}
