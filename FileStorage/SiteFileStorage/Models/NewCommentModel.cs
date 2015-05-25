using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class NewCommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}