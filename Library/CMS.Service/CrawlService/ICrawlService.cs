using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Domain;
using HtmlAgilityPack;

namespace CMS.Service.CrawlService
{
    public interface ICrawlService
    {
        Task<string> GetHtmlStringAsync(string url);
        Task CrawlComicAsync(string url);
        void ProcessUrl(string url);

        int CheckPaging(HtmlDocument htmlDoc);

        Task<string> GetContentChapterAsync(string url);

        void WriteLog();

        MetaModel GetMetaComic(HtmlDocument htmlDoc);
    }
}