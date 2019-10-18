using Sphix.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.Countries
{
   public interface ICountriesService
    {
       Task<IEnumerable<CountriesDataModel>> GetList();
    }
}
