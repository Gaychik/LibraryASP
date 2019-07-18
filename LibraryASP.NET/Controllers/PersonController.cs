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
using PagedList;
using System.Globalization;
using Xceed.Words.NET;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration.Install;


namespace LibraryASP.NET
{
    public class PersonController : Controller
    {
        private LibraryContext db = new LibraryContext();
        // GET: Person
        public ActionResult Index( string searchString,int? page)
        {
            var persons = db.Persons.ToList();
          
            if (searchString != null)
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                persons = persons.Where
               (
                    p => p.LastName.Contains(searchString)
                    || p.FirstName.Contains(searchString)
               ).ToList();
            }
            int pageSize = 3;
            int pageNumber = page ?? 1;
            return View(persons.ToPagedList(pageNumber,pageSize));
        }

        // GET: Person/Details/5
        static HttpCookie cookie;
        public ActionResult Details(Guid? id,Guid? BookID=null)
        {
            cookie = new HttpCookie("KeyBook");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (BookID != null && cookie.Values.Count==0)
            {
                cookie["Guid"] = BookID.ToString();
                var booktarget = db.Books.Find(BookID);
                booktarget.Stock -= 1;
                db.Entry(booktarget).State = EntityState.Modified;
                History newHistory = new History()
                {
                    BookName = booktarget.Name,
                    BookID = booktarget.BookID,
                    HistoryID = Guid.NewGuid(),
                    Person = person.FullName,
                    PersonID = person.PersonID,
                    DateIssue = DateTime.Now.Date,
                    DeadLine=DateTime.Now.Date.AddMonths(1)
                };
                db.Histories.Add(newHistory);
                db.SaveChanges();
                Dispose(true);
            }
            BookID = null;
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();  
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,FirstName,LastName,MidName,BirthDate,Address,TelephoneNumber")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.PersonID = Guid.NewGuid();
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            
            return View(person);
        }

      
        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,FirstName,LastName,MidName,Age,Address,TelephoneNumber")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: Person/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ReturnRecycleBin(Guid id)
        {
            var person = db.Persons.Find(id);
            return View("RecycleBin",person);
        }

         [HttpPost]
        public void PostRecycleBin(Guid guid,string date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            date = date.Remove(date.IndexOf(','), date.Length - date.IndexOf(','));          
            History history = db.Histories.Find(guid);
            db.Books.Find(history.BookID).Stock++;
            history.DateReceipt = DateTime.ParseExact(date, "d/M/yyyy",provider);
            db.SaveChanges(); 
        }
        [HttpGet]
        public ActionResult TakeBook(bool ModeTake,Guid guid)
        {
            cookie = null;
            return RedirectToAction("Index", "Book", new { ModeTake = true, guid = guid.ToString()});
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetReport(Guid id)
        {
            var person = db.Persons.Find(id);

            var PropsInfo = person.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string template = AppDomain.CurrentDomain.BaseDirectory + "Report.docx";
            DocX document = DocX.Load(template);
            var bookmarks = document.Bookmarks.ToList();
           
            var datapriority = person.Recorders.Select(h => new { h.BookName, DateReceipt=h.DateReceipt.Value.ToShortDateString(), DateIssue=h.DateIssue.ToShortDateString() }).ToList();

            foreach (var prop in PropsInfo)
            {
                var bookmark = bookmarks.FirstOrDefault(bm => bm.Name == prop.Name);
                if (bookmark != null)
                {
                    DateTime result;
                    if (DateTime.TryParse(prop.GetValue(person).ToString(), out result))
                    {
                        bookmark.SetText(result.ToShortDateString());
                    }
                    else {bookmark.SetText(prop.GetValue(person).ToString()); }
                   
                }
            }
            var table = document.AddTable(datapriority.Count, 3);
            table.Alignment = Alignment.center;
            table.Design = TableDesign.TableGrid;
            for (int i = 0; i < table.RowCount; i++)
            {
                var propinfo = datapriority[i].GetType().GetProperties();
                for (int j = 0; j < table.ColumnCount; j++)
                {
                    table.Rows[i].Cells[j].InsertParagraph(propinfo[j].GetValue(datapriority[i]).ToString());
                }
            }
            document.InsertParagraph().InsertTableAfterSelf(table);
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.InitializeLifetimeService();
                saveFileDialog1.Filter = "Document Files(*.docx)|*.docx";
                saveFileDialog1.Title = "Save a document as";
                DialogResult ret = STAShowDialog(saveFileDialog1);
                if (ret == DialogResult.OK)
                {
                    FileStream fs =
                        (FileStream)saveFileDialog1.OpenFile();
                    document.SaveAs(fs); 
                }
            }

         DialogResult STAShowDialog(FileDialog dialog)
            {
                DialogState state = new DialogState();
                state.dialog = dialog;
                System.Threading.Thread t = new System.Threading.Thread(state.ThreadProcShowDialog);
                t.SetApartmentState(System.Threading.ApartmentState.STA);
                t.Start();
                t.Join();
                return state.result;
            }
            
            return View("Details", person);
        }
        public class DialogState
        {
        public DialogResult result;
        public FileDialog dialog;
        public void ThreadProcShowDialog()
            {
                result = dialog.ShowDialog();
            }
        }
        List<Person> debtors = new List<Person>();
        public ActionResult GetDebtor()
        {
            var persons = db.Persons;
            var histories = db.Histories.Where(h => h.DeadLine < DateTime.Now).ToList();
            ViewBag.BookList = histories.Select(h => h.BookName).ToList();
            ViewBag.DatesIssueList = histories.Select(h => h.DateIssue).ToList();
            ViewBag.DeadLines= histories.Select(h => h.DeadLine).ToList();

            foreach (History h in histories)
            {
                debtors.Add(persons.Find(h.PersonID));
            }
            return View("Debtor", debtors.ToList());
        }
    }
}
