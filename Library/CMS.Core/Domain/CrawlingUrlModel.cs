using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Domain
{
    public class CrawlingUrlModel
    {
        public string Url { get; set; }
        public string UrlTitle { get; set; }
        public string UrlPage { get; set; }
    }
}