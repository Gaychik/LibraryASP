using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LibraryASP.NET.Models
{
    public class History
    {
        public Guid HistoryID { get; set; }
        public Guid PersonID { get; set; }
        public Guid BookID { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateIssue { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateReceipt { get; set; }
        public string  Person { get; set; }
        public string BookName { get; set; }
        [DataType(DataType.Date)]
        [ScaffoldColumn(false)]
        public DateTime DeadLine { get; set; }

    }
}