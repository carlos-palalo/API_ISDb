using System.Collections.Generic;

namespace API_ISDb.Models
{
    public class RequestSeries
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Item
        {
            public string id { get; set; }
            public string rank { get; set; }
            public string title { get; set; }
            public string fullTitle { get; set; }
            public string year { get; set; }
            public string image { get; set; }
            public string crew { get; set; }
            public string imDbRating { get; set; }
            public string imDbRatingCount { get; set; }
        }

        public class Root
        {
            public List<Item> items { get; set; }
            public string errorMessage { get; set; }
        }
    }
}
