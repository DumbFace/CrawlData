#pragma checksum "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7c91fd8e8608cdb58d03e0c075e1ec0159052ceb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Read), @"mvc.1.0.view", @"/Views/Home/Read.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\_ViewImports.cshtml"
using Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\_ViewImports.cshtml"
using Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\_ViewImports.cshtml"
using CMS.Core.Domain;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c91fd8e8608cdb58d03e0c075e1ec0159052ceb", @"/Views/Home/Read.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"70105755930da41d04ed60076eb27dcdf312b8e3", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Read : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ComicViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n<div class=\"container\">\r\n    <div class=\"row justify-content-center mb-3\">\r\n        <h1>\r\n            <a");
            BeginWriteAttribute("href", " href=\"", 131, "\"", 149, 2);
            WriteAttributeValue("", 138, "/", 138, 1, true);
#nullable restore
#line 7 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml"
WriteAttributeValue("", 139, Model.Url, 139, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                ");
#nullable restore
#line 8 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml"
           Write(Model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </a>\r\n        </h1>\r\n    </div>\r\n    <div class=\"row justify-content-center mb-3\">\r\n        <h2 class=\"text-align-center\">");
#nullable restore
#line 13 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml"
                                 Write(Model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - Chap ");
#nullable restore
#line 13 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml"
                                                     Write(Model.Chap);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    </div>\r\n    <div class=\"row\">\r\n        <div class=\"content\" id=\"content\">\r\n            ");
#nullable restore
#line 17 "D:\WorkSpace\repo\Github\CrawlData\Prensentation\Web\Views\Home\Read.cshtml"
       Write(Html.Raw(Model.Content));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ComicViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591