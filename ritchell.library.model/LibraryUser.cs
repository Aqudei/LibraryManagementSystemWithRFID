using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.infrastructure;
using RijndaelEncryptDecrypt;
using System.ComponentModel;

namespace ritchell.library.model
{
    public class LibraryUser : EntityBase<Guid>, INotifyPropertyChanged
    {
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;
        public DateTime Birthday { get; set; }

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

        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(EncryptedPassword))
                    return string.Empty;

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
                FirePropertyChanged("Fullname");
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
    }
}
