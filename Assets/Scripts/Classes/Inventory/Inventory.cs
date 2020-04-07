using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.InventoryHandlers;
using Scripts.Weapons;
using UnityEngine;

namespace Scripts.Classes.Inventory
{
    public class Inventory
    {
        private readonly int id;
        private readonly int userId;
        private List<ActualWeapon> weaponHolders = new List<ActualWeapon>();
        private List<Part> parts = new List<Part>();
        private List<Item> miscItems = new List<Item>();
        private Dictionary<WeaponType, ActualWeapon> equippedWeapons = new Dictionary<WeaponType, ActualWeapon>
        {
            {WeaponType.Melee, null},
            {WeaponType.Rifle, null},
            {WeaponType.Shotgun, null},
            {WeaponType.Sniper, null},
            {WeaponType.Minigun, null},
            {WeaponType.Bazooka, null},
            {WeaponType.Grenade, null}
        };
        
        private Dictionary<WeaponType, string> basicWeapons = new Dictionary<WeaponType, string>
        {
            {WeaponType.Melee, "Folding Shovel"},
            {WeaponType.Rifle, "Cricket"},
            {WeaponType.Shotgun, "Zolo"},
            {WeaponType.Sniper, "Jam"},
            {WeaponType.Minigun, "Microgun"},
            {WeaponType.Bazooka, "Sting Ray"},
            {WeaponType.Grenade, "Hot Dog"}
        };

        private Dictionary<PartSlot, Part> equippedParts = getEmptyEquippedParts();

        public Dictionary<PartSlot, Part> getEquippedParts()
        {
            return equippedParts;
        }

        private static Dictionary<PartSlot, Part> getEmptyEquippedParts()
        {
            Dictionary<PartSlot, Part> dictionary = new Dictionary<PartSlot, Part>();
            foreach (PartSlot slot in Enum.GetValues(typeof(PartSlot)))
            {
                dictionary.Add(slot, null);
            }
            return dictionary;
        }

        public void setWeaponList(List<ActualWeapon> weapons)
        {
            if (weapons != null)
            {
                weaponHolders = weapons;
            }
        }

        public void equipWeapon(WeaponType type, ActualWeapon weapon)
        {
            ActualWeapon prevEquipped = equippedWeapons[type];
            if (prevEquipped != null && !prevEquipped.Name.Equals(basicWeapons[type]))
            {
                weaponHolders.Add(prevEquipped);
            }
            equippedWeapons[type] = weapon;
            List<ActualWeapon> unequippedWeapons = weaponHolders.FindAll(w => w != weapon);
            weaponHolders = unequippedWeapons;
        }
        
        public void unequipWeapon(WeaponType type)
        {
            ActualWeapon wep = equippedWeapons[type];
            equippedWeapons[type] = null;
            weaponHolders.Add(wep);
        }

        public void equipPart(Part part)
        {
            foreach (var slot in part.PartEquip)
            {
                Part previous = equippedParts[slot];
                if (previous != null)
                {
                    parts.Add(previous);
                    foreach (var oldSlot in previous.PartEquip)
                    {
                       // if (!part.PartEquip.Contains(oldSlot))
                       // {
                            equippedParts[oldSlot] = null;
                      //  }
                    }
                }
                equippedParts[slot] = part;
            }
            List<Part> unequippedParts = parts.FindAll(p => p != part);
            parts = unequippedParts;
        }

        public void unequipPart(PartSlot slot)
        {
            Part toUnequip = equippedParts[slot];
            foreach (PartSlot equipped in toUnequip.PartEquip)
            {
                equippedParts[equipped] = null;
            }
            parts.Add(toUnequip);
            
            
        }

     

        public Dictionary<WeaponType, ActualWeapon> getEquippedWeapons()
        {
            return equippedWeapons;
        }

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