using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Domain
{
    public class Series
    {
        [Key]
        public int Id { get; set; }
        public int ComicId { get; set; }
        public string Content { get; set; }
        public Comic Comic { get; set; }
    }
}