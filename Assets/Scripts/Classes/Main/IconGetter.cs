using Scripts.Classes.Icons;
using Scripts.Weapons;
using System;
using System.Collections.Generic;

namespace Scripts.Classes.Main
{
    public class IconGetter 
    {
        private const string MV_ICONS_PATH = "MV/iconsinfo_descr.json";
        private const string TW_ICONS_PATH = "TW/iconsinfo.json";

        public static List<PrimitiveIcon> getPrimIcons()
        {
            return getPrimIcons(MV_ICONS_PATH);
        }

        public static List<PrimitiveIcon> getPrimIcons(string iconsPath)
        {
            Console.WriteLine("!!! Debugger looks for the files in: " + Environment.CurrentDirectory);

            CgdDataReader<PrimitiveIcon> PrimitiveIconDataList = new CgdDataReader<PrimitiveIcon>(iconsPath);
            
            List<PrimitiveIcon> icons = PrimitiveIconDataList.GetDataList();
            return icons;
        }
    }
}