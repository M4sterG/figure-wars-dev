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
        private List<Part> parts = new List<Part>();
        private List<Item> miscItems = new List<Item>();

        public List<Part> getParts()
        {
            return parts;
        }

        public List<Item> getMisc()
        {
            return miscItems;
        }

        public void addParts(List<Part> data)
        {
            data.ForEach(p => parts.Add(p));
        }

        public void addActualWeapon(Weapon wep)
        {
            weaponHolders.Add(new ActualWeapon(wep));
        }

        public void addActualWeapon(ActualWeapon wep)
        {
            weaponHolders.Add(wep);
        }

        public void addWeapons(List<Weapon> weapons)
        {
            weapons.ForEach(w => weaponHolders.Add(new ActualWeapon(w)));
        }

        public void addWeapons(List<ActualWeapon> weapons)
        {
            weapons.ForEach(w => weaponHolders.Add(w));
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