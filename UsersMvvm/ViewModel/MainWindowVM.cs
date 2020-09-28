using GalaSoft.MvvmLight.Command;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using UsersMvvm.Data;
using UsersMvvm.Model;

namespace UsersMvvm.ViewModel
{
    class MainWindowVM : BindableBase
    {
        public ObservableCollection<User> PublicUsers { get; set; }

        bool xmlSerialize = true, binarSerialize = false, jsonSerializer = false;

        public MainWindowVM()
        {
            UsersData usersData = new UsersData(true, false, false);
            PublicUsers = usersData.Users;

            #region Initialize commands
            AddUser = new DelegateCommand<List<object>>(i =>
            {
                int id            = (int.TryParse(i[0].ToString(), out int num)) ? num : 0;
                string lastName   = (i[1] != null) ? i[1].ToString() : "";
                string name       = (i[2] != null) ? i[2].ToString() : "";
                string secondName = (i[3] != null) ? i[3].ToString() : "";
                string email      = (i[4] != null) ? i[4].ToString() : "";

                if (id != 0 && lastName != "" && lastName != "" && name != "" && secondName != "" && email != "") {
                    if (xmlSerialize)
                        usersData.xmlSerializationAddUser(id, lastName, name, secondName, email);
                    if (binarSerialize)
                        usersData.binarySerializationAddUser(id, lastName, name, secondName, email);
                    if (jsonSerializer)
                        usersData.jsonSerializationAddUser(id, lastName, name, secondName, email);

                    PublicUsers = usersData.Users;
                    RaisePropertyChanged(nameof(PublicUsers));
                }
            });

            RemoveUser = new DelegateCommand<List<object>>(i => 
            {
                int index = (int)i[0];

                if (index != -1)
                {
                    if (xmlSerialize)
                        usersData.xmlSerializationRemoveUser(index);
                    if (binarSerialize)
                        usersData.binarySerializationRemoveUser(index);
                    if (jsonSerializer)
                        usersData.jsonSerializationRemoveUser(index);

                    PublicUsers = usersData.Users;
                    RaisePropertyChanged(nameof(PublicUsers));
                }
            });

            ChangeUser = new DelegateCommand<List<object>>(i => 
            {
                int index = (int)i[0];
                int id = (int.TryParse(i[1].ToString(), out int n)) ? n : 0;
                string lastName = (i[2] != null) ? i[2].ToString() : "";
                string name = (i[3] != null) ? i[3].ToString() : "";
                string secondName = (i[4] != null) ? i[4].ToString() : "";
                string email = (i[5] != null) ? i[5].ToString() : "";

                if (id != 0 && lastName != "" && lastName != "" && name != "" && secondName != "" && email != "" && index != -1)
                {
                    if (xmlSerialize)
                        usersData.xmlSerializationChangeUser(index, id, lastName, name, secondName, email);
                    if (binarSerialize)
                        usersData.binarySerializationChangeUser(index, id, lastName, name, secondName, email);
                    if (jsonSerializer)
                        usersData.jsonSerializationChangeUser(index, id, lastName, name, secondName, email);

                    PublicUsers = usersData.Users;
                    RaisePropertyChanged(nameof(PublicUsers));
                }
            });
            #endregion
        }

        public DelegateCommand<List<object>> AddUser { get; }
        public DelegateCommand<List<object>> RemoveUser { get; }
        public DelegateCommand<List<object>> ChangeUser { get; }


        RelayCommand xmlSerialization;
        public RelayCommand XmlSerialization 
        {
            get 
            {
                return xmlSerialization ??
                     (xmlSerialization = new RelayCommand(() => 
                     {
                         xmlSerialize = true;
                         binarSerialize = false;
                         jsonSerializer = false;

                         UsersData usersData = new UsersData(xmlSerialize, binarSerialize, jsonSerializer);
                         PublicUsers = usersData.Users;
                         RaisePropertyChanged(nameof(PublicUsers));
                     }));
            }
        }

        RelayCommand binarSerialization;
        public RelayCommand BinarSerialization
        {
            get 
            {
                return binarSerialization ??
                    (binarSerialization = new RelayCommand(() => 
                    {
                        xmlSerialize = false;
                        binarSerialize = true;
                        jsonSerializer = false;

                        UsersData usersData = new UsersData(xmlSerialize, binarSerialize, jsonSerializer);
                        PublicUsers = usersData.Users;
                        RaisePropertyChanged(nameof(PublicUsers));
                    }));
            }
        }

        RelayCommand jsonSerialization;
        public RelayCommand JsonSerialization
        {
            get
            {
                return jsonSerialization ??
                    (jsonSerialization = new RelayCommand(() =>
                    {
                        xmlSerialize = false;
                        binarSerialize = false;
                        jsonSerializer = true;

                        UsersData usersData = new UsersData(xmlSerialize, binarSerialize, jsonSerializer);
                        PublicUsers = usersData.Users;
                        RaisePropertyChanged(nameof(PublicUsers));
                    }));
            }
        }

    }
}
