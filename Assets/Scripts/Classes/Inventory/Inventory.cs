using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.Weapons;

namespace Scripts.Classes.Inventory
{
    public class Inventory
    {
        private readonly int id;
        private readonly int userId;
        private List<ActualWeapon> weaponHolders = new List<ActualWeapon>();
        private List<Part> parts;
        private List<Item> miscItems;

        public void addActualWeapon(Weapon wep)
        {
            weaponHolders.Add(new ActualWeapon(wep));
        }

        public void addActualWeapon(ActualWeapon wep)
        {
            weaponHolders.Add(wep);
        }

        public ActualWeapon weaponAt(int i)
        {
            if (!(i >= 0 && i < weaponHolders.Count))
            {
                throw new ArgumentException("Illegal index ");
            }

            return weaponHolders.ElementAt(i);
        }

        public List<ActualWeapon> getWeapons()
        {
            return weaponHolders;
        }
        
        
        
        
        

    }
}