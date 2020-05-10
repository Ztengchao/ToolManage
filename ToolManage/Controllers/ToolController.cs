using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ToolManage.Models;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

namespace ToolManage.Controllers
{
    public class ToolController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private Account Account => (Account)Session["account"];
        private static int CountPerPage => 10;

        public ActionResult Img(int id)
        {
            var def = db.ToolDef.Find(id);
            if (def == null || !System.IO.File.Exists(def.Picture))
            {
                return new EmptyResult();
            }
            var img = System.IO.File.ReadAllBytes(def.Picture);
            return new FileContentResult(img, "image/jpeg");
        }

        public JsonResult Export(string FamilyNo, string ModelNo, string PartNo, string Code, string UsedFor)
        {

            var data = db.ToolEntity.Where(i => i.State == "0" && i.ToolDef.WorkCellId == Account.WorkCellId);
            #region 搜索
            if (!string.IsNullOrWhiteSpace(FamilyNo))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(FamilyNo) && t.Type == "2" && t.Id == i.FamilyId) != null);
            }
            if (!string.IsNullOrWhiteSpace(UsedFor))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(UsedFor) && t.Type == "1" && i.UsedForId == t.Id) != null);
            }
            if (!string.IsNullOrWhiteSpace(PartNo))
            {
                data = data.Where(i => i.PartNo.Contains(PartNo));
            }
            if (!string.IsNullOrWhiteSpace(ModelNo))
            {
                data = data.Where(i => i.Model.Contains(ModelNo));
            }
            if (!string.IsNullOrWhiteSpace(Code))
            {
                data = data.Where(i => i.Code.Contains(Code));
            }
            #endregion

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = data.Select(i => new
                {
                    code = i.Code.Split('-')[0],
                    seqId = i.Code.Split('-')[1],
                    billNo = i.BillNo,
                    regDate = i.RegDate.ToString(),
                    usedCount = i.UsedCount,
                    location = i.Location,
                })
            };
        }

        // GET: Tool
        public ActionResult Index(ToolDef toolDef, int? toolId, string errorMessage, string familyNoSearch, string modelNoSearch, string partNoSearch, string codeSearch, string UseForSearch, int nowPage = 0)
        {
            var account = (Account)Session["account"];
            var data = db.ToolDef.Where(i => i.State == "0" && i.WorkCellId == account.WorkCellId);
            var showModal = false;
            if (toolId.HasValue)
            {
                toolDef = db.ToolDef.Find(toolId.Value);
                if (toolDef != null)
                {
                    showModal = true;
                }
                else
                {
                    toolDef = new ToolDef();
                }
            }

            #region 搜索
            if (!string.IsNullOrWhiteSpace(familyNoSearch))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(familyNoSearch) && t.Type == "2" && t.Id == i.FamilyId) != null);
            }
            if (!string.IsNullOrWhiteSpace(UseForSearch))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(UseForSearch) && t.Type == "1" && i.UsedForId == t.Id) != null);
            }
            if (!string.IsNullOrWhiteSpace(partNoSearch))
            {
                data = data.Where(i => i.PartNo.Contains(partNoSearch));
            }
            if (!string.IsNullOrWhiteSpace(modelNoSearch))
            {
                data = data.Where(i => i.Model.Contains(modelNoSearch));
            }
            if (!string.IsNullOrWhiteSpace(codeSearch))
            {
                data = data.Where(i => i.Code.Contains(codeSearch));
            }
            #endregion

            ViewBag.ErrorMessage = errorMessage;
            ViewBag.MaxPage = data.Count() / CountPerPage;
            data = data.OrderBy(i => i.Id).Skip(nowPage * CountPerPage).Take(CountPerPage);
            ViewBag.Data = data.ToArray();
            ViewBag.NowPage = nowPage;
            ViewBag.ShowModal = showModal;
            return View(toolDef);
        }

        public ActionResult Create()
        {
            return View(new ToolDef());
        }

        public ActionResult Return()
        {
            ViewBag.Data = db.ConsumeReturn.Where(i => i.AccountId == Account.Id).ToList();
            return View();
        }

        public ActionResult Detail(int id)
        {
            var toolDef = db.ToolDef.Find(id);
            if (toolDef == null || toolDef.WorkCellId != ((Account)Session["account"]).WorkCellId || toolDef.State == "1")
            {
                return RedirectToAction("Index", new { errorMessage = "未找到该工夹具定义" });
            }
            ViewBag.Data = toolDef.ToolEntity.Where(i => i.State != "9").ToList();
            return View(toolDef);
        }

        public ActionResult Borrow(string errorMessage, string familyNoSearch, string modelNoSearch, string partNoSearch, string codeSearch, string UseForSearch, int nowPage = 0)
        {
            var workcellId = ((Account)Session["account"]).WorkCellId;
            var account = (Account)Session["account"];
            var data = db.ToolDef.Where(i => i.State == "0" && i.WorkCellId == account.WorkCellId);

            #region 搜索
            if (!string.IsNullOrWhiteSpace(familyNoSearch))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(familyNoSearch) && t.Type == "2" && t.Id == i.FamilyId) != null);
            }
            if (!string.IsNullOrWhiteSpace(UseForSearch))
            {
                data = data.Where(i => db.Inner.FirstOrDefault(t => t.Detail.Contains(UseForSearch) && t.Type == "1" && i.UsedForId == t.Id) != null);
            }
            if (!string.IsNullOrWhiteSpace(partNoSearch))
            {
                data = data.Where(i => i.PartNo.Contains(partNoSearch));
            }
            if (!string.IsNullOrWhiteSpace(modelNoSearch))
            {
                data = data.Where(i => i.Model.Contains(modelNoSearch));
            }
            if (!string.IsNullOrWhiteSpace(codeSearch))
            {
                data = data.Where(i => i.Code.Contains(codeSearch));
            }
            #endregion

            ViewBag.ErrorMessage = errorMessage;
            ViewBag.MaxPage = data.Count() / CountPerPage;
            data = data.OrderBy(i => i.Id).Skip(nowPage * CountPerPage).Take(CountPerPage);
            ViewBag.Data = data.ToArray();
            ViewBag.NowPage = nowPage;
            return View();
        }

        public ActionResult BorrowDetail(int id)
        {
            var define = db.ToolDef.Find(id);
            if (define == null)
            {
                return HttpNotFound();
            }

            return View(define);
        }

        public ActionResult Delete(int id)
        {
            var toolEntity = db.ToolEntity.Find(id);
            if (toolEntity == null)
            {
                return RedirectToAction("Index");
            }
            toolEntity.State = "9";
            db.Entry(toolEntity).State = EntityState.Modified;
            db.SaveChanges();
            Log(id, "删除工夹具实体：" + toolEntity.Code, "1");
            return RedirectToAction("Detail", new { id = toolEntity.ToolDefId });
        }

        public ActionResult Repair(int nowPage = 0)
        {
            var data = db.RepairApplication.Where(i => i.WorkCellId == Account.WorkCellId && i.State == "0");
            ViewBag.MaxPage = data.Count() / CountPerPage;
            data = data.OrderBy(i => i.Date).Skip(nowPage * CountPerPage).Take(CountPerPage);
            ViewBag.Data = data.ToArray();
            ViewBag.NowPage = nowPage;
            return View();
        }

        public ActionResult EntityDetail(int id)
        {
            var entity = db.ToolEntity.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            var data = new List<EntityDetailData>();
            foreach (var item in entity.Maintenance)
            {
                data.Add(new EntityDetailData
                {
                    Date = item.Date,
                    Name = item.Account.Name,
                    Type = "维护",
                });
            }

            foreach (var item in entity.ConsumeReturn)
            {
                if (item.BorrowReturn)
                {
                    //已归还
                    data.Add(new EntityDetailData
                    {
                        Date = item.ReturnDate.Value,
                        Name = item.Account.Name,
                        Type = "归还"
                    });
                }

                data.Add(new EntityDetailData
                {
                    Date = item.Date,
                    Name = item.Account.Name,
                    Type = "归还"
                });
            }

            foreach (var item in entity.RepairApplication.Where(i => i.State == "3"))
            {
                //维修完成的维修申请
                data.Add(new EntityDetailData
                {
                    Date = item.RepairDate,
                    Name = item.Account.Name,
                    Type = "维修"
                });
            }

            ViewBag.Data = data.OrderByDescending(i => i.Date);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RepairAgree()
        {
            if (Request["applicationId"] == null || Request["agree"] == null)
            {
                return HttpNotFound();
            }
            var applicationId = int.Parse(Request["applicationId"]);
            var agree = bool.Parse(Request["agree"]);

            var application = db.RepairApplication.Find(applicationId);
            if (application == null)
            {
                return HttpNotFound();
            }
            if (agree)
            {
                application.State = "1";
            }
            else
            {
                application.State = "2";
                application.ToolEntity.State = "0";
            }

            application.HandleId = Account.Id;
            db.Entry(application).State = EntityState.Modified;
            db.Entry(application.ToolEntity).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Repair");
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
            Log(toolDef.Id, "新增工夹具定义：" + toolDef.Name, "0");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(ToolDef toolDef)
        {
            var toolDefineOld = db.ToolDef.Find(toolDef.Id);
            if (toolDefineOld == null || toolDefineOld.WorkCellId != ((Account)Session["account"]).WorkCellId)
            {
                return RedirectToAction("Index", new { errorMessage = "修改失败，未找到该工夹具定义" });
            }

            var familyNo = Request["familyNo"];
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

            toolDefineOld.EditDate = DateTime.Now;
            toolDefineOld.EditId = ((Account)Session["account"]).Id;
            toolDefineOld.FamilyId = familyId.Id;
            toolDefineOld.Code = toolDef.Code;
            toolDefineOld.Model = toolDef.Model;
            toolDefineOld.Name = toolDef.Name;
            toolDefineOld.PartNo = toolDef.PartNo;
            db.Entry(toolDefineOld).State = EntityState.Modified;
            db.SaveChanges();

            Log(toolDefineOld.Id, "修改工夹具定义：" + toolDef.Name, "0");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrow(ConsumeReturn cr)
        {
            var entity = db.ToolEntity.Find(cr.ToolEntityId);
            if (entity == null || entity.State != "0")
            {
                return RedirectToAction("Index");
            }
            cr.Date = DateTime.Now;
            cr.BorrowReturn = false;
            cr.AccountId = ((Account)Session["account"]).Id;
            db.Entry(cr).State = EntityState.Added;

            entity.State = "1";
            entity.UsedCount++;
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BorrowDetail", new { id = cr.ToolEntity.ToolDef.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(int id)
        {
            var cr = db.ConsumeReturn.Find(id);
            if (cr == null)
            {
                return RedirectToAction("Index");
            }

            cr.BorrowReturn = true;
            cr.ToolEntity.State = "0";
            cr.ReturnDate = DateTime.Now;

            db.Entry(cr.ToolEntity).State = EntityState.Modified;
            db.Entry(cr).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Return");
        }

        /// <summary>
        /// 修改工夹具实体
        /// </summary>
        /// <param name="toolEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEntity(ToolEntity toolEntity)
        {
            if (toolEntity == null)
            {
                return RedirectToAction("Index");
            }

            if (toolEntity.Id == 0)
            {
                //新增
                toolEntity.RegDate = DateTime.Now;
                toolEntity.State = "0";
                toolEntity.UsedCount = 0;
                toolEntity.ProductionDate = DateTime.Now; //添加修改生产日期
                toolEntity.BringId = ((Account)Session["account"]).Id;
                db.Entry(toolEntity).State = EntityState.Added;
                db.SaveChanges();
                Log(toolEntity.Id, "新增工夹具实体：" + toolEntity.Code, "1");
            }
            else
            {
                var oldEntity = db.ToolEntity.Find(toolEntity.Id);
                if (oldEntity == null)
                {
                    return RedirectToAction("Index");
                }
                oldEntity.Location = toolEntity.Location;
                oldEntity.Code = toolEntity.Code;
                oldEntity.BillNo = toolEntity.BillNo;
                db.Entry(oldEntity).State = EntityState.Modified;
                db.SaveChanges();
                Log(oldEntity.Id, "修改工夹具实体：" + oldEntity.Code, "1");
            }
            return RedirectToAction("Detail", new { id = toolEntity.ToolDefId });
        }

        /// <summary>
        /// 申请报修
        /// </summary>
        /// <param name="repairApplication"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Repair(RepairApplication repairApplication)
        {
            repairApplication.ApplicationId = Account.Id;
            repairApplication.Date = DateTime.Now;
            repairApplication.State = "0";
            repairApplication.WorkCellId = Account.WorkCellId;
            repairApplication.Date = DateTime.Now;
            var entity = db.ToolEntity.Find(repairApplication.ToolEntityId);
            entity.State = "2";
            db.Entry(entity).State = EntityState.Modified;
            db.Entry(repairApplication).State = EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("BorrowDetail", new { id = entity.ToolDefId });
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

        private void Log(int id, string message, string type)
        {
            var log = new ChangeLog
            {
                ChangeAccountId = ((Account)Session["account"]).Id,
                Type = type,
                Detail = message,
                ChangeId = id,
                ChangeDate = DateTime.Now
            };
            db.Entry(log).State = EntityState.Added;
            db.SaveChanges();
        }
    }

    /// <summary>
    /// 工夹具实体详细信息使用的类
    /// </summary>
    public class EntityDetailData
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}