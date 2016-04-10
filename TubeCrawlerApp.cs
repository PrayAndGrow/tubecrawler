using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeCrawler {

    class TubeCrawlerApp {

        static void Main(string[] args) {
            if (args.Length != 1) {
                Console.WriteLine("One parameter should be passed to thos application, and it should be a google Api Key");
            } else {
                new TubeCrawler().Run(args[0]).Wait();
            }
        }
    }
}