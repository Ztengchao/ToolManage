using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ToolManage.Models;
using System.IO;

namespace ToolManage.Controllers
{
    public class ToolController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
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
            return RedirectToAction("Detail", new { id = toolEntity.ToolDefId });
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
                ChangeId = toolDef.Id,
                Detail = "新建工夹具定义：" + toolDef.Name,
                ChangeDate = DateTime.Now,
                Type = "0",
            };
            db.Entry(changLog).State = EntityState.Added;
            db.SaveChanges();

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

            var changLog = new ChangeLog
            {
                ChangeAccountId = ((Account)Session["account"]).Id,
                ChangeId = toolDefineOld.Id,
                Detail = "修改工夹具定义：" + toolDef.Name,
                ChangeDate = DateTime.Now,
                Type = "0",
            };
            db.Entry(changLog).State = EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrow(ConsumeReturn cr)
        {
            return RedirectToAction("BorrowDetail", new { id = 1 });
        }

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
                toolEntity.Picture = "";
                toolEntity.State = "0";
                toolEntity.UsedCount = 0;
                toolEntity.ProductionDate = DateTime.Now; //添加修改生产日期
                toolEntity.BringId = ((Account)Session["account"]).Id;
                db.Entry(toolEntity).State = EntityState.Added;
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
            }
            db.SaveChanges();
            return RedirectToAction("Detail", new { id = toolEntity.ToolDefId });
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