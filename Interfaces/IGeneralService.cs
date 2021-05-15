using API_ISDb.Models;
using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IGeneralService
    {
        ICollection<Serie> GetAll();
        InfoSerie GetInfoSerie(int serie);
        IEnumerable<SearchSerie> SearchSerie();
    }
}