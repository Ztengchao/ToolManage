using System.Linq;
using System.Security.AccessControl;
using System.Web.Mvc;
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
            var acc = db.Account.FirstOrDefault(i => i.UserName == account.UserName);
            if (acc == null || acc.PassWord.TrimEnd() != account.PassWord)
            {
                ModelState.AddModelError("", "账号或密码不正确");
                return View(account);
            }

            Session.Add("account", acc);
            return RedirectToAction("Index", "Authority");// TODO根据账户类型跳转
        }

        public ActionResult LogOut()
        {
            if (Session["account"] != null)
            {
                Session.Remove("account");
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
