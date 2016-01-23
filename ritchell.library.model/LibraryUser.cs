using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using RijndaelEncryptDecrypt;
using System.ComponentModel;
using ritchell.library.model.LibraryTransactions;

namespace ritchell.library.model
{
    public class LibraryUser : EntityBase<Guid>, INotifyPropertyChanged
    {
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;
        public DateTime Birthday { get; set; }

        //public ICollection<TransactionInfo> TransactionInfoes { get; set; }

        public enum UserType
        {
            Student,
            Teacher,
            Admin
        };

        public UserType LibraryUserType { get; set; }

        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                FirePropertyChanged("Fullname");
            }
        }

        public LibraryUser()
        {
            //TransactionInfo = new List<TransactionInfo>();
            Id = Guid.NewGuid();
        }

        public string Fullname
        {
            get
            {
                return string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
            }
        }

        public string UserRFIDTag { get; set; }

        public string Username { get; set; }

        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                FirePropertyChanged("Password");

                EncryptedPassword = RijndaelEncryptDecrypt.EncryptDecryptUtils.Encrypt(value, this.Birthday.ToString(),
                   this.Birthday.ToString(), "SHA1");

            }
        }




        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }

            set
            {
                _MiddleName = value;
                FirePropertyChanged("Fullname");
                FirePropertyChanged("MiddleName");
            }
        }

        public string LastName
        {
            get
            {
                return _LastName;
            }

            set
            {
                _LastName = value;
                FirePropertyChanged("LastName");
                FirePropertyChanged("Fullname");
            }
        }

        private string _EncryptedPassword;
        public string EncryptedPassword
        {
            get
            {
                return _EncryptedPassword;
            }

            set
            {
                _EncryptedPassword = value;
                _Password = RijndaelEncryptDecrypt.EncryptDecryptUtils.Decrypt(value, Birthday.ToString(),
                    Birthday.ToString(), "SHA1");
                FirePropertyChanged("EncryptedPassword");
                FirePropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Guid DepartmentId { get; set; }
    }
}
