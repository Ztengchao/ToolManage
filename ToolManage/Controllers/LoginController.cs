using System.Linq;
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
            return View(account); // TODO 跳转到登陆后的页面
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
