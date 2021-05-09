using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface ISerieGeneroService
    {
        bool DeleteSerieGenero(int serie, int genero);
        ICollection<SerieGenero> GetAll();
        SerieGenero GetSerieGenero(int serie, int genero);
        SerieGenero PostSerieGenero(SerieGenero serieGenero);
        bool PutSerieGenero(SerieGenero serieGenero, int serie, int genero);
    }
}