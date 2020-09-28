using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UsersMvvm.abstraction;
using UsersMvvm.Model;

namespace UsersMvvm.Data
{
    public class UsersData
    {
        public ObservableCollection<UserAbstraction> PublicUsers;
        public ObservableCollection<User> Users { get; set; }

        public UsersData(bool xmlSerialize, bool binarSerialize, bool jsonSerialize)
        {
            Users = new ObservableCollection<User>();
            PublicUsers = new ObservableCollection<UserAbstraction>();

            try
            {
                if (xmlSerialize)
                    xmlSerializationOutput();
                if (binarSerialize)
                    binarySerializationOutput();
                if (jsonSerialize)
                    jsonSerializationOutput();
            }
            catch (Exception) { }
        }

        #region XML Serialization
        public void xmlSerializationOutput() 
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<UserAbstraction>));

            PublicUsers.Clear();

            using (FileStream fs = new FileStream("users.xml", FileMode.OpenOrCreate))
            {
                PublicUsers = (ObservableCollection<UserAbstraction>)formatter.Deserialize(fs);

                addDataToUsers();
            }
        }

        public void xmlSerializationAddUser(int id, string lastName, string name, string secondName, string email) 
        {
            xmlSerializationOutput();

            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<UserAbstraction>));

            using (FileStream fs = new FileStream("users.xml", FileMode.OpenOrCreate))
            {
                PublicUsers.Add(new UserAbstraction { ID = id, LastName = lastName, Name = name, SecondName = secondName, Email = email });

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public void xmlSerializationRemoveUser(int index) 
        {
            xmlSerializationOutput();

            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<UserAbstraction>));

            using (FileStream fs = new FileStream("users.xml", FileMode.Create))
            {
                PublicUsers.RemoveAt(index);

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public void xmlSerializationChangeUser(int index, int id, string lastName, string name, string secondName, string email) 
        {
            xmlSerializationOutput();

            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<UserAbstraction>));

            using (FileStream fs = new FileStream("users.xml", FileMode.Create)) 
            {
                PublicUsers[index] = new UserAbstraction { ID = id, LastName = lastName, Name = name, SecondName = secondName, Email = email };

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }
        #endregion

        #region Binary Serialization
        public void binarySerializationOutput() 
        {
            BinaryFormatter formatter = new BinaryFormatter();

            PublicUsers.Clear();

            using (FileStream fs = new FileStream("users.dat", FileMode.OpenOrCreate)) 
            {
                PublicUsers = (ObservableCollection<UserAbstraction>)formatter.Deserialize(fs);

                addDataToUsers();
            }
        }

        public void binarySerializationAddUser(int id, string lastName, string name, string secondName, string email) 
        {
            binarySerializationOutput();

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("users.dat", FileMode.OpenOrCreate))
            {
                PublicUsers.Add(new UserAbstraction(id, lastName, name, secondName, email));

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public void binarySerializationRemoveUser(int index)
        {
            binarySerializationOutput();

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("users.dat", FileMode.Create))
            {
                PublicUsers.RemoveAt(index);

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public void binarySerializationChangeUser(int index, int id, string lastName, string name, string secondName, string email)
        {
            binarySerializationOutput();

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("users.dat", FileMode.Create))
            {
                PublicUsers[index] = new UserAbstraction(id, lastName, name, secondName, email);

                formatter.Serialize(fs, PublicUsers);

                addDataToUsers();
            }
        }
        #endregion

        #region Json Serialization
        public async Task jsonSerializationOutput()
        {
            PublicUsers.Clear();

            using (FileStream fs = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                PublicUsers = await JsonSerializer.DeserializeAsync<ObservableCollection<UserAbstraction>>(fs);

                addDataToUsers();
            }
        }

        public async Task jsonSerializationAddUser(int id, string lastName, string name, string secondName, string email)
        {
            await jsonSerializationOutput();

            using (FileStream fs = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                PublicUsers.Add(new UserAbstraction(id, lastName, name, secondName, email));

                await JsonSerializer.SerializeAsync(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public async Task jsonSerializationRemoveUser(int index)
        {
            await jsonSerializationOutput();

            using (FileStream fs = new FileStream("users.json", FileMode.Create))
            {
                PublicUsers.RemoveAt(index);

                await JsonSerializer.SerializeAsync(fs, PublicUsers);

                addDataToUsers();
            }
        }

        public async Task jsonSerializationChangeUser(int index, int id, string lastName, string name, string secondName, string email)
        {
            await jsonSerializationOutput();

            using (FileStream fs = new FileStream("users.json", FileMode.Create))
            {
                PublicUsers[index] = new UserAbstraction(id, lastName, name, secondName, email);

                await JsonSerializer.SerializeAsync(fs, PublicUsers);

                addDataToUsers();
            }
        }
        #endregion

        void addDataToUsers() 
        {
            Users.Clear();

            foreach (var u in PublicUsers)
                Users.Add(new User { ID = u.ID, LastName = u.LastName, Name = u.Name, SecondName = u.SecondName, Email = u.Email });
        }
    }
}
