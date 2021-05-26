namespace API_ISDb.Interfaces
{
    public interface IImdbService
    {
        bool GenerateBBDD();
        string Request(string url);
    }
}