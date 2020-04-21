using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ToolManage.Models;

namespace ToolManage.Controllers
{
    public class MaintanceController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private Account Account => (Account)Session["account"];
        private Authority Authority => (Authority)Session["authority"];
        private static int CountPerPage => 10;

        public JsonResult CheckDetails(int CheckId)
        {
            var details = db.CheckDetail.Where(i => i.State == "0" && i.CheckTypeId == CheckId);
            return new JsonResult
            {
                Data = details.Select(i => new { i.Id, Name = i.Name.Trim() }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult Index(int nowPage = 0)
        {
            var tooldefs = db.ToolDef.Where(i => i.WorkCellId == Account.WorkCellId);
            var data = new List<ToolEntity>();
            foreach (var toolDef in tooldefs)
            {
                foreach (var toolEntity in toolDef.ToolEntity)
                {
                    if (!toolEntity.CheckDate.HasValue)
                    {
                        data.Add(toolEntity);
                    }
                    else if (toolEntity.CheckDate.Value.AddDays(toolDef.PMPeriod) <= DateTime.Now)
                    {
                        data.Add(toolEntity);
                    }
                }
            }
            ViewBag.MaxPage = data.Count() / CountPerPage;
            ViewBag.Data = data.OrderBy(i => i.CheckDate).Skip(nowPage * CountPerPage).Take(CountPerPage).ToArray();
            ViewBag.NowPage = nowPage;
            return View();
        }

        /// <summary>
        /// 维修管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Type()
        {
            ViewBag.Data = db.CheckType.Where(i => i.WorkcellId == Account.WorkCellId && i.State == "0").Select(
                i => new MaintanceType
                {
                    CheckType = i,
                    CheckDetails = i.CheckDetail.ToList()
                }).ToList();
            return View();
        }

        public ActionResult Detail(int id)
        {
            var toolEntity = db.ToolEntity.Find(id);
            if (toolEntity == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ToolEntity = toolEntity;
            ViewBag.ToolDef = toolEntity.ToolDef;
            ViewBag.SearchWorkcell = new SelectList(db.CheckType.Where(i => i.WorkcellId == Account.WorkCellId && i.State == "0").ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Submit(int Id, int Type, string Name)
        {
            // type对应操作
            // 0 --新增检修项
            // 1 --新增检修类目
            // 2 --删除检修类目
            // 3 --删除检修项
            switch (Type)
            {
                case 0:
                    var item = new CheckDetail
                    {
                        CheckTypeId = Id,
                        Name = Name,
                        State = "0",
                    };
                    db.Entry(item).State = EntityState.Added;
                    break;
                case 1:
                    var category = new CheckType
                    {
                        Name = Name,
                        WorkcellId = Account.WorkCellId,
                        State = "0",
                    };
                    db.Entry(category).State = EntityState.Added;
                    break;
                case 2:
                    var category2 = db.CheckType.Find(Id);
                    if (category2 != null)
                    {
                        category2.State = "1";
                        db.Entry(category2).State = EntityState.Modified;
                    }
                    break;
                case 3:
                    var item2 = db.CheckDetail.Find(Id);
                    if (item2 != null)
                    {
                        item2.State = "1";
                        db.Entry(item2).State = EntityState.Modified;
                    }
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return RedirectToAction("Type");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Check(int selectCheck, string Remark, int ToolEntityId)
        {
            var maintance = new Maintenance
            {
                AccountId = Account.Id,
                CheckTypeId = selectCheck,
                Date = DateTime.Now,
                Remark = Remark,
                ToolEntityId = ToolEntityId
            };
            var toolEntity = db.ToolEntity.Find(ToolEntityId);
            var checkType = db.CheckType.Find(selectCheck);
            bool isPass = true;
            db.Entry(maintance).State = EntityState.Added;
            foreach (var item in checkType.CheckDetail)
            {
                if(Request["radio" + item.Id] == null)
                {
                    continue;
                }
                var detail = new MaintenanceDetail
                {
                    CheckDetailId = item.Id,
                    Maintenance = maintance,
                };
                if (Request["radio" + item.Id] == "1")
                {
                    detail.Success = true;
                }
                else if (Request["radio" + item.Id] == "0")
                {
                    detail.Success = false;
                    isPass = false;
                }

                db.Entry(detail).State = EntityState.Added;
            }

            toolEntity.CheckDate = DateTime.Now;

            if (!isPass)
            {
                var repair = new RepairApplication
                {
                    State = "1",
                    ApplicationId = Account.Id,
                    Date = DateTime.Now,
                    Describe = Remark,
                    WorkCellId = Account.WorkCellId,
                    ToolEntityId = ToolEntityId,
                };
                toolEntity.State = "2";
                db.Entry(repair).State = EntityState.Added;
            }

            db.Entry(toolEntity).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    public class MaintanceType
    {
        public CheckType CheckType { get; set; }
        public List<CheckDetail> CheckDetails { get; set; }
    }
}