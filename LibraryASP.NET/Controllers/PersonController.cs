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
            history.DateReceipt = DateTime.ParseExact(date, "M/d/yyyy",provider);
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

            //var PropsInfo = person.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //Word.Application application;
            //Word.Document document;
            ////Object _missingObj = Missing.Value;
            ////Object trueObj = true;
            ////Object falseObj = false;
            //application = new Word.Application();
            string template = AppDomain.CurrentDomain.BaseDirectory + "Report.docx";
            DocX document = DocX.Create(template);
            // Вставляем параграф и указываем текст
            document.InsertParagraph("Тест");

            // вставляем параграф и передаём текст
            document.InsertParagraph("Тест").
                     // устанавливаем шрифт
                     Font("Calibri").
                     // устанавливаем размер шрифта
                     FontSize(36).
                     // устанавливаем цвет
                     Color(Color.Navy).
                     // делаем текст жирным
                     Bold().
                     // устанавливаем интервал между символами
                     Spacing(15).
                     // выравниваем текст по центру
                     Alignment = Alignment.center;

            // вставляем параграф и добавляем текст
            Paragraph paragraph = document.InsertParagraph();
            // выравниваем параграф по правой стороне
            paragraph.Alignment = Alignment.right;

            // добавляем отдельную строку со своим форматированием
            paragraph.AppendLine("Test").
                     // устанавливаем размер шрифта
                     FontSize(20).
                     // добавляем курсив
                     Italic().
                     // устанавливаем точечное подчёркивание
                     UnderlineStyle(UnderlineStyle.dotted).
                     // устанавливаем цвет подчёркивания
                     UnderlineColor(Color.DarkOrange).
                     // добавляем выделение текста
                     Highlight(Highlight.yellow);
            // добавляем пустую строку
            paragraph.AppendLine();
            // добавляем ещё одну строку
            paragraph.AppendLine("Тест");

            // сохраняем документ
            document.Save();
            //Object newTemplate = false;
            //Object documentType = Type.Missing;
            //Object visible = true;
            ////Object templatePathObj = AppDomain.CurrentDomain.BaseDirectory + "Report.docx";
            ////Object confirmConversions = true;
            ////Object readOnly = false;
            ////Object addToRecentFiles = true;
            ////Object passwordDocument = Type.Missing;
            ////Object passwordTemplate = Type.Missing;
            ////Object revert = false;
            ////Object writePasswordDocument = Type.Missing;
            ////Object writePasswordTemplate = Type.Missing;
            ////Object format = Type.Missing;
            ////Object encoding = Type.Missing; ;
            ////Object oVisible = true;
            ////Object openConflictDocument = Type.Missing;
            ////Object openAndRepair = true;
            ////Object documentDirection = Type.Missing;
            ////Object noEncodingDialog = false;
            ////Object xmlTransform = Type.Missing;
            //try
            //{
            //    document = application.Documents.CheckOut(template);
            //}
            //catch (Exception error)
            //{
            //    application.Quit(Type.Missing, Type.Missing, Type.Missing);
            //    application = null;
            //    throw error;
            //}
            //application.Visible = true;
            //Thread.Sleep(5000);
            //var wordRange = document.Bookmarks.Cast<Word.Bookmark>().ToList();
            //var datapriority = person.Recorders.Select(h => new { h.BookName, h.DateReceipt, h.DateIssue }).ToList();
            //Word.Table table;
            //foreach (var prop in PropsInfo)
            //{ 
            //    var bookmark = wordRange.FirstOrDefault(bm=>bm.Name == prop.Name);
            //    if (bookmark != null)
            //    {
            //        bookmark.Range.Text = prop.GetValue(person).ToString();
            //        break;
            //    }
            //}
            //var Paragraph = application.ActiveDocument.Paragraphs.Add();
            //var tableRange = Paragraph.Range;
            //Object defaultTableBehavior = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
            //Object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitWindow;
            //application.ActiveDocument.Tables.Add(tableRange, datapriority.Count, 3,defaultTableBehavior,autoFitBehavior);
            //table = application.ActiveDocument.Tables[application.ActiveDocument.Tables.Count];
            //table.ApplyStyleHeadingRows = true;
            //table.ApplyStyleLastRow = false;
            //table.ApplyStyleFirstColumn = true;
            //table.ApplyStyleLastColumn = false;
            //table.ApplyStyleRowBands = true;
            //table.ApplyStyleColumnBands = false;
            //for (int i = 0; i < table.Columns.Count; i++)
            //{
            //    table.Rows.Add(datapriority[i]);
            //}
            //Object pathToSaveObj = @"D:\Report1";
            //document.SaveAs(ref pathToSaveObj, Word.WdSaveFormat.wdFormatDocument, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //application.Quit(Type.Missing , Type.Missing, Type.Missing);
            //application = null;
            //Dispose(true);
            //GC.SuppressFinalize(this);

            return View("Details", person);
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
