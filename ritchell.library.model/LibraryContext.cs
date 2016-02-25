using MySql.Data.Entity;
using ritchell.library.infrastructure.Logging;
using ritchell.library.model.LibraryTransactions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class LibraryContext : DbContext
    {
        public DbSet<infrastructure.Logging.ActionLog> ActionLogs { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<BookInfo> BookInfos { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<LibraryUser> LibraryUsers { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<TransactionInfo> BookTransactionInfos { get; set; }
        public DbSet<Course> Courses { get; set; }

        public LibraryContext()
            : base("name=LibraryContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActionLog>().HasKey(a => a.Id);

            modelBuilder.Entity<Course>().HasKey(c => c.Id);

            modelBuilder.Entity<Department>().HasKey(d => d.Id);

            modelBuilder.Entity<Holiday>().HasKey(h => h.Id);


            modelBuilder.Entity<TransactionInfo>().HasKey(bt => bt.Id);

            modelBuilder.Entity<LibraryUser>().HasKey(u => u.Id);
            modelBuilder.Entity<LibraryUser>().Ignore(u => u.Password);
            modelBuilder.Entity<LibraryUser>().Ignore(u => u.Fullname);

            modelBuilder.Entity<Section>().HasKey(s => s.Id);

            modelBuilder.Entity<BookInfo>().HasKey(bi => bi.Id);
            modelBuilder.Entity<BookInfo>().HasMany(b => b.BookCopies).WithRequired().HasForeignKey(bc => bc.BookInfoId);

            modelBuilder.Entity<BookCopy>().HasKey(bc => bc.Id);


            modelBuilder.Entity<Section>().HasMany(s => s.BookInfos).WithRequired().HasForeignKey(b => b.SectionId);

        }
    }
}
