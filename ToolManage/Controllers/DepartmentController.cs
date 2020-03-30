using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManage.Models;

namespace ToolManage.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private static int CountPerPage => 10;
        // GET: Department
        public ActionResult Index(WorkCell workCell, int? workcellId, int nowPage = 0)
        {
            var showModal = false;
            var data = db.WorkCell.Where(i => i.State == "0");

            if (workCell == null)
            {
                workCell = new WorkCell();
            }

            if (workcellId != null)
            {
                if (workcellId != -1)
                {
                    workCell = db.WorkCell.Find(workcellId);
                    workCell.ContactPhone = workCell.ContactPhone.Trim();
                    workCell.ContactName = workCell.ContactName.Trim();
                    workCell.Name = workCell.Name.Trim();
                }
                if (workCell.Id == -1 || workCell.State != "0")
                {
                    workCell = new WorkCell
                    {
                        Id = -1,
                        State = "0",
                        Name = "",
                        ContactName = "",
                        ContactPhone = ""
                    };
                }
                showModal = true;
            }
            ViewBag.MaxPage = data.Count() / CountPerPage;
            ViewBag.NowPage = nowPage;
            ViewBag.ShowModal = showModal;
            ViewBag.Data = data.OrderBy(i => i.Id).Skip(nowPage * CountPerPage).Take(CountPerPage).ToArray();
            return View(workCell);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(WorkCell workcell)
        {
            if (workcell.Id != -1)
            {
                db.Entry(workcell).State = EntityState.Modified;
            }
            else
            {
                workcell.State = "0";
                db.Entry(workcell).State = EntityState.Added;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int workcellId)
        {
            var workcell = db.WorkCell.Find(workcellId);
            if (workcell == null)
            {
                return RedirectToAction("Index");
            }

            workcell.State = "1";
            db.Entry(workcell).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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