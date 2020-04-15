using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Index(int nowPage=0)
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
    }
}