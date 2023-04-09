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
        Task InitCrawlingAsync(string url);
        Task CrawlingPagingAsync(string url, int page, HtmlDocument htmlDoc = null);

        void ProcessUrl(string url);

        int CheckPaging(HtmlDocument htmlDoc);

        Task<string> GetContentChapterAsync(string url);

        void WriteLog();
        Comic CheckIfComicExist(string url, HtmlDocument htmlDoc);

        MetaModel GetMetaComic(HtmlDocument htmlDoc);

    }
}