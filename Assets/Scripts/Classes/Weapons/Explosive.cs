using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
   public abstract class Explosive : Weapon
   {
        protected int BlastRadius { set; get; }
        public new string ToUniquePropertyList()
        {
            return "blast_radius";
        }

        public Explosive()
        {
            this.ItemType = ItemType.Weapon;
        }
    }
}
