using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Core.Helper;
using CMS.Data.EFCore;
using HtmlAgilityPack;

namespace CMS.Service.CrawlService
{
    public class CrawlService : ICrawlService
    {

        public int CheckPaging(HtmlDocument htmlDoc)
        {
            int page = 0;
            //Check if paging has Cuối
            var nodesPaging = htmlDoc.DocumentNode.SelectNodes("//ul[@class='pagination pagination-sm']//li//a");

            if (nodesPaging != null)
            {
                foreach (var item in nodesPaging)
                {
                    Console.WriteLine(item.InnerText);
                    if (item.InnerText == "Cuối &raquo;")
                    {
                        if (item.Attributes["title"] != null)
                        {
                            if (item.Attributes["title"].Value != null)
                            {
                                string pageAsString = item.Attributes["title"].Value;
                                Console.WriteLine($"Title is {pageAsString}");
                                var splitString = pageAsString.Split("Trang");
                                page = int.Parse(splitString[splitString.Length - 1]);
                                Console.WriteLine(page);
                                // Console.WriteLine(splitTest)
                                // foreach(string testString in splitTest)
                                // {
                                //     Console.WriteLine($"For string {testString}");
                                // }
                                // Console.WriteLine(splitTest[splitTest.Length - 1].Trim());
                                // page = int.Parse(pageAsString.Substring(pageAsString.Length - 1, 1));
                                // Console.WriteLine($"Page is {page}");
                            }
                        }
                    }
                    else
                    {
                        page = nodesPaging.Count;
                    }
                }
            }
            else
            {
                page = 1;
            }

            return page;
        }

        public async Task InitCrawlingAsync(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string htmlString = await GetHtmlStringAsync(url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }

            //Check if Comic exist
            string urlComic = Extensions.GetUrlTitle(url.Trim());


            //Get page
            int page = CheckPaging(htmlDoc);

            for (int i = 0; i < page; i++)
            {
                if (i == 0)
                {
                    await CrawlingPagingAsync(url, i + 1, htmlDoc);
                }
                else
                {
                    await CrawlingPagingAsync(url, i + 1);

                }
            }
        }

        public async Task<string> GetContentChapterAsync(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string htmlString = await GetHtmlStringAsync(url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }
            HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            var content = nodeUrlLink.InnerHtml;
            return content;
        }

        public async Task<string> GetHtmlStringAsync(string url)
        {
            using (var request = new HttpClient())
            {
                request.DefaultRequestHeaders.Add("User-Agent", "C# program");
                return await request.GetStringAsync(url);
            }
        }

        public MetaModel GetMetaComic(HtmlDocument htmlDoc)
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

        public void ProcessUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void WriteLog()
        {
            throw new NotImplementedException();
        }

        public HtmlNodeCollection SelectMetaNodes(string element, string attribute, string name, HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.SelectNodes($"//{element}[contains(@{attribute},'{name}')]");
        }



        public Comic CheckIfComicExist(string url, HtmlDocument htmlDoc)
        {
            using (var context = new CrawlDB())
            {
                var comic = context.Comics.FirstOrDefault(s => s.Url == url);
                if (comic is null)
                {
                    MetaModel meta = GetMetaComic(htmlDoc);
                    comic = new Comic
                    {
                        Title = meta.Title,
                        Thumb = meta.Image,
                        Url = Extensions.FriendlyUrl(meta.Title)
                    };
                    context.Comics.Add(comic);
                    context.SaveChanges();
                }
                return comic;
            }
        }

        public async Task CrawlingPagingAsync(string url, int page, HtmlDocument htmlDoc = null)
        {

            string urlPaging = $"{url}trang-{page}/";
            if (htmlDoc == null)
            {
                htmlDoc = new HtmlDocument();
                string htmlString = await GetHtmlStringAsync(urlPaging);
                if (htmlString != null && htmlString != "")
                {
                    htmlDoc.LoadHtml(htmlString);
                }
            }

            List<string> lstLinkCrawling = new List<string>();
            HtmlNodeCollection nodeList = htmlDoc.DocumentNode.SelectNodes("//ul[@class='list-chapter']//li//a");


            //Lấy danh sách đường dẫn chap truyện
            if (nodeList != null)
            {
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
            }


            string titleUrl = Extensions.GetUrlTitle(url);

            //Crawl dữ liệu và lưu xuống db
            if (lstLinkCrawling.Count > 0)
            {
                foreach (string urlContent in lstLinkCrawling)
                {

                    string content = await GetContentChapterAsync(urlContent);
                    if (!string.IsNullOrEmpty(content))
                    {
                        using (var context = new CrawlDB())
                        {
                            var comic = context.Comics.FirstOrDefault(s => s.Url == titleUrl);
                            if (comic is null)
                            {
                                MetaModel meta = GetMetaComic(htmlDoc);
                                comic = new Comic
                                {
                                    Title = meta.Title,
                                    Thumb = meta.Image,
                                    Url = Extensions.FriendlyUrl(meta.Title)
                                };
                                context.Comics.Add(comic);
                                context.SaveChanges();
                            }

                            string chap = Extensions.GetChapterViaUrl(urlContent);
                            Series series = new Series
                            {
                                Content = content,
                                Chap = chap,
                                Url = $"{titleUrl}/trang-{chap}/",
                            };
                            // if (comic != null)
                            // {
                            //     comic.Series.Add(series);
                            // }
                            // context.SaveChanges();
                            Console.WriteLine($"{comic.Title} - Chap {series.Chap} - Url - {urlContent}");
                        }
                    }

                }
            }

        }

        // public Task CrawlingAsync(string url, int page, Comic comic, HtmlDocument htmlDoc = null)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
