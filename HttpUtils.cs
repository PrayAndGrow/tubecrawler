using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TubeCrawler {
    class HttpUtils {
        public static async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest postData) {
            var respString = await RawPost(url, postData);
            var respContent = JsonConvert.DeserializeObject<TResponse>(respString);
            return respContent;
        }

        public static async Task<string> RawPost<TRequest>(string url, TRequest postData) {
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            Stream dataStream = req.GetRequestStream();
            var sw = new StreamWriter(dataStream);

            var reqContent = JsonConvert.SerializeObject(postData);

            sw.Write(reqContent);
            sw.Close();
            dataStream.Close();

            var resp = await req.GetResponseAsync();
            Stream responseStream = resp.GetResponseStream();

            var sr = new StreamReader(responseStream);

            var respContent = sr.ReadToEnd();
            return respContent;
        }

        public static async Task<TResponse> Get<TResponse>(string url) {
            var respString = await RawGet(url);
            var respContent = JsonConvert.DeserializeObject<TResponse>(respString);
            return respContent;

        }

        public static async Task<string> RawGet(string url) {
            WebRequest req = WebRequest.Create(url);
            req.Method = "GET";

            var resp = await req.GetResponseAsync();
            Stream responseStream = resp.GetResponseStream();

            var sr = new StreamReader(responseStream);

            var respContent = sr.ReadToEnd();
            return respContent;
        }
    }
}