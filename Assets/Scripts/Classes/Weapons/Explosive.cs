using System;
using System.Collections.Generic;
using System.Text;

namespace Scripts.Weapons
{
   public abstract class Explosive : Weapon
   {
        protected int BlastRadius { set; get; }
        public new string ToUniquePropertyList()
        {
            return "blast_radius";
        }
    }
}
