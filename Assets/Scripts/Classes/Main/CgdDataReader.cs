using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Scripts.Classes.Main
{
    public class CgdDataReader<T>
    {
        private List<T> cgdObjectList;
        private string path;

        public CgdDataReader(string path)
        {
            this.path = path;
            cgdObjectList = new List<T>();
        }

        public List<T> GetDataList()
        {
            string json = new StreamReader(path).ReadToEnd();
            CgdDescriptor descriptor = JsonConvert.DeserializeObject<CgdDescriptor>(json);
            BinaryReader binFile = new BinaryReader(File.OpenRead(descriptor.binpath));
            int datablocksize = descriptor.fieldsizes.Sum();
            Byte[] datablock = new Byte[datablocksize];
            int index = 0;

            while (binFile.BaseStream.Position != binFile.BaseStream.Length)
            {
                datablock = binFile.ReadBytes(datablocksize);
                T target = (T)Activator.CreateInstance(typeof(T), new object[] { descriptor, datablock });

                cgdObjectList.Add(target);
                index += datablocksize;
            }
            return cgdObjectList;
        }
    }

    public class CgdDescriptor
    {
        public string binpath;
        public int[] fieldsizes;
        public String[] fieldnames;
    }
}