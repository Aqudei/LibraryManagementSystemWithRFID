using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ritchell.library.model;

namespace ritchell.library.tests
{
    [TestFixture]
    public class CRUDFixture
    {
        private model.Services.BookService bookService;
        private model.Services.SectionService sectionService;
        private model.Services.BookCopyService bookCopyService;

        [SetUp]
        public void Setup()
        {
            bookService = new library.model.Services.BookService();
            sectionService = new model.Services.SectionService();
            bookCopyService = new model.Services.BookCopyService();
        }

        [Test]
        public void CanAddSection()
        {
            Section section = new Section
            {
                Id = Guid.NewGuid(),
                LateReturningFee = 50,
                MaxDaysAllowedForBorrowing = 6,
                Name = "Filipiniana"
            };

            sectionService.CreateNewSection(section);

            var sections = sectionService.GetAllSections();

            Assert.That(sections.Where(s => s.Name == "Filipiniana").Any());
        }

        [Test]
        public void CanAddBookToSection()
        {
            Section section = new Section
            {
                Id = Guid.NewGuid(),
                LateReturningFee = 50,
                MaxDaysAllowedForBorrowing = 6,
                Name = "Philosophy"
            };

            sectionService.CreateNewSection(section);

            BookInfo bookInfo = new BookInfo
            {
                Author = "ME",
                BookTitle = "Proggy",
                CallNumber = "G123",
                Copyright = "2012",
                Id = Guid.NewGuid(),
                ISBN = new ISBN
                {
                    ISBN10 = "123"
                },
                SectionId = section.Id
            };

            bookService.AddOrUpdateBook(bookInfo);

            Assert.That(sectionService.GetBooks(section.Id).Count() == 1);
        }


        [Test]
        public void AddBookCopyTest()
        {
            Section section = new Section
            {
                Id = Guid.NewGuid(),
                LateReturningFee = 50,
                MaxDaysAllowedForBorrowing = 6,
                Name = "Education"
            };

            sectionService.CreateNewSection(section);

            BookInfo bookInfo = new BookInfo
            {
                Author = "ME",
                BookTitle = "MAED",
                CallNumber = "G123",
                Copyright = "2015",
                Id = Guid.NewGuid(),
                ISBN = new ISBN
                {
                    ISBN10 = "3333"
                },
                SectionId = section.Id
            };

            bookService.AddOrUpdateBook(bookInfo);

            BookCopy bookCopy1 = BookCopy.MakeCopy(bookInfo, "bookShortTag1","bookLongTag1");
            BookCopy bookCopy2 = BookCopy.MakeCopy(bookInfo, "bookShortTag2","bookLongTag2");
            bookCopyService.AddBookCopy(bookCopy1);
            bookCopyService.AddBookCopy(bookCopy2);

            var bookCopies = bookCopyService.BookCopiesOf(bookInfo.Id);

            Assert.That(bookCopies.Count(), Is.EqualTo(2));
        }
    }
}
