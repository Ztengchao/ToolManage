using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ToolManage.Models;

namespace ToolManage.Controllers
{
    public class UserController : Controller
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private static int CountPerPage => 10;
        // GET: User
        public ActionResult Index(Account account, int? accountId, string searchName, string searchNum, int? searchAuthority, int? searchWorkcell, int nowPage = 0)
        {
            var data = db.Account.AsQueryable();
            var showModal = false;
            if (account.WorkCell == null)
            {
                account.WorkCellId = -1;
            }
            if (searchWorkcell == null)
            {
                searchWorkcell = db.WorkCell.First().Id;
            }
            else if (searchWorkcell != -1)
            {
                data = data.Where(i => i.WorkCellId == searchWorkcell.Value);
            }
            if (!searchName.IsNullOrWhiteSpace())
            {
                data = data.Where(i => i.Name.Contains(searchName));
            }
            if (!searchNum.IsNullOrWhiteSpace())
            {
                data = data.Where(i => i.JobNumber.Contains(searchNum));
            }
            if (searchAuthority != null && searchAuthority != -1)
            {
                data = data.Where(i => i.Authority == searchAuthority.Value);
            }
            ViewBag.MaxPage = data.Count() / CountPerPage;
            data = data.OrderBy(i => i.Id).Skip(nowPage * CountPerPage).Take(CountPerPage);
            if (accountId != null)
            {
                if (accountId == -1)
                {
                    account = new Account
                    {
                        Name = "",
                        UserName = "",
                        PassWord = "",
                        JobNumber = "",
                        WorkCellId = -1,
                        Id = -1
                    };
                }
                else
                {
                    account = db.Account.Find(accountId.Value);
                }
                account.Name = account.Name.Trim();
                account.PassWord = account.PassWord.Trim();
                account.UserName = account.UserName.Trim();
                account.JobNumber = account.JobNumber.Trim();
                showModal = true;
            }

            ViewBag.ShowModal = showModal;
            ViewBag.NowPage = nowPage;
            ViewBag.Data = data.ToArray();
            ViewBag.SearchWorkcell = new SelectList(db.WorkCell.ToList(), "Id", "Name");
            ViewBag.SearchOption = db.WorkCell.Select(i => new WorkCellContainer() { WorkCell = i, Authorities = i.Authority.ToList() }).ToList();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(Account account)
        {
            if (account.Id != -1)
            {
                db.Entry(account).State = EntityState.Modified;
            }
            else
            {
                db.Entry(account).State = EntityState.Added;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    /// <summary>
    /// 部门与权限的对应类
    /// </summary>
    public class WorkCellContainer
    {
        public WorkCell WorkCell { get; set; }
        public List<Authority> Authorities { get; set; }

        public string ToJsonString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{{name:\"{0}\",id:{1},authorities:[", WorkCell.Name.Trim(), WorkCell.Id);
            foreach (var authority in Authorities)
            {
                sb.AppendFormat("{{name:\"{0}\",id:{1}}},", authority.Name.Trim(), authority.Id);
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }
}