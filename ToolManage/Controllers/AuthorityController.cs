using System.Linq;
using System.Web.Mvc;
using ToolManage.Models;
using ToolManage.Helper;
using System.Data.Entity;

namespace ToolManage.Controllers
{
    public class AuthorityController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();

        private Account Account => (Account)Session["account"];
        private Authority Authority => (Authority)Session["authority"];

        // GET: Authority
        public ActionResult Index(int? workcellId, int? authorityId)
        {
            ViewBag.ShowModal = false;
            if (workcellId == null)
            {
                workcellId = db.WorkCell.First().Id;
            }
            Authority authority;
            if (authorityId == null)
            {
                authority = new Authority();
            }
            else
            {
                authority = db.Authority.Find(authorityId.Value);
                if (authority == null||authority.State!="0")
                {
                    authority = new Authority();
                }
                else
                {
                    workcellId = authority.WorkCellId;
                    ViewBag.ShowModal = true;
                }
            }

            ViewBag.workcellId = workcellId;
            ViewBag.Workcells = new SelectList(db.WorkCell, "Id", "Name", workcellId.Value);
            ViewBag.Authoritys = db.WorkCell.Find(workcellId.Value).Authority;
            ViewBag.AllAuthoritys = new SelectList(authority.TypesAuthorityNotHave(), "Id", "Name");
            return View(authority);
        }

        public ActionResult Change(int authorityId)
        {
            var authority = db.Authority.Find(authorityId);
            if (authority == null || authority.State != "0")
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { authorityId, workcellId = authority.WorkCellId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveChange(Authority authority)
        {
            if (authority.Id != -1)
            {
                db.Entry(authority).State = EntityState.Modified;
            }
            else
            {
                authority.State = "0";
                db.Entry(authority).State = EntityState.Added;
            }

            db.SaveChanges();
            if (Account.Authority == authority.Id)
            {
                Session.Remove("authority");
                Session.Add("authority", authority);
            }
            return RedirectToAction("Index", new { authorityId= authority.Id, workcellId = authority.WorkCellId });
        }

        [HttpGet]
        public ActionResult Delete(int authorityId)
        {
            var authority = db.Authority.Find(authorityId);
            if (authority == null)
            {
                return RedirectToAction("Index");
            }

            authority.State = "1";
            db.Entry(authority).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { workcellId = authority.WorkCellId });
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