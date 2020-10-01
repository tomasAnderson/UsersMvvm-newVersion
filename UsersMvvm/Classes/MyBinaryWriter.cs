using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMvvm.abstraction;

namespace UsersMvvm.Classes
{
    class MyBinaryWriter
    {
        string fileName;

        public void Create(string outputFileName) 
        {
            BinaryWriter bw;

            //create the file
            try
            {
                bw = new BinaryWriter(new FileStream(outputFileName, FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }

            bw.Close();

            fileName = outputFileName;
        }

        public void Write(ObservableCollection<UserAbstraction> users) 
        {
            //writing into the file
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Open)))
            {
                try
                {
                    foreach (var u in users)
                    {
                        bw.Write(u.ID);
                        bw.Write(u.LastName);
                        bw.Write(u.Name);
                        bw.Write(u.SecondName);
                        bw.Write(u.Email);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    return;
                }
            }
        }

        public ObservableCollection<UserAbstraction> Read(string filePath) 
        {
            BinaryReader br;
            ObservableCollection<UserAbstraction> list = new ObservableCollection<UserAbstraction>();

            int id;
            string lastName, name, secondName, email;

            //reading from the file
            try
            {
                br = new BinaryReader(new FileStream(filePath, FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot open file.");
                return null;
            }

            try
            {
                while (true) {
                    id = br.ReadInt32();
                    Console.WriteLine("Integer data: {0}", id);
                    lastName = br.ReadString();
                    Console.WriteLine("String data: {0}", lastName);
                    name = br.ReadString();
                    Console.WriteLine("String data: {0}", name);
                    secondName = br.ReadString();
                    Console.WriteLine("String data: {0}", secondName);
                    email = br.ReadString();
                    Console.WriteLine("String data: {0}", email);


                    list.Add(new UserAbstraction { ID = id, LastName = lastName, Name = name, SecondName = secondName, Email = email });

                }
            }
            catch (IOException) { }

            br.Close();

            return list;
        }

    }
}
