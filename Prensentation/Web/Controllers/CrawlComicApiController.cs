using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;
using CMS.Data.Service.ComicService;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/comic")]
    public class CrawComicApiController : ControllerBase
    {
        private readonly IComicService _service;
        public CrawComicApiController(IComicService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CrawlModel model)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string htmlString = await GetHtmlAsync(model.Url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }
            // HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            // var content = nodeUrlLink.InnerHtml;

            var meta = GetMetaTag(htmlDoc);

            var comic = AddNewComicIfExist(meta);

            await CrawlDataAsync(htmlDoc, comic);

            return Ok();
        }

        public async Task<string> SendRequestAsync(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string content = "";
            string htmlString = await GetHtmlAsync(url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }

            HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            content = nodeUrlLink.InnerHtml;

            return content;
        }

        public string GetChapter(string url)
        {
            var split = url.Split("/");
            var lastArray = split[split.Length - 2];
            string chapter = lastArray.Substring(lastArray.Length - 1, 1);
            return chapter;
        }


        public async Task CrawlDataAsync(HtmlDocument htmlDocument, Comic comic)
        {
            List<string> lstLinkCrawling = new List<string>();
            var nodeList = htmlDocument.DocumentNode.SelectNodes("//ul[@class='list-chapter']//li//a");
            foreach (var node in nodeList)
            {
                if (node.Attributes["href"] != null)
                {
                    if (node.Attributes["href"].Value != null)
                    {
                        lstLinkCrawling.Add(WebUtility.HtmlDecode(node.Attributes["href"].Value.ToString()));
                    }
                }

            }

            if (lstLinkCrawling.Count > 0)
            {
                foreach (string url in lstLinkCrawling)
                {
                    string content = await SendRequestAsync(url);
                    string chapter = GetChapter(url);
                    Series series = new Series
                    {
                        Content = content,
                        Chap = chapter,
                        Url = "chuong-" + chapter
                    };
                    using (var context = new CrawlDB())
                    {

                        comic.Series.Add(series);
                        context.SaveChanges();
                    }
                }
            }

            //Check if paging has Cuối
            // var nodePaging = htmlDocument.DocumentNode.SelectNodes("//ul[@class='pagination pagination-sm']//li//a");
            // foreach(var item in test)
            // {



            //     if (item.InnerText == "Cuối &raquo;")
            //     {
            //     }

            // }
        }


        public Comic AddNewComicIfExist(MetaModel meta)
        {
            Comic comic = null;
            using (var context = new CrawlDB())
            {
                comic = context.Comics.FirstOrDefault(s => s.Url == myCommon.FriendlyUrl(meta.Title));

                if (comic == null)
                {
                    comic = new Comic
                    {
                        Title = meta.Title,
                        Thumb = meta.Image,
                        Url = myCommon.FriendlyUrl(meta.Title)
                    };

                    context.Comics.Add(comic);
                    context.SaveChanges();
                }
            }
            return comic;
        }

        public MetaModel GetMetaTag(HtmlDocument htmlDoc)
        {
            MetaModel meta = new MetaModel();
            HtmlNodeCollection metaNodes = SelectMetaNodes("meta", "property", "og:", htmlDoc);

            if (metaNodes != null)
            {
                foreach (HtmlNode node in metaNodes)
                {
                    if (node.Attributes["property"].Value.ToLower() == "og:title")
                    {
                        //Giải mã các kí tự " & < >... có trong title
                        meta.Title = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:description")
                    {
                        meta.Description = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:image")
                    {
                        meta.Image = node.Attributes["content"].Value;
                    }
                }
            }
            return meta;
        }

        public HtmlNodeCollection SelectMetaNodes(string element, string attribute, string name, HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.SelectNodes($"//{element}[contains(@{attribute},'{name}')]");
        }

        public async Task<string> GetHtmlAsync(string url)
        {
            using (var request = new HttpClient())
            {
                request.DefaultRequestHeaders.Add("User-Agent", "C# program");
                return await request.GetStringAsync(url);
            }
        }
    }
}