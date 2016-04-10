using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeCrawler.TubeResponses {
    class VideosResponse {
        public List<Item> items;

        public class Item {
            public string id;
            public Snippet snippet;
            public ContentDetails contentDetails;
        }

        public class Snippet {
            //System.DateTime publishedAt;

            public string title;
            public string description;
            public List<string> tags;

            public Thumbnails thumbnails;
        }

        public class ContentDetails {
            public string duration;
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
