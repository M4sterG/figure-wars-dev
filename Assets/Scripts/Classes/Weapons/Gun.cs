using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public abstract class Gun : Weapon
    {
        public int Accuracy { set; get; }
        public new string ToUniquePropertyList()
        {
            return "accuracy";
        }

        public Gun()
        {
            this.ItemType = ItemType.Weapon;
        }
    }
}
