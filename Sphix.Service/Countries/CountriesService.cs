using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels;
using Sphix.UnitOfWorks;

namespace Sphix.Service.Countries
{
    public class CountriesService : ICountriesService
    {
        private UnitOfWork _unitOfWork;
        public CountriesService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<IEnumerable<CountriesDataModel>> GetList()
        {
            return await _unitOfWork.CountryRepository.FindAllBy(c=>c.IsActive==true);
        }
    }
}
