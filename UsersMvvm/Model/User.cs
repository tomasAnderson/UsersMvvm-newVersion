using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMvvm.Model
{
    [Serializable]
    public class User : BindableBase
    {
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
                RaisePropertyChanged(nameof(ID));
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
                RaisePropertyChanged(nameof(LastName));
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
                RaisePropertyChanged(nameof(Name));
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
                RaisePropertyChanged(nameof(SecondName));
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
                RaisePropertyChanged(nameof(Email));
            }
        }

    }
}
