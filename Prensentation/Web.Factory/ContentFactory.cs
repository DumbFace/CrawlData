using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;

namespace Web.Factory
{
    public class ContentFactory : IContentFactory
    {
        private readonly IRepository<Comic> _repoComic;
        public ContentFactory(
            IRepository<Comic> repoComic
        )
        {
            _repoComic = repoComic;
        }
    }
}