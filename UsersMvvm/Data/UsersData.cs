using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UsersMvvm.abstraction;
using UsersMvvm.Classes;
using UsersMvvm.Model;

namespace UsersMvvm.Data
{
    public class UsersData
    {
        public ObservableCollection<UserAbstraction> PublicUsers { get; set; }
        public ObservableCollection<User> Users { get; set; }
        MyBinaryWriter writer;

        public UsersData()
        {
            Users = new ObservableCollection<User>();
            PublicUsers = new ObservableCollection<UserAbstraction>();
            writer = new MyBinaryWriter();
        }

        #region XML Serialization
        public void openXmlFile() 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "users.xml";
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Xml documents (.xml)|*.xml";

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PublicUsers.Clear();

                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    XmlReader reader = new XmlTextReader(fs);
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<UserAbstraction>));

                    if (serializer.CanDeserialize(reader))
                    {
                        PublicUsers = (ObservableCollection<UserAbstraction>)serializer.Deserialize(reader);
                    }

                    addDataToUsers();
                }
            }
        }

        public void saveXmlFile(ObservableCollection<User> Users) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "users.xml";
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Xml documents (.xml)|*.xml";

            var result = saveFileDialog.ShowDialog();
            if (result == true) 
            {
                foreach (var u in Users)
                    PublicUsers.Add(new UserAbstraction
                    {
                        ID = u.ID,
                        LastName = u.LastName,
                        Name = u.Name,
                        SecondName = u.SecondName,
                        Email = u.Email
                    });

                using (XmlWriter xmlWriter = XmlWriter.Create(saveFileDialog.FileName))
                {
                    xmlWriter.WriteStartElement("ArrayOfUserAbstraction");

                    foreach (var users in PublicUsers)
                    {
                        xmlWriter.WriteStartElement("UserAbstraction");

                        xmlWriter.WriteStartElement("ID");
                        xmlWriter.WriteString(users.ID.ToString().Trim());
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("LastName");
                        xmlWriter.WriteString(users.LastName);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("Name");
                        xmlWriter.WriteString(users.Name);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("SecondName");
                        xmlWriter.WriteString(users.SecondName);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("Email");
                        xmlWriter.WriteString(users.Email);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                    }
                    //xmlWriter.WriteStartElement("user");
                    //xmlWriter.WriteAttributeString("email", "1vasya@gmail.com");
                    //xmlWriter.WriteString("1Vasya 1Pupkin");

                    xmlWriter.WriteEndDocument();
                }
            }
        }
        #endregion

        #region Binary Serialization
        public void openBinaryFile() 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "users.bin";
            openFileDialog.DefaultExt = ".bin";
            openFileDialog.Filter = "Binar documents (.bin)|*.bin";

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PublicUsers.Clear();

                PublicUsers = writer.Read(openFileDialog.FileName);

                addDataToUsers();
            }
        }

        public void saveBinaryFile(ObservableCollection<User> Users) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "users.bin";
            saveFileDialog.DefaultExt = ".bin";
            saveFileDialog.Filter = "Binary documents (.bin)|*.bin";

            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                writer.Create(saveFileDialog.FileName);

                foreach (var u in Users)
                    PublicUsers.Add(new UserAbstraction { ID = u.ID, LastName = u.LastName, Name = u.Name, 
                        SecondName = u.SecondName, Email = u.Email });

                writer.Write(PublicUsers);
            }
        }
        #endregion

        void addDataToUsers() 
        {
            Users = new ObservableCollection<User>();

            foreach (var u in PublicUsers)
                Users.Add(new User { ID = u.ID, LastName = u.LastName, Name = u.Name, SecondName = u.SecondName, Email = u.Email });
        }
    }
}
