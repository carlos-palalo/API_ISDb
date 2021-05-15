using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface ISerieService
    {
        bool DeleteSerie(int id);
        ICollection<Serie> GetAll();
        Serie GetSerie(int id);
        Serie PostSerie(Serie serie);
        bool PutSerie(Serie serie);
        Serie Search(string cad);
        IEnumerable<SearchSerie> SearchSerie();
    }
}