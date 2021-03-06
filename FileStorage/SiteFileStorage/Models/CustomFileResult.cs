﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SiteFileStorage.Models
{
    public class CustomFileResult : ActionResult
    {
        private string FileName { get; set; }
        public CustomFileResult(string name)
        {
            FileName = name;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var filePath = HostingEnvironment.MapPath(@"~/UploadFiles/" + FileName);
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }
        }
    }
}