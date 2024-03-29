﻿using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Main;

namespace Scripts.Classes.Parts
{
    public class Part : Item
    {
        public HashSet<PartSlot> PartEquip { get; set; }

        public bool isAcc()
        {
            return PartEquip.Contains(PartSlot.HeadAcc) || PartEquip.Contains(PartSlot.BackAcc)
                                                        || PartEquip.Contains(PartSlot.WaistAcc);
        }

        public bool isSet()
        {
            return PartEquip.Contains(PartSlot.Top) && PartEquip.Count > 1;
        }

        public PartSlot DominantElement
        {
            get
            {
                if (isSet())
                {
                    return PartSlot.Top;
                }

                if (isAcc())
                {
                    return PartEquip.ElementAt(0);
                }

                if (PartEquip.Count > 1 && PartEquip.Contains(PartSlot.Legs))
                {
                    return PartSlot.Legs;
                }

                return PartEquip.ElementAt(0);
            }
        }
        
        public HashSet<Character> CharacterEquip { get; set; }
    }
    
   
    public enum PartSlot
    {
        HeadAcc,
        Hair,
        Face,
        Top,
        Hands,
        Skirt, // Naomi only
        Legs,
        Shoes,
        BackAcc,
        WaistAcc
    }

    public enum ExtraStats
    {
        Grenade9,
        Bazooka9,
        Sniper9,
        Shotgun9,
        Rifle60,
        Minigun60,
        Tank160,
        Tank80,
        Tank240,
        Tank480,
        Tank520,
        RunSpeed4,
        RunSpeed2,
        RunSpeed8,
        RunSpeed12,
        RunSpeed1,
        NoOption
    }

   
}