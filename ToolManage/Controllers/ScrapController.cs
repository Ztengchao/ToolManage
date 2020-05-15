using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManage.Models;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;

namespace ToolManage.Controllers
{
    public class ScrapController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private Account Account => (Account)Session["account"];
        private Authority Authority => (Authority)Session["authority"];

        public ActionResult FirstTrail()
        {
            var scrapApplications = db.ScrapApplication.Where(i => i.State == "0" && i.WorkCellId == Account.WorkCellId);
            ViewBag.Data = scrapApplications.Select(i => new ScrapDetail { Application = i, Entity = i.ToolEntity, Name = i.Account.Name });
            return View();
        }

        public ActionResult FinalTrail()
        {
            var scrapApplications = db.ScrapApplication.Where(i => i.State == "2" && i.WorkCellId == Account.WorkCellId);
            ViewBag.Data = scrapApplications.Select(i => new ScrapDetail { Application = i, Entity = i.ToolEntity, Name = i.Account.Name });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FirstTrail(int Id, bool IsPass)
        {
            var application = db.ScrapApplication.Find(Id);
            if (application == null)
            {
                return RedirectToAction("FirstTrail");
            }
            application.FirstTrialId = Account.Id;
            application.FirstTrialDate = DateTime.Now;
            if (IsPass)
            {
                application.State = "2";
            }
            else
            {
                application.State = "1";
                application.ToolEntity.State = "0";
                db.Entry(application.ToolEntity).State = EntityState.Modified;
            }
            db.Entry(application).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FirstTrail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalTrail(int Id, bool IsPass)
        {
            var application = db.ScrapApplication.Find(Id);
            if (application == null)
            {
                return RedirectToAction("FinalTrail");
            }
            application.FirstTrialId = Account.Id;
            application.FirstTrialDate = DateTime.Now;
            if (IsPass)
            {
                application.State = "3";
                application.ToolEntity.State = "3";
            }
            else
            {
                application.State = "1";
                application.ToolEntity.State = "0";
            }
            db.Entry(application.ToolEntity).State = EntityState.Modified;
            db.Entry(application).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FinalTrail");
        }

        //public ActionResult Detail(int id)
        //{
        //    ViewBag.Data = db.ScrapApplication.Where(i => i.ScrapDocId == id).Select(i =>
        //        new ScrapDetail
        //        {
        //            Application = i,
        //            Entity = i.ToolEntity,
        //            Def = i.ToolEntity.ToolDef,
        //        }).ToList();
        //    ViewBag.Id = id;
        //    return View();
        //}

        //public JsonResult ScrapDoc(int id)
        //{
        //    var doc = new ScrapDoc(db.ScrapDoc.Find(id));
        //    return new JsonResult
        //    {
        //        Data = doc,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        //public JsonResult ScrapApplication(int id)
        //{
        //    var applications = db.ScrapDoc.Find(id).ScrapApplication
        //        .Where(i => i.State == "0")
        //        .Select(i => new
        //        {
        //            i.ToolEntity.Code,
        //            i.ToolEntity.ToolDef.Name,
        //            Family = ToolController.GetInner(i.ToolEntity.ToolDef.FamilyId),
        //            Date = i.Date.ToString(),
        //            ApplicationName = i.Account.Name.Trim(),
        //            i.Reason,
        //        });
        //    return new JsonResult
        //    {
        //        Data = applications,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        //public ActionResult Approval()
        //{
        //    ViewBag.Data = db.WorkCell.Find(Account.WorkCellId).ScrapDoc
        //        .Where(i => i.State != "0" && i.State != "9")
        //        .Select(i => new ScrapApproval { ScrapDoc = i, ApplicationName = i.Account.Name });

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int deleteId)
        //{
        //    var doc = db.ScrapDoc.Find(deleteId);
        //    if (doc == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    doc.State = "9";
        //    db.Entry(doc).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Add(ScrapDoc scrapDoc)
        //{
        //    if (scrapDoc.Id == 0)
        //    {
        //        scrapDoc.CreateDate = DateTime.Now;
        //        scrapDoc.WorkcellId = Account.WorkCellId;
        //        scrapDoc.State = "0";
        //        db.Entry(scrapDoc).State = EntityState.Added;
        //    }
        //    else
        //    {
        //        var oldDoc = db.ScrapDoc.Find(scrapDoc.Id);
        //        if (oldDoc == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        oldDoc.Name = scrapDoc.Name;
        //        oldDoc.Remark = scrapDoc.Remark;
        //        db.Entry(oldDoc).State = EntityState.Modified;
        //    }
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Bind(int bindId)
        //{
        //    var workcell = db.WorkCell.Find(Account.WorkCellId);
        //    workcell.ScrapDocId = bindId;
        //    db.Entry(workcell).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ApprovalConfirm(bool IsPass, int DocId)
        //{
        //    var doc = db.ScrapDoc.Find(DocId);
        //    if (doc == null)
        //    {
        //        return RedirectToAction("Approval");
        //    }

        //    if (IsPass)
        //    {
        //        doc.State = "2";
        //        //doc.ScrapApplication.ForEach(i =>
        //        //{
        //        //    i.State = "1";
        //        //    db.Entry(i).State = EntityState.Modified;
        //        //    i.ToolEntity.State = "3";
        //        //});
        //    }
        //    else
        //    {
        //        doc.State = "3";
        //    }
        //    db.Entry(doc).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Approval");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RequestApproval(int docId)
        //{
        //    var doc = db.ScrapDoc.Find(docId);
        //    if (doc == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    doc.State = "1";
        //    db.Entry(doc).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }

    public class ScrapDetail
    {
        public ScrapApplication Application { get; set; }
        public ToolEntity Entity { get; set; }
        public string Name { get; set; }
    }

    //public class ScrapApproval
    //{
    //    public ScrapDoc ScrapDoc { get; set; }
    //    public string ApplicationName { get; set; }
    //}
}