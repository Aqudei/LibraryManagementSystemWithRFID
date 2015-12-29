using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using RijndaelEncryptDecrypt;

namespace ritchell.library.model
{
    public class LibraryUser : EntityBase<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public string Fullname
        {
            get
            {
                return string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
            }
        }


        public string UserRFIDTag { get; set; }

        public string Username { get; set; }

        public string Password
        {
            get
            {
                return RijndaelEncryptDecrypt.EncryptDecryptUtils.Decrypt(EncryptedPassword, this.Birthday.ToString(),
                   this.Birthday.ToString(), "SHA1");
            }
            set
            {
                EncryptedPassword = RijndaelEncryptDecrypt.EncryptDecryptUtils.Encrypt(value, this.Birthday.ToString(),
                   this.Birthday.ToString(), "SHA1");
            }
        }

        public string EncryptedPassword { get; set; }
    }
}
