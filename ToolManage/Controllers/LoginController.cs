using System.Linq;
using System.Web.Mvc;
using ToolManage.Helper;
using ToolManage.Models;

namespace ToolManage.Controllers
{
    public class LoginController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();

        // GET: Login
        public ActionResult Index()
        {
            return View(new Account());
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,PassWord")] Account account)
        {
            var acc = db.Account.FirstOrDefault(i => i.State == "0" && i.UserName == account.UserName);
            if (acc == null || acc.PassWord.TrimEnd() != account.PassWord)
            {
                ModelState.AddModelError("", "账号或密码不正确");
                return View(account);
            }

            Session.Add("account", acc);
            Session.Add("authority", acc.Authority1);
            return acc.Authority1.AllAuthority().First().ToActionResult();
        }

        public ActionResult LogOut()
        {
            if (Session["account"] != null)
            {
                Session.Remove("account");
            }

            if (Session["authority"]!=null)
            {
                Session.Remove("authority");
            }

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
