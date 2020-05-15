using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToolManage.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult RepairChart()
        {
            return View();
        }

        public ActionResult ToolChart()
        {
            return View();
        }
    }
}