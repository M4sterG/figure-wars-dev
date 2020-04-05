using Scripts.Classes.Main;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scripts.Classes.Icons
{
    public class PrimitiveIcon : IComparable<PrimitiveIcon>
    {
        public int ii_id { get; set; }
        public string ii_filename { get; set; }
        public int ii_offset { get; set; }
        public int ii_width { get; set; }
        public int ii_height { get; set; }
        public int ii_filesize { get; set; }
        public int ii_common { get; set; }

        public PrimitiveIcon(CgdDescriptor descr, Byte[] datablock)
        {
            PropertyInfo[] listProperties = this.GetType().GetProperties();

            int cursorpos = 0;

            for (int i = 0; i < listProperties.Length; i++)
            {
                Byte[] x = new Byte[descr.fieldsizes[i]];
                Buffer.BlockCopy(datablock, cursorpos, x, 0, x.Length);
                if (descr.fieldsizes[i] > 4)
                {
                    var value = Encoding.UTF8.GetString(x, 0, x.Length);
                    listProperties[i].SetValue(this, value);
                }
                else if (descr.fieldsizes[i] < 4)
                {
                    listProperties[i].SetValue(this, BitConverter.ToBoolean(x, 0));
                }
                else
                {
                    var value = BitConverter.ToInt32(x, 0);
                    listProperties[i].SetValue(this, value);
                }
                cursorpos += descr.fieldsizes[i];
            }
        }

        public int CompareTo(PrimitiveIcon other)
        {
            if (other == null)
                return 1;
            if (this.ii_id > other.ii_id && this.ii_offset > other.ii_offset)
                return 1;
            else if (this.ii_id < other.ii_id && this.ii_offset < other.ii_offset)
                return -1;
            else
                return 0;
        }
    }
}
