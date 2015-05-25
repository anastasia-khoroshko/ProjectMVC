using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class FileViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int CurrentUserId { get; set; }
        public double Score { get; set; }
        public IEnumerable<int> RatedUser { get; set; }
    }
}