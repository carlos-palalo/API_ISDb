using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface ISerieRepartoService
    {
        bool DeleteSerieReparto(int serie, int reparto);
        ICollection<SerieReparto> GetAll();
        SerieReparto GetSerieReparto(int serie, int reparto);
        SerieReparto PostSerieReparto(SerieReparto serieReparto);
        bool PutSerieReparto(SerieReparto serieReparto, int serie, int reparto);
    }
}