using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;

namespace CMS.Data.Service.ComicService
{
    public class ComicService : Repository<Comic>, IComicService 
    {
        
    }
}