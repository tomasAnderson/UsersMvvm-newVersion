using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    class MainWindowVM : ViewModelBase
    {
        public ObservableCollection<User> PublicUsers { get; set; }

        UsersData usersData;

        public MainWindowVM()
        {
            PublicUsers = new ObservableCollection<User>();
            usersData = new UsersData();
        }

        public RelayCommand<List<object>> AddUser => new RelayCommand<List<object>>(i =>
        {
            int id = (int.TryParse(i[0].ToString(), out int num)) ? num : 0;
            string lastName = (i[1] != null) ? i[1].ToString() : "";
            string name = (i[2] != null) ? i[2].ToString() : "";
            string secondName = (i[3] != null) ? i[3].ToString() : "";
            string email = (i[4] != null) ? i[4].ToString() : "";

            if (id != 0 && lastName != "" && lastName != "" && name != "" && secondName != "" && email != "")
            {
                PublicUsers.Add(new User {ID = id, LastName = lastName, Name = name, SecondName = secondName, Email = email });

                RaisePropertyChanged(nameof(PublicUsers));
            }
        });
        public RelayCommand<List<object>> RemoveUser => new RelayCommand<List<object>>(i =>
        {
            int index = (int)i[0];

            if (index != -1)
            {
                PublicUsers.RemoveAt(index);

                RaisePropertyChanged(nameof(PublicUsers));
            }
        });
        public RelayCommand<List<object>> ChangeUser => new RelayCommand<List<object>>(i =>
        {
            int index = (int)i[0];
            int id = (int.TryParse(i[1].ToString(), out int n)) ? n : 0;
            string lastName = (i[2] != null) ? i[2].ToString() : "";
            string name = (i[3] != null) ? i[3].ToString() : "";
            string secondName = (i[4] != null) ? i[4].ToString() : "";
            string email = (i[5] != null) ? i[5].ToString() : "";

            if (id != 0 && lastName != "" && lastName != "" && name != "" && secondName != "" && email != "" && index != -1)
            {
                PublicUsers[index] = new User { ID = id, LastName = lastName, Name = name, SecondName = secondName, Email = email };

                RaisePropertyChanged(nameof(PublicUsers));
            }
        });

        #region clear Button command
        RelayCommand binarSerialization;
        public RelayCommand BinarSerialization
        {
            get
            {
                return binarSerialization ??
                    (binarSerialization = new RelayCommand(() =>
                    {
                        PublicUsers.Clear();
                        RaisePropertyChanged(nameof(PublicUsers));
                    }));
            }
        }
        #endregion

        #region Xml Commands for buttons
        RelayCommand choseXml;
        public RelayCommand ChoseXml
        {
            get
            {
                return choseXml ??
                     (choseXml = new RelayCommand(() =>
                     {
                         //Create "File Creator"
                         FileCreator fileCreator = new FileCreator();
                         //Create Builder for binary files
                         Builder builder = new XmlFilesBuilder();
                         //Create file
                         MainFile binarFile = fileCreator.CreateFile(builder);
                         
                         PublicUsers = binarFile.OpenFile();
                         RaisePropertyChanged(nameof(PublicUsers));
                     }));
            }
        }

        RelayCommand saveXml;
        public RelayCommand SaveXml
        {
            get
            {
                return saveXml ??
                    (saveXml = new RelayCommand(() =>
                    {
                        //Create "File Creator"
                        FileCreator fileCreator = new FileCreator();
                        //Create Builder for binary files
                        Builder builder = new XmlFilesBuilder();
                        //Create file
                        MainFile binarFile = fileCreator.CreateFile(builder);
                        binarFile.SaveFile(PublicUsers);
                    }));
            }
        }
        #endregion

        #region Binary Commands for buttons
        RelayCommand choseBinary;
        public RelayCommand ChoseBinary
        {
            get
            {
                return choseBinary ??
                     (choseBinary = new RelayCommand(() =>
                     {
                         //Create "File Creator"
                         FileCreator fileCreator = new FileCreator();
                         //Create Builder for binary files
                         Builder builder = new BinaryFilesBuilder();
                         //Create file
                         MainFile binarFile = fileCreator.CreateFile(builder);

                         PublicUsers = binarFile.OpenFile();
                         RaisePropertyChanged(nameof(PublicUsers));
                     }));
            }
        }

        RelayCommand saveBinary;
        public RelayCommand SaveBinary
        {
            get
            {
                return saveBinary ??
                    (saveBinary = new RelayCommand(() =>
                    {
                        //Create "File Creator"
                        FileCreator fileCreator = new FileCreator();
                        //Create Builder for binary files
                        Builder builder = new BinaryFilesBuilder();
                        //Create file
                        MainFile binarFile = fileCreator.CreateFile(builder);
                        binarFile.SaveFile(PublicUsers);
                    }));
            }
        }
        #endregion

    }



    // type of creating file
    class FileType
    {
        public string Type { get; set; }
    }

    // working file
    class MainFile
    {
        UsersData usersData = new UsersData();

        public FileType FileType { get; set; }

        public void SaveFile(ObservableCollection<User> PublicUsers) 
        {
            if (FileType.Type == "bin") 
            {
                usersData.saveBinaryFile(PublicUsers);
            }

            if (FileType.Type == "xml")
            {
                usersData.saveXmlFile(PublicUsers);
            }
        }

        public ObservableCollection<User> OpenFile() 
        {
            if (FileType.Type == "bin")
            {
                usersData.openBinaryFile();
            }

            if (FileType.Type == "xml")
            {
                usersData.openXmlFile();
            }

            return usersData.Users;
        }
    }

    // abstract class of builder
    abstract class Builder 
    {
        public MainFile MainFile { get; private set; }

        public void WorkWithFile() 
        {
            MainFile = new MainFile();
        }

        public abstract void SetFileType();
    }

    // File creator
    class FileCreator 
    {
        public MainFile CreateFile(Builder builder) 
        {
            builder.WorkWithFile();

            builder.SetFileType();

            return builder.MainFile;
        }
    }

    // Builder for Binary files
    class BinaryFilesBuilder : Builder
    {
        public override void SetFileType()
        {
            this.MainFile.FileType = new FileType { Type = "bin" };
        }
    }

    // Builder for Xml files
    class XmlFilesBuilder : Builder 
    {
        public override void SetFileType()
        {
            this.MainFile.FileType = new FileType { Type = "xml" };
        }
    }
}
