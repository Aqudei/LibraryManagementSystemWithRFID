using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model.Services
{
    public class TransactionService
    {
        private BookCopyService _BookCopyService;

        public TransactionService()
        {
            _BookCopyService = new BookCopyService();
        }

        public IEnumerable<TransactionDTO> GetTransactionDTO(Guid userID)
        {
            List<TransactionDTO> transDTOs = new List<TransactionDTO>();
            using (var libUserRepo = new Repositories.LibraryUserRepository())
            using (var bookCopyRepo = new Repositories.BookCopyRepository())
            using (var bookInfoRepo = new Repositories.BookInfoRepository())
            using (var transRepo = new Repositories.BookTransactionInfoRepository())
            {
                var allTrans = transRepo.GetTransactionsOf(userID);
                foreach (var trans in allTrans)
                {
                    var newDTO = new TransactionDTO
                    {
                        BookInfo = _BookCopyService.GetBookInfo(trans.BookCopyId),
                        LibraryUser = libUserRepo.FindById(userID),
                        TransactionInfo = trans,
                        BookCopy = bookCopyRepo.FindById(trans.BookCopyId)
                    };
                    transDTOs.Add(newDTO);
                }
                return transDTOs;
            }
        }
    }
}
