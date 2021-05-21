using API_ISDb.Models;
using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IImdbService
    {
        bool GenerateBBDD();
        string Request(string url);
    }
}