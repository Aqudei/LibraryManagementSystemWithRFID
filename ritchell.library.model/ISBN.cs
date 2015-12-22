using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.model
{
    public class ISBN : IEquatable<ISBN>
    {
        private string _ISBN10;
        private string _ISBN13;

        public string ISBN10
        {
            get { return _ISBN10; }
            set { _ISBN10 = value; }
        }

        public string ISBN13
        {
            get { return _ISBN13; }
            set { _ISBN13 = value; }
        }

        public ISBN()
        {
            ISBN10 = ISBN13 = "000";
        }

        public bool Equals(ISBN other)
        {
            if (this.ISBN10 != null && other.ISBN10 != null)
            {
                return this.ISBN10.Equals(other.ISBN10);
            }

            if (this.ISBN13 != null && other.ISBN13 != null)
            {
                return this.ISBN13.Equals(other.ISBN13);
            }

            return false;
        }
    }
}
