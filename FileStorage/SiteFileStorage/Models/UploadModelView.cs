using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class UploadModelView
    {
        public string FileName { get; set; }
        public string FileType{get;set;}
        public string FileSize { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Categories { get; set; }
    }
}