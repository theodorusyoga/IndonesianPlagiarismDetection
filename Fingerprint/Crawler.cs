using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrawlerLib.Engine;

namespace Fingerprint
{
    public class Crawler
    {
        private List<string> urlList;
        private List<string> queryList;
        public Crawler(string[] ul, string[] ql)
        {
            this.urlList = ul.ToList();
            this.queryList = ql.ToList();

        }

        private void StartCrawler()
        {
            CrawlerEngineConfig config = new CrawlerEngineConfig();
            config.MaxTasksPerMinute = 10;
            config.MaxFinishedTasks = 1000;
            config.MaxWorkingTasks = 3;
            using (CrawlerEngine engine = new CrawlerEngine(config))
            {
                
            }
        }
    }
}
