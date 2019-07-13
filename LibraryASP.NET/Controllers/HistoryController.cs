using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryASP.NET.DAL;
using LibraryASP.NET.Models;

namespace LibraryASP.NET
{
    public class HistoryController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: History
        public ActionResult Index()
        {
            var histories = db.Histories;
            return View(histories.ToList());
        }

        // GET: History/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
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
