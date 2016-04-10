using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TubeCrawler.Models;
using TubeCrawler.TubeResponses;

namespace TubeCrawler {
    public class TubeCrawler {

        string OutputFile = "output.txt";

        // TODO move this key to a sane place when this ht is over
        string ApiKey;

        List<BrainBit> brainBitList = new List<BrainBit>();

        // TODO move it to a server or at least to a file when this ht is over
        static string[] ChannelNames = new string[] {
            "portalDEONpl",
            "stacja7pl",
            "Langustanapalmie",
        };

        public async Task Run(string ApiKey) {
            //var tasks = ChannelNames.Select(channel => CrawlChannel(channel)).ToArray();
            //foreach (var task in tasks) {
            //    await task;
            //}
            foreach (var ch in ChannelNames) {
                await CrawlChannel(ch);
            }
        }

        private async Task CrawlChannel(string channelName) {
            var url1 = string.Format("https://www.googleapis.com/youtube/v3/channels?key={0}&forUsername={1}&part=id", ApiKey, channelName);
            var channelDetails = await HttpUtils.Get<ChannelsResponse>(url1);

            string channelId = channelDetails.items[0].id;

            await CrawlPortion(channelId, null);

            //File.WriteAllText(channelName + ".json", JsonConvert.SerializeObject(brainBitList, Newtonsoft.Json.Formatting.Indented));

            foreach (var vid in brainBitList) {
                string ret = "";
                try {
                    ret = await HttpUtils.RawPost("http://api.prayandgrow.garapich.pl/brainfood/", vid);
                } catch(Exception ex) {
                    Console.WriteLine(ex);
                    Console.WriteLine("Failed" + vid.title);
                }
                await Task.Delay(3000);
                Console.WriteLine(vid.title);
            }

            Console.ReadKey();

        }

        private async Task CrawlPortion(string channelId, string continuationToken) {
            SearchResponse searchResults = await GetSearchResults(channelId, continuationToken);
            VideosResponse videosDetails = await GetVideosDetails(searchResults);

            foreach (var vidDet in videosDetails.items) {
                CollectVideo(searchResults, vidDet);
            }

            if (!string.IsNullOrEmpty(searchResults.nextPageToken)) {
                await CrawlPortion(channelId, searchResults.nextPageToken);
            }
        }

        private void CollectVideo(SearchResponse searchResults, VideosResponse.Item vidDet) {
            brainBitList.Add(new BrainBit {
                title = vidDet.snippet.title,
                description = searchResults.items.Find(x => x.id.videoId == vidDet.id).snippet.description,
                url = "https://www.youtube.com/watch?v=" + vidDet.id,
                type = "VI",
                image = vidDet.snippet.thumbnails.high.url,
                duration = XmlConvert.ToTimeSpan(vidDet.contentDetails.duration).ToString("g"),
                tags = vidDet.snippet.tags ?? new List<string>()
            });
        }

        private static async Task<VideosResponse> GetVideosDetails(SearchResponse searchResults) {
            StringBuilder sb3 = new StringBuilder();
            sb3.Append("https://www.googleapis.com/youtube/v3/videos");
            sb3.Append("?key=");
            sb3.Append(ApiKey);
            sb3.Append("&id=");
            sb3.Append(string.Join(",", searchResults.items.Select(x => x.id.videoId)));
            sb3.Append("&part=id,snippet,contentDetails");

            var videosDetails = await HttpUtils.Get<VideosResponse>(sb3.ToString());
            return videosDetails;
        }

        private static async Task<SearchResponse> GetSearchResults(string channelId, string continuationToken) {
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("https://www.googleapis.com/youtube/v3/search");
            sb2.Append("?key=");
            sb2.Append(ApiKey);
            sb2.Append("&channelId=");
            sb2.Append(channelId);
            sb2.Append("&part=id,snippet");
            sb2.Append("&maxResults=50");

            if (!string.IsNullOrEmpty(continuationToken)) {
                sb2.Append("&pageToken=");
                sb2.Append(continuationToken);
            }

            var searchResults = await HttpUtils.Get<SearchResponse>(sb2.ToString());
            return searchResults;
        }
    }
}