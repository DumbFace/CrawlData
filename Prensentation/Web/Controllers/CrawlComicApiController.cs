using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;
using CMS.Service.CrawlService;
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
        private readonly ICrawlService _crawlService;
        public CrawComicApiController(ICrawlService crawlService)
        {
            _crawlService = crawlService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CrawlModel model)
        {
            // HtmlDocument htmlDoc = new HtmlDocument();
            // string htmlString = await GetHtmlAsync(model.Url);
            // if (htmlString != null && htmlString != "")
            // {
            //     htmlDoc.LoadHtml(htmlString);
            // }

            // HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            // var content = nodeUrlLink.InnerHtml;

            // var meta = GetMetaTag(htmlDoc);

            // var comic = AddNewComicIfExist(meta);

            // await CrawlDataAsync(htmlDoc, comic);


            await _crawlService.CrawlComicAsync(model.Url);

            return Ok();
        }
    }
}