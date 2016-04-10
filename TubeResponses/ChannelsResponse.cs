using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeCrawler.TubeResponses {
    public class ChannelsResponse {
        //public string kind;
        //public string etag;
        //public PageInfo pageInfo;

        public List<Item> items;

        public class Item {
            public string id;
        }

        //public class PageInfo {
        //    public int totalResults;
        //    public int resultsPerPage;
        //}
    }
}
