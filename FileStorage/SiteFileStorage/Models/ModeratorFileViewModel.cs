using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class ModeratorFileViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool Permit { get; set; }
        public List<string> Categories { get; set; }
    }
}