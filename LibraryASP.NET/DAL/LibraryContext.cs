using LibraryASP.NET.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace LibraryASP.NET.DAL
{
    public class LibraryContext:DbContext
    {
        public LibraryContext():base("LibraryContext")
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<History> Histories { get; set; }

       
    }
}