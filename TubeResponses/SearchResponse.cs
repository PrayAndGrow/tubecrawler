using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeCrawler.TubeResponses {
    class SearchResponse {

        public List<Item> items;
        public string nextPageToken;
        public string prevPageToken;

        public class Item {
            public Id id;
            public Snippet snippet;
        }

        public class Id {
            public string videoId;
        }

        public class Snippet {
            //System.DateTime publishedAt;

            public string title;
            public string description;

            public Thumbnails thumbnails;
        }

        public class Thumbnails {
            public Thumbnail @default;
            public Thumbnail medium;
            public Thumbnail high;
        }

        public class Thumbnail {
            public string url;
            public int width;
            public int height;
        }
    }
}
