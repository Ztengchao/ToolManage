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

        public ActionResult Detail(int id)
        {
            ViewBag.Data = db.ScrapApplication.Where(i => i.ScrapDocId == id).Select(i =>
                new ScrapDetail
                {
                    Application = i,
                    Entity = i.ToolEntity,
                    Def = i.ToolEntity.ToolDef,
                }).ToList();
            ViewBag.Id = id;
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

        public JsonResult ScrapApplication(int id)
        {
            var applications = db.ScrapDoc.Find(id).ScrapApplication
                .Where(i => i.State == "0")
                .Select(i => new
                {
                    i.ToolEntity.Code,
                    i.ToolEntity.ToolDef.Name,
                    Family = ToolController.GetInner(i.ToolEntity.ToolDef.FamilyId),
                    Date = i.Date.ToString(),
                    ApplicationName = i.Account.Name.Trim(),
                    i.Reason,
                });
            return new JsonResult
            {
                Data = applications,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult Approval()
        {
            ViewBag.Data = db.WorkCell.Find(Account.WorkCellId).ScrapDoc
                .Where(i => i.State != "0" && i.State != "9")
                .Select(i => new ScrapApproval { ScrapDoc = i, ApplicationName = i.Account.Name });

            return View();
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
            doc.State = "9";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovalConfirm(bool IsPass, int DocId)
        {
            var doc = db.ScrapDoc.Find(DocId);
            if (doc == null)
            {
                return RedirectToAction("Approval");
            }

            if (IsPass)
            {
                doc.State = "2";
            }
            else
            {
                doc.State = "3";
            }
            db.Entry(doc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Approval");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestApproval(int docId)
        {
            var doc = db.ScrapDoc.Find(docId);
            if (doc == null)
            {
                return HttpNotFound();
            }
            doc.State = "1";
            db.Entry(doc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    public class ScrapDetail
    {
        public ScrapApplication Application { get; set; }
        public ToolEntity Entity { get; set; }
        public ToolDef Def { get; set; }
    }

    public class ScrapApproval
    {
        public ScrapDoc ScrapDoc { get; set; }
        public string ApplicationName { get; set; }
    }
}