using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IGeneralService
    {
        ICollection<Serie> GetAll();
        string GetInfoSerie(int serie);
        ICollection<Serie> SearchSerie(string serie);
    }
}