using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IRepartoService
    {
        bool DeleteReparto(int id);
        ICollection<Reparto> GetAll();
        Reparto GetReparto(int id);
        Reparto PostReparto(Reparto reparto);
        bool PutReparto(Reparto reparto);
        Reparto Search(string cad);
        ICollection<ListaReparto> GetRepartos(int id);
    }
}