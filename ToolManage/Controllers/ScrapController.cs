using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManage.Models;
using System.Data.Entity;

namespace ToolManage.Controllers
{
    public class ScrapController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private Account Account => (Account)Session["account"];
        private Authority Authority => (Authority)Session["authority"];

        public ActionResult Index()
        {
            var scrapDocs = db.ScrapDoc.Where(i => i.WorkcellId == Account.WorkCellId && i.State == "0");

            ViewBag.SelectedDoc = db.WorkCell.Where(i => i.Id == Account.WorkCellId).First().ScrapDocId;
            ViewBag.Data = scrapDocs;
            return View();
        }

        public JsonResult ScrapDoc(int id)
        {
            var doc = new ScrapDoc(db.ScrapDoc.Find(id));
            return new JsonResult
            {
                Data = doc,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int deleteId)
        {
            var doc = db.ScrapDoc.Find(deleteId);
            if (doc == null)
            {
                return HttpNotFound();
            }
            doc.State = "1";
            db.Entry(doc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ScrapDoc scrapDoc)
        {
            if (scrapDoc.Id == 0)
            {
                scrapDoc.CreateDate = DateTime.Now;
                scrapDoc.WorkcellId = Account.WorkCellId;
                scrapDoc.State = "0";
                db.Entry(scrapDoc).State = EntityState.Added;
            }
            else
            {
                var oldDoc = db.ScrapDoc.Find(scrapDoc.Id);
                if (oldDoc == null)
                {
                    return HttpNotFound();
                }
                oldDoc.Name = scrapDoc.Name;
                oldDoc.Remark = scrapDoc.Remark;
                db.Entry(oldDoc).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bind(int bindId)
        {
            var workcell = db.WorkCell.Find(Account.WorkCellId);
            workcell.ScrapDocId = bindId;
            db.Entry(workcell).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}