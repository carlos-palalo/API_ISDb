using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IGeneroService
    {
        bool DeleteGenero(int id);
        ICollection<Genero> GetAll();
        Genero GetGenero(int id);
        Genero PostGenero(Genero genero);
        bool PutGenero(Genero genero);
        Genero Search(string cad);
    }
}