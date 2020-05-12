using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManage.Models;

namespace ToolManage.Controllers
{
    public class RepairController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private Account Account => (Account)Session["account"];
        private Authority Authority => (Authority)Session["authority"];
        private static int CountPerPage => 10;

        public ActionResult Index(int nowPage = 0)
        {
            var data = db.RepairApplication.Where(i => i.WorkCellId == Account.WorkCellId && i.State == "1").Select(
                i => new RepairView
                {
                    Id = i.Id,
                    Code = i.ToolEntity.Code,
                    Location = i.ToolEntity.Location,
                    Remark = i.Describe,
                    Date = i.Date
                }).ToList();
            ViewBag.MaxPage = data.Count() / CountPerPage;
            ViewBag.Data = data.OrderBy(i => i.Date).Skip(nowPage * CountPerPage).Take(CountPerPage).ToArray();
            ViewBag.NowPage = nowPage;
            return View();
        }

        public JsonResult Export()
        {
            var data = db.RepairApplication //维修完成和通过审核和报废的维修申请
                .Where(i => i.WorkCellId == Account.WorkCellId && i.State == "1")
                .Select(i => new
                {
                    code = i.ToolEntity.Code,
                    location = i.ToolEntity.Location,
                    createDate = i.Date.ToString(),
                    remark = i.Describe
                });

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = data
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmRepair(int Id, bool state)
        {
            var repairApplication = db.RepairApplication.Find(Id);
            if (repairApplication == null)
            {
                return RedirectToAction("Index");
            }

            if (state)
            {
                repairApplication.State = "3";
                repairApplication.RepairDate = DateTime.Now;
                repairApplication.HandleId = Account.Id;
                repairApplication.ToolEntity.State = "0";
            }
            else
            {
                repairApplication.State = "4";
                repairApplication.ToolEntity.State = "3";
                var application = new ScrapApplication
                {
                    ApplicationId = Account.Id,
                    WorkCellId = Account.WorkCellId,
                    Date = DateTime.Now,
                    Reason = "",
                    ScrapDocId = db.WorkCell.Find(Account.WorkCellId).ScrapDocId,
                    ToolEntityId = repairApplication.ToolEntityId,
                    State = "0",
                    LifeCount = repairApplication.ToolEntity.UsedCount,
                };
                db.Entry(application).State = System.Data.Entity.EntityState.Added;
            }
            db.Entry(repairApplication).State = System.Data.Entity.EntityState.Modified;
            db.Entry(repairApplication.ToolEntity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
    public class RepairView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Location { get; set; }
        public string Remark { get; set; }
        public DateTime Date { get; set; }
    }
}