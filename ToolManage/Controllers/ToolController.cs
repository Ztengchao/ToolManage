using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ToolManage.Models;
using System.IO;

namespace ToolManage.Controllers
{
    public class ToolController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();

        // GET: Tool
        public ActionResult Index(ToolDef toolDef, int? id,string familyNoSearch,string modelNoSearch,string partNoSearch,string codeSearch,string UseForSearch, int nowPage=0)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new ToolDef());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToolDef toolDef)
        {
            var account = (Account)Session["account"];
            var file = Request.Files["upload"];
            int index = 0;
            var url = System.Configuration.ConfigurationManager.AppSettings["ToolPictureUrl"];
            var path = string.Format("{1}/{0}", file.FileName, url);
            while (System.IO.File.Exists(path))
            {
                path = string.Format("{2}/{1}{0}", file.FileName, index++, url);
            }
            file.SaveAs(path);
            toolDef.Picture = path;
            toolDef.WorkCellId = account.WorkCellId;
            toolDef.RecordDate = DateTime.Now;
            toolDef.RecordId = account.Id;
            toolDef.EditDate = DateTime.Now;
            toolDef.EditId = account.Id;
            toolDef.OwnerId = account.Id;
            toolDef.State = "0";
            
            var usedFor = Request["usefor"];
            var familyNo = Request["familyNo"];
            var usedForId = db.Inner.FirstOrDefault(i => i.Type == "1" && i.Detail == usedFor);
            if (usedForId == null)
            {
                usedForId = new Inner { Type = "1", Detail = usedFor };
                db.Entry(usedForId).State = EntityState.Added;
                db.SaveChanges();
            }
            else
            {
                toolDef.UsedForId = usedForId.Id;
            }
            var familyId = db.Inner.FirstOrDefault(i => i.Type == "2" && i.Detail == familyNo);
            if (familyId == null)
            {
                familyId = new Inner { Type = "2", Detail = familyNo };
                db.Entry(familyId).State = EntityState.Added;
                db.SaveChanges();
            }
            else
            {
                toolDef.FamilyId = familyId.Id;
            }
            db.Entry(toolDef).State = EntityState.Added;
            db.SaveChanges();
            var changLog = new ChangeLog
            {
                ChangeAccountId = account.Id,
                BeforeChange = -1,
                AfterChange = toolDef.Id,
                Detail = "新建工夹具定义：" + toolDef.Name,
                ChangeDate = DateTime.Now,
                Type = "0",
            };
            db.Entry(changLog).State = EntityState.Added;
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public static string GetInner(int id)
        {
            using (ToolManageDataContext db = new ToolManageDataContext())
            {
                var inner = db.Inner.Find(id);
                if (inner == null)
                {
                    return null;
                }
                return inner.Detail;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}