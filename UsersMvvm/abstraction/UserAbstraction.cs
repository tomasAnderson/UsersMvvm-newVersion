using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMvvm.abstraction
{
    [Serializable]
    public class UserAbstraction
    {
        public UserAbstraction(int id, string lastName, string name, string secondName, string email)
        {
            ID = id;
            LastName = lastName;
            Name = name;
            SecondName = secondName;
            Email = email;
        }
        public UserAbstraction() { }

        int id;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        string lastName;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string secondName;
        public string SecondName
        {
            get
            {
                return secondName;
            }
            set
            {
                secondName = value;
            }
        }

        string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
    }
}
