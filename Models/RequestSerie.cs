using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Models
{
    public class RequestSerie
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class StarList
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class ActorList
        {
            public string id { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public string asCharacter { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Directors
        {
            public string job { get; set; }
            public List<Item> items { get; set; }
        }

        public class Writers
        {
            public string job { get; set; }
            public List<Item> items { get; set; }
        }

        public class Actor
        {
            public string id { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public string asCharacter { get; set; }
        }

        public class Others
        {
            public string job { get; set; }
            public List<Item> items { get; set; }
        }

        public class FullCast
        {
            public string imDbId { get; set; }
            public string title { get; set; }
            public string fullTitle { get; set; }
            public string type { get; set; }
            public string year { get; set; }
            public Directors directors { get; set; }
            public Writers writers { get; set; }
            public List<Actor> actors { get; set; }
            public List<Others> others { get; set; }
            public string errorMessage { get; set; }
        }

        public class GenreList
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class CompanyList
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class CountryList
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class LanguageList
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class Trailer
        {
            public string imDbId { get; set; }
            public string title { get; set; }
            public string fullTitle { get; set; }
            public string type { get; set; }
            public string year { get; set; }
            public string videoId { get; set; }
            public string videoTitle { get; set; }
            public string videoDescription { get; set; }
            public string thumbnailUrl { get; set; }
            public string uploadDate { get; set; }
            public string link { get; set; }
            public string linkEmbed { get; set; }
            public string errorMessage { get; set; }
        }

        public class BoxOffice
        {
            public string budget { get; set; }
            public string openingWeekendUSA { get; set; }
            public string grossUSA { get; set; }
            public string cumulativeWorldwideGross { get; set; }
        }

        public class Similar
        {
            public string id { get; set; }
            public string title { get; set; }
            public string fullTitle { get; set; }
            public string year { get; set; }
            public string image { get; set; }
            public string plot { get; set; }
            public string directors { get; set; }
            public string stars { get; set; }
            public string genres { get; set; }
            public string imDbRating { get; set; }
        }

        public class CreatorList
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class TvSeriesInfo
        {
            public string yearEnd { get; set; }
            public string creators { get; set; }
            public List<CreatorList> creatorList { get; set; }
            public List<string> seasons { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string title { get; set; }
            public string originalTitle { get; set; }
            public string fullTitle { get; set; }
            public string type { get; set; }
            public string year { get; set; }
            public string image { get; set; }
            public string releaseDate { get; set; }
            public string runtimeMins { get; set; }
            public string runtimeStr { get; set; }
            public string plot { get; set; }
            public string plotLocal { get; set; }
            public bool plotLocalIsRtl { get; set; }
            public string awards { get; set; }
            public string directors { get; set; }
            public List<object> directorList { get; set; }
            public string writers { get; set; }
            public List<object> writerList { get; set; }
            public string stars { get; set; }
            public List<StarList> starList { get; set; }
            public List<ActorList> actorList { get; set; }
            public FullCast fullCast { get; set; }
            public string genres { get; set; }
            public List<GenreList> genreList { get; set; }
            public string companies { get; set; }
            public List<CompanyList> companyList { get; set; }
            public string countries { get; set; }
            public List<CountryList> countryList { get; set; }
            public string languages { get; set; }
            public List<LanguageList> languageList { get; set; }
            public string contentRating { get; set; }
            public string imDbRating { get; set; }
            public string imDbRatingVotes { get; set; }
            public string metacriticRating { get; set; }
            public object ratings { get; set; }
            public object wikipedia { get; set; }
            public object posters { get; set; }
            public object images { get; set; }
            public Trailer trailer { get; set; }
            public BoxOffice boxOffice { get; set; }
            public string tagline { get; set; }
            public string keywords { get; set; }
            public List<string> keywordList { get; set; }
            public List<Similar> similars { get; set; }
            public TvSeriesInfo tvSeriesInfo { get; set; }
            public object tvEpisodeInfo { get; set; }
            public string errorMessage { get; set; }
        }


    }
}
