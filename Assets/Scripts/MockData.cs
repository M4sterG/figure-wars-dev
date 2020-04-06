using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Parts;

namespace DefaultNamespace
{
    public class MockData
    {

        public static List<Part> fiveEachType(List<Part> parts)
        {
            var list = new List<Part>();
            Dictionary<PartSlot, int> dic = new Dictionary<PartSlot, int>();
            foreach (var slot in Enum.GetValues(typeof(PartSlot)))
            {
                dic.Add((PartSlot) slot, 0);
            }

            foreach (Part p in parts)
            {
                if (dic[p.PartEquip.ElementAt(0)] < 5){
                {
                    dic[p.PartEquip.ElementAt(0)]++;
                    list.Add(p);
                }}
            }

            foreach (var p in parts.FindAll(p => p.isSet()))
            {
                list.Add(p);
            }

            return list;
        }
    }
}