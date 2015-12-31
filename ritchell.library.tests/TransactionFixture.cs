using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ritchell.library.model;
using ritchell.library.model.Services;

namespace ritchell.library.tests
{
    [TestFixture]
    public class TransactionFixture
    {
        private LibraryUserService libUserService;
        private LibraryUser sampleUser;
        private SectionService sectionService;
        private BookCopyService bookCopyService;
        private BookService bookService;
        private Section sampleSection;
        private Guid sampleSectionId;
        private BookInfo sampleBookInfo;
        private LibrarianService librarianService;

        [SetUp]
        public void Setup()
        {
            librarianService = new LibrarianService();
            libUserService = new LibraryUserService();
            bookService = new BookService();
            bookCopyService = new BookCopyService();
            sectionService = new SectionService();

            CreateSampleUser();
            CreateSampleSection();
            CreateSampleBookInfo();

        }

        private void CreateSampleBookInfo()
        {
            sampleBookInfo = new BookInfo
            {
                Author = "Aqudei",
                BookTitle = "Pilosopong Tasyo",
                CallNumber = "123",
                Copyright = "2005",
                ISBN = new ISBN { ISBN10 = "ISBN10-000", ISBN13 = "ISBN13-000" },
                SectionId = sampleSectionId
            };

            bookService.EnrollOrUpdateBook(sampleBookInfo);
        }

        private void CreateSampleSection()
        {
            sampleSection = new Section
            {
                LateReturningFee = 100,
                MaxDaysAllowedForBorrowing = 10,
                Name = "Management"
            };

            sectionService.CreateNewSection(sampleSection);

            sampleSectionId = sampleSection.Id;
        }

        private void CreateSampleUser()
        {
            sampleUser = new LibraryUser
            {
                FirstName = "Francis",
                Birthday = new DateTime(1989, 12, 22),
                Password = "pass",
                Username = "kiko"
            };

            libUserService.AddLibraryUser(sampleUser);
        }

        [Test]
        public void CanAuthenticateByUsernameAndPassword()
        {
            LibraryUser user = libUserService.FindUserByUsernameAndPassword("kiko", "pass");

            Assert.That(user, Is.Not.Null);
            Assert.That(user.FirstName, Is.EqualTo("Francis"));
        }

        [Test]
        public void BorrowBookTest()
        {
            var book = bookService.GetBooks().First();
            var bookCopy = BookCopy.MakeCopy(book, "short123", "long123");
            bookCopyService.AddBookCopy(bookCopy);

            librarianService.BorrowBook(sampleUser, "short123");

            var bc = bookCopyService.FindByShortRange(bookCopy.BookTagShort);
            Assert.That(bc.IsBorrowed, Is.EqualTo(true));


        }
    }
}
