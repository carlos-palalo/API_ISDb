using API_ISDb.Models;
using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IImdbService
    {
        bool GenerateBBDD();
        string Request(string url);
        void WriteActor(List<RequestSerie.Actor> items, Serie ser, string tipo);
        bool WriteBBDD(string infoserie);
        void WriteGenero(List<RequestSerie.GenreList> items, Serie ser);
        void WriteItem(List<RequestSerie.Item> items, Serie ser, string tipo);
    }
}