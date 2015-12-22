using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class LibraryContext : DbContext
    {
        public DbSet<Section> Sections { get; set; }
        public DbSet<BookInfo> BookInfos { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }

        public LibraryContext()
            : base("LibraryContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Section>().HasKey(s => s.Id);
            modelBuilder.Entity<BookInfo>().HasKey(bi => bi.Id);
            modelBuilder.Entity<BookCopy>().HasKey(bc => bc.Id);
            modelBuilder.Entity<BookInfo>().HasMany(b => b.BookCopies).WithRequired().HasForeignKey(bc => bc.BookInfoId);
            modelBuilder.Entity<Section>().HasMany(s => s.BookInfos).WithRequired().HasForeignKey(b => b.SectionId);

        }
    }
}
